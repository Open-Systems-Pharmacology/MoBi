using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using static MoBi.Assets.AppConstants.Commands;

namespace MoBi.Presentation.Tasks
{
   public interface ISimulationCommitTask
   {
      /// <summary>
      /// Commits <paramref name="simulationWithChanges"/> changes to the last module in the simulation configuration
      /// by creating or updating it's initial conditions and parameter values building blocks
      /// The last module has to be used because if you re-create the simulation from the same modules
      /// this is the only way you will get the same simulation
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

      public SimulationCommitTask(IMoBiContext context,
         ITemplateResolverTask templateResolverTask,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IInitialConditionsCreator initialConditionsCreator,
         IParameterValuesCreator parameterValuesCreator,
         INameCorrector nameCorrector,
         IObjectTypeResolver objectTypeResolver)
      {
         _context = context;
         _templateResolverTask = templateResolverTask;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _initialConditionsCreator = initialConditionsCreator;
         _parameterValuesCreator = parameterValuesCreator;
         _nameCorrector = nameCorrector;
         _objectTypeResolver = objectTypeResolver;
      }

      public IMoBiCommand CommitSimulationChanges(IMoBiSimulation simulationWithChanges)
      {
         var lastModuleConfiguration = simulationWithChanges.Configuration.ModuleConfigurations.Last();
         var templateModule = _templateResolverTask.TemplateModuleFor(lastModuleConfiguration.Module);
         
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = CommitCommand,
            Description = CommitCommandDescription(templateModule, simulationWithChanges),
            ObjectType = _objectTypeResolver.TypeFor<Module>()
         };
         macroCommand.Add(lastModuleConfiguration.SelectedInitialConditions == null ? 
            addNewInitialConditionsFromSimulationChanges(simulationWithChanges, templateModule) : 
            updateInitialConditionsFromSimulationChanges(simulationWithChanges, lastModuleConfiguration.SelectedInitialConditions));

         macroCommand.Add(lastModuleConfiguration.SelectedParameterValues == null ? 
            addNewParameterValuesFromSimulationChanges(simulationWithChanges, templateModule) : 
            updateParameterValuesFromSimulationChanges(simulationWithChanges, lastModuleConfiguration.SelectedParameterValues));

         return macroCommand.Run(_context);
      }

      private ICommand updateParameterValuesFromSimulationChanges(IMoBiSimulation simulation, ParameterValuesBuildingBlock selectedParameterValues)
      {
         throw new NotImplementedException();
      }

      private ICommand updateInitialConditionsFromSimulationChanges(IMoBiSimulation simulation, InitialConditionsBuildingBlock selectedInitialConditions)
      {
         throw new NotImplementedException();
      }

      private IMoBiCommand addNewParameterValuesFromSimulationChanges(IMoBiSimulation simulation, Module lastModule)
      {
         var parameterValuesToAdd = changesFrom<Parameter>(simulation).Select(x => createParameterValue(x.quantity, x.quantityPath)).ToList();
         return createAddBuildingBlockCommand<ParameterValuesBuildingBlock, ParameterValue>(simulation.Name, lastModule, parameterValuesToAdd);
      }

      private IMoBiCommand addNewInitialConditionsFromSimulationChanges(IMoBiSimulation simulation, Module lastModule)
      {
         var initialConditionsToAdd = changesFrom<MoleculeAmount>(simulation).Select(x => createInitialCondition(x.quantity, x.quantityPath)).ToList();
         return createAddBuildingBlockCommand<InitialConditionsBuildingBlock, InitialCondition>(simulation.Name, lastModule, initialConditionsToAdd);
      }

      /// <summary>
      ///    Returns path and quantity tuple for all changes in the <paramref name="simulation" /> of type
      ///    <typeparamref name="TQuantity" />
      /// </summary>
      private IEnumerable<(ObjectPath quantityPath, TQuantity quantity)> changesFrom<TQuantity>(IMoBiSimulation simulation) where TQuantity : Quantity
      {
         var quantities = _entitiesInSimulationRetriever.EntitiesFrom<TQuantity>(simulation);
         return simulation.OriginalQuantityValues.Where(x => quantities.Contains(x.Path)).Select(x => (objectPath: x.Path, quantity: quantities[x.Path]));
      }

      private IMoBiCommand createAddBuildingBlockCommand<TBuildingBlock, TPathAndValueEntity>(string buildingBlockName, Module moduleToAddTo, IReadOnlyList<TPathAndValueEntity> entitiesToAdd)
         where TBuildingBlock : PathAndValueEntityBuildingBlock<TPathAndValueEntity>, IBuildingBlock where TPathAndValueEntity : PathAndValueEntity
      {
         if (!entitiesToAdd.Any())
            return new MoBiEmptyCommand();

         var buildingBlock = _context.Create<TBuildingBlock>().WithName(buildingBlockName);
         buildingBlock.AddRange(entitiesToAdd);
         _nameCorrector.AutoCorrectName(moduleToAddTo.BuildingBlocks.OfType<TBuildingBlock>().AllNames(), buildingBlock);
         return new AddBuildingBlockToModuleCommand<TBuildingBlock>(buildingBlock, moduleToAddTo);
      }

      private InitialCondition createInitialCondition(MoleculeAmount moleculeAmount, ObjectPath moleculeAmountPath)
      {
         return _initialConditionsCreator.CreateInitialCondition(moleculeAmountPath, moleculeAmount);
      }

      private ParameterValue createParameterValue(Parameter parameter, ObjectPath parameterPath)
      {
         return _parameterValuesCreator.CreateParameterValue(parameterPath, parameter);
      }
   }
}