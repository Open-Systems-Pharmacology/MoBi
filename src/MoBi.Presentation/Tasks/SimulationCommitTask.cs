using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants.Commands;

namespace MoBi.Presentation.Tasks
{
   public interface ISimulationCommitTask
   {
      /// <summary>
      ///    Commits <paramref name="simulationWithChanges" /> changes to a module of the simulation configuration
      ///    by creating or updating it's initial conditions and parameter values building blocks.
      ///    The user selects the module and, for each building block type, the building block (or a new one) to commit to,
      ///    the last module and its selected building blocks being the default. When a target building block
      ///    is not the one used by the simulation, the simulation stays out of sync and has to be configured manually
      /// </summary>
      /// <returns>An executed command</returns>
      IMoBiCommand CommitSimulationChanges(IMoBiSimulation simulationWithChanges);
   }

   public class SimulationCommitTask : ISimulationCommitTask
   {
      private readonly IMoBiContext _context;
      private readonly ITemplateResolverTask _templateResolverTask;
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly INameCorrector _nameCorrector;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IInteractionTaskContext _interactionTaskContext;

      public SimulationCommitTask(IMoBiContext context,
         ITemplateResolverTask templateResolverTask,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         INameCorrector nameCorrector,
         IObjectTypeResolver objectTypeResolver,
         IInteractionTaskContext interactionTaskContext)
      {
         _context = context;
         _templateResolverTask = templateResolverTask;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _nameCorrector = nameCorrector;
         _objectTypeResolver = objectTypeResolver;
         _interactionTaskContext = interactionTaskContext;
      }

      public IMoBiCommand CommitSimulationChanges(IMoBiSimulation simulationWithChanges)
      {
         if (simulationWithChanges.HasUntraceableChanges)
         {
            showErrorForUntraceableChanges(simulationWithChanges);
            return new MoBiEmptyCommand();
         }

         var moleculeChanges = changesFrom<MoleculeAmount>(simulationWithChanges).ToList();
         var parameterChanges = new List<(ObjectPath quantityPath, IParameter quantity)>();
         changesFrom<Parameter>(simulationWithChanges).Each(x => parameterChanges.Add(x));
         changesFrom<DistributedParameter>(simulationWithChanges).Each(x => parameterChanges.Add(x));

         var commitTarget = selectCommitTargetFrom(simulationWithChanges, parameterChanges.Any(), moleculeChanges.Any());
         if (commitTarget == null)
            return null;

         var moduleConfiguration = commitTarget.ModuleConfiguration;
         var commitsToNotUsedBuildingBlock = parameterChanges.Any() && !commitsToUsedParameterValues(commitTarget) ||
                                             moleculeChanges.Any() && !commitsToUsedInitialConditions(commitTarget);
         var changesShadowedByLaterModule = changesAreShadowedByLaterModule(simulationWithChanges, commitTarget, parameterChanges, moleculeChanges);
         var simulationStaysOutOfSync = commitsToNotUsedBuildingBlock || changesShadowedByLaterModule;

         var message = CommitingChangesToModulesMessage(moduleConfiguration.Module, commitTarget.InitialConditions, commitTarget.ParameterValues, moleculeChanges.Any(), parameterChanges.Any());
         if (commitsToNotUsedBuildingBlock)
            message += $"{Environment.NewLine}{Environment.NewLine}{CommitToNotUsedBuildingBlockWarning}";
         if (changesShadowedByLaterModule)
            message += $"{Environment.NewLine}{Environment.NewLine}{CommitShadowedByLaterModuleWarning}";

         if (_interactionTaskContext.DialogCreator.MessageBoxYesNo(message) != ViewResult.Yes)
            return null;

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = CommitCommand,
            Description = CommitCommandDescription(simulationWithChanges, moduleConfiguration.Module),
            ObjectType = _objectTypeResolver.TypeFor<Module>()
         };

         macroCommand.AddRange(initialConditionsCommandsFor(simulationWithChanges, commitTarget, moleculeChanges));
         macroCommand.AddRange(parameterValuesCommandsFor(simulationWithChanges, commitTarget, parameterChanges));

         if (!simulationStaysOutOfSync)
            macroCommand.Add(new ClearOriginalQuantitiesTrackerCommand(simulationWithChanges));

         macroCommand.RunCommand(_context);
         _context.PublishEvent(new SimulationReloadEvent(simulationWithChanges));

         return macroCommand;
      }

