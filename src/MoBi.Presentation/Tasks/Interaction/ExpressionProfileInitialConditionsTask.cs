using System;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IExpressionProfileInitialConditionsTask : IBuildingBlockWithInitialConditionsTask<ExpressionProfileBuildingBlock>
   {
   }
   
   public class ExpressionProfileInitialConditionsTask : BaseInitialConditionsTask<ExpressionProfileBuildingBlock>, IExpressionProfileInitialConditionsTask
   {
      public ExpressionProfileInitialConditionsTask(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock> editTask,
         IInitialConditionsBuildingBlockExtendManager extendManager,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IImportedQuantityToInitialConditionMapper dtoMapper,
         IInitialConditionPathTask initialConditionPathTask,
         IMoleculeResolver moleculeResolver,
         IReactionDimensionRetriever dimensionRetriever,
         IInitialConditionsCreator initialConditionsCreator) : base(interactionTaskContext, editTask, extendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoMapper, initialConditionPathTask, moleculeResolver, dimensionRetriever, initialConditionsCreator)
      {
      }

      public override ExpressionProfileBuildingBlock CreatePathAndValueEntitiesForSimulation(SimulationConfiguration simulationConfiguration)
      {
         throw new NotImplementedException();
      }
   }
}