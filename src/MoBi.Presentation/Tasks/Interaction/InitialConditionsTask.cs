using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IBuildingBlockWithInitialConditionsTask<TBuildingBlock> : IStartValuesTask<TBuildingBlock, InitialCondition> where TBuildingBlock : class, IBuildingBlock<InitialCondition>
   {
      IMoBiCommand SetIsPresent(TBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool isPresent);

      IMoBiCommand SetNegativeValuesAllowed(TBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool negativeValuesAllowed);

      /// <summary>
      ///    Updates the scale divisor for a initial condition
      /// </summary>
      /// <param name="buildingBlock">The building block that this initial condition is part of</param>
      /// <param name="initialCondition">The initial condition being updated</param>
      /// <param name="newScaleDivisor">The new value of the scale divisor</param>
      /// <param name="oldScaleDivisor">The old value of the scale divisor</param>
      /// <returns>The command used to modify the initial condition</returns>
      IMoBiCommand UpdateInitialConditionScaleDivisor(TBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor);
   }

   public interface IInitialConditionsTask : IBuildingBlockWithInitialConditionsTask<InitialConditionsBuildingBlock>
   {
   }

   public class InitialConditionsTask : BaseInitialConditionsTask<InitialConditionsBuildingBlock>, IInitialConditionsTask
   {
      public InitialConditionsTask(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<InitialConditionsBuildingBlock> editTask,
         IInitialConditionsCreator initialConditionsCreator,
         IImportedQuantityToInitialConditionMapper dtoMapper,
         IInitialConditionsBuildingBlockExtendManager extendManager,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IReactionDimensionRetriever dimensionRetriever,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory, IInitialConditionPathTask initialConditionPathTask, IMoleculeResolver moleculeResolver)
         : base(interactionTaskContext, editTask, extendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoMapper, initialConditionPathTask, moleculeResolver, dimensionRetriever, initialConditionsCreator)
      {
      }

      public override InitialConditionsBuildingBlock CreatePathAndValueEntitiesForSimulation(SimulationConfiguration simulationConfiguration)
      {
         //TODO OSMOSES combining multiple spatial structures and molecule building blocks is not supported yet
         var simulationInitialConditions = _initialConditionsCreator.CreateFrom(simulationConfiguration.All<SpatialStructure>().First(), simulationConfiguration.All<MoleculeBuildingBlock>().First().ToList())
            .WithName(simulationConfiguration.All<InitialConditionsBuildingBlock>().First().Name);

         var templateValues = UpdateValuesFromTemplate(simulationInitialConditions, simulationConfiguration.All<InitialConditionsBuildingBlock>().First());
         updateDefaultIsPresentToFalseForSpecificExtendedValues(simulationInitialConditions, templateValues);
         return simulationInitialConditions;
      }

      private void updateDefaultIsPresentToFalseForSpecificExtendedValues(InitialConditionsBuildingBlock startValues, ICache<string, InitialCondition> templateValues)
      {
         var startValuesThatShouldPotentiallyNotBePresent = startValues.ToCache().KeyValues.Where(x => AppConstants.Organs.DefaultIsPresentShouldBeFalse.Any(organ => x.Key.Contains(organ)));
         var extendedStartValuesThatShouldNotBePresent = startValuesThatShouldPotentiallyNotBePresent.Where(x => !templateValues.Contains(x.Key));
         extendedStartValuesThatShouldNotBePresent.Each(x => x.Value.IsPresent = false);
      }
   }
}