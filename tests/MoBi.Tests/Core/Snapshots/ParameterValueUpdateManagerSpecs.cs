using FakeItEasy;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Core.Snapshots;

internal class concern_for_ParameterValueUpdateManager : ContextSpecification<ParameterValueUpdateManager>
{
   private IOSPSuiteLogger _logger;

   protected override void Context()
   {
      base.Context();
      _logger = A.Fake<IOSPSuiteLogger>();
      sut = new ParameterValueUpdateManager(_logger);
   }
}

internal class When_mapping_from_ParameterValueWithInitialState : concern_for_ParameterValueUpdateManager
{
   private UpdatedParameterValue _result;
   private ParameterValueWithInitialState _source;

   protected override void Context()
   {
      base.Context();
      _source = new ExpressionParameter()
      {
         Value = null,
         DisplayUnit = new Unit("unit", 1, 0),
         Formula = new ExplicitFormula("0").WithId("formulaId"),
         Path = new ObjectPath("a", "path"),

         InitialValue = 1.0,
         InitialUnit = new Unit("oldUnit", 1, 0),
      };
   }

   protected override void Because()
   {
      _result = sut.MapFrom(_source);
   }

   [Observation]
   public void the_result_should_have_the_original_new_values()
   {
      _result.NewFormulaId.ShouldBeEqualTo("formulaId");
      _result.NewUnit.ShouldBeEqualTo("unit");
      _result.NewValue.ShouldBeNull();
   }
}

internal class When_updating_a_ParameterValue_in_a_BuildingBlock : concern_for_ParameterValueUpdateManager
{
   private ExpressionProfileBuildingBlock _buildingBlock;
   private UpdatedParameterValue _updatedParameterValue;
   private ExpressionParameter _parameterValue;
   private IDimension _dimension;

   protected override void Context()
   {
      base.Context();
      _dimension = new Dimension(new BaseDimensionRepresentation(), "length", "initialUnit");
      _dimension.AddUnit("newUnit", 2.0, 0);
      _parameterValue = new ExpressionParameter
      {
         Value = 5.0,
         DisplayUnit = _dimension.Unit("initialUnit"),
         Formula = new ExplicitFormula("x + 1").WithId("initialFormulaId"),
         Path = new ObjectPath("buildingBlock", "parameterValue"),
         Dimension = _dimension
      };

      _buildingBlock =
      [
         _parameterValue
      ];

      _buildingBlock.FormulaCache.Add(new ExplicitFormula("2 * x").WithId("newFormulaId"));
      _buildingBlock.FormulaCache.Add(_parameterValue.Formula);

      _updatedParameterValue = new UpdatedParameterValue
      {
         Path = _parameterValue.Path,
         NewValue = 20.0,
         NewUnit = "newUnit",
         NewFormulaId = "newFormulaId"
      };
   }

   protected override void Because()
   {
      sut.UpdateParameterValueIn<ExpressionProfileBuildingBlock, ExpressionParameter>(_buildingBlock, _updatedParameterValue, new FormulaCache());
   }

   [Observation]
   public void the_parameter_value_should_have_updated_its_value_unit_and_formula()
   {
      _parameterValue.Value.ShouldBeEqualTo(20.0);
      _parameterValue.DisplayUnit.Name.ShouldBeEqualTo("newUnit");
      _parameterValue.Formula.Id.ShouldBeEqualTo("newFormulaId");
   }

   [Observation]
   public void the_parameter_value_should_have_stored_its_initial_state_correctly()
   {
      _parameterValue.InitialValue.ShouldBeEqualTo(5.0);
      _parameterValue.InitialUnit.Name.ShouldBeEqualTo("initialUnit");
      _parameterValue.InitialFormulaId.ShouldBeEqualTo("initialFormulaId");
   }
}