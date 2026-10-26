using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetXenobioticPropertyCommand : ContextSpecification<SetXenobioticPropertyCommand>
   {
      protected MoleculeBuilder _moleculeBuilder;
      protected MoleculeBuildingBlock _buildingBlock;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _moleculeBuilder = new MoleculeBuilder().WithName("Enzyme").WithId("moleculeId");
         _moleculeBuilder.IsXenobiotic = true;
         _buildingBlock = new MoleculeBuildingBlock().WithId("buildingBlockId");
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<MoleculeBuilder>(_moleculeBuilder.Id)).Returns(_moleculeBuilder);
         A.CallTo(() => _context.Get<IBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         sut = new SetXenobioticPropertyCommand(_moleculeBuilder, newValue: false, oldValue: true, _buildingBlock);
      }
   }

   public class When_executing_the_set_xenobiotic_property_command : concern_for_SetXenobioticPropertyCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_set_the_xenobiotic_flag_of_the_molecule()
      {
         _moleculeBuilder.IsXenobiotic.ShouldBeFalse();
      }
   }

   public class When_reverting_the_set_xenobiotic_property_command : concern_for_SetXenobioticPropertyCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_restore_the_original_xenobiotic_flag_of_the_molecule()
      {
         _moleculeBuilder.IsXenobiotic.ShouldBeTrue();
      }
   }
}