      private void showErrorForUntraceableChanges(IMoBiSimulation simulationWithChanges)
      {
         _interactionTaskContext.DialogCreator.MessageBoxError(AppConstants.Captions.SimulationHasChangesThatCannotBeCommitted(simulationWithChanges.Name));
      }

      /// <summary>
      ///    Returns the module and the building block of each type where the changes will be committed, all selected
      ///    by the user, the last module and its selected building blocks being the default.
      ///    Returns null if the user cancels the selection
      /// </summary>
      private CommitTarget selectCommitTargetFrom(IMoBiSimulation simulation, bool hasParameterChanges, bool hasMoleculeChanges)
      {
         var moduleConfigurations = simulation.Configuration.ModuleConfigurations;

         // there is no selection to make when the simulation uses a single module without selected building
         // blocks and the template module does not contain any to select from
         if (moduleConfigurations.Count == 1 && noBuildingBlockToSelectFor(moduleConfigurations[0], hasParameterChanges, hasMoleculeChanges))
            return new CommitTarget(moduleConfigurations[0], parameterValues: null, initialConditions: null);

         using (var presenter = _context.Resolve<ISelectCommitTargetPresenter>())
         {
            return presenter.SelectCommitTargetFor(simulation, hasParameterChanges, hasMoleculeChanges);
         }
      }

      /// <summary>
      ///    Returns true when there is no building block selection to offer for any of the types with changes
      /// </summary>
      private bool noBuildingBlockToSelectFor(ModuleConfiguration moduleConfiguration, bool hasParameterChanges, bool hasMoleculeChanges)
      {
         var templateModule = _templateResolverTask.TemplateModuleFor(moduleConfiguration.Module);
         var parameterValuesToSelect = hasParameterChanges && (moduleConfiguration.SelectedParameterValues != null || templateModule.ParameterValuesCollection.Any());
         var initialConditionsToSelect = hasMoleculeChanges && (moduleConfiguration.SelectedInitialConditions != null || templateModule.InitialConditionsCollection.Any());
         return !parameterValuesToSelect && !initialConditionsToSelect;
      }

      /// <summary>
      ///    Returns true when any committed path is also defined in a building block selected in a module configuration
      ///    coming after the target module. Those values win when the simulation is created, so the created simulation
      ///    would not reproduce the committed values
      /// </summary>
      private bool changesAreShadowedByLaterModule(IMoBiSimulation simulation, CommitTarget commitTarget, IReadOnlyList<(ObjectPath quantityPath, IParameter quantity)> parameterChanges, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges)
      {
         var laterModuleConfigurations = simulation.Configuration.ModuleConfigurations.SkipWhile(x => !Equals(x, commitTarget.ModuleConfiguration)).Skip(1).ToList();

         return parameterChanges.Any(change => laterModuleConfigurations.Any(x => x.SelectedParameterValues?[change.quantityPath] != null)) ||
                moleculeChanges.Any(change => laterModuleConfigurations.Any(x => x.SelectedInitialConditions?[change.quantityPath] != null));
      }

      /// <summary>
      ///    Returns true when the changes will be committed to the parameter values building block used by the
      ///    simulation, meaning the simulation is in sync with its building blocks after the commit
      /// </summary>
      private bool commitsToUsedParameterValues(CommitTarget commitTarget)
      {
         var selectedParameterValues = commitTarget.ModuleConfiguration.SelectedParameterValues;
         if (commitTarget.CreateNewParameterValues)
            return selectedParameterValues == null;

         return selectedParameterValues != null && Equals(_templateResolverTask.TemplateBuildingBlockFor(selectedParameterValues), commitTarget.ParameterValues);
      }

      /// <summary>
      ///    Returns true when the changes will be committed to the initial conditions building block used by the
      ///    simulation, meaning the simulation is in sync with its building blocks after the commit
      /// </summary>
      private bool commitsToUsedInitialConditions(CommitTarget commitTarget)
      {
         var selectedInitialConditions = commitTarget.ModuleConfiguration.SelectedInitialConditions;
         if (commitTarget.CreateNewInitialConditions)
            return selectedInitialConditions == null;

         return selectedInitialConditions != null && Equals(_templateResolverTask.TemplateBuildingBlockFor(selectedInitialConditions), commitTarget.InitialConditions);
      }

