using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using System;

namespace MoBi.Core.Commands
{
   public class concern_for_MoveBuildingBlockToModuleCommand : ContextSpecification<MoveBuildingBlockToModuleCommand>
   {
      protected Module _targetModule;
      protected Module _sourceModule;
      protected IMoBiContext _context;
      protected MoBiProject _project;
      protected RegisterTask _registrationTask;
      protected WithIdRepository _withIdRepository;
      protected ReactionBuildingBlock _newReactionBuildingBlock;

      protected override void Context()
      {
         _newReactionBuildingBlock = new ReactionBuildingBlock().WithId("reactionId");
         _sourceModule = new Module().WithId("sourceModuleId");
         _sourceModule.Add(_newReactionBuildingBlock);
         _targetModule = new Module().WithId("targetModuleId");
         
         _withIdRepository = new WithIdRepository();
         _registrationTask = new RegisterTask(_withIdRepository);
         sut = new MoveBuildingBlockToModuleCommand(_newReactionBuildingBlock, _targetModule);
         _project = new MoBiProject();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);

         _targetModule.Add(new SpatialStructure().WithId("SpatialStructure"));
         _targetModule.Add(new MoleculeBuildingBlock().WithId("Molecule"));
      }
   }

   public class When_executing_the_move_blocks_to_module_command : concern_for_MoveBuildingBlockToModuleCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_building_blocks_must_be_moved_to_the_module()
      {
         _targetModule.Reactions.ShouldBeEqualTo(_newReactionBuildingBlock);
         _sourceModule.Reactions.ShouldBeNull();
      }
   }

   public class When_executing_the_move_blocks_to_module_command_with_no_module : concern_for_MoveBuildingBlockToModuleCommand
   {
      protected override void Context()
      {
         base.Context();
         _newReactionBuildingBlock.Module = null;
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Execute(_context)).ShouldThrowAn<NullReferenceException>();
      }
   }

   public class When_reversing_the_move_blocks_to_module_command : concern_for_MoveBuildingBlockToModuleCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<Module>(_sourceModule.Id)).Returns(_sourceModule);
         A.CallTo(() => _context.Get<Module>(_targetModule.Id)).Returns(_targetModule);
         A.CallTo(() => _context.Get<IBuildingBlock>(_newReactionBuildingBlock.Id)).Returns(_newReactionBuildingBlock);
         A.CallTo(() => _context.Deserialize<IBuildingBlock>(A<byte[]>._)).Returns(_newReactionBuildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_building_blocks_must_be_moved_to_the_module()
      {
         _targetModule.Reactions.ShouldBeNull();
         _sourceModule.Reactions.ShouldBeEqualTo(_newReactionBuildingBlock);
      }
   }
}