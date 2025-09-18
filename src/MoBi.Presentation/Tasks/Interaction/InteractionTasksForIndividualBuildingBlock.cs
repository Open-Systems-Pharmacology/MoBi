using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
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

      public IndividualBuildingBlock LoadFromSnapshot(string snapshot)
      {
         // Cloning required to reset object ids
         return _cloneManagerForBuildingBlock.Clone(_pkSimStarter.LoadIndividualFromSnapshot(snapshot));
      }

      public override IMoBiCommand GetAddCommand(IndividualBuildingBlock individualBuildingBlockToAdd, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         return new AddIndividualBuildingBlockToProjectCommand(individualBuildingBlockToAdd);
      }

      protected override string SnapshotFrom(IndividualBuildingBlock buildingBlock)
      {
         return buildingBlock.Snapshot.FromBase64String();
      }
   }
}