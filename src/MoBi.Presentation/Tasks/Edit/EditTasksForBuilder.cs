using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForBuilder
   {
      IEnumerable<string> GetForbiddenNames(IBuildingBlock buildingBlock);
   }

   public abstract class EditTasksForBuilder<TBuilder, TBuildingBlock> : EditTaskFor<TBuilder>, IEditTasksForBuilder where TBuilder : class, IBuilder where TBuildingBlock : class
   {
      protected EditTasksForBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected virtual IEnumerable<string> GetUnallowedNames(TBuildingBlock buildingBlock)
      {
         var enumerableBuildingBlock = buildingBlock as IBuildingBlock<TBuilder>;
         if (enumerableBuildingBlock == null)
            return Enumerable.Empty<string>();

         return enumerableBuildingBlock.AllNames();
      }

      public virtual IEnumerable<string> GetForbiddenNames(IBuildingBlock buildingBlock)
      {
         return GetUnallowedNames(buildingBlock.DowncastTo<TBuildingBlock>()).Union(_interactionTask.ForbiddenNamesFor(buildingBlock));
      }

      protected override IEnumerable<string> GetUnallowedNames(TBuilder objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return GetUnallowedNames(getBuildingBlockFrom(existingObjectsInParent));
      }

      private TBuildingBlock getBuildingBlockFrom(IEnumerable<IObjectBase> parent)
      {
         return parent as TBuildingBlock ?? _interactionTaskContext.Active<TBuildingBlock>();
      }
   }
}