      private IEnumerable<IMoBiCommand> parameterValuesCommandsFor(IMoBiSimulation simulation, CommitTarget commitTarget, IReadOnlyList<(ObjectPath quantityPath, IParameter quantity)> parameterChanges)
      {
         var moduleConfiguration = commitTarget.ModuleConfiguration;

         if (commitTarget.CreateNewParameterValues)
            return moduleConfiguration.SelectedParameterValues == null
               ? addNewParameterValuesFromSimulationChanges(simulation, moduleConfiguration, parameterChanges)
               : addNewParameterValuesToTemplateModule(simulation, moduleConfiguration, parameterChanges);

         return commitsToUsedParameterValues(commitTarget)
            ? updateParameterValuesFromSimulationChanges(moduleConfiguration, parameterChanges, simulation)
            : updateNotUsedParameterValues(commitTarget.ParameterValues, parameterChanges, simulation);
      }

      private IEnumerable<IMoBiCommand> initialConditionsCommandsFor(IMoBiSimulation simulation, CommitTarget commitTarget, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges)
      {
         var moduleConfiguration = commitTarget.ModuleConfiguration;

         if (commitTarget.CreateNewInitialConditions)
            return moduleConfiguration.SelectedInitialConditions == null
               ? addNewInitialConditionsFromSimulationChanges(simulation, moduleConfiguration, moleculeChanges)
               : addNewInitialConditionsToTemplateModule(simulation, moduleConfiguration, moleculeChanges);

         return commitsToUsedInitialConditions(commitTarget)
            ? updateInitialConditionsFromSimulationChanges(moduleConfiguration, moleculeChanges, simulation)
            : updateNotUsedInitialConditions(commitTarget.InitialConditions, moleculeChanges, simulation);
      }

      /// <summary>
      ///    Creates commands that synchronize the changes from the simulation into <paramref name="templateBuildingBlock" />,
      ///    a building block that is not used by the simulation. The simulation configuration is not updated
      /// </summary>
      private IEnumerable<IMoBiCommand> updateNotUsedParameterValues(ParameterValuesBuildingBlock templateBuildingBlock, IReadOnlyList<(ObjectPath quantityPath, IParameter quantity)> parameterChanges, IMoBiSimulation simulation)
      {
         return parameterChanges.Select(x => synchronizeParameterValueCommand(x.quantity, x.quantityPath, templateBuildingBlock, simulation));
      }

      /// <summary>
      ///    Creates commands that synchronize the changes from the simulation into <paramref name="templateBuildingBlock" />,
      ///    a building block that is not used by the simulation. The simulation configuration is not updated
      /// </summary>
      private IEnumerable<IMoBiCommand> updateNotUsedInitialConditions(InitialConditionsBuildingBlock templateBuildingBlock, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges, IMoBiSimulation simulation)
      {
         return moleculeChanges.Select(x => synchronizeInitialConditionCommand(x.quantity, x.quantityPath, templateBuildingBlock, simulation));
      }

