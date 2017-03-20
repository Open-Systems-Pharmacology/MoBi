using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_MoBiFormulaTask : ContextSpecification<MoBiFormulaTask>
   {
      protected IMoBiContext _context;
      protected IMoBiApplicationController _applicationController;
      protected IFormulaTask _formulaTask;
      protected INameCorrector _nameCorrector;
      protected IBuildingBlock _buildingBlock;
      protected UsingFormulaDecoder _withFormulaDecoder;
      protected IUsingFormula _usingFormulaObject;
      private IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _withFormulaDecoder = new UsingFormulaDecoder();
         _buildingBlock = A.Fake<IBuildingBlock>();
         A.CallTo(() => _buildingBlock.FormulaCache).Returns(new FormulaCache());
         _context = A.Fake<IMoBiContext>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _formulaTask = A.Fake<IFormulaTask>();
         _nameCorrector = A.Fake<INameCorrector>();
         sut = new MoBiFormulaTask(_context, _applicationController, _formulaTask, _nameCorrector, _dialogCreator);

         _usingFormulaObject = A.Fake<IUsingFormula>();
         A.CallTo(() => _usingFormulaObject.Formula).Returns(MvExplicitFormula());
      }

      protected static ExplicitFormula MvExplicitFormula()
      {
         return new ExplicitFormula("M/V") { Name = "MOverV" };
      }

      protected static ExplicitFormula MvExplicitFormula2()
      {
         return new ExplicitFormula("M/V/V") { Name = "MOverV" };
      }


      protected override void Because()
      {
         sut.AddFormulaToCacheOrFixReferenceCommand(_buildingBlock, _usingFormulaObject, _withFormulaDecoder).Execute(_context);
      }
   }

   public class when_finding_equal_formula_not_in_building_block : concern_for_MoBiFormulaTask
   {
      [Observation]
      public void formula_cache_should_contain_new_formula()
      {
         A.CallTo(() => _buildingBlock.AddFormula(_usingFormulaObject.Formula)).MustHaveHappened();
      }
   }

   public class when_adding_formula_to_cache_with_existing_formula_name : concern_for_MoBiFormulaTask
   {
      private IFormula _cachedFormula;
      protected override void Context()
      {
         base.Context();
         _cachedFormula = MvExplicitFormula2();
         _buildingBlock.FormulaCache.Add(_cachedFormula);
         A.CallTo(() => _formulaTask.FormulasAreTheSame(_cachedFormula, _usingFormulaObject.Formula)).Returns(false);
      }

      [Observation]
      public void new_formula_should_not_have_been_added_to_cache()
      {
         A.CallTo(() => _buildingBlock.AddFormula(_usingFormulaObject.Formula)).MustHaveHappened();
      }

      [Observation]
      public void formula_must_have_been_renamed()
      {
         A.CallTo(() => _nameCorrector.AutoCorrectName(A<IEnumerable<string>>._, _usingFormulaObject.Formula)).MustHaveHappened();
      }
   }

   public class when_fixing_references_to_existing_equivalent_formula : concern_for_MoBiFormulaTask
   {
      private IFormula _cachedFormula;
      protected override void Context()
      {
         base.Context();
         _cachedFormula = MvExplicitFormula();
         _buildingBlock.FormulaCache.Add(_cachedFormula);
         A.CallTo(() => _formulaTask.FormulasAreTheSame(_cachedFormula, _usingFormulaObject.Formula)).Returns(true);
      }

      [Observation]
      public void referenced_formula_must_have_changed()
      {
         ReferenceEquals(_usingFormulaObject.Formula, _cachedFormula).ShouldBeTrue();
      }

      [Observation]
      public void new_formula_should_not_have_been_added_to_cache()
      {
         A.CallTo(() => _buildingBlock.AddFormula(_usingFormulaObject.Formula)).MustNotHaveHappened();
      }
   }

   public class When_setting_the_alias_of_a_formula_path : concern_for_MoBiFormulaTask
   {
      private ExplicitFormula _formula;
      private FormulaUsablePath _formulaUsablePath;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula { Id = "id" };
         _formulaUsablePath = new FormulaUsablePath("A", "B").WithAlias("OLD");
         _formula.AddObjectPath(_formulaUsablePath);
         A.CallTo(() => _context.Get<IFormula>(_formula.Id)).Returns(_formula);
      }

      protected override void Because()
      {
         sut.EditAliasInFormula(_formula, "NEW", "OLD", _formulaUsablePath, _buildingBlock);
      }

      [Observation]
      public void should_set_the_alias_in_the_given_formula_path_if_the_alias_is_unique()
      {
         _formula.FormulaUsablePathBy("NEW").ShouldNotBeNull();
      }
   }
}
