using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_AddNewBuildingBlockCommand : ContextSpecification<AddNewBuildingBlockCommand<IBuildingBlock>>
   {
      protected IMoBiContext _context;
      protected IInteractionTasksForBuildingBlock<IBuildingBlock> _interactionTask;

      protected override void Context()
      {
         _context= A.Fake<IMoBiContext>();
         _interactionTask= A.Fake<IInteractionTasksForBuildingBlock<IBuildingBlock>>();
         sut = new AddNewBuildingBlockCommand<IBuildingBlock>(_interactionTask,_context);
      }
   }

   public class When_adding_a_new_building_block_to_the_project : concern_for_AddNewBuildingBlockCommand
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _command= A.Fake<IMoBiCommand>();
         A.CallTo(() => _interactionTask.AddNew()).Returns(_command);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_leverage_the_interaction_task_to_add_a_new_building_block()
      {
         _command.ShouldNotBeNull();
      }

      [Observation]
      public void should_add_the_resulting_command_to_the_history()
      {
         A.CallTo(() => _context.AddToHistory(_command)).MustHaveHappened();
      }
   }
}	