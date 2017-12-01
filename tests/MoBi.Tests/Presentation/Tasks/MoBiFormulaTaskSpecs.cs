using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

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
      protected IDialogCreator _dialogCreator;

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
         _usingFormulaObject.Formula = MvExplicitFormula();
      }

      protected static ExplicitFormula MvExplicitFormula() => new ExplicitFormula("M/V") { Name = "MOverV" };

      protected static ExplicitFormula MvExplicitFormula2() => new ExplicitFormula("M/V/V") { Name = "MOverV" };
   }

   public class When_the_formulat_task_is_editing_a_new_formula : concern_for_MoBiFormulaTask
   {
      private IParameter _parameter;
      private IFormula _formula;
      private ICommandCollector _commandCollector;
      private INewFormulaPresenter _editNewFormulaPresenter;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _commandCollector= A.Fake<ICommandCollector>();
         _formula= A.Fake<IFormula>();
         _parameter= A.Fake<IParameter>();
         _editNewFormulaPresenter= A.Fake<INewFormulaPresenter>();
         A.CallTo(() => _applicationController.Start<INewFormulaPresenter>()).Returns(_editNewFormulaPresenter);
         A.CallTo(_editNewFormulaPresenter).WithReturnType<bool>().Returns(true);

      }
      protected override void Because()
      {
         _result = sut.EditNewFormula(_formula, _commandCollector, _buildingBlock, _parameter);
      }  

      [Observation]
      public void should_retrieve_a_new_formula_presenter_and_edit_the_new_formula()
      {
         A.CallTo(() => _editNewFormulaPresenter.InitializeWith(_commandCollector)).MustHaveHappened();
         A.CallTo(() => _editNewFormulaPresenter.Edit(_formula, _buildingBlock, _parameter)).MustHaveHappened();
      }

      [Observation]
      public void should_return_the_result_of_the_formula_editing_action()
      {
         _result.ShouldBeEqualTo(true);
      }
   }

   public class When_creating_a_new_mobi_formula_for_a_given_building_block_and_the_user_cancels_the_action : concern_for_MoBiFormulaTask
   {
      private IDimension _formulaDimension;
      private IEnumerable<string> _existingNames;
      private IMoBiCommand _command;
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formulaDimension = DomainHelperForSpecs.ConcentrationDimension;
         _existingNames = new List<string>();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(null);
      }

      protected override void Because()
      {
         (_command, _formula) = sut.CreateNewFormulaInBuildingBlock(typeof(ConstantFormula), _formulaDimension, _existingNames, _buildingBlock);
      }

      [Observation]
      public void should_return_a_null_formula()
      {
         _formula.ShouldBeNull();
      }

      [Observation]
      public void should_return_an_empty_command()
      {
         _command.IsEmpty().ShouldBeTrue();
      }
   }

   public class When_creating_a_new_mobi_formula_for_a_given_building_block_and_the_user_enters_a_valid_name_for_the_new_formula : concern_for_MoBiFormulaTask
   {
      private IDimension _formulaDimension;
      private IEnumerable<string> _existingNames;
      private IMoBiCommand _command;
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formulaDimension = DomainHelperForSpecs.ConcentrationDimension;
         _existingNames = new List<string>();
         A.CallTo(() => _dialogCreator.AskForInput(AppConstants.Captions.NewName, AppConstants.Captions.EnterNewFormulaName, string.Empty, _existingNames, null)).Returns("FORMULA");
      }

      protected override void Because()
      {
         (_command, _formula) = sut.CreateNewFormulaInBuildingBlock(typeof(ConstantFormula), _formulaDimension, _existingNames, _buildingBlock);
      }

      [Observation]
      public void should_return_a_formula_haveing_the_expected_type()
      {
         _formula.ShouldBeAnInstanceOf<ConstantFormula>();
      }

      [Observation]
      public void should_have_set_the_name_of_the_formula_to_the_user_input()
      {
         _formula.Name.ShouldBeEqualTo("FORMULA");
      }

      [Observation]
      public void should_have_set_the_dimension_of_the_formula_to_the_provided_dimension()
      {
         _formula.Dimension.ShouldBeEqualTo(_formulaDimension);
      }

      [Observation]
      public void should_add_the_formula_to_the_building_block_formula_cache_and_return_the_resulting_command()
      {
         _command.ShouldBeAnInstanceOf<AddFormulaToFormulaCacheCommand>();
      }
   }

   public class When_the_mobi_formula_task_is_creating_formula_for_a_given_type_and_dimension : concern_for_MoBiFormulaTask
   {
      private IDimension _formulaDimension;
      private IDimension _timeDimension;

      protected override void Context()
      {
         base.Context();
         _formulaDimension = DomainHelperForSpecs.ConcentrationDimension;
         _timeDimension = DomainHelperForSpecs.TimeDimension;
         A.CallTo(() => _context.DimensionFactory.Dimension(Constants.Dimension.TIME)).Returns(_timeDimension);
         A.CallTo(() => _context.Create<TableFormula>()).Returns(new TableFormula());
      }

      [Observation]
      public void should_return_a_constant_formula_for_constant_formula_type()
      {
         validateFormula<ConstantFormula>();
      }

      [Observation]
      public void should_return_a_black_box_formula_for_black_box_formula_type()
      {
         validateFormula<BlackBoxFormula>();
      }

      [Observation]
      public void should_return_a_explicit_formula_for_explicit_formula_type()
      {
         validateFormula<ExplicitFormula>();
      }

      [Observation]
      public void should_return_a_explicit_formula_for_an_unknow_formula_type()
      {
         var formula = sut.CreateNewFormula<IFormula>(_formulaDimension);
         formula.ShouldBeAnInstanceOf<ExplicitFormula>();
      }

      [Observation]
      public void should_return_a_table_formula_with_default_properties_for_table_formula_type()
      {
         var tableFormula = validateFormula<TableFormula>();
         tableFormula.UseDerivedValues.ShouldBeEqualTo(false);
         tableFormula.XDimension.ShouldBeEqualTo(_timeDimension);
         tableFormula.XName.ShouldBeEqualTo(_timeDimension.DisplayName);
         tableFormula.YName.ShouldBeEqualTo(AppConstants.Captions.DisplayNameYValue);
         tableFormula.AllPoints().Count().ShouldBeEqualTo(1);
      }


      [Observation]
      public void should_return_a_table_formula_with_off_set_and_default_properties_for_table_formula_with_offset_type()
      {
         var tableFormulaWithOffset = validateFormula<TableFormulaWithOffset>();
         tableFormulaWithOffset.OffsetObjectAlias.ShouldBeEqualTo(AppConstants.OffsetAlias);
         tableFormulaWithOffset.TableObjectAlias.ShouldBeEqualTo(AppConstants.TableAlias);
      }

      [Observation]
      public void should_return_a_sum_formula_for_sum_formula_type()
      {
         validateFormula<SumFormula>();
      }

      private T validateFormula<T>() where T : IFormula
      {
         var formula = sut.CreateNewFormula<T>(_formulaDimension);
         formula.ShouldBeAnInstanceOf<T>();
         formula.Dimension.ShouldBeEqualTo(_formulaDimension);
         return formula;
      }
   }

   public class When_finding_equal_formula_not_in_building_block : concern_for_MoBiFormulaTask
   {
      protected override void Because()
      {
         sut.AddFormulaToCacheOrFixReferenceCommand(_buildingBlock, _usingFormulaObject, _withFormulaDecoder).Execute(_context);
      }

      [Observation]
      public void formula_cache_should_contain_new_formula()
      {
         A.CallTo(() => _buildingBlock.AddFormula(_usingFormulaObject.Formula)).MustHaveHappened();
      }
   }

   public class When_adding_formula_to_cache_with_existing_formula_name : concern_for_MoBiFormulaTask
   {
      private IFormula _cachedFormula;

      protected override void Context()
      {
         base.Context();
         _cachedFormula = MvExplicitFormula2();
         _buildingBlock.FormulaCache.Add(_cachedFormula);
         A.CallTo(() => _formulaTask.FormulasAreTheSame(_cachedFormula, _usingFormulaObject.Formula)).Returns(false);
      }

      protected override void Because()
      {
         sut.AddFormulaToCacheOrFixReferenceCommand(_buildingBlock, _usingFormulaObject, _withFormulaDecoder).Execute(_context);
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

   public class When_fixing_references_to_existing_equivalent_formula : concern_for_MoBiFormulaTask
   {
      private IFormula _cachedFormula;
      protected override void Context()
      {
         base.Context();
         _cachedFormula = MvExplicitFormula();
         _buildingBlock.FormulaCache.Add(_cachedFormula);
         A.CallTo(() => _formulaTask.FormulasAreTheSame(_cachedFormula, _usingFormulaObject.Formula)).Returns(true);
      }

      protected override void Because()
      {
         sut.AddFormulaToCacheOrFixReferenceCommand(_buildingBlock, _usingFormulaObject, _withFormulaDecoder).Execute(_context);
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
