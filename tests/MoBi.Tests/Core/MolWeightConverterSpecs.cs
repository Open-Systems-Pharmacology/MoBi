using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core
{
   public abstract class concern_for_MolWeightDimensionConverterForColumn : ContextSpecification<MolWeightDimensionConverterForDataColumn>
   {
      private DataColumn _column;
      protected double _mw;
      private IDimension _source;
      private IDimension _target;

      protected override void Context()
      {
         _mw = 10;
         _column = A.Fake<DataColumn>();
         _column.Dimension = _source;
         _column.DataInfo = A.Fake<DataInfo>();
         _column.DataInfo.MolWeight=_mw;
         _source = new Dimension(new BaseDimensionRepresentation(), "DrugMass", "g");
         _source.AddUnit("mg", 1000, 0);
         _source.DefaultUnit = _source.Unit("mg");
         _target = new Dimension(new BaseDimensionRepresentation(), "Target", "mol");
         _target.AddUnit("mmol", 300, 0);
         _target.DefaultUnit = _target.Unit("mmol");
         sut = new ConcentrationToMolarConcentrationConverterForDataColumn(_source, _target);
         sut.SetRefObject(_column);
      }
   }

   public class When_asked_to_convert_to_target_base_unit : concern_for_MolWeightDimensionConverterForColumn
   {
      private double _res;

      protected override void Because()
      {
         _res = sut.ConvertToTargetBaseUnit(10);
      }

      [Observation]
      public void should_return_value_in_target_base_unit()
      {
         _res.ShouldBeEqualTo(10/_mw);
      }
   }

   public class When_is_initialiesed_correct : concern_for_MolWeightDimensionConverterForColumn
   {
      private bool _res;

      protected override void Because()
      {
         _res = sut.CanResolveParameters();
      }

      [Observation]
      public void should_be_able_to_resolve_parameters()
      {
         _res.ShouldBeEqualTo(true);
      }
   }

   public abstract class concern_for_type_MolWeightDimensionConverterForFormulaUsable : ContextSpecification<MolWeightDimensionConverterForFormulaUsable>
   {
      protected IQuantity _formulaUsable;
      protected double _mw;
      private IDimension _source;
      private IDimension _target;

      protected override void Context()
      {
         _mw = 10;
         _formulaUsable = A.Fake<IQuantity>();
         _formulaUsable.Dimension = _source;
         _source = new Dimension(new BaseDimensionRepresentation(), "DrugMass", "g");
         _source.AddUnit("mg", 1000, 0);
         _source.DefaultUnit = _source.Unit("mg");
         _target = new Dimension(new BaseDimensionRepresentation(), "Target", "mol");
         _target.AddUnit("mmol", 300, 0);
         _target.DefaultUnit = _target.Unit("mmol");
         sut = new MolWeightDimensionConverterForFormulaUsable(_source, _target);
         sut.SetRefObject(_formulaUsable);
      }
   }

   public class When_asked_to_convert_to_target_base_unit_FormulaUsable : concern_for_type_MolWeightDimensionConverterForFormulaUsable
   {
      private double _res;

      protected override void Context()
      {
         base.Context();
         IContainer root = new Container().WithName("Top");
         IContainer compound = new Container().WithName("Bla").WithParentContainer(root);
         IContainer organism  = new Container().WithName("Organism").WithParentContainer(root);
         IParameter mw = A.Fake<IParameter>().WithName(AppConstants.Parameters.MOLECULAR_WEIGHT).WithParentContainer(compound);
         mw.Value = _mw;
         _formulaUsable = new Parameter().WithName("Bla").WithParentContainer(organism);
         sut.SetRefObject(_formulaUsable);
      }

      protected override void Because()
      {
         _res = sut.ConvertToTargetBaseUnit(10);
      }

      [Observation]
      public void should_return_value_in_target_base_unit()
      {
         _res.ShouldBeEqualTo(10/_mw);
      }
   }

   public class When_is_initialiesed_correct_FormulaUsable : concern_for_type_MolWeightDimensionConverterForFormulaUsable
   {
      private bool _res;

      protected override void Context()
      {
         base.Context();
         IContainer root = new Container().WithName("Top");
         IContainer compound = new Container().WithName("Bla").WithParentContainer(root);
         IContainer organism = new Container().WithName("Organism").WithParentContainer(root);
         IParameter mw = A.Fake<IParameter>().WithName(AppConstants.Parameters.MOLECULAR_WEIGHT).WithParentContainer(compound);
         mw.Value = _mw;

         _formulaUsable = new Parameter().WithName("Bla").WithParentContainer(organism);
         sut.SetRefObject(_formulaUsable);
      }

      protected override void Because()
      {
         _res = sut.CanResolveParameters();
      }

      [Observation]
      public void should_be_able_to_resolve_parameters()
      {
         _res.ShouldBeEqualTo(true);
      }
   }
}