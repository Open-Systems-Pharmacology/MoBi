using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
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
      private readonly IInitialConditionsCreator _initialConditionsCreator;
      private readonly IParameterValuesCreator _parameterValuesCreator;
      private readonly INameCorrector _nameCorrector;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IInteractionTaskContext _interactionTaskContext;

      public SimulationCommitTask(IMoBiContext context,
         ITemplateResolverTask templateResolverTask,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IInitialConditionsCreator initialConditionsCreator,
         IParameterValuesCreator parameterValuesCreator,
         INameCorrector nameCorrector,
         IObjectTypeResolver objectTypeResolver,
         IInteractionTaskContext interactionTaskContext)
      {
         _context = context;
         _templateResolverTask = templateResolverTask;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _initialConditionsCreator = initialConditionsCreator;
         _parameterValuesCreator = parameterValuesCreator;
         _nameCorrector = nameCorrector;
         _objectTypeResolver = objectTypeResolver;
         _interactionTaskContext = interactionTaskContext;
      }

      public IMoBiCommand CommitSimulationChanges(IMoBiSimulation simulationWithChanges)
      {
         var changes = getChanges(simulationWithChanges);
         if (_interactionTaskContext.DialogCreator.MessageBoxYesNo(changes.message) != ViewResult.Yes)
            return null;

         var lastModuleConfiguration = simulationWithChanges.Configuration.ModuleConfigurations.Last();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = CommitCommand,
            Description = CommitCommandDescription(simulationWithChanges, lastModuleConfiguration.Module),
            ObjectType = _objectTypeResolver.TypeFor<Module>()
         };

         if (lastModuleConfiguration.SelectedInitialConditions == null)
            macroCommand.AddRange(addNewInitialConditionsFromSimulationChanges(simulationWithChanges, lastModuleConfiguration, changes.moleculeChanges));
         else
            macroCommand.AddRange(updateInitialConditionsFromSimulationChanges(lastModuleConfiguration, changes.moleculeChanges));

         if (lastModuleConfiguration.SelectedParameterValues == null)
            macroCommand.AddRange(addNewParameterValuesFromSimulationChanges(simulationWithChanges, lastModuleConfiguration, changes.parameterChanges));
         else
            macroCommand.AddRange(updateParameterValuesFromSimulationChanges(lastModuleConfiguration, changes.parameterChanges));

         macroCommand.Add(new ClearOriginalQuantitiesTrackerCommand(simulationWithChanges));

         macroCommand.RunCommand(_context);
         _context.PublishEvent(new SimulationStatusChangedEvent(simulationWithChanges));
         return macroCommand;
      }

      private (string message, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges, IReadOnlyList<(ObjectPath quantityPath, Parameter quantity)> parameterChanges) getChanges(IMoBiSimulation simulationWithChanges)
      {
         var lastModuleConfiguration = simulationWithChanges.Configuration.ModuleConfigurations.Last();
         var changesForICValues = changesFrom<MoleculeAmount>(simulationWithChanges).ToList();
         var changesForParameterValues = changesFrom<Parameter>(simulationWithChanges).ToList();

         var message = CommitingChangesToModulesMessage(lastModuleConfiguration, changesForICValues, changesForParameterValues);

         return (message, changesForICValues, changesForParameterValues);
      }

      /// <summary>
      ///    Creates two new macro commands that have synchronized the parameter values building blocks from the template module
      ///    and simulation module
      ///    with the changes from the <paramref name="simulation" />. The simulation module is identified by
      ///    <paramref name="moduleConfiguration" /> and it must
      ///    have SelectedParameterValues as that building block will receive the updates. The template module and building block
      ///    is resolved from the project by name
      /// </summary>
      private IEnumerable<IMoBiCommand> updateParameterValuesFromSimulationChanges(ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, Parameter quantity)> parameterChanges)
      {
         var templateBuildingBlock = _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedParameterValues);

         var valueTuples = parameterChanges;
         return valueTuples.Select(x => synchronizeParameterValueCommand(x.quantity, x.quantityPath, templateBuildingBlock)).Concat(valueTuples.Select(x => synchronizeParameterValueCommand(x.quantity, x.quantityPath, moduleConfiguration.SelectedParameterValues).AsHidden()));
      }

      /// <summary>
      ///    Creates two new macro commands that have synchronized the initial conditions building blocks from the template
      ///    module and simulation module respectively
      ///    with the changes from the <paramref name="simulation" />. The simulation module is identified by
      ///    <paramref name="moduleConfiguration" /> and it must
      ///    have SelectedInitialConditions as that building block will receive the updates. The template module and building
      ///    block is resolved from the project by name
      /// </summary>
      private IEnumerable<IMoBiCommand> updateInitialConditionsFromSimulationChanges(ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges)
      {
         var templateBuildingBlock = _templateResolverTask.TemplateBuildingBlockFor(moduleConfiguration.SelectedInitialConditions);

         var valueTuples = moleculeChanges;
         return valueTuples.Select(x => synchronizeInitialConditionCommand(x.quantity, x.quantityPath, templateBuildingBlock)).Concat(valueTuples.Select(x => synchronizeInitialConditionCommand(x.quantity, x.quantityPath, moduleConfiguration.SelectedInitialConditions).AsHidden()));
      }

      /// <summary>
      ///    Creates two new commands that add new ParameterValuesBuildingBlock to a simulation module and to a template
      ///    module respectively. The simulation module is
      ///    identified by <paramref name="moduleConfiguration" /> and the template module is resolved from the project by name.
      ///    The new building blocks will contain values for changes
      ///    in the <paramref name="simulation" />. The new building block will be selected in
      ///    <paramref name="moduleConfiguration" />
      /// </summary>
      private IEnumerable<IMoBiCommand> addNewParameterValuesFromSimulationChanges(IMoBiSimulation simulation, ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, Parameter quantity)> parameterChanges)
      {
         // Create two parameter values for each Parameter change, one for the project building block and one for the simulation building block
         var parameterValuesToAdd = parameterChanges.Select(x => (simulation: createParameterValue(x.quantity, x.quantityPath), project: createParameterValue(x.quantity, x.quantityPath))).ToList();
         return createAddBuildingBlockCommands<ParameterValuesBuildingBlock, ParameterValue>(simulation, moduleConfiguration, parameterValuesToAdd);
      }

      /// <summary>
      ///    Creates two new commands that add new InitialConditionsBuildingBlock to a simulation module and to a template
      ///    module respectively. The simulation module is
      ///    identified by <paramref name="moduleConfiguration" /> and the template module is resolved from the project by name.
      ///    The new building blocks will contain values for changes
      ///    in the <paramref name="simulation" />. The new building block will be selected in
      ///    <paramref name="moduleConfiguration" />
      /// </summary>
      private IEnumerable<IMoBiCommand> addNewInitialConditionsFromSimulationChanges(IMoBiSimulation simulation, ModuleConfiguration moduleConfiguration, IReadOnlyList<(ObjectPath quantityPath, MoleculeAmount quantity)> moleculeChanges)
      {
         // Create two initial conditions for each MoleculeAmount change, one for the project building block and one for the simulation building block
         var initialConditionsToAdd = moleculeChanges.Select(x => (simulation: createInitialCondition(x.quantity, x.quantityPath), project: createInitialCondition(x.quantity, x.quantityPath))).ToList();
         return createAddBuildingBlockCommands<InitialConditionsBuildingBlock, InitialCondition>(simulation, moduleConfiguration, initialConditionsToAdd);
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

      private IEnumerable<IMoBiCommand> createAddBuildingBlockCommands<TBuildingBlock, TPathAndValueEntity>(IMoBiSimulation simulation,
         ModuleConfiguration moduleConfiguration,
         IReadOnlyList<(TPathAndValueEntity simulationEntity, TPathAndValueEntity projectEntity)> entitiesToAdd)
         where TBuildingBlock : PathAndValueEntityBuildingBlock<TPathAndValueEntity>, IBuildingBlock where TPathAndValueEntity : PathAndValueEntity
      {
         if (!entitiesToAdd.Any())
            return new[] { new MoBiEmptyCommand() };

         var templateModule = _templateResolverTask.TemplateModuleFor(moduleConfiguration.Module);

         var templateBuildingBlock = createBuildingBlockAndAddEntities<TBuildingBlock, TPathAndValueEntity>(entitiesToAdd.Select(x => x.projectEntity)).WithName(simulation.Name);
         _nameCorrector.AutoCorrectName(templateModule.BuildingBlocks.OfType<TBuildingBlock>().AllNames(), templateBuildingBlock);

         var simulationBuildingBlock = createBuildingBlockAndAddEntities<TBuildingBlock, TPathAndValueEntity>(entitiesToAdd.Select(x => x.simulationEntity)).WithName(templateBuildingBlock.Name);

         return new[]
         {
            new AddBuildingBlockToModuleCommand<TBuildingBlock>(templateBuildingBlock, templateModule),
            new AddSelectedBuildingBlockToLastModuleConfigurationCommand<TBuildingBlock>(simulationBuildingBlock, simulation).AsHidden()
         };
      }

      private TBuildingBlock createBuildingBlockAndAddEntities<TBuildingBlock, TPathAndValueEntity>(IEnumerable<TPathAndValueEntity> pathAndValueEntities) where TBuildingBlock : PathAndValueEntityBuildingBlock<TPathAndValueEntity>, IBuildingBlock where TPathAndValueEntity : PathAndValueEntity
      {
         var templateBuildingBlock = _context.Create<TBuildingBlock>();
         templateBuildingBlock.AddRange(pathAndValueEntities);
         return templateBuildingBlock;
      }

      private InitialCondition createInitialCondition(MoleculeAmount moleculeAmount, ObjectPath moleculeAmountPath)
      {
         return _initialConditionsCreator.CreateInitialCondition(moleculeAmountPath, moleculeAmount);
      }

      private ParameterValue createParameterValue(Parameter parameter, ObjectPath parameterPath)
      {
         return _parameterValuesCreator.CreateParameterValue(parameterPath, parameter);
      }

      private IMoBiCommand synchronizeInitialConditionCommand(MoleculeAmount moleculeAmount, ObjectPath quantityPath, InitialConditionsBuildingBlock initialConditionsBuildingBlock)
      {
         var initialConditionToUpdate = initialConditionsBuildingBlock[quantityPath];

         if (initialConditionToUpdate != null)
            return new SynchronizeInitialConditionCommand(moleculeAmount, initialConditionToUpdate, initialConditionsBuildingBlock);

         return new AddInitialConditionFromQuantityInSimulationCommand(moleculeAmount, initialConditionsBuildingBlock);
      }

      private IMoBiCommand synchronizeParameterValueCommand(IParameter parameter, ObjectPath quantityPath, ParameterValuesBuildingBlock parameterValuesBuildingBlock)
      {
         var parameterToUpdate = parameterValuesBuildingBlock[quantityPath];

         if (parameterToUpdate != null)
            return new SynchronizeParameterValueCommand(parameter, parameterToUpdate, parameterValuesBuildingBlock);

         return new AddParameterValueFromQuantityInSimulationCommand(parameter, parameterValuesBuildingBlock);
      }
   }
}