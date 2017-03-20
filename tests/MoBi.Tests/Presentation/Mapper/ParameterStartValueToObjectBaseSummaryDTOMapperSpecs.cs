using System.Linq;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_ParameterStartValueToObjectBaseSummaryDTOMapperSpecs : ContextSpecification<ParameterStartValueToObjectBaseSummaryDTOMapper>
   {
      protected ParameterStartValue _builder;

      protected override void Context()
      {
         _builder = new ParameterStartValue();
         sut = new ParameterStartValueToObjectBaseSummaryDTOMapper();
      }
   }


   public class when_mapping_start_value_with_formulat : concern_for_ParameterStartValueToObjectBaseSummaryDTOMapperSpecs
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         _builder.Name = "Name";
         _builder.ContainerPath = new ObjectPath("A");
         _builder.Formula = new ExplicitFormula("3");
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_builder);
      }

      [Observation]
      public void dictionary_should_contain_appropriate_fields()
      {
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Type) && x.Value.Equals("Parameter Start Value")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Path) && x.Value.Equals("A|Name")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Value)).ShouldBeEqualTo(0);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Formula) && x.Value.Equals("3")).ShouldBeEqualTo(1);
      }
   }

   public class when_mapping_start_value_with_startvalue : concern_for_ParameterStartValueToObjectBaseSummaryDTOMapperSpecs
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         _builder.Name = "Name";
         _builder.StartValue = 9.0;
         _builder.ContainerPath = new ObjectPath("A");
         _builder.Dimension = HelperForSpecs.AmountDimension;
         _builder.DisplayUnit = _builder.Dimension.DefaultUnit;

      }

      protected override void Because()
      {
         _result = sut.MapFrom(_builder);
      }

      [Observation]
      public void dto_should_have_correct_fields_mapped()
      {
         _result.EntityName.ShouldBeEqualTo("Name");
         
         _result.ApplicationIcon.ShouldBeEqualTo(ApplicationIcons.ParameterStartValues);
      }

      [Observation]
      public void dictionary_should_contain_appropriate_fields()
      {
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Type) && x.Value.Equals("Parameter Start Value")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Path) && x.Value.Equals("A|Name")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Value) && x.Value.Equals(string.Format("{0} {1}", 9, _builder.DisplayUnit))).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Formula)).ShouldBeEqualTo(0);
      }
   }

}
