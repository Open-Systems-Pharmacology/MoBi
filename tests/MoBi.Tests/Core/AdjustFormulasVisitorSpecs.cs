using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core
{
   public abstract class concern_for_AdjustFormulaVisitor : ContextSpecification<AdjustFormulasVisitor>
   {
      private IMoBiContext _context;
      protected IMoBiHistoryManager _historyManager;
      protected INameCorrector _nameCorrector;
      protected IWithIdRepository _objectBaseRepository;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _historyManager = A.Fake<IMoBiHistoryManager>();
         _objectBaseRepository = A.Fake<IWithIdRepository>();
         A.CallTo(() => _context.ObjectRepository).Returns(_objectBaseRepository);
         ;
         A.CallTo(() => _context.HistoryManager).Returns(_historyManager);
         _nameCorrector = A.Fake<INameCorrector>();
         sut = new AdjustFormulasVisitor(_context, _nameCorrector);
      }
   }

   public class When_checking_equality_for_table_formula : concern_for_AdjustFormulaVisitor
   {
      private bool _resultEqual;
      private bool _resultUnequal;
      private TableFormula _equalFormula1;
      private TableFormula _equalFormula2;
      private TableFormula _unequalFormula;

      protected override void Context()
      {
         base.Context();
         _equalFormula1 = new TableFormula();
         _equalFormula2 = new TableFormula();
         _equalFormula1.AddPoint(1, 2);
         _equalFormula1.AddPoint(2, 3);
         _equalFormula2.AddPoint(1, 2);
         _equalFormula2.AddPoint(2, 3);

         _unequalFormula = new TableFormula();
         _unequalFormula.AddPoint(3, 4);
         _unequalFormula.AddPoint(4, 5);
      }

      protected override void Because()
      {
         _resultEqual = sut.AreEqualTableFormula(_equalFormula1, _equalFormula2);
         _resultUnequal = sut.AreEqualTableFormula(_equalFormula1, _unequalFormula);
      }

      [Observation]
      public void should_return_true_for_equal_formulas()
      {
         _resultEqual.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_for_unequal_formulas()
      {
         _resultUnequal.ShouldBeFalse();
      }
   }

   public class When_checking_equality_for_Sum_formula : concern_for_AdjustFormulaVisitor
   {
      private bool _equalResult;
      private bool _unequalResult;
      private SumFormula _equalFormula1;
      private SumFormula _equalFormula2;
      private SumFormula _unequalFormula;

      protected override void Context()
      {
         base.Context();
         _equalFormula1 = new SumFormula();
         _equalFormula2 = new SumFormula();
         _equalFormula1.Criteria.Add(new MatchTagCondition("A"));
         _equalFormula1.Criteria.Add(new NotMatchTagCondition("B"));
         _equalFormula2.Criteria.Add(new MatchTagCondition("A"));
         _equalFormula2.Criteria.Add(new NotMatchTagCondition("B"));

         _unequalFormula = new SumFormula();
         _unequalFormula.Criteria.Add(new NotMatchTagCondition("A"));
         _unequalFormula.Criteria.Add(new MatchTagCondition("B"));
      }

      protected override void Because()
      {
         _equalResult = sut.AreEqualSumFormula(_equalFormula1, _equalFormula2);
         _unequalResult = sut.AreEqualSumFormula(_equalFormula1, _unequalFormula);
      }

      [Observation]
      public void should_return_true_for_equal_formulas()
      {
         _equalResult.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_for_unequal_formulas()
      {
         _unequalResult.ShouldBeFalse();
      }
   }

   public class When_checking_equality_for_TableFormulaWithObjectBase : concern_for_AdjustFormulaVisitor
   {
      private bool _result1;
      private bool _result2;
      private bool _result3;
      private TableFormulaWithOffset _equalFormula1;
      private TableFormulaWithOffset _equalFormula2;
      private TableFormulaWithOffset _unequalFormula;

      protected override void Context()
      {
         base.Context();
         _equalFormula1 = new TableFormulaWithOffset().WithName("Equal");
         _equalFormula2 = new TableFormulaWithOffset().WithName("Equal");
         var offsetObjectPath = new FormulaUsablePath(new[] {"X"}).WithAlias("Offset");
         _equalFormula1.AddOffsetObjectPath(offsetObjectPath);
         _equalFormula2.AddOffsetObjectPath(offsetObjectPath);
         var tableObjectPath = new FormulaUsablePath(new[] {"Y"}).WithAlias("Table");
         _equalFormula1.AddTableObjectPath(tableObjectPath);
         _equalFormula2.AddTableObjectPath(tableObjectPath);

         _unequalFormula = new TableFormulaWithOffset().WithName("Equal");
         _unequalFormula.AddOffsetObjectPath(new FormulaUsablePath(new[] {"A"}).WithAlias("Offset"));
         _unequalFormula.AddTableObjectPath(new FormulaUsablePath(new[] {"B"}).WithAlias("Table"));
      }

      protected override void Because()
      {
         _result1 = sut.AreEqualTableFormulaWithOffset(_equalFormula1, _equalFormula2);
         _result2 = sut.AreEqualTableFormulaWithOffset(_equalFormula1, _unequalFormula);
         _result3 = sut.AreEqualTableFormulaWithOffset(_unequalFormula, _equalFormula1);
      }

      [Observation]
      public void should_return_true_for_equal_formulas()
      {
         _result1.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_unequal_formulas()
      {
         _result2.ShouldBeFalse();
         _result3.ShouldBeFalse();
      }
   }

   public class When_checkuing_equality_for_black_box_formulas : concern_for_AdjustFormulaVisitor
   {
      private bool _result1;
      private bool _result2;
      private bool _result3;

      protected override void Because()
      {
         _result1 = sut.AreEqualBalckBoxFormula(null, A.Fake<BlackBoxFormula>());
         _result2 = sut.AreEqualBalckBoxFormula(A.Fake<BlackBoxFormula>(), null);
         _result3 = sut.AreEqualBalckBoxFormula(A.Fake<BlackBoxFormula>(), A.Fake<BlackBoxFormula>());
      }

      [Observation]
      public void should_return_false_then_any_formula_is_null()
      {
         _result1.ShouldBeFalse();
         _result2.ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_for_not_null_formulas()
      {
         _result3.ShouldBeTrue();
      }
   }

   public class When_adjusting_Formulas_at_a_totaly_new_object : concern_for_AdjustFormulaVisitor
   {
      private IUsingFormula _totalyNewObject;
      private IBuildingBlock _buildingBlockToAddTo;
      private IFormulaCache _formulaCache;
      private ExplicitFormula _newFormula;

      protected override void Context()
      {
         base.Context();
         _buildingBlockToAddTo = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockToAddTo.FormulaCache).Returns(_formulaCache);
         _totalyNewObject = new Parameter().WithName("New");
         _newFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(A.Fake<IDimension>()).WithFormulaString("1+1").WithId("1");
         _newFormula.ObjectPaths = new FormulaUsablePath[0];
         _totalyNewObject.Formula = _newFormula;
         A.CallTo(() => _objectBaseRepository.ContainsObjectWithId(A<string>._)).Returns(false);
      }

      protected override void Because()
      {
         sut.AdjustFormulasIn(_totalyNewObject, _buildingBlockToAddTo);
      }

      [Observation]
      public void should_add_formula_to_formula_cache()
      {
         A.CallTo(() => _buildingBlockToAddTo.AddFormula(_newFormula)).MustHaveHappened();
      }
   }

   public class When_adjusting_Formulas_at_a_totaly_new_moleculeBuilder : concern_for_AdjustFormulaVisitor
   {
      private MoleculeBuilder _totalyNewObject;
      private IBuildingBlock _buildingBlockToAddTo;
      private IFormulaCache _formulaCache;
      private ExplicitFormula _newFormula;

      protected override void Context()
      {
         base.Context();
         _buildingBlockToAddTo = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockToAddTo.FormulaCache).Returns(_formulaCache);
         _totalyNewObject = new MoleculeBuilder().WithName("New");
         _newFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(A.Fake<IDimension>()).WithFormulaString("1+1").WithId("1");
         _newFormula.ObjectPaths = new FormulaUsablePath[0];
         _totalyNewObject.DefaultStartFormula = _newFormula;
      }

      protected override void Because()
      {
         sut.AdjustFormulasIn(_totalyNewObject, _buildingBlockToAddTo);
      }

      [Observation]
      public void should_add_formula_to_formula_cache()
      {
         A.CallTo(() => _buildingBlockToAddTo.AddFormula(_newFormula)).MustHaveHappened();
      }
   }

   public class When_adjusting_Formulas_at_a_object_with_allready_existing_Formula : concern_for_AdjustFormulaVisitor
   {
      private IUsingFormula _totalyNewObject;
      private IBuildingBlock _buildingBlockToAddTo;
      private IFormulaCache _formulaCache;
      private ExplicitFormula _newFormula;
      private ExplicitFormula _oldFormula;
      private IDimension _theDimension;

      protected override void Context()
      {
         base.Context();
         _buildingBlockToAddTo = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockToAddTo.FormulaCache).Returns(_formulaCache);

         _totalyNewObject = new Parameter().WithName("New");
         _theDimension = A.Fake<IDimension>();
         _oldFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(_theDimension).WithFormulaString("1+1").WithId("1");
         _newFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(_theDimension).WithFormulaString("1+1").WithId("1");
         _newFormula.ObjectPaths = new FormulaUsablePath[0];
         _oldFormula.ObjectPaths = new FormulaUsablePath[0];
         _formulaCache.Add(_oldFormula);
         _totalyNewObject.Formula = _newFormula;
      }

      protected override void Because()
      {
         sut.AdjustFormulasIn(_totalyNewObject, _buildingBlockToAddTo);
      }

      [Observation]
      public void should_not_add_formula_to_formula_cache()
      {
         A.CallTo(() => _buildingBlockToAddTo.AddFormula(_newFormula)).MustNotHaveHappened();
      }
   }

   public class When_adjusting_Formulas_at_a_object_with_allready_existing_Formula_and_new_rhs_Formula : concern_for_AdjustFormulaVisitor
   {
      private IParameter _totalyNewObject;
      private IBuildingBlock _buildingBlockToAddTo;
      private IFormulaCache _formulaCache;
      private ExplicitFormula _newFormula;
      private ExplicitFormula _oldFormula;
      private IDimension _theDimension;
      private IFormula _newRHSFormula;

      protected override void Context()
      {
         base.Context();
         _buildingBlockToAddTo = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockToAddTo.FormulaCache).Returns(_formulaCache);

         _totalyNewObject = new Parameter().WithName("New");
         _theDimension = A.Fake<IDimension>();
         _oldFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(_theDimension).WithFormulaString("1+1").WithId("1");
         _newFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(_theDimension).WithFormulaString("1+1").WithId("1");
         _newFormula.ObjectPaths = Array.Empty<FormulaUsablePath>();
         _oldFormula.ObjectPaths = Array.Empty<FormulaUsablePath>();
         _formulaCache.Add(_oldFormula);
         _newRHSFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(_theDimension).WithFormulaString("1+5").WithId("RHS");
         _totalyNewObject.Formula = _newFormula;
         A.CallTo(() => _nameCorrector.CorrectName(_formulaCache, _newRHSFormula)).Returns(true);
         A.CallTo(() => _objectBaseRepository.ContainsObjectWithId(_newRHSFormula.Id)).Returns(false);
         _totalyNewObject.RHSFormula = _newRHSFormula;
      }

      protected override void Because()
      {
         sut.AdjustFormulasIn(_totalyNewObject, _buildingBlockToAddTo);
      }

      [Observation]
      public void should_not_add_formula_to_formula_cache()
      {
         A.CallTo(() => _buildingBlockToAddTo.AddFormula(_newFormula)).MustNotHaveHappened();
      }

      [Observation]
      public void should_add_rhs_formula_to_formula_cache()
      {
         A.CallTo(() => _buildingBlockToAddTo.AddFormula(_newRHSFormula)).MustHaveHappened();
      }
   }

   public class When_adjusting_Formulas_at_a_object_with_a_Formula_with_a_allready_used_name_and_id : concern_for_AdjustFormulaVisitor
   {
      private IUsingFormula _totalyNewObject;
      private IBuildingBlock _buildingBlockToAddTo;
      private IFormulaCache _formulaCache;
      private ExplicitFormula _newFormula;
      private ExplicitFormula _oldFormula;
      private IDimension _theDimension;

      protected override void Context()
      {
         base.Context();
         _buildingBlockToAddTo = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockToAddTo.FormulaCache).Returns(_formulaCache);
         _totalyNewObject = new Parameter().WithName("New");
         _theDimension = A.Fake<IDimension>();
         _oldFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(_theDimension).WithFormulaString("1+1").WithId("1");
         _newFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(_theDimension).WithFormulaString("1+5").WithId("1");
         _newFormula.ObjectPaths = Array.Empty<FormulaUsablePath>();
         _oldFormula.ObjectPaths = Array.Empty<FormulaUsablePath>();
         _formulaCache.Add(_oldFormula);
         _totalyNewObject.Formula = _newFormula;
         A.CallTo(() => _nameCorrector.CorrectName(_formulaCache, _newFormula)).Returns(true);
         A.CallTo(() => _objectBaseRepository.ContainsObjectWithId(_newFormula.Id)).Returns(true);
      }

      protected override void Because()
      {
         sut.AdjustFormulasIn(_totalyNewObject, _buildingBlockToAddTo);
      }

      [Observation]
      public void should_add_formula_to_formula_cache()
      {
         A.CallTo(() => _buildingBlockToAddTo.AddFormula(_newFormula)).MustHaveHappened();
      }

      [Observation]
      public void should_ask_for_new_formulas_name()
      {
         A.CallTo(() => _nameCorrector.CorrectName(_formulaCache, _newFormula)).MustHaveHappened();
      }

      [Observation]
      public void should_have_changed_the_new_formulas_id()
      {
         _newFormula.Id.ShouldNotBeEqualTo(_oldFormula.Id);
      }
   }

   public class When_adjusting_Formulas_at_a_object_with_allready_existing_Formula_with_differnt_name : concern_for_AdjustFormulaVisitor
   {
      private IParameter _totalyNewObject;
      private IBuildingBlock _buildingBlockToAddTo;
      private IFormulaCache _formulaCache;
      private ExplicitFormula _newFormula;
      private ExplicitFormula _oldFormula;
      private IDimension _theDimension;

      protected override void Context()
      {
         base.Context();
         _buildingBlockToAddTo = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockToAddTo.FormulaCache).Returns(_formulaCache);

         _totalyNewObject = new Parameter().WithName("New");
         _theDimension = A.Fake<IDimension>();
         _oldFormula = A.Fake<ExplicitFormula>().WithName("New Formula").WithDimension(_theDimension).WithFormulaString("1+1").WithId("1");
         _newFormula = A.Fake<ExplicitFormula>().WithName("New Formula_2").WithDimension(_theDimension).WithFormulaString("1+1").WithId("1");
         _newFormula.ObjectPaths = Array.Empty<FormulaUsablePath>();
         _oldFormula.ObjectPaths = Array.Empty<FormulaUsablePath>();
         _formulaCache.Add(_oldFormula);
         _totalyNewObject.Formula = _newFormula;
      }

      protected override void Because()
      {
         sut.AdjustFormulasIn(_totalyNewObject, _buildingBlockToAddTo);
      }

      [Observation]
      public void should_not_add_formula_to_formula_cache()
      {
         A.CallTo(() => _buildingBlockToAddTo.AddFormula(_newFormula)).MustNotHaveHappened();
      }

      [Observation]
      public void should_set_the_formula_for_the_new_object_to_the_old_formula()
      {
         _totalyNewObject.Formula.ShouldBeEqualTo(_oldFormula);
      }
   }
}