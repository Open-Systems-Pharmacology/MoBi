using MoBi.Core.Domain;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public class concern_for_QuantityToParameterValueMapper : ContextSpecification<QuantityToOriginalQuantityValueMapper>
   {
      protected IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _entityPathResolver = new EntityPathResolverForSpecs();
         sut = new QuantityToOriginalQuantityValueMapper(_entityPathResolver);
      }
   }

   public class When_mapping_from_parameter_to_parameter_value : concern_for_QuantityToParameterValueMapper
   {
      private IQuantity _quantity;
      private OriginalQuantityValue _result;

      protected override void Context()
      {
         base.Context();
         _quantity = new Parameter
         {
            Name = "parameterName",
            Value = 1.0,
            Formula = new ExplicitFormula("1"),
            Dimension = DimensionFactoryForSpecs.MassDimension,
            DisplayUnit = DimensionFactoryForSpecs.MassDimension.DefaultUnit
         };
         _quantity.UpdateValueOriginFrom(new ValueOrigin { Source = ValueOriginSources.Internet });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_quantity);
      }

      [Observation]
      public void the_properties_should_be_mapped_correctly()
      {
         _result.Path.ShouldBeEqualTo(_entityPathResolver.ObjectPathFor(_quantity).ToString());
         _result.Value.ShouldBeEqualTo(_quantity.Value);
         _result.Dimension.ShouldBeEqualTo(_quantity.Dimension);
         _result.DisplayUnit.ShouldBeEqualTo(_quantity.DisplayUnit);
         _result.ValueOrigin.ShouldBeEqualTo(_quantity.ValueOrigin);
      }
   }
}