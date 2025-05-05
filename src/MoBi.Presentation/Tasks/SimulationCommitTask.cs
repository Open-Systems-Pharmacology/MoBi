using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
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
      ///    Commits <paramref name="simulationWithChanges" /> changes to the last module in the simulation configuration
      ///    by creating or updating it's initial conditions and parameter values building blocks
      ///    The last module has to be used because if you re-create the simulation from the same modules
      ///    this is the only way you will get the same simulation
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

         var message = CommitingChangesToModulesMessage(simulationWithChanges.Configuration.ModuleConfigurations.Last(), moleculeChanges.Any(), parameterChanges.Any());

         if (_interactionTaskContext.DialogCreator.MessageBoxYesNo(message) != ViewResult.Yes)
            return null;

         var lastModuleConfiguration = simulationWithChanges.Configuration.ModuleConfigurations.Last();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = CommitCommand,
            Description = CommitCommandDescription(simulationWithChanges, lastModuleConfiguration.Module),
            ObjectType = _objectTypeResolver.TypeFor<Module>()
         };

         if (lastModuleConfiguration.SelectedInitialConditions == null)
            macroCommand.AddRange(addNewInitialConditionsFromSimulationChanges(simulationWithChanges, lastModuleConfiguration, moleculeChanges));
         else
            macroCommand.AddRange(updateInitialConditionsFromSimulationChanges(lastModuleConfiguration, moleculeChanges, simulationWithChanges));

         if (lastModuleConfiguration.SelectedParameterValues == null)
            macroCommand.AddRange(addNewParameterValuesFromSimulationChanges(simulationWithChanges, lastModuleConfiguration, parameterChanges));
         else
            macroCommand.AddRange(updateParameterValuesFromSimulationChanges(lastModuleConfiguration, parameterChanges, simulationWithChanges));

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
            new AddSelectedBuildingBlockToLastModuleConfigurationCommand<ParameterValuesBuildingBlock>(simulationBuildingBlock, simulation).AsHidden()
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
            new AddSelectedBuildingBlockToLastModuleConfigurationCommand<InitialConditionsBuildingBlock>(simulationBuildingBlock, simulation).AsHidden()
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