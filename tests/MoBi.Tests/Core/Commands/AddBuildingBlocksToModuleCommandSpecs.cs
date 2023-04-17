using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_AddBuildingBlocksToModuleCommand : ContextSpecification<AddMultipleBuildingBlocksToModuleCommand>
   {
      protected Module _existingModule;
      protected List<IBuildingBlock> _listOfNewBuildingBlocks;
      protected IMoBiContext _context;
      protected MoBiProject _project;
      protected RegisterTask _registrationTask;
      protected WithIdRepository _withIdRepository;
      protected ReactionBuildingBlock _newReactionBuildingBlock;
      protected EventGroupBuildingBlock _newEventGroupBuildingBlock;

      protected override void Context()
      {
         _newReactionBuildingBlock = new ReactionBuildingBlock().WithId("reactionId");
         _newEventGroupBuildingBlock = new EventGroupBuildingBlock().WithId("eventGroupId");
         _existingModule = new Module().WithId("moduleId");

         _listOfNewBuildingBlocks = new List<IBuildingBlock>
         {
            _newReactionBuildingBlock,
            _newEventGroupBuildingBlock
         };
         _withIdRepository = new WithIdRepository();
         _registrationTask = new RegisterTask(_withIdRepository);
         sut = new AddMultipleBuildingBlocksToModuleCommand(_existingModule, _listOfNewBuildingBlocks);
         _project = new MoBiProject();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);

         _existingModule.SpatialStructure = new SpatialStructure().WithId("SpatialStructure");
         _existingModule.Molecules = new MoleculeBuildingBlock().WithId("Molecule");
      }
   }

   public class When_executing_the_add_building_blocks_to_module_command : concern_for_AddBuildingBlocksToModuleCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_building_blocks_must_be_added_to_the_module()
      {
         _existingModule.Reactions.ShouldBeEqualTo(_newReactionBuildingBlock);
         _existingModule.EventGroups.ShouldBeEqualTo(_newEventGroupBuildingBlock);
      }
   }
}