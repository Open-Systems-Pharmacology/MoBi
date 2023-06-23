using System;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddNewApplicationsEventGroup:ObjectUICommand<EventGroupBuildingBlock>
   {
      private readonly IInteractionTasksForRootEventGroup _interactionTasksForRootEventGroup;

      public AddNewApplicationsEventGroup(IInteractionTasksForRootEventGroup interactionTasksForRootEventGroup)
      {
         _interactionTasksForRootEventGroup = interactionTasksForRootEventGroup;
      }

      protected override void PerformExecute()
      {
         _interactionTasksForRootEventGroup.CreateApplicationsEventGroup(Subject);
      }
   }
}