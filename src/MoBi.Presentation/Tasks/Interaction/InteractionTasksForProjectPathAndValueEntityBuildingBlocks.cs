using System.Collections.Generic;
using System.Data;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
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
         IParameterFactory parameterFactory,
         IExportDataTableToExcelTask exportDataTableToExcelTask,
         IMapper<TBuildingBlock, List<DataTable>> dataTableMapper) : base(interactionTaskContext, editTask, moBiFormulaTask, parameterFactory, exportDataTableToExcelTask, dataTableMapper)
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

      public IMoBiCommand AddToProject(TBuildingBlock buildingBlockToAdd)
      {
         return AddTo(buildingBlockToAdd, Context.CurrentProject, null);
      }

      public IMoBiCommand AddToProject(IBuildingBlock buildingBlock)
      {
         return AddToProject(buildingBlock as TBuildingBlock);
      }

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(TBuildingBlock buildingBlockToClone)
      {
         return Context.CurrentProject.All<TBuildingBlock>();
      }
   }
}