using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_AddBuildingBlocksToModuleCommand : ContextSpecification<AddBuildingBlocksToModuleCommand>
   {
      protected Module _existingModule;
      protected Module _moduleWithNewBuildingBlocks;
      protected IMoBiContext _context;
      protected IMoBiProject _project;
      protected RegisterTask _registrationTask;
      protected WithIdRepository _withIdRepository;

      protected override void Context()
      {
         _existingModule = new Module().WithId("moduleId");
         _moduleWithNewBuildingBlocks = new Module().WithId("moduleWithNewBuildingBlocksId");
         _withIdRepository = new WithIdRepository();
         _registrationTask = new RegisterTask(_withIdRepository);
         sut = new AddBuildingBlocksToModuleCommand(_existingModule, _moduleWithNewBuildingBlocks);
         _project = new MoBiProject();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);

         _existingModule.SpatialStructure = new SpatialStructure().WithId("SpatialStructure");
         _existingModule.Molecule = new MoleculeBuildingBlock().WithId("Molecule");

         _moduleWithNewBuildingBlocks.Reaction = new ReactionBuildingBlock().WithId("reactionId");
         _moduleWithNewBuildingBlocks.EventGroup = new EventGroupBuildingBlock().WithId("eventGroupId");
      }
   }

   public class When_executing_the_add_building_blocks_to_module_command : concern_for_AddBuildingBlocksToModuleCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_building_blocks_must_be_registered_in_the_context()
      {
         A.CallTo(() => _context.Register(A<ReactionBuildingBlock>.That.Matches(buildingBlock => buildingBlock.Id == "reactionId")))
            .MustHaveHappened();
         A.CallTo(() => _context.Register(A<EventGroupBuildingBlock>.That.Matches(buildingBlock => buildingBlock.Id == "eventGroupId")))
            .MustHaveHappened();
      }

      [Observation]
      public void the_building_blocks_must_be_added_to_the_module()
      {
         _existingModule.Reaction.ShouldBeEqualTo(_moduleWithNewBuildingBlocks.Reaction);
         _existingModule.EventGroup.ShouldBeEqualTo(_moduleWithNewBuildingBlocks.EventGroup);
      }
   }

   public class When_reversing_the_add_building_blocks_to_module_command : concern_for_AddBuildingBlocksToModuleCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<Module>(_existingModule.Id)).Returns(_existingModule);
         A.CallTo(() => _context.Get<Module>(_moduleWithNewBuildingBlocks.Id)).Returns(_moduleWithNewBuildingBlocks);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void only_the_building_blocks_are_removed_from_the_module()
      {
         _existingModule.Reaction.ShouldBeNull();
         _existingModule.EventGroup.ShouldBeNull();

         _existingModule.SpatialStructure.ShouldNotBeNull();
         _existingModule.Molecule.ShouldNotBeNull();
      }
   }
}