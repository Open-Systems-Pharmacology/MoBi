using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForRootEventGroup : ContextSpecification<InteractionTasksForRootEventGroup>
   {
      protected IInteractionTaskContext _context;
      private IEditTaskFor<EventGroupBuilder> _editTask;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTaskFor<EventGroupBuilder>>();
         sut = new InteractionTasksForRootEventGroup(_context, _editTask);
      }
   }

   internal class When_Asking_for_an_add_command : concern_for_InteractionTasksForRootEventGroup
   {
      private EventGroupBuildingBlock _buildingBlock;
      private EventGroupBuilder _eventGroupBuilder;
      private IMoBiCommand _addCommand;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<EventGroupBuildingBlock>().WithId("BB");
         _eventGroupBuilder = A.Fake<EventGroupBuilder>().WithId("EGB");
      }

      protected override void Because()
      {
         _addCommand = sut.GetAddCommand(_eventGroupBuilder, _buildingBlock, _buildingBlock);
      }

      [Observation]
      public void should_return_a_right_configured_add_command()
      {
         _addCommand.ShouldBeAnInstanceOf<AddRootEventGroupBuilderCommand>();
      }
   }
}