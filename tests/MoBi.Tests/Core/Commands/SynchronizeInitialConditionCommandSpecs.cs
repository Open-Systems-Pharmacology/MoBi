using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SynchronizeInitialConditionCommand : ContextSpecification<SynchronizeInitialConditionCommand>
   {
      protected InitialCondition _initialCondition;
      protected MoleculeAmount _moleculeAmount;
      protected IDimension _dimension1;
      protected IDimension _dimension2;
      private Unit _displayUnit1;
      protected Unit _displayUnit2;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _dimension1 = A.Fake<IDimension>();
         _dimension2 = A.Fake<IDimension>();
         _displayUnit1 = A.Fake<Unit>();
         _displayUnit2 = A.Fake<Unit>();

         _moleculeAmount = new MoleculeAmount()
            .WithFormula(new ConstantFormula(10))
            .WithDimension(_dimension1)
            .WithDisplayUnit(_displayUnit1);

         _moleculeAmount.ScaleDivisor = 5;

         _moleculeAmount.ValueOrigin.Method = ValueOriginDeterminationMethods.Assumption;
         _moleculeAmount.ValueOrigin.Source = ValueOriginSources.Internet;
         _moleculeAmount.ValueOrigin.Description = "Hello";

         _initialCondition = new InitialCondition();
         sut = new SynchronizeInitialConditionCommand(_moleculeAmount, _initialCondition, new InitialConditionsBuildingBlock());

         _context = A.Fake<IMoBiContext>();
      }
   }

   public class When_synchronizing_the_value_of_a_molecule_start_value_with_a_start_value_and_the_same_dimension : concern_for_SynchronizeInitialConditionCommand
   {
      protected override void Context()
      {
         base.Context();
         _initialCondition.Dimension = _dimension1;
         _initialCondition.DisplayUnit = _displayUnit2;
         _initialCondition.Value = 20;
         _initialCondition.ValueOrigin.Method  = ValueOriginDeterminationMethods.InVitro;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_start_value()
      {
         _initialCondition.Value.ShouldBeEqualTo(_moleculeAmount.Value);
      }

      [Observation]
      public void should_update_the_display_unit()
      {
         _initialCondition.DisplayUnit.ShouldBeEqualTo(_moleculeAmount.DisplayUnit);
      }

      [Observation]
      public void should_update_the_scale_divisor()
      {
         _initialCondition.ScaleDivisor.ShouldBeEqualTo(_moleculeAmount.ScaleDivisor);
      }

      [Observation]
      public void should_update_the_value_origin_from_the_molecule_amount()
      {
         _initialCondition.ValueOrigin.ShouldBeEqualTo(_moleculeAmount.ValueOrigin);
      }
   }

   public class When_synchronizing_the_value_of_a_molecule_start_value_with_another_dimension : concern_for_SynchronizeInitialConditionCommand
   {
      protected override void Context()
      {
         base.Context();
         _initialCondition.Dimension = _dimension2;
         _initialCondition.DisplayUnit = _displayUnit2;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_not_update_the_display_unit()
      {
         _initialCondition.DisplayUnit.ShouldBeEqualTo(_displayUnit2);
      }
   }

   public class When_synchronizing_the_value_of_a_molecule_start_value_without_a_start_value : concern_for_SynchronizeInitialConditionCommand
   {
      protected override void Context()
      {
         base.Context();

         _moleculeAmount.Formula = new ExplicitFormula("1+2");
         _initialCondition.Value = null;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_not_update_the_start_value()
      {
         _initialCondition.Value.ShouldBeNull();
      }
   }

   public class When_synchronizing_the_value_of_a_molecule_start_value_for_a_start_value_defined_as_a_formula_but_overriden_by_the_user_: concern_for_SynchronizeInitialConditionCommand
   {
      protected override void Context()
      {
         base.Context();

         _moleculeAmount.Formula = new ExplicitFormula("1+2");
         _moleculeAmount.Value = 5;
         _initialCondition.Value = null;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_start_value()
      {
         _initialCondition.Value.ShouldBeEqualTo(5);
      }
   }
}