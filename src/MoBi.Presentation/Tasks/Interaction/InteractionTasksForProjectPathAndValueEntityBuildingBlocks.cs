using System.Collections.Generic;
using System.Data;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Mappers;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForProjectPathAndValueEntityBuildingBlocks<TBuildingBlock, TParameter> : IInteractionTasksForPathAndValueEntity<TBuildingBlock, TParameter>
   {
   }

   public abstract class InteractionTasksForProjectPathAndValueEntityBuildingBlocks<TBuildingBlock, TParameter> :
      InteractionTasksForPathAndValueEntity<MoBiProject, TBuildingBlock, TParameter>,
      IInteractionTasksForProjectPathAndValueEntityBuildingBlocks<TBuildingBlock, TParameter>
      where TParameter : PathAndValueEntity
      where TBuildingBlock : PathAndValueEntityBuildingBlock<TParameter>, ILookupBuildingBlock<TParameter>
   {
      protected InteractionTasksForProjectPathAndValueEntityBuildingBlocks(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<TBuildingBlock> editTask,
         IMoBiFormulaTask moBiFormulaTask,
         IExportDataTableToExcelTask exportDataTableToExcelTask,
         ICloneManagerForBuildingBlock cloneManager,
         IPathAndValueEntityToDistributedParameterMapper pathAndValueEntityToDistributedParameterMapper,
         IMapper<TBuildingBlock, List<DataTable>> dataTableMapper) : 
         base(interactionTaskContext, 
            editTask, 
            moBiFormulaTask, 
            exportDataTableToExcelTask, 
            dataTableMapper,
            pathAndValueEntityToDistributedParameterMapper, 
            cloneManager)
      {
      }

      public IMoBiCommand Clone(TBuildingBlock buildingBlockToClone)
      {
         var name = GetNewNameForClone(buildingBlockToClone);

         if (string.IsNullOrEmpty(name))
            return new MoBiEmptyCommand();

         var clone = InteractionTask.Clone(buildingBlockToClone).WithName(name);

         return AddToProject(clone);
      }

      public IMoBiCommand AddToProject(TBuildingBlock buildingBlockToAdd) => AddTo(buildingBlockToAdd, Context.CurrentProject, null);

      public IMoBiCommand AddToProject(IBuildingBlock buildingBlock) => AddToProject(buildingBlock as TBuildingBlock);

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(TBuildingBlock buildingBlockToClone) => Context.CurrentProject.All<TBuildingBlock>();
   }
}