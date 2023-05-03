using FakeItEasy;
using MoBi.Core.Domain.Extensions;
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
      protected IBuildingBlock _bb;
      protected Module _existingModule;

      protected override void Context()
      {
         _existingModule = new Module();
      }
   }

   internal class When_adding_a_event_group_to_module_command : concern_for_AddBuildingBlockToModuleCommand
   {
      private IMoBiContext _context;
      private AddedEvent _event;

      protected override void Context()
      {
         base.Context();
         _bb = new EventGroupBuildingBlock().WithId("newEventGroupBuildingBlockId");
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.PublishEvent(A<AddedEvent<IBuildingBlock>>._))
            .Invokes(x => _event = x.GetArgument<AddedEvent>(0));
         sut = new AddBuildingBlockToModuleCommand<IBuildingBlock>(_bb, _existingModule);
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

   public class When_RestoreExecutionData_is_called_for_AddBuildingBlockToModuleCommand : concern_for_AddProjectBuildingBlockCommand
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

   public abstract class concern_for_RemoveBuildingBlockFromModuleCommand : ContextSpecification<RemoveBuildingBlockFromModuleCommand<IBuildingBlock>>
   {
      protected IBuildingBlock _bb;
      protected Module _existingModule;

      protected override void Context()
      {
         _existingModule = new Module();
         _bb = new SpatialStructure().WithId("newSpatialStructureBuildingBlockId");
         sut = new RemoveBuildingBlockFromModuleCommand<IBuildingBlock>(_bb, _existingModule);
      }
   }

   internal class When_removing_spatial_structure_from_module : concern_for_RemoveBuildingBlockFromModuleCommand
   {
      private IMoBiContext _context;
      private RemovedEvent _event;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.PublishEvent(A<RemovedEvent>._))
            .Invokes(x => _event = x.GetArgument<RemovedEvent>(0));

         _existingModule.Add(_bb);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_building_block_from_project()
      {
         _existingModule.BuildingBlocks.ShouldNotContain(_bb);
      }

      [Observation]
      public void should_publish_removed_event()
      {
         _event.RemovedObjects.ShouldOnlyContain(_bb);
      }
   }
}