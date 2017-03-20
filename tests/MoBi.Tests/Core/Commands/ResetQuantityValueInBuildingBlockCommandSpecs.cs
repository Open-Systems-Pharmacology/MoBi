using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ResetQuantityValueInBuildingBlockCommand : ContextSpecification<ResetQuantityValueInBuildingBlockCommand>
   {
      protected Quantity _quantity;
      private IBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      private IFormula _formula;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _formula = A.Fake<IFormula>();
         A.CallTo(_formula).WithReturnType<double>().Returns(6.0);
         _quantity = new Parameter
         {
            Formula = _formula,
            Value = 202.5
         };

         _buildingBlock = A.Fake<IBuildingBlock>();
         sut = new ResetQuantityValueInBuildingBlockCommand(_quantity, _buildingBlock);
         A.CallTo(() => _context.Get<IQuantity>(_quantity.Id)).Returns(_quantity);
      }
   }

   public class When_reverting_the_command_to_reset_the_quantity_value : concern_for_ResetQuantityValueInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_quantity_value_should_be_the_old_fixed_value()
      {
         _quantity.Value.ShouldBeEqualTo(202.5);
      }

      [Observation]
      public void the_quantity_value_should_be_fixed()
      {
         _quantity.IsFixedValue.ShouldBeTrue();
      }
   }

   public class When_executing_the_command_to_reset_the_quantity_value : concern_for_ResetQuantityValueInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_quantity_value_should_not_be_fixed()
      {
         _quantity.IsFixedValue.ShouldBeFalse();
      }
   }
}
