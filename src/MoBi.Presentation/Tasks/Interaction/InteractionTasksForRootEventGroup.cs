using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForRootEventGroup : IInteractionTasksForBuilder<EventGroupBuilder>
   {
   }

   public class InteractionTasksForRootEventGroup : InteractionTasksForBuilder<EventGroupBuilder, EventGroupBuildingBlock>, IInteractionTasksForRootEventGroup
   {
      public InteractionTasksForRootEventGroup(IInteractionTaskContext interactionTaskContext, IEditTaskFor<EventGroupBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(EventGroupBuilder eventGroupBuilderToRemove, EventGroupBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return new RemoveRootEventGroupBuilderCommand(parent, eventGroupBuilderToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(EventGroupBuilder builder, EventGroupBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(EventGroupBuilder eventGroupBuilder, EventGroupBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(eventGroupBuilder, buildingBlock as EventGroupBuildingBlock);
      }

      public override IMoBiCommand GetAddCommand(EventGroupBuilder builder, EventGroupBuildingBlock buildingBlock)
      {
         return new AddRootEventGroupBuilderCommand(buildingBlock, builder);
      }

      public override IReadOnlyCollection<EventGroupBuilder> LoadItems(string filename)
      {
         try
         {
            var sourceEventGroup = InteractionTask.LoadItems<EventGroupBuildingBlock>(filename).First();
            return selectTopEventGroupBuilder(sourceEventGroup).ToList();
         }
         catch (NotMatchingSerializationFileException)
         {
            //No Event Group Building Block in File so we are unable to determine the Root EventGroups in it. so we try normal Load
            return base.LoadItems(filename);
         }
      }

      private IEnumerable<EventGroupBuilder> selectTopEventGroupBuilder(EventGroupBuildingBlock sourceEventGroupBuilders)
      {
         if (!sourceEventGroupBuilders.Any())
            throw new NotMatchingSerializationFileException($"Top {ObjectTypes.EventGroupBuildingBlock}");

         if (sourceEventGroupBuilders.Count() == 1)
            return sourceEventGroupBuilders;

         using (var modal = ApplicationController.Start<IModalPresenter>())
         {
            var presenter = ApplicationController.Start<ISelectManyPresenter<EventGroupBuilder>>();
            presenter.InitializeWith(sourceEventGroupBuilders);
            modal.Encapsulate(presenter);
            return modal.Show() ? presenter.Selections : null;
         }
      }
   }
}