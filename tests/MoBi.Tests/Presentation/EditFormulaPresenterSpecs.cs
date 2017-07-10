using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditFormulaPresenter : ContextSpecification<IEditFormulaPresenter>
   {
      protected IEditFormulaView _editFormulaView;
      protected IFormulaPresenterCache _formulaPresenterCache;
      protected IMoBiContext _context;
      protected IFormulaToFormulaInfoDTOMapper _formulaToDTOInfoMapper;
      protected ICommandCollector _commandCollector;
      protected IMoBiFormulaTask _formulaTask;
      private ICircularReferenceChecker _circularReferenceChecker;

      protected override void Context()
      {
         _editFormulaView = A.Fake<IEditFormulaView>();
         _context = A.Fake<IMoBiContext>();
         _formulaPresenterCache = A.Fake<IFormulaPresenterCache>();
         _formulaToDTOInfoMapper = new FormulaToFormulaInfoDTOMapper();
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _circularReferenceChecker = A.Fake<ICircularReferenceChecker>();
         sut = new EditFormulaPresenter(_editFormulaView, _formulaPresenterCache, _context, _formulaToDTOInfoMapper, new FormulaTypeCaptionRepository(), _formulaTask, _circularReferenceChecker);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
      }
   }

   internal class When_selecting_named_Formula_type : concern_for_EditFormulaPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buidingBlockWithFormulaCache;
      private IFormulaCache _formulaCache;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _buidingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _formulaCache = A.Fake<IFormulaCache>();
         A.CallTo(() => _buidingBlockWithFormulaCache.FormulaCache).Returns(_formulaCache);

         IFormula formula = new ExplicitFormula();
         _parameter.Formula = formula;
         A.CallTo(() => _context.Create<ExplicitFormula>()).Returns(A.Fake<ExplicitFormula>());


         sut.Init(_parameter, _buidingBlockWithFormulaCache, new UsingFormulaDecoder());
      }

      protected override void Because()
      {
         sut.FormulaSelectionChanged(string.Empty);
      }

      [Observation]
      public void should_Not_Add_a_formula_to_formula_cache()
      {
         A.CallTo(() => _formulaCache.Add(A<Formula>._)).MustNotHaveHappened();
         A.CallTo(() => _buidingBlockWithFormulaCache.AddFormula(A<Formula>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_add_any_commmand()
      {
         A.CallTo(() => _commandCollector.AddCommand(A<IMoBiCommand>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_clear_the_formula_view()
      {
         A.CallTo(() => _editFormulaView.ClearFormulaView()).MustHaveHappened();
      }
   }

   public class When_selecting_an_constant_formula : concern_for_EditFormulaPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buidingBlockWithFormulaCache;

      protected override void Context()
      {
         base.Context();
         _buidingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _parameter = new Parameter().WithDimension(A.Fake<IDimension>());
         var constantFormula = new ConstantFormula().WithDimension(_parameter.Dimension);
         A.CallTo(() => _formulaTask.CreateNewFormula<ConstantFormula>(_parameter.Dimension)).Returns(constantFormula);
      }

      protected override void Because()
      {
         sut.Init(_parameter, _buidingBlockWithFormulaCache);
      }

      [Observation]
      public void should_create_a_constant_formula_using_the_dimension_of_the_parameter()
      {
         var formula = sut.Subject as ConstantFormula;
         formula.ShouldNotBeNull();
         formula.Dimension.ShouldBeEqualTo(_parameter.Dimension);
      }
   }

   internal class When_adding_a_named_Formula_after_selecting_named_Formula_type : concern_for_EditFormulaPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buidingBlockWithFormulaCache;
      private ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _buidingBlockWithFormulaCache = new ParameterStartValuesBuildingBlock();


         _parameter.Formula = new ExplicitFormula();
         _explicitFormula = new ExplicitFormula {Id = "Formula", Name = "toto"};
         A.CallTo(() => _formulaTask.CreateNewFormula(typeof(ExplicitFormula), _parameter.Dimension)).Returns(_explicitFormula);

         sut.Init(_parameter, _buidingBlockWithFormulaCache);
         sut.FormulaSelectionChanged(string.Empty);

         //add so that it will be found when setting the value in the parameter
         _buidingBlockWithFormulaCache.AddFormula(_explicitFormula);
         A.CallTo(() => _formulaTask.CreateNewFormulaInBuildingBlock(A<Type>._, A<IDimension>._, A<IEnumerable<string>>._, _buidingBlockWithFormulaCache))
            .Returns((A.Fake<IMoBiCommand>(), _explicitFormula));
      }

      protected override void Because()
      {
         sut.AddNewFormula();
      }

      [Observation]
      public void should_Add_a_formula_to_formula_cache()
      {
         A.CallTo(() => _formulaTask.CreateNewFormulaInBuildingBlock(typeof (ExplicitFormula), A<IDimension>._, A<IEnumerable<string>>._, _buidingBlockWithFormulaCache))
            .MustHaveHappened();
      }

      [Observation]
      public void should_change_parents_formula_to_new_formula()
      {
         _parameter.Formula.ShouldBeEqualTo(_explicitFormula);
      }
   }

   public class When_adding_a_new_formula_to_the_edited_building_block : concern_for_EditFormulaPresenter
   {
      private IBuildingBlock _buidingBlockWithFormulaCache;
      private IFormulaCache _formulaCache;
      private IParameter _parameter;
      private IEnumerable<string> _availableFormulaNames;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _buidingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buidingBlockWithFormulaCache.FormulaCache).Returns(_formulaCache);

         _formulaCache.Add(new ExplicitFormula().WithId("A").WithName("A"));
         _formulaCache.Add(new TableFormulaWithOffset().WithId("B").WithName("B"));
         _formulaCache.Add(new SumFormula().WithId("C").WithName("C"));

         A.CallTo(() => _formulaTask.CreateNewFormulaInBuildingBlock(A<Type>._, A<IDimension>._, A<IEnumerable<string>>._, _buidingBlockWithFormulaCache))
            .Invokes(x => _availableFormulaNames = x.GetArgument<IEnumerable<string>>(2))
            .Returns((A.Fake<IMoBiCommand>(), null));


         sut.Init(_parameter, _buidingBlockWithFormulaCache, new UsingFormulaDecoder());
      }

      protected override void Because()
      {
         sut.AddNewFormula();
      }

      [Observation]
      public void should_ensure_that_the_formula_created_as_a_unique_name()
      {
         _availableFormulaNames.ShouldOnlyContain("A", "B", "C");
      }
   }

   public class When_retrieving_the_list_of_all_available_formula_for_the_edited_parameter : concern_for_EditFormulaPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buidingBlockWithFormulaCache;
      private FormulaCache _formulaCache;
      private IDimension _dimEquivalent;
      private IDimension _dimParameter;
      private IDimension _anotherDimension;

      protected override void Context()
      {
         base.Context();
         _dimParameter = HelperForSpecs.AmountDimension;
         _dimEquivalent = new Dimension(_dimParameter.BaseRepresentation, "Equivalent", _dimParameter.BaseUnit.Name);
         _anotherDimension = HelperForSpecs.TimeDimension;
         _parameter = A.Fake<IParameter>().WithDimension(_dimParameter);
         _parameter.Formula = new ExplicitFormula("1+2").WithDimension(_dimParameter);
         _buidingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buidingBlockWithFormulaCache.FormulaCache).Returns(_formulaCache);
         _formulaCache.Add(new ExplicitFormula().WithId("B").WithName("B").WithDimension(_dimEquivalent));
         _formulaCache.Add(new ExplicitFormula().WithId("C").WithName("C").WithDimension(_anotherDimension));
         _formulaCache.Add(new ExplicitFormula().WithId("A").WithName("A").WithDimension(_dimParameter));

         sut.Init(_parameter, _buidingBlockWithFormulaCache, new UsingFormulaDecoder());
      }

      [Observation]
      public void should_return_all_available_formula_with_a_dimension_equivalent_to_the_one_of_the_parameter()
      {
         sut.DisplayFormulaNames().ShouldOnlyContainInOrder("A", "B");
      }
   }

   public class When_retrieving_the_list_of_all_available_formula_for_the_edited_rhs_parameter_formula : concern_for_EditFormulaPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buidingBlockWithFormulaCache;
      private FormulaCache _formulaCache;
      private IDimension _dimParameter;
      private IDimension _rhsDim;

      protected override void Context()
      {
         base.Context();
         _dimParameter = HelperForSpecs.AmountDimension;
         _rhsDim = HelperForSpecs.AmountPerTimeDimension;
         _parameter = A.Fake<IParameter>().WithDimension(_dimParameter);
         _parameter.RHSFormula = new ExplicitFormula("1+2").WithDimension(_rhsDim);

         _buidingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buidingBlockWithFormulaCache.FormulaCache).Returns(_formulaCache);
         _formulaCache.Add(new ExplicitFormula().WithId("C").WithName("C").WithDimension(_rhsDim));
         _formulaCache.Add(new ExplicitFormula().WithId("A").WithName("A").WithDimension(_dimParameter));

         sut.IsRHS = true;
         sut.Init(_parameter, _buidingBlockWithFormulaCache);
      }

      [Observation]
      public void should_return_all_available_formula_with_a_dimension_equivalent_to_the_one_of_the_parameter()
      {
         sut.DisplayFormulaNames().ShouldOnlyContainInOrder("C");
      }
   }
}