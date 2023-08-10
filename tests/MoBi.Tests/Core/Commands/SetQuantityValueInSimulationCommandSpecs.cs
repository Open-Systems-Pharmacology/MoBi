using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetQuantityValueInSimulationCommand : ContextSpecification<SetQuantityValueInSimulationCommand>
   {
      private IMoBiSimulation _simulation;
      protected IQuantity _quantity;
      protected double _valueToSet;
      protected IMoBiContext _context;
      protected double _oldValue;

      protected override void Context()
      {
         _simulation = A.Fake<IMoBiSimulation>().WithId("Sim");
         _valueToSet = 10;
         _oldValue = 5;
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Resolve<IQuantityValueInSimulationChangeTracker>()).Returns(DomainHelperForSpecs.QuantityValueChangeTracker(A.Fake<IEventPublisher>()));
         _quantity = CreateQuantity().WithId("Quantity");
         A.CallTo(() => _context.Get<IMoBiSimulation>(_simulation.Id)).Returns(_simulation);
         A.CallTo(() => _context.Get<IQuantity>(_quantity.Id)).Returns(_quantity);
         sut = new SetQuantityValueInSimulationCommand(_quantity, _valueToSet, _simulation);
      }

      protected virtual IQuantity CreateQuantity()
      {
         return new Parameter().WithFormula(new ConstantFormula(_oldValue));
      }
   }

   public class When_setting_a_value_in_a_quantity_with_constant_formula : concern_for_SetQuantityValueInSimulationCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_have_set_the_expected_value_in_the_quantity()
      {
         _quantity.Value.ShouldBeEqualTo(_valueToSet);
      }

      [Observation]
      public void the_quantity_should_be_marked_as_fixed()
      {
         _quantity.IsFixedValue.ShouldBeTrue();
      }
   }

   public class When_executing_the_inverse_of_a_setting_a_value_in_a_quantity_with_constant_formula : concern_for_SetQuantityValueInSimulationCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_have_reseted_the_value_in_the_quantity()
      {
         _quantity.Value.ShouldBeEqualTo(_oldValue);
      }

      [Observation]
      public void the_quantity_should_be_marked_as_not_fixed()
      {
         _quantity.IsFixedValue.ShouldBeFalse();
      }
   }

   public class When_setting_a_value_in_a_quantity_with_constant_formula_equal_to_the_constant_formula_value : concern_for_SetQuantityValueInSimulationCommand
   {
      protected override IQuantity CreateQuantity()
      {
         return new Parameter().WithFormula(new ConstantFormula(_valueToSet));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_quantity_should_be_marked_as_not_fixed()
      {
         _quantity.IsFixedValue.ShouldBeFalse();
      }
   }

   public class When_setting_a_value_in_a_quantity_with_explicit_formula : concern_for_SetQuantityValueInSimulationCommand
   {
      protected override IQuantity CreateQuantity()
      {
         return new Parameter().WithFormula(new ExplicitFormula("2+3"));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_have_set_the_expected_value_in_the_quantity()
      {
         _quantity.Value.ShouldBeEqualTo(_valueToSet);
      }

      [Observation]
      public void the_quantity_should_be_marked_as_fixed()
      {
         _quantity.IsFixedValue.ShouldBeTrue();
      }
   }

   public class When_executing_the_inverse_of_setting_a_value_in_a_quantity_with_explicit_formula : concern_for_SetQuantityValueInSimulationCommand
   {
      protected override IQuantity CreateQuantity()
      {
         return new Parameter().WithFormula(new ExplicitFormula("2+3"));
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_quantity_should_be_marked_as_not_fixed()
      {
         _quantity.IsFixedValue.ShouldBeFalse();
      }
   }
}