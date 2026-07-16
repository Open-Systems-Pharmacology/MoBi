using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForIndividualBuildingBlock : IInteractionTasksForProjectBuildingBlock<IndividualBuildingBlock>,
      IInteractionTasksForProjectPathAndValueEntityBuildingBlocks<IndividualBuildingBlock, IndividualParameter>,
      IInteractionTasksForProjectBuildingBlock
   {
      IMoBiCommand ResetToInitialState(IndividualParameter individualParameter, IndividualBuildingBlock buildingBlock);
   }

   public class InteractionTasksForIndividualBuildingBlock : InteractionTasksForProjectPathAndValueEntityBuildingBlocks<IndividualBuildingBlock, IndividualParameter>, IInteractionTasksForIndividualBuildingBlock
   {
      private readonly IPKSimStarter _pkSimStarter;

      //The parameter for the mapper is set to null since we still haven`t implemented it for IndividualBuildingBlock
      public InteractionTasksForIndividualBuildingBlock(IInteractionTaskContext interactionTaskContext,
         IEditTasksForIndividualBuildingBlock editTask,
         IMoBiFormulaTask moBiFormulaTask,
         IExportDataTableToExcelTask exportDataTableToExcelTask,
         ICloneManagerForBuildingBlock cloneManager,
         IPathAndValueEntityToDistributedParameterMapper pathAndValueEntityToDistributedParameterMapper,
         IIndividualParametersToIndividualParametersDataTableMapper dataTableMapper,
         IPKSimStarter pkSimStarter) :
         base(interactionTaskContext,
            editTask,
            moBiFormulaTask,
            exportDataTableToExcelTask,
            cloneManager,
            pathAndValueEntityToDistributedParameterMapper,
            dataTableMapper)
      {
         _pkSimStarter = pkSimStarter;
      }

      public override IMoBiCommand GetRemoveCommand(IndividualBuildingBlock individualBuildingBlockToRemove, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         return new RemoveIndividualBuildingBlockFromProjectCommand(individualBuildingBlockToRemove);
      }

      public IndividualBuildingBlock LoadFromSnapshot(string snapshot) =>
         // Cloning required to reset object ids
         _cloneManagerForBuildingBlock.Clone(_pkSimStarter.LoadIndividualFromSnapshot(snapshot));

      public IMoBiCommand ResetToInitialState(IndividualParameter individualParameter, IndividualBuildingBlock buildingBlock) => 
         new ResetInitialStateCommand<IndividualParameter, IndividualBuildingBlock>(individualParameter, buildingBlock).RunCommand(Context);

      public override IMoBiCommand GetAddCommand(IndividualBuildingBlock individualBuildingBlockToAdd, MoBiProject parent, IBuildingBlock buildingBlock) => 
         new AddIndividualBuildingBlockToProjectCommand(individualBuildingBlockToAdd);

      protected override string SnapshotFrom(IndividualBuildingBlock buildingBlock) => buildingBlock.Snapshot.FromBase64String();
   }
}