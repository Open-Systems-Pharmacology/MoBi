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
   public abstract class concern_for_ParameterStartValueToDTOParameterStartValueMapperSpecs : ContextSpecification<IParameterStartValueToParameterStartValueDTOMapper>
   {
      protected override void Context()
      {
         sut = new ParameterStartValueToParameterStartValueDTOMapper();
      }
   }

   internal class When_mapping_a_parameterStartValueToADTOParameterStartValue_with_a_formula :
      concern_for_ParameterStartValueToDTOParameterStartValueMapperSpecs
   {
      private ParameterStartValueDTO _resultDTO;
      private IParameterStartValue _parameterStartValue;
      private ExplicitFormula _formula;
      private IDimension _dimension;
      private string _formulaString;
      private IObjectPath _parameterPath;

      protected override void Context()
      {
         base.Context();
         _dimension = A.Fake<IDimension>();
         _parameterPath = new ObjectPath( new[]{"Container","Parameter"});
         _parameterStartValue = new ParameterStartValue {Path = _parameterPath, StartValue = 0.0, Dimension = _dimension };
         _formula = A.Fake<ExplicitFormula>();
         _parameterStartValue.Formula = _formula;
         _formulaString = "Hello Again";
         _formula.FormulaString = _formulaString;
      }

      protected override void Because()
      {
         _resultDTO = sut.MapFrom(_parameterStartValue, A.Fake<IParameterStartValuesBuildingBlock>());
      }

      [Observation]
      public void should_Map_the_properties_right()
      {
         _resultDTO.Name.ShouldBeEqualTo("Parameter");
         _resultDTO.ContainerPath.ShouldOnlyContain("Container");
         _resultDTO.StartValue.ShouldBeEqualTo(double.NaN);
         _resultDTO.ParameterStartValue.ShouldBeEqualTo(_parameterStartValue);
         _resultDTO.Formula.Formula.ShouldBeEqualTo(_formula);
         _resultDTO.Formula.FormulaString.ShouldBeEqualTo(_formulaString);
         _resultDTO.Dimension.ShouldBeEqualTo(_dimension);
      }
   }

   class When_mapping_a_parameterStartValueToADTOParameterStartValue_with_a_startValue : concern_for_ParameterStartValueToDTOParameterStartValueMapperSpecs
   {
      private ParameterStartValueDTO _resultDTO;
      private IParameterStartValue _parameterStartValue;
      private IDimension _dimension;
      private double _startValue;
      private IObjectPath _parameterPath;

      protected override void Context()
      {
         base.Context();
         _dimension = A.Fake<IDimension>();
         _parameterPath = new ObjectPath(new[] { "Container", "Parameter" });
         _startValue = 1.2;
         _parameterStartValue = new ParameterStartValue { Path = _parameterPath, StartValue = _startValue, Dimension = _dimension, Formula = null };
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_dimension.DefaultUnit, _startValue)).Returns(_startValue);
      }

      protected override void Because()
      {
         _resultDTO = sut.MapFrom(_parameterStartValue, A.Fake<IParameterStartValuesBuildingBlock>());
      }

      [Observation]
      public void should_Map_the_properties_right()
      {
         _resultDTO.Name.ShouldBeEqualTo("Parameter");
         _resultDTO.ContainerPath.ShouldOnlyContain("Container");
         _resultDTO.StartValue.ShouldBeEqualTo(_startValue);
         _resultDTO.ParameterStartValue.ShouldBeEqualTo(_parameterStartValue);
         _resultDTO.Formula.Formula.ShouldBeNull();
         _resultDTO.Formula.FormulaString.ShouldBeEqualTo(AppConstants.Captions.FormulaNotAvailable);
         _resultDTO.Dimension.ShouldBeEqualTo(_dimension);
      }
   }
}