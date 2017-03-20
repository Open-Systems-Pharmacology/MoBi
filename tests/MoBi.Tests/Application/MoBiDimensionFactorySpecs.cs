using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Application
{
   public abstract class concern_for_MoBiDimensionFactory : ContextSpecification<IMoBiDimensionFactory>
   {
      protected Dimension _drugMassDimension;
      protected Dimension _volumeDimension;
      protected Dimension _flowDimension;

      protected override void Context()
      {
         sut = new MoBiDimensionFactory();

         _drugMassDimension = new Dimension(new BaseDimensionRepresentation(), "DrugMass", "g");
         _volumeDimension = new Dimension(new BaseDimensionRepresentation {MassExponent = 3}, "Volume", "l");
         _flowDimension = new Dimension(new BaseDimensionRepresentation {MassExponent = 3, TimeExponent = -1}, "flow", "l/min");

         sut.AddDimension(_drugMassDimension);
         sut.AddDimension(_volumeDimension);
         sut.AddDimension(_flowDimension);
      }
   }

   public abstract class when_retreiving_dimension_from_unit_with_multiple_matching_units : concern_for_MoBiDimensionFactory
   {
      protected IDimension _result;
      protected Dimension _accelerationDimension;
      protected abstract string ConvertUnitCase(string unit);

      protected override void Context()
      {
         base.Context();
         _accelerationDimension = new Dimension(new BaseDimensionRepresentation(), "Acceleration", "G");
         sut.AddDimension(_accelerationDimension);
      }

      protected override void Because()
      {
         _result = sut.DimensionForUnit(ConvertUnitCase("g"));
      }
   }

   public class retreive_upper_case_unit_from_multiple_matching_units : when_retreiving_dimension_from_unit_with_multiple_matching_units
   {
      protected override string ConvertUnitCase(string unit)
      {
         return unit.ToUpper();
      }

      [Observation]
      public void results_in_lower_case_match()
      {
         _result.ShouldBeEqualTo(_accelerationDimension);
      }
   }

   public class retreive_lower_case_unit_from_multiple_matching_units : when_retreiving_dimension_from_unit_with_multiple_matching_units
   {
      protected override string ConvertUnitCase(string unit)
      {
         return unit.ToLower();
      }

      [Observation]
      public void results_in_lower_case_match()
      {
         _result.ShouldBeEqualTo(_drugMassDimension);
      }
   }

   public class When_told_to_retrieve_a_dimension_by_name_that_does_not_exist_and_that_is_not_an_RHS_dimension : concern_for_MoBiDimensionFactory
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.GetDimension("TRALALA")).ShouldThrowAn<KeyNotFoundException>();
      }
   }

   public class When_told_to_retrieve_a_dimension_by_name_that_does_not_exist_and_but_that_is_an_RHS_dimension : concern_for_MoBiDimensionFactory
   {
      [Observation]
      public void should_return_a_new_dimension()
      {
         sut.GetDimension(AppConstants.RHSDimensionName(_drugMassDimension)).ShouldNotBeNull();
      }
   }

   public class When_told_to_retrieve_a_rhs_dimension_for_a_dimension : concern_for_MoBiDimensionFactory
   {
      [Observation]
      public void should_return_the_equivalent_dimension_if_it_exists_instead_of_creating_a_new_one()
      {
         sut.RHSDimensionFor(_volumeDimension).ShouldBeEqualTo(_flowDimension);
      }
   }

   public class When_told_to_retrieve_a_dimension_by_name_that_does_not_exist_and_that_is_not_a_possible_RHS_dimension : concern_for_MoBiDimensionFactory
   {
      [Observation]
      public void should_return_a_new_dimension()
      {
         The.Action(() => sut.GetDimension("TRALALAL" + AppConstants.RHSDimensionSuffix)).ShouldThrowAn<KeyNotFoundException>();
      }
   }
}