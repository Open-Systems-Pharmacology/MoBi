using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{

   public class When_inverting_a_building_block_change_command : StaticContextSpecification
   {
      private BuildingBlockChangeCommandBase<IBuildingBlock> _originalCommand;
      private IMoBiContext _context;
      private IReversibleCommand<IMoBiContext> _inverseCommand;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         _originalCommand = new TestBuildingBlockCommand {ShouldIncrementVersion = true};
      }

      protected override void Because()
      {
         _inverseCommand = _originalCommand.InverseCommand(_context);
      }

      [Observation]
      public void should_invert_Should_Increment_version_property()
      {
         _inverseCommand.DowncastTo<TestBuildingBlockCommand>().ShouldIncrementVersion.ShouldNotBeEqualTo(_originalCommand.ShouldIncrementVersion);
      }

      private class TestBuildingBlockCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
      {
         public TestBuildingBlockCommand() : base(A.Fake<IBuildingBlock>())
         {
         }

         protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
         {
            return new TestBuildingBlockCommand().AsInverseFor(this);
         }
      }
   }
}