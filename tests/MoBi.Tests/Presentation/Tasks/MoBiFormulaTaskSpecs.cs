using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
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
      protected IQuantityTask _quantityTask;
      protected IEntitiesInBuildingBlockRetriever<IParameter> _parametersInBuildingBlockRetriever;

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
         _quantityTask = A.Fake<IQuantityTask>();
         _parametersInBuildingBlockRetriever = A.Fake<IEntitiesInBuildingBlockRetriever<IParameter>>();

         sut = new MoBiFormulaTask(_context, _applicationController, _formulaTask, _nameCorrector, _dialogCreator, _quantityTask, _parametersInBuildingBlockRetriever);

         _usingFormulaObject = A.Fake<IUsingFormula>();
         _usingFormulaObject.Formula = MvExplicitFormula();
      }

      protected static ExplicitFormula MvExplicitFormula() => new ExplicitFormula("M/V") {Name = "MOverV"};

      protected static ExplicitFormula MvExplicitFormula2() => new ExplicitFormula("M/V/V") {Name = "MOverV"};
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
         _commandCollector = A.Fake<ICommandCollector>();
         _formula = A.Fake<IFormula>();
         _parameter = A.Fake<IParameter>();
         _editNewFormulaPresenter = A.Fake<INewFormulaPresenter>();
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
         tableFormulaWithOffset.OffsetObjectAlias.ShouldBeEqualTo(AppConstants.OFFSET_ALIAS);
         tableFormulaWithOffset.TableObjectAlias.ShouldBeEqualTo(AppConstants.TABLE_ALIAS);
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
         _formula = new ExplicitFormula {Id = "id"};
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

   public class When_adding_a_formula_path_to_an_explicit_formula : concern_for_MoBiFormulaTask
   {
      private ExplicitFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("1+2");
         _formula.AddObjectPath(new FormulaUsablePath());
      }

      protected override void Because()
      {
         sut.AddFormulaUsablePath(_formula, new FormulaUsablePath(), _buildingBlock);
      }

      [Observation]
      public void a_new_empty_path_is_created_in_the_formula()
      {
         _formula.ObjectPaths.Count.ShouldBeEqualTo(2);
         string.IsNullOrEmpty(_formula.ObjectPaths[1].PathAsString).ShouldBeTrue();
      }
   }

   public class When_retrieving_the_formula_caption : concern_for_MoBiFormulaTask
   {
      [Observation]
      public void should_return_the_expected_caption_for_a_transport_builder()
      {
         sut.GetFormulaCaption(A.Fake<ITransportBuilder>(), ReactionDimensionMode.ConcentrationBased, true).ShouldBeEqualTo(AppConstants.Captions.AmountRightHandSide);
      }

      [Observation]
      public void should_return_the_expected_caption_for_a_parameter_with_RHS()
      {
         var parameter = new Parameter().WithName("P");
         sut.GetFormulaCaption(parameter, ReactionDimensionMode.AmountBased, true).ShouldBeEqualTo(AppConstants.Captions.ParameterRightHandSide(parameter.Name));
      }

      [Observation]
      public void should_return_an_amount_based_caption_for_a_reaction_in_amount_base_mode()
      {
         sut.GetFormulaCaption(A.Fake<IReactionBuilder>(), ReactionDimensionMode.AmountBased, true).ShouldBeEqualTo(AppConstants.Captions.AmountRightHandSide);
      }

      [Observation]
      public void should_return_a_concentration_based_caption_for_a_reaction_in_amount_base_mode()
      {
         sut.GetFormulaCaption(A.Fake<IReactionBuilder>(), ReactionDimensionMode.ConcentrationBased, false).ShouldBeEqualTo(AppConstants.Captions.ConcentrationRightHandSide);
      }
   }

   public class When_updating_the_formula_of_an_explicit_formula_used_by_some_parameters : concern_for_MoBiFormulaTask
   {
      private ExplicitFormula _formula;
      private IParameter _parameter1;
      private IParameter _parameter2;
      private MoBiMacroCommand _command;
      private ICommand _updateCommand1;
      private ICommand _updateCommand2;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("1+3");
         _parameter1 = new Parameter().WithName("P1");
         _parameter2 = new Parameter().WithName("P1");

         A.CallTo(() => _parametersInBuildingBlockRetriever.AllFrom(_buildingBlock, A<Func<IParameter, bool>>._)).Returns(new[] {_parameter1, _parameter2});
         _updateCommand1 = A.Fake<IMoBiCommand>();
         A.CallTo(() => _quantityTask.UpdateDefaultStateAndValueOriginFor(_parameter1, _buildingBlock)).Returns(_updateCommand1);

         _updateCommand2 = new MoBiEmptyCommand();
         A.CallTo(() => _quantityTask.UpdateDefaultStateAndValueOriginFor(_parameter2, _buildingBlock)).Returns(_updateCommand2);
      }

      protected override void Because()
      {
         _command = sut.SetFormulaString(_formula, "1+2", _buildingBlock).DowncastTo<MoBiMacroCommand>();
      }

      [Observation]
      public void should_update_the_default_state_and_value_origin_of_parameters_using_the_formula_by_leverage_the_quantity_task_command()
      {
         _command.IsEmtpy.ShouldBeFalse();
         _command.All().ShouldContain(_updateCommand1);
      }

      [Observation]
      public void should_not_add_command_for_parameters_for_which_no_update_command_was_performed()
      {
         _command.All().ShouldNotContain(_updateCommand2);
      }
   }

   public class When_updating_the_constant_formula_of_a_formula_used_by_one_non_default_parameters : concern_for_MoBiFormulaTask
   {
      private ConstantFormula _formula;
      private IParameter _parameter;
      private IMoBiCommand _command;
      private ICommand _updateCommand;

      protected override void Context()
      {
         base.Context();
         _formula = new ConstantFormula(1);
         _parameter = new Parameter().WithName("P1");
         _parameter.Dimension = DomainHelperForSpecs.AmountDimension;
         _parameter.DisplayUnit = _parameter.Dimension.DefaultUnit;
         _parameter.IsDefault = false;

         A.CallTo(() => _parametersInBuildingBlockRetriever.AllFrom(_buildingBlock, A<Func<IParameter, bool>>._)).Returns(new[] {_parameter});
         _updateCommand = new MoBiEmptyCommand();
         A.CallTo(() => _quantityTask.UpdateDefaultStateAndValueOriginFor(_parameter, _buildingBlock)).Returns(_updateCommand);
      }

      protected override void Because()
      {
         _command = sut.SetConstantFormulaValue(_formula, 4, _parameter.DisplayUnit, _parameter.DisplayUnit, _buildingBlock, _parameter);
      }

      [Observation]
      public void should_not_return_any_special_command_to_update_the_parameter_and_instead_should_only_return_the_update_formula_command()
      {
         _command.ShouldBeAnInstanceOf<SetConstantFormulaValueCommand>();
      }
   }
}