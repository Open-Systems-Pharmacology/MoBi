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
      private IEditTaskFor<IEventGroupBuilder> _editTask;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTaskFor<IEventGroupBuilder>>();
         sut = new InteractionTasksForRootEventGroup(_context, _editTask);
      }
   }

   internal class When_Asking_for_an_add_command : concern_for_InteractionTasksForRootEventGroup
   {
      private IEventGroupBuildingBlock _buildingBlock;
      private IEventGroupBuilder _eventGroupBuilder;
      private IMoBiCommand _addCommand;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IEventGroupBuildingBlock>().WithId("BB");
         _eventGroupBuilder = A.Fake<IEventGroupBuilder>().WithId("EGB");
      }

      protected override void Because()
      {
         _addCommand = sut.GetAddCommand(_eventGroupBuilder, _buildingBlock, _buildingBlock);
      }

      [Observation]
      public void should_return_a_right_configuered_add_command()
      {
         _addCommand.ShouldBeAnInstanceOf<AddRootEventGroupBuilderCommand>();
      }
   }

   public class When_creating_the_application_event_group : concern_for_InteractionTasksForRootEventGroup
   {
      private IEventGroupBuildingBlock _eventGroupBuilder;

      protected override void Context()
      {
         base.Context();
         _eventGroupBuilder = new EventGroupBuildingBlock();
         A.CallTo(() => _context.Context.Create<IEventGroupBuilder>()).Returns(new EventGroupBuilder());
      }
      protected override void Because()
      {
         sut.CreateApplicationsEventGroup(_eventGroupBuilder);
      }

      [Observation]
      public void should_create_an_event_group_builder_with_the_expected_properties()
      {
         var applications = _eventGroupBuilder.FindByName(Constants.APPLICATIONS);
         applications.ShouldNotBeNull();
         var tags = new Tags(new[] { new Tag(Constants.ROOT_CONTAINER_TAG)});
         applications.SourceCriteria.IsSatisfiedBy(new EntityDescriptor{Tags = tags}).ShouldBeTrue();
      }
   }
}