using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Assets;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForRootEventGroup : IInteractionTasksForBuilder<IEventGroupBuilder>
   {
      void CreateApplicationsEventGroup(IEventGroupBuildingBlock subject);
   }

   public class InteractionTasksForRootEventGroup : InteractionTasksForBuilder<IEventGroupBuilder, IEventGroupBuildingBlock>, IInteractionTasksForRootEventGroup
   {
      public InteractionTasksForRootEventGroup(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IEventGroupBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IEventGroupBuilder eventGroupBuilderToRemove, IEventGroupBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return new RemoveRootEventGroupBuilderCommand(parent, eventGroupBuilderToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(IEventGroupBuilder builder, IEventGroupBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(IEventGroupBuilder eventGroupBuilder, IEventGroupBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(eventGroupBuilder, buildingBlock as IEventGroupBuildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IEventGroupBuilder builder, IEventGroupBuildingBlock buildingBlock)
      {
         return new AddRootEventGroupBuilderCommand(buildingBlock, builder);
      }

      public void CreateApplicationsEventGroup(IEventGroupBuildingBlock eventGroupBuildingBlock)
      {
         if(eventGroupBuildingBlock.ExistsByName(Constants.APPLICATIONS))
            return;

         var context = _interactionTaskContext.Context;
         var applications = context.Create<IEventGroupBuilder>()
            .WithName(Constants.APPLICATIONS)
            .WithIcon(ApplicationIcons.Applications.IconName);

         applications.SourceCriteria = Create.Criteria(x => x.With(Constants.ROOT_CONTAINER_TAG));
         context.AddToHistory(GetAddCommand(applications, eventGroupBuildingBlock).Run(context));
      }

      public override IReadOnlyCollection<IEventGroupBuilder> LoadItems(string filename)
      {
         try
         {
            var sourceEventGroup = InteractionTask.LoadItems<IEventGroupBuildingBlock>(filename).First();
            return selectTopEventGroupBuilder(sourceEventGroup).ToList();
         }
         catch (NotMatchingSerializationFileException)
         {
            //No Event Group Building Block in File so we are unable to determine the Root EventGroups in it. so we try normal Load
            return base.LoadItems(filename);
         }
      }

      private IEnumerable<IEventGroupBuilder> selectTopEventGroupBuilder(IEventGroupBuildingBlock sourceEventGroupBuilders)
      {
         if (!sourceEventGroupBuilders.Any())
            throw new NotMatchingSerializationFileException($"Top {ObjectTypes.EventGroupBuildingBlock}");

         if (sourceEventGroupBuilders.Count() == 1)
            return sourceEventGroupBuilders;

         using (var modal = ApplicationController.Start<IModalPresenter>())
         {
            var presenter = ApplicationController.Start<ISelectManyPresenter<IEventGroupBuilder>>();
            presenter.InitializeWith(sourceEventGroupBuilders);
            modal.Encapsulate(presenter);
            return modal.Show() ? presenter.Selections : null;
         }
      }
   }
}