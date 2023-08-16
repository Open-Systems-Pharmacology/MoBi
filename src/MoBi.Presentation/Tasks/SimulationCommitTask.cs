using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants.Commands;

namespace MoBi.Presentation.Tasks
{
   public interface ISimulationCommitTask
   {
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

      public SimulationCommitTask(IMoBiContext context,
         ITemplateResolverTask templateResolverTask,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IInitialConditionsCreator initialConditionsCreator,
         IParameterValuesCreator parameterValuesCreator,
         INameCorrector nameCorrector)
      {
         _context = context;
         _templateResolverTask = templateResolverTask;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _initialConditionsCreator = initialConditionsCreator;
         _parameterValuesCreator = parameterValuesCreator;
         _nameCorrector = nameCorrector;
      }

      public IMoBiCommand CommitSimulationChanges(IMoBiSimulation simulationWithChanges)
      {
         var lastModuleConfiguration = simulationWithChanges.Configuration.ModuleConfigurations.Last();
         var templateModule = _templateResolverTask.TemplateModuleFor(lastModuleConfiguration.Module);


         var moBiMacroCommand = new MoBiMacroCommand
         {
            CommandType = CommitCommand,
            Description = CommitCommandDescription(templateModule, simulationWithChanges),
            ObjectType = new ObjectTypeResolver().TypeFor<Module>()
         };
         moBiMacroCommand.Add(lastModuleConfiguration.SelectedInitialConditions == null ? 
            addNewInitialConditionsFromSimulationChanges(simulationWithChanges, templateModule) : 
            updateInitialConditionsFromSimulationChanges(simulationWithChanges, lastModuleConfiguration.SelectedInitialConditions));

         moBiMacroCommand.Add(lastModuleConfiguration.SelectedParameterValues == null ? 
            addNewParameterValuesFromSimulationChanges(simulationWithChanges, templateModule) : 
            updateParameterValuesFromSimulationChanges(simulationWithChanges, lastModuleConfiguration.SelectedParameterValues));

         return moBiMacroCommand.Run(_context);
      }

      private ICommand updateParameterValuesFromSimulationChanges(IMoBiSimulation simulationWithChanges, ParameterValuesBuildingBlock selectedParameterValues)
      {
         return new MoBiEmptyCommand();
      }

      private ICommand updateInitialConditionsFromSimulationChanges(IMoBiSimulation simulationWithChanges, InitialConditionsBuildingBlock selectedInitialConditions)
      {
         return new MoBiEmptyCommand();
      }

      private IMoBiCommand addNewParameterValuesFromSimulationChanges(IMoBiSimulation simulationWithChanges, Module lastModule)
      {
         var parameterValuesToAdd = changesFrom<Parameter>(simulationWithChanges).Select(x => createParameterValue(x.quantity, x.objectPath)).ToList();
         return createAddBuildingBlockCommand<ParameterValuesBuildingBlock, ParameterValue>(simulationWithChanges.Name, lastModule.ParameterValuesCollection.AllNames(), lastModule, parameterValuesToAdd);
      }

      private IMoBiCommand addNewInitialConditionsFromSimulationChanges(IMoBiSimulation simulationWithChanges, Module lastModule)
      {
         var initialConditionsToAdd = changesFrom<MoleculeAmount>(simulationWithChanges).Select(x => createInitialCondition(x.quantity, x.objectPath)).ToList();
         return createAddBuildingBlockCommand<InitialConditionsBuildingBlock, InitialCondition>(simulationWithChanges.Name, lastModule.InitialConditionsCollection.AllNames(), lastModule, initialConditionsToAdd);
      }

      /// <summary>
      ///    Returns path and quantity tuple for all changes in the <paramref name="simulationWithChanges" /> of type
      ///    <typeparamref name="TQuantity" />
      /// </summary>
      private IEnumerable<(ObjectPath objectPath, TQuantity quantity)> changesFrom<TQuantity>(IMoBiSimulation simulationWithChanges) where TQuantity : Quantity
      {
         var quantities = _entitiesInSimulationRetriever.EntitiesFrom<TQuantity>(simulationWithChanges);
         return simulationWithChanges.OriginalQuantityValues.Where(x => quantities.Contains(x.Path)).Select(x => (objectPath: x.Path, quantity: quantities[x.Path]));
      }

      private IMoBiCommand createAddBuildingBlockCommand<TBuildingBlock, TPathAndValueEntity>(string buildingBlockName, IEnumerable<string> usedNames, Module moduleToAddTo, IReadOnlyList<TPathAndValueEntity> entitiesToAdd)
         where TBuildingBlock : PathAndValueEntityBuildingBlock<TPathAndValueEntity>, IBuildingBlock where TPathAndValueEntity : PathAndValueEntity
      {
         if (!entitiesToAdd.Any())
            return new MoBiEmptyCommand();

         var buildingBlock = _context.Create<TBuildingBlock>().WithName(buildingBlockName);
         entitiesToAdd.Each(buildingBlock.Add);
         _nameCorrector.AutoCorrectName(usedNames, buildingBlock);
         return new AddBuildingBlockToModuleCommand<TBuildingBlock>(buildingBlock, moduleToAddTo);
      }

      private InitialCondition createInitialCondition(MoleculeAmount moleculeAmount, ObjectPath path)
      {
         var newPath = path.Clone<ObjectPath>();
         // Remove the name from the path before creating the initial condition with separate arguments for name and path
         newPath.RemoveAt(newPath.Count - 1);
         return _initialConditionsCreator.CreateInitialCondition(newPath, path.Last(), moleculeAmount.Dimension, moleculeAmount.DisplayUnit, moleculeAmount.ValueOrigin);
      }

      private ParameterValue createParameterValue(Parameter parameter, ObjectPath objectPath)
      {
         return _parameterValuesCreator.CreateParameterValue(objectPath, parameter);
      }
   }
}