      /// <summary>
      ///    Creates commands that add a new ParameterValuesBuildingBlock containing the changes from the simulation to the
      ///    template module only. The simulation configuration is not updated because it already uses another building block
      /// </summary>
      private IEnumerable<IMoBiCommand> addNewParameterValuesToTemplateModule(IMoBiSimulation simulation, ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, IParameter quantity)> parameterChanges)
      {
         if (!parameterChanges.Any())
            return new[] { new MoBiEmptyCommand() };

         var templateModule = _templateResolverTask.TemplateModuleFor(moduleConfiguration.Module);
         var templateBuildingBlock = _context.Create<ParameterValuesBuildingBlock>().WithName(simulation.Name);
         _nameCorrector.AutoCorrectName(templateModule.BuildingBlocks.OfType<ParameterValuesBuildingBlock>().AllNames(), templateBuildingBlock);

         var commands = new List<IMoBiCommand>
         {
            new AddBuildingBlockToModuleCommand<ParameterValuesBuildingBlock>(templateBuildingBlock, templateModule)
         };

         commands.AddRange(parameterChanges.Select(x => synchronizeParameterValueCommand(x.quantity, x.quantityPath, templateBuildingBlock, simulation).AsHidden()));
         return commands;
      }

      /// <summary>
      ///    Creates commands that add a new InitialConditionsBuildingBlock containing the changes from the simulation to the
      ///    template module only. The simulation configuration is not updated because it already uses another building block
      /// </summary>
      private IEnumerable<IMoBiCommand> addNewInitialConditionsToTemplateModule(IMoBiSimulation simulation, ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges)
      {
         if (!moleculeChanges.Any())
            return new[] { new MoBiEmptyCommand() };

         var templateModule = _templateResolverTask.TemplateModuleFor(moduleConfiguration.Module);
         var templateBuildingBlock = _context.Create<InitialConditionsBuildingBlock>().WithName(simulation.Name);
         _nameCorrector.AutoCorrectName(templateModule.BuildingBlocks.OfType<InitialConditionsBuildingBlock>().AllNames(), templateBuildingBlock);

         var commands = new List<IMoBiCommand>
         {
            new AddBuildingBlockToModuleCommand<InitialConditionsBuildingBlock>(templateBuildingBlock, templateModule)
         };

         commands.AddRange(moleculeChanges.Select(x => synchronizeInitialConditionCommand(x.quantity, x.quantityPath, templateBuildingBlock, simulation).AsHidden()));
         return commands;
      }

      /// <summary>
      ///    Creates two new macro commands that have synchronized the parameter values building blocks from the template module
      ///    and simulation module with the changes from the simulation. The simulation module is identified by
      ///    <paramref name="moduleConfiguration" /> and it must  have SelectedParameterValues as that building block will
      ///    receive the updates. The template module and building block is resolved from the project by name
      /// </summary>
      private IEnumerable<IMoBiCommand> updateParameterValuesFromSimulationChanges(ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, IParameter quantity)> parameterChanges, IMoBiSimulation simulation)
      {
         var templateBuildingBlock = _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedParameterValues);
         return parameterChanges.Select(x => synchronizeParameterValueCommand(x.quantity, x.quantityPath, templateBuildingBlock, simulation)).Concat(parameterChanges.Select(x => synchronizeParameterValueCommand(x.quantity, x.quantityPath, moduleConfiguration.SelectedParameterValues, simulation).AsHidden()));
      }

      /// <summary>
      ///    Creates two new macro commands that have synchronized the initial conditions building blocks from the template
      ///    module and simulation module respectively with the changes from the simulation. The simulation module is identified
      ///    by    <paramref name="moduleConfiguration" /> and it must have SelectedInitialConditions as that building block will
      ///    receive the updates. The template module and building block is resolved from the project by name
      /// </summary>
      private IEnumerable<IMoBiCommand> updateInitialConditionsFromSimulationChanges(ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges, IMoBiSimulation simulation)
      {
         var templateBuildingBlock = _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedInitialConditions);

         return moleculeChanges.Select(x => synchronizeInitialConditionCommand(x.quantity, x.quantityPath, templateBuildingBlock, simulation)).Concat(moleculeChanges.Select(x => synchronizeInitialConditionCommand(x.quantity, x.quantityPath, moduleConfiguration.SelectedInitialConditions, simulation).AsHidden()));
      }

      /// <summary>
      ///    Creates new commands that add new ParameterValuesBuildingBlock to a simulation module and to a template
      ///    module respectively. The simulation module is identified by <paramref name="moduleConfiguration" /> and the
      ///    template module is resolved from the project by name. The new building blocks will contain values for changes
      ///    in the <paramref name="simulation" />. The new building block will be selected in <paramref name="moduleConfiguration" />
      /// </summary>
      private IEnumerable<IMoBiCommand> addNewParameterValuesFromSimulationChanges(IMoBiSimulation simulation, ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, IParameter quantity)> parameterChanges)
      {
         if (!parameterChanges.Any())
            return new[] { new MoBiEmptyCommand() };

         var (templateModule, templateBuildingBlock, simulationBuildingBlock) = createNewBuildingBlocks<ParameterValuesBuildingBlock>(simulation, moduleConfiguration);

         var commands = new List<IMoBiCommand>
         {
            new AddBuildingBlockToModuleCommand<ParameterValuesBuildingBlock>(templateBuildingBlock, templateModule),
            new AddSelectedBuildingBlockToModuleConfigurationCommand<ParameterValuesBuildingBlock>(simulationBuildingBlock, moduleConfiguration, simulation).AsHidden()
         };

         commands.AddRange(parameterChanges.Select(x => synchronizeParameterValueCommand(x.quantity, x.quantityPath, templateBuildingBlock, simulation).AsHidden()));
         commands.AddRange(parameterChanges.Select(x => synchronizeParameterValueCommand(x.quantity, x.quantityPath, simulationBuildingBlock, simulation).AsHidden()));
         return commands;
      }

      /// <summary>
      ///    Creates new commands that add new InitialConditionsBuildingBlock to a simulation module and to a template
      ///    module respectively. The simulation module is identified by <paramref name="moduleConfiguration" /> and the
      ///    template module is resolved from the project by name. The new building blocks will contain values for changes
      ///    in the <paramref name="simulation" />. The new building block will be selected in <paramref name="moduleConfiguration" />
      /// </summary>
      private IEnumerable<IMoBiCommand> addNewInitialConditionsFromSimulationChanges(IMoBiSimulation simulation, ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges)
      {
         if (!moleculeChanges.Any())
            return new[] { new MoBiEmptyCommand() };

         var (templateModule, templateBuildingBlock, simulationBuildingBlock) = createNewBuildingBlocks<InitialConditionsBuildingBlock>(simulation, moduleConfiguration);

         var commands = new List<IMoBiCommand>
         {
            new AddBuildingBlockToModuleCommand<InitialConditionsBuildingBlock>(templateBuildingBlock, templateModule),
            new AddSelectedBuildingBlockToModuleConfigurationCommand<InitialConditionsBuildingBlock>(simulationBuildingBlock, moduleConfiguration, simulation).AsHidden()
         };

         commands.AddRange(moleculeChanges.Select(x => synchronizeInitialConditionCommand(x.quantity, x.quantityPath, templateBuildingBlock, simulation).AsHidden()));
         commands.AddRange(moleculeChanges.Select(x => synchronizeInitialConditionCommand(x.quantity, x.quantityPath, simulationBuildingBlock, simulation).AsHidden()));
         return commands;
      }

      /// <summary>
      ///    Returns path and quantity tuple for all changes in the <paramref name="simulation" /> of type
      ///    <typeparamref name="TQuantity" />
      /// </summary>
      private IEnumerable<(ObjectPath quantityPath, TQuantity quantity)> changesFrom<TQuantity>(IMoBiSimulation simulation) where TQuantity : Quantity
      {
         var quantities = _entitiesInSimulationRetriever.EntitiesFrom<TQuantity>(simulation);
         return simulation.OriginalQuantityValues.Select(x => (objectPath: x.Path.ToObjectPath(), quantity: quantities[x.Path])).Where(x => x.quantity != null);
      }

      private (Module templateModule, TBuildingBlock templateBuildingBlock, TBuildingBlock simulationBuildingBlock) createNewBuildingBlocks<TBuildingBlock>(IMoBiSimulation simulation,
         ModuleConfiguration moduleConfiguration)
         where TBuildingBlock : class, IBuildingBlock
      {
         var templateModule = _templateResolverTask.TemplateModuleFor(moduleConfiguration.Module);

         var templateBuildingBlock = _context.Create<TBuildingBlock>().WithName(simulation.Name);
         _nameCorrector.AutoCorrectName(templateModule.BuildingBlocks.OfType<TBuildingBlock>().AllNames(), templateBuildingBlock);

         var simulationBuildingBlock = _context.Create<TBuildingBlock>().WithName(templateBuildingBlock.Name);
         return (templateModule, templateBuildingBlock, simulationBuildingBlock);
      }

      private IMoBiCommand synchronizeInitialConditionCommand(MoleculeAmount moleculeAmount, ObjectPath quantityPath, InitialConditionsBuildingBlock initialConditionsBuildingBlock, IMoBiSimulation simulation)
      {
         var initialConditionToUpdate = initialConditionsBuildingBlock[quantityPath];

         if (initialConditionToUpdate != null)
            return new SynchronizeInitialConditionCommand(moleculeAmount, initialConditionToUpdate, initialConditionsBuildingBlock, simulation);

         return new AddInitialConditionFromQuantityInSimulationCommand(moleculeAmount, initialConditionsBuildingBlock, simulation);
      }

      private IMoBiCommand synchronizeParameterValueCommand(IParameter parameter, ObjectPath quantityPath, ParameterValuesBuildingBlock parameterValuesBuildingBlock, IMoBiSimulation simulation)
      {
         var parameterValueToUpdate = parameterValuesBuildingBlock[quantityPath];

         if (parameterValueToUpdate != null)
            return new SynchronizeParameterValueCommand(parameter, parameterValueToUpdate, parameterValuesBuildingBlock, simulation);

         return new AddParameterValueFromQuantityInSimulationCommand(parameter, parameterValuesBuildingBlock, simulation);
      }
   }
}