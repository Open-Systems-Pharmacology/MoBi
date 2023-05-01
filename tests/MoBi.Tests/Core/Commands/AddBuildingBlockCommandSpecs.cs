using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddBuildingBlockCommand : ContextSpecification<AddProjectBuildingBlockCommand<IBuildingBlock>>
   {
      protected IBuildingBlock _bb;

      protected override void Context()
      {
         _bb = A.Fake<IBuildingBlock>().WithId("ID");
         sut = new AddProjectBuildingBlockCommand<IBuildingBlock>(_bb);
      }
   }

   internal class When_executing_the_adding_command : concern_for_AddBuildingBlockCommand
   {
      private IMoBiContext _context;
      private MoBiProject _project;
      private AddedEvent _event;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.PublishEvent(A<AddedEvent<IBuildingBlock>>._))
            .Invokes(x => _event = x.GetArgument<AddedEvent>(0));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_add_building_block_to_project()
      {
         _project.AllBuildingBlocks().ShouldContain(_bb);
      }

      [Observation]
      public void should_publish_added_event()
      {
         _event.AddedObject.ShouldBeEqualTo(_bb);
         _event.Parent.ShouldBeEqualTo(_project);
      }
   }

   public class When_RestoreExecutionData_is_called_for_AddCommand : concern_for_AddBuildingBlockCommand
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
      }

      protected override void Because()
      {
         sut.RestoreExecutionData(_context);
      }

      [Observation]
      public void should_get_building_block_from_context()
      {
         A.CallTo(() => _context.Get<IBuildingBlock>(_bb.Id)).MustHaveHappened();
      }
   }

   public abstract class concern_for_RemoveBuildingBlockCommand : ContextSpecification<RemoveProjectBuildingBlockCommand<IBuildingBlock>>
   {
      protected IBuildingBlock _bb;

      protected override void Context()
      {
         _bb = A.Fake<IBuildingBlock>();
         sut = new RemoveProjectBuildingBlockCommand<IBuildingBlock>(_bb);
      }
   }

   internal class When_executing_the_remove_BuildingBlockCommand_command : concern_for_RemoveBuildingBlockCommand
   {
      private IMoBiContext _context;
      private MoBiProject _project;
      private RemovedEvent _event;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.PublishEvent(A<RemovedEvent>._))
            .Invokes(x => _event = x.GetArgument<RemovedEvent>(0));
         
         _project.AddBuildingBlock(_bb);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_building_block_from_project()
      {
         _project.AllBuildingBlocks().ShouldNotContain(_bb);
      }

      [Observation]
      public void should_publish_removed_event()
      {
         _event.RemovedObjects.ShouldOnlyContain(_bb);
      }
   }
}