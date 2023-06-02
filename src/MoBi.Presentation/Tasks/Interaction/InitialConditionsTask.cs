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

   }
}