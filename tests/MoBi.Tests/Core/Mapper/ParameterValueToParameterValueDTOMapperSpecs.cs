using FakeItEasy;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;


namespace MoBi.Core.Mapper
{
   public abstract class concern_for_ParameterValueToParameterValueDTOMapper : ContextSpecification<ParameterValueToParameterValueDTOMapper>
   {
      protected override void Context()
      {
         sut = new ParameterValueToParameterValueDTOMapper(new FormulaToValueFormulaDTOMapper());
      }
   }

   internal class mapping_a_parameter_value_to_parameter_value_add_to_parameter_value_with_a_formula :
      concern_for_ParameterValueToParameterValueDTOMapper
   {
      private ParameterValueDTO _resultDTO;
      private ParameterValue _parameterValue;
      private ExplicitFormula _formula;
      private IDimension _dimension;
      private string _formulaString;
      private ObjectPath _parameterPath;

      protected override void Context()
      {
         base.Context();
         _dimension = A.Fake<IDimension>();
         _parameterPath = new ObjectPath( new[]{"Container","Parameter"});
         _parameterValue = new ParameterValue {Path = _parameterPath, Value = 0.0, Dimension = _dimension };
         _formula = A.Fake<ExplicitFormula>();
         _parameterValue.Formula = _formula;
         _formulaString = "Hello Again";
         _formula.FormulaString = _formulaString;
      }

      protected override void Because()
      {
         _resultDTO = sut.MapFrom(_parameterValue, A.Fake<ParameterValuesBuildingBlock>());
      }

      [Observation]
      public void should_Map_the_properties_right()
      {
         _resultDTO.Name.ShouldBeEqualTo("Parameter");
         _resultDTO.ContainerPath.ShouldOnlyContain("Container");
         _resultDTO.Value.ShouldBeEqualTo(double.NaN);
         _resultDTO.ParameterValue.ShouldBeEqualTo(_parameterValue);
         _resultDTO.Formula.Formula.ShouldBeEqualTo(_formula);
         _resultDTO.Formula.FormulaString.ShouldBeEqualTo(_formulaString);
         _resultDTO.Dimension.ShouldBeEqualTo(_dimension);
      }
   }

   class mapping_a_parameter_value_to_parameter_value_add_to_parameter_value_with_a_value : concern_for_ParameterValueToParameterValueDTOMapper
   {
      private ParameterValueDTO _resultDTO;
      private ParameterValue _parameterValue;
      private IDimension _dimension;
      private double _startValue;
      private ObjectPath _parameterPath;

      protected override void Context()
      {
         base.Context();
         _dimension = A.Fake<IDimension>();
         _parameterPath = new ObjectPath(new[] { "Container", "Parameter" });
         _startValue = 1.2;
         _parameterValue = new ParameterValue { Path = _parameterPath, Value = _startValue, Dimension = _dimension, Formula = null };
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_dimension.DefaultUnit, _startValue)).Returns(_startValue);
      }

      protected override void Because()
      {
         _resultDTO = sut.MapFrom(_parameterValue, A.Fake<ParameterValuesBuildingBlock>());
      }

      [Observation]
      public void should_Map_the_properties_right()
      {
         _resultDTO.Name.ShouldBeEqualTo("Parameter");
         _resultDTO.ContainerPath.ShouldOnlyContain("Container");
         _resultDTO.Value.ShouldBeEqualTo(_startValue);
         _resultDTO.ParameterValue.ShouldBeEqualTo(_parameterValue);
         _resultDTO.Formula.Formula.ShouldBeNull();
         _resultDTO.Formula.FormulaString.ShouldBeEqualTo(AppConstants.Captions.FormulaNotAvailable);
         _resultDTO.Dimension.ShouldBeEqualTo(_dimension);
      }
   }
}