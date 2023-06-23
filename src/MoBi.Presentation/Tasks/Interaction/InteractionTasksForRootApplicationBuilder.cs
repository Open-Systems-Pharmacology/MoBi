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
   public class InteractionTasksForRootApplicationBuilder : InteractionTasksForBuilder<ApplicationBuilder, EventGroupBuildingBlock>
   {
      public InteractionTasksForRootApplicationBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<ApplicationBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(ApplicationBuilder applicationBuilderToRemove, EventGroupBuildingBlock parent, IBuildingBlock buildingBlock1)
      {
         return new RemoveRootApplicationBuilderCommand(parent, applicationBuilderToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(ApplicationBuilder builder, EventGroupBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(ApplicationBuilder applicationBuilder, EventGroupBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(applicationBuilder, parent);
      }

      public override IMoBiCommand GetAddCommand(ApplicationBuilder builder, EventGroupBuildingBlock buildingBlock)
      {
         return new AddRootApplicationBuilderCommand(buildingBlock, builder);
      }

      public override IReadOnlyCollection<ApplicationBuilder> LoadItems(string filename)
      {
         try
         {
            var sourceEventGroup = InteractionTask.LoadItems<EventGroupBuildingBlock>(filename).First();
            return selectTopEventGroupBuilder(sourceEventGroup.OfType<ApplicationBuilder>().ToList()).ToList();
         }
         catch (NotMatchingSerializationFileException)
         {
            //No Event Group Building Block in File so we are unable to determine the Root EventGroups in it. so we try normal Load
            return base.LoadItems(filename);
         }
      }

      private IEnumerable<ApplicationBuilder> selectTopEventGroupBuilder(IReadOnlyCollection<ApplicationBuilder> sourceEventGroupBuilders)
      {
         //TODO: Code is identical to the one in InteractionTasksForRootEventGroup and should be refactored
         if (!sourceEventGroupBuilders.Any())
            throw new NotMatchingSerializationFileException($"Top {ObjectTypes.EventGroupBuildingBlock}");

         if (sourceEventGroupBuilders.Count == 1)
            return sourceEventGroupBuilders;

         using (var modal = ApplicationController.Start<IModalPresenter>())
         {
            var presenter = ApplicationController.Start<ISelectManyPresenter<ApplicationBuilder>>();
            presenter.InitializeWith(sourceEventGroupBuilders);
            modal.Encapsulate(presenter);
            return modal.Show() ? presenter.Selections : null;
         }
      }
   }
}