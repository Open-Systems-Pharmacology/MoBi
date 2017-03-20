using System;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.Presenter;
using NUnit.Framework;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation
{
   public abstract class concern_for_FormulaPresenterCache : ContextSpecification<IFormulaPresenterCache>
   {
      private IContainer _container;

      protected override void Context()
      {
         _container= A.Fake<IContainer>();
         sut = new FormulaPresenterCache(_container);
      }
   }

   public class When_retrieving_the_formula_presenter_for_a_given_formula : concern_for_FormulaPresenterCache
   {
      [TestCase(typeof(ConstantFormula), typeof(IEditConstantFormulaPresenter))]
      [TestCase(typeof(ExplicitFormula), typeof(IEditExplicitFormulaPresenter))]
      [TestCase(typeof(BlackBoxFormula), typeof(IEditBlackBoxFormulaPresenter))]
      [TestCase(typeof(TableFormula), typeof(IEditTableFormulaPresenter))]
      [TestCase(typeof(TableFormulaWithOffset), typeof(IEditTableFormulaWithOffSetFormulaPresenter))]
      [TestCase(typeof(SumFormula), typeof(IEditSumFormulaPresenter))]
      public void should_return_the_expected_presenter_type(Type formulaType, Type expectedPresenterType)
      {
         sut.PresenterFor(formulaType).IsAnImplementationOf(expectedPresenterType).ShouldBeTrue();
      }
   }
}	