using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddBuildingBlockToModuleCommand : ContextSpecification<AddBuildingBlockToModuleCommand<IBuildingBlock>>
   {
      protected EventGroupBuildingBlock _bb;
      protected Module _existingModule;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _existingModule = new Module().WithId("existingModuleId");
         _context = A.Fake<IMoBiContext>();
         _bb = new EventGroupBuildingBlock().WithId("newEventGroupBuildingBlockId");
         sut = new AddBuildingBlockToModuleCommand<IBuildingBlock>(_bb, _existingModule);
      }
   }

   internal class When_adding_a_event_group_to_module_command : concern_for_AddBuildingBlockToModuleCommand
   {
      private AddedEvent _event;

      protected override void Context()
      {
         base.Context();
         
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
         _existingModule.BuildingBlocks.ShouldContain(_bb);
      }

      [Observation]
      public void should_publish_added_event()
      {
         _event.AddedObject.ShouldBeEqualTo(_bb);
         _event.Parent.ShouldBeEqualTo(_existingModule);
      }
   }

   public class When_reverting_a_AddBuildingBlockToModuleCommand : concern_for_AddBuildingBlockToModuleCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<Module>(_existingModule.Id)).Returns(_existingModule);
         A.CallTo(() => _context.Get<IBuildingBlock>(_bb.Id)).Returns(_bb);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_building_block_should_be_removed()
      {
         _existingModule.BuildingBlocks.ShouldNotContain(_bb);
      }
   }
}