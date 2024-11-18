using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveBuildingBlockFromModuleCommand : ContextSpecification<RemoveBuildingBlockFromModuleCommand<IBuildingBlock>>
   {
      protected IBuildingBlock _bb;
      protected Module _existingModule;
      protected IMoBiContext _context;
      
      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _existingModule = new Module
         {
            IsPKSimModule = true
         };
         _bb = new SpatialStructure().WithId("newSpatialStructureBuildingBlockId");
         sut = new RemoveBuildingBlockFromModuleCommand<IBuildingBlock>(_bb, _existingModule);
      }
   }
   
   public class When_reverting_remove_building_block_from_module_command : concern_for_RemoveBuildingBlockFromModuleCommand
   {
      private readonly byte[] _bbToken = { 1, 2, 3 };
      private IBuildingBlock _deserializedBB;

      protected override void Context()
      {
         base.Context();
         _deserializedBB = new SpatialStructure().WithId("a");
         A.CallTo(() => _context.Get<Module>(_existingModule.Id)).Returns(_existingModule);
         A.CallTo(() => _context.Serialize(_bb)).Returns(_bbToken);
         A.CallTo(() => _context.Deserialize<IBuildingBlock>(_bbToken)).Returns(_deserializedBB);
         _existingModule.Add(_bb);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_building_block_should_be_added()
      {
         _existingModule.BuildingBlocks.ShouldContain(_deserializedBB);
      }

      [Observation]
      public void the_module_should_be_pksim_module()
      {
         _existingModule.IsPKSimModule.ShouldBeTrue();
      }
   }

   internal class When_removing_spatial_structure_from_module : concern_for_RemoveBuildingBlockFromModuleCommand
   {
      private RemovedEvent _event;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.PublishEvent(A<RemovedEvent>._))
            .Invokes(x => _event = x.GetArgument<RemovedEvent>(0));

         _existingModule.Add(_bb);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_should_not_be_pksim_module()
      {
         _existingModule.IsPKSimModule.ShouldBeFalse();
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