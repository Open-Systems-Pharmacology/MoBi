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
   public class InteractionTasksForRootApplicationBuilder : InteractionTasksForBuilder<IApplicationBuilder, IEventGroupBuildingBlock>
   {
      public InteractionTasksForRootApplicationBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IApplicationBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IApplicationBuilder applicationBuilderToRemove, IEventGroupBuildingBlock parent, IBuildingBlock buildingBlock1)
      {
         return new RemoveRootApplicationBuilderCommand(parent, applicationBuilderToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(IApplicationBuilder builder, IEventGroupBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(IApplicationBuilder applicationBuilder, IEventGroupBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(applicationBuilder, parent);
      }

      public override IMoBiCommand GetAddCommand(IApplicationBuilder builder, IEventGroupBuildingBlock buildingBlock)
      {
         return new AddRootApplicationBuilderCommand(buildingBlock, builder);
      }

      public override IReadOnlyCollection<IApplicationBuilder> LoadItems(string filename)
      {
         try
         {
            var sourceEventGroup = InteractionTask.LoadItems<IEventGroupBuildingBlock>(filename).First();
            return selectTopEventGroupBuilder(sourceEventGroup.OfType<IApplicationBuilder>().ToList()).ToList();
         }
         catch (NotMatchingSerializationFileException)
         {
            //No Event Group Building Block in File so we are unable to determine the Root EventGroups in it. so we try normal Load
            return base.LoadItems(filename);
         }
      }

      private IEnumerable<IApplicationBuilder> selectTopEventGroupBuilder(IReadOnlyCollection<IApplicationBuilder> sourceEventGroupBuilders)
      {
         //TODO: Code is identical to the one in InteractionTasksForRootEventGroup and should be refactored
         if (!sourceEventGroupBuilders.Any())
            throw new NotMatchingSerializationFileException(string.Format("Top {0}",ObjectTypes.EventGroupBuildingBlock));

         if (sourceEventGroupBuilders.Count == 1)
            return sourceEventGroupBuilders;

         using (var modal = ApplicationController.Start<IModalPresenter>())
         {
            var presenter = ApplicationController.Start<ISelectManyPresenter<IApplicationBuilder>>();
            presenter.InitializeWith(sourceEventGroupBuilders);
            modal.Encapsulate(presenter);
            return modal.Show() ? presenter.Selections : null;
         }
      }
   }
}