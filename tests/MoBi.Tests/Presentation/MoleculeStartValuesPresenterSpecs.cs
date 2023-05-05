using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_MoleculeStartValuesPresenter : ContextSpecification<MoleculeStartValuesPresenter>
   {
      protected IMoleculeStartValuesView _view;
      protected IMoleculeStartValueToMoleculeStartValueDTOMapper _mapper;
      private IMoBiContext _context;
      protected IMoleculeIsPresentSelectionPresenter _isPresentSelectionPresenter;
      protected IMoleculeStartValuesTask _moleculeStartValueTask;
      protected ICommandCollector _commandCollector;
      protected InitialConditionsBuildingBlock _moleculeStartValueBuildingBlock;
      private IRefreshStartValueFromOriginalBuildingBlockPresenter _refreshStartValuesPresenter;
      protected IDeleteStartValuePresenter _deleteStartValuePresenter;
      private IMoleculeNegativeValuesAllowedSelectionPresenter _negativeStartValuesAllowedSelectionPresenter;
      protected ILegendPresenter _legendPresenter;
      protected IInitialConditionsCreator _moleculeStartValuesCreator;
      private IFormulaToValueFormulaDTOMapper _formulaToValueFormulaDTOMapper;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _view = A.Fake<IMoleculeStartValuesView>();
         _mapper = A.Fake<IMoleculeStartValueToMoleculeStartValueDTOMapper>();
         _context = A.Fake<IMoBiContext>();
         _isPresentSelectionPresenter = A.Fake<IMoleculeIsPresentSelectionPresenter>();
         _refreshStartValuesPresenter = A.Fake<IRefreshStartValueFromOriginalBuildingBlockPresenter>();
         _negativeStartValuesAllowedSelectionPresenter = A.Fake<IMoleculeNegativeValuesAllowedSelectionPresenter>();
         _moleculeStartValueTask = A.Fake<IMoleculeStartValuesTask>();
         _commandCollector = A.Fake<ICommandCollector>();
         _deleteStartValuePresenter = A.Fake<IDeleteStartValuePresenter>();
         _legendPresenter = A.Fake<ILegendPresenter>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _moleculeStartValuesCreator = A.Fake<IInitialConditionsCreator>();
         _formulaToValueFormulaDTOMapper = new FormulaToValueFormulaDTOMapper();
         sut = new MoleculeStartValuesPresenter(
            _view, _mapper, _isPresentSelectionPresenter, _refreshStartValuesPresenter, _negativeStartValuesAllowedSelectionPresenter, _moleculeStartValueTask,
            _moleculeStartValuesCreator, _context, _legendPresenter, _deleteStartValuePresenter, _formulaToValueFormulaDTOMapper, _dimensionFactory);
         _moleculeStartValueBuildingBlock = new InitialConditionsBuildingBlock();

         sut.InitializeWith(_commandCollector);
      }
   }

   public abstract class When_filtering : concern_for_MoleculeStartValuesPresenter
   {
      protected MoleculeStartValueDTO _originalUnchanged;
      protected MoleculeStartValueDTO _originalChanged;
      protected MoleculeStartValueDTO _newUnchanged;
      protected MoleculeStartValueDTO _newChanged;

      protected override void Context()
      {
         base.Context();

         _originalUnchanged = new MoleculeStartValueDTO(new InitialCondition().WithName("originalUnchanged"), _moleculeStartValueBuildingBlock);
         _originalChanged = new MoleculeStartValueDTO(new InitialCondition().WithName("originalChanged"), _moleculeStartValueBuildingBlock);
         _newUnchanged = new MoleculeStartValueDTO(new InitialCondition().WithName("newUnchanged"), _moleculeStartValueBuildingBlock);
         _newChanged = new MoleculeStartValueDTO(new InitialCondition().WithName("newChanged"), _moleculeStartValueBuildingBlock);

         _moleculeStartValueBuildingBlock.Add(_originalUnchanged.MoleculeStartValue);
         _moleculeStartValueBuildingBlock.Add(_originalChanged.MoleculeStartValue);

         A.CallTo(() => _moleculeStartValueTask.IsEquivalentToOriginal(_originalUnchanged.PathWithValueObject, _moleculeStartValueBuildingBlock)).Returns(true);
         A.CallTo(() => _moleculeStartValueTask.IsEquivalentToOriginal(_originalChanged.PathWithValueObject, _moleculeStartValueBuildingBlock)).Returns(false);

         A.CallTo(() => _moleculeStartValueTask.IsEquivalentToOriginal(_newUnchanged.PathWithValueObject, _moleculeStartValueBuildingBlock)).Returns(true);
         A.CallTo(() => _moleculeStartValueTask.IsEquivalentToOriginal(_newChanged.PathWithValueObject, _moleculeStartValueBuildingBlock)).Returns(false);

         sut.Edit(_moleculeStartValueBuildingBlock);
      }
   }

   public class When_filtering_for_changed_and_modified_values : When_filtering
   {
      protected override void Because()
      {
         sut.IsModifiedFilterOn = true;
         sut.IsNewFilterOn = true;
      }

      [Observation]
      public void only_changed_values_should_be_shown()
      {
         sut.ShouldShow(_originalChanged).ShouldBeFalse();
         sut.ShouldShow(_originalUnchanged).ShouldBeFalse();

         sut.ShouldShow(_newChanged).ShouldBeTrue();
         sut.ShouldShow(_newUnchanged).ShouldBeFalse();
      }
   }

   public class When_filtering_for_only_modified_values : When_filtering
   {
      protected override void Because()
      {
         sut.IsModifiedFilterOn = true;
      }

      [Observation]
      public void only_changed_values_should_be_shown()
      {
         sut.ShouldShow(_originalChanged).ShouldBeTrue();
         sut.ShouldShow(_originalUnchanged).ShouldBeFalse();

         sut.ShouldShow(_newChanged).ShouldBeTrue();
         sut.ShouldShow(_newUnchanged).ShouldBeFalse();
      }
   }

   public class When_filtering_for_only_changed_values : When_filtering
   {
      protected override void Because()
      {
         sut.IsNewFilterOn = true;
      }

      [Observation]
      public void only_new_values_should_be_shown()
      {
         sut.ShouldShow(_originalChanged).ShouldBeFalse();
         sut.ShouldShow(_originalUnchanged).ShouldBeFalse();

         sut.ShouldShow(_newChanged).ShouldBeTrue();
         sut.ShouldShow(_newUnchanged).ShouldBeTrue();
      }
   }

   public class When_filtering_for_only_new_values : When_filtering
   {
      protected override void Because()
      {
         sut.IsNewFilterOn = true;
      }

      [Observation]
      public void only_new_values_should_be_shown()
      {
         sut.ShouldShow(_originalChanged).ShouldBeFalse();
         sut.ShouldShow(_originalUnchanged).ShouldBeFalse();

         sut.ShouldShow(_newChanged).ShouldBeTrue();
         sut.ShouldShow(_newUnchanged).ShouldBeTrue();
      }
   }

   public class When_adding_a_new_empty_start_value : concern_for_MoleculeStartValuesPresenter
   {
      private IDimension _defaultDimension;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_moleculeStartValueBuildingBlock);
         _defaultDimension = A.Fake<IDimension>();
         A.CallTo(() => _moleculeStartValueTask.GetDefaultDimension()).Returns(_defaultDimension);
      }

      protected override void Because()
      {
         sut.AddNewEmptyStartValue();
      }

      [Observation]
      public void the_start_value_creator_should_create_the_empty_start_value()
      {
         A.CallTo(() => _moleculeStartValuesCreator.CreateEmptyStartValue(_moleculeStartValueTask.GetDefaultDimension())).MustHaveHappened();
      }
   }

   public class When_determining_the_background_color_of_a_start_value : concern_for_MoleculeStartValuesPresenter
   {
      private MoleculeStartValueDTO _originalResolvableUnchangedStartValueDTO;
      private MoleculeStartValueDTO _originalResolvableChangedStartValueDTO;
      private MoleculeStartValueDTO _originalUnresolvableStartValueDTO;
      private MoleculeStartValueDTO _nonOriginalStartValueDTO;

      protected override void Context()
      {
         base.Context();
         _originalResolvableUnchangedStartValueDTO = new MoleculeStartValueDTO(new InitialCondition().WithName("originalResolvableUnchanged"), _moleculeStartValueBuildingBlock);
         _originalResolvableChangedStartValueDTO = new MoleculeStartValueDTO(new InitialCondition().WithName("originalResolvableChanged"), _moleculeStartValueBuildingBlock);
         _originalUnresolvableStartValueDTO = new MoleculeStartValueDTO(new InitialCondition().WithName("originalUnresolvable"), _moleculeStartValueBuildingBlock);
         _nonOriginalStartValueDTO = new MoleculeStartValueDTO(new InitialCondition().WithName("nonOriginal"), _moleculeStartValueBuildingBlock);
         _moleculeStartValueBuildingBlock.Add(_originalResolvableUnchangedStartValueDTO.MoleculeStartValue);
         _moleculeStartValueBuildingBlock.Add(_originalResolvableChangedStartValueDTO.MoleculeStartValue);
         _moleculeStartValueBuildingBlock.Add(_originalUnresolvableStartValueDTO.MoleculeStartValue);

         A.CallTo(() => _moleculeStartValueTask.CanResolve(_moleculeStartValueBuildingBlock, _originalResolvableUnchangedStartValueDTO.MoleculeStartValue)).Returns(true);
         A.CallTo(() => _moleculeStartValueTask.IsEquivalentToOriginal(_originalResolvableUnchangedStartValueDTO.MoleculeStartValue, _moleculeStartValueBuildingBlock)).Returns(true);

         A.CallTo(() => _moleculeStartValueTask.CanResolve(_moleculeStartValueBuildingBlock, _originalResolvableChangedStartValueDTO.MoleculeStartValue)).Returns(true);
         A.CallTo(() => _moleculeStartValueTask.IsEquivalentToOriginal(_originalResolvableChangedStartValueDTO.MoleculeStartValue, _moleculeStartValueBuildingBlock)).Returns(false);

         A.CallTo(() => _moleculeStartValueTask.CanResolve(_moleculeStartValueBuildingBlock, _originalUnresolvableStartValueDTO.MoleculeStartValue)).Returns(false);
      }

      protected override void Because()
      {
         sut.Edit(_moleculeStartValueBuildingBlock);
      }

      [Observation]
      public void the_background_colors_should_be_correct()
      {
         sut.BackgroundColorRetriever(_originalResolvableUnchangedStartValueDTO).ShouldBeEqualTo(MoBiColors.Default);
         sut.BackgroundColorRetriever(_originalResolvableChangedStartValueDTO).ShouldBeEqualTo(MoBiColors.Modified);
         sut.BackgroundColorRetriever(_originalUnresolvableStartValueDTO).ShouldBeEqualTo(MoBiColors.CannotResolve);
         sut.BackgroundColorRetriever(_nonOriginalStartValueDTO).ShouldBeEqualTo(MoBiColors.Extended);
      }
   }

   public class When_the_molecule_start_values_presenter_is_constructed : concern_for_MoleculeStartValuesPresenter
   {
      [Observation]
      public void the_legend_view_is_added_to_the_view()
      {
         A.CallTo(() => _view.AddLegendView(_legendPresenter.View)).MustHaveHappened();
      }

      [Observation]
      public void the_legend_presenter_adds_three_legend_items_to_the_view()
      {
         A.CallTo(() => _legendPresenter.AddLegendItems(A<IReadOnlyList<LegendItemDTO>>.That.Matches(x => x.Count == 3))).MustHaveHappened();
      }
   }

   public class When_molecule_start_values_presenter_is_used_to_extend_molecule_start_values : concern_for_MoleculeStartValuesPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_moleculeStartValueBuildingBlock);
      }

      protected override void Because()
      {
         sut.ExtendStartValues();
      }

      [Observation]
      public void the_molecule_start_values_task_should_be_used_to_extend_the_building_block()
      {
         A.CallTo(() => _moleculeStartValueTask.ExtendStartValues(_moleculeStartValueBuildingBlock)).MustHaveHappened();
      }
   }

   internal abstract class When_deleting_start_values_from_building_block : concern_for_MoleculeStartValuesPresenter
   {
      protected InitialCondition _startValue1;
      protected InitialCondition _startValue2;
      protected InitialCondition _startValue3;

      protected override void Context()
      {
         base.Context();
         _startValue1 = new InitialCondition { Name = "startValue1" };
         _startValue2 = new InitialCondition { Name = "startValue2" };
         _startValue3 = new InitialCondition { Name = "startValue3" };
         _moleculeStartValueBuildingBlock.Add(_startValue1);
         _moleculeStartValueBuildingBlock.Add(_startValue2);
         _moleculeStartValueBuildingBlock.Add(_startValue3);

         A.CallTo(() => _mapper.MapFrom(_startValue1, _moleculeStartValueBuildingBlock)).Returns(new MoleculeStartValueDTO(_startValue1, _moleculeStartValueBuildingBlock));
         A.CallTo(() => _mapper.MapFrom(_startValue2, _moleculeStartValueBuildingBlock)).Returns(new MoleculeStartValueDTO(_startValue2, _moleculeStartValueBuildingBlock));
         A.CallTo(() => _mapper.MapFrom(_startValue3, _moleculeStartValueBuildingBlock)).Returns(new MoleculeStartValueDTO(_startValue3, _moleculeStartValueBuildingBlock));

         sut.Edit(_moleculeStartValueBuildingBlock);
      }

      [Observation]
      public void should_remove_the_selected_start_values_from_the_building_block()
      {
         A.CallTo(() => _moleculeStartValueTask.RemoveStartValueFromBuildingBlockCommand(_startValue1, _moleculeStartValueBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _moleculeStartValueTask.RemoveStartValueFromBuildingBlockCommand(_startValue2, _moleculeStartValueBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _moleculeStartValueTask.RemoveStartValueFromBuildingBlockCommand(_startValue3, _moleculeStartValueBuildingBlock)).MustNotHaveHappened();
      }
   }

   internal class When_deleting_unresolved_start_values_from_building_block : When_deleting_start_values_from_building_block
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _moleculeStartValueTask.CanResolve(_moleculeStartValueBuildingBlock, _startValue1)).Returns(false);
         A.CallTo(() => _moleculeStartValueTask.CanResolve(_moleculeStartValueBuildingBlock, _startValue2)).Returns(false);
         A.CallTo(() => _moleculeStartValueTask.CanResolve(_moleculeStartValueBuildingBlock, _startValue3)).Returns(true);
      }

      protected override void Because()
      {
         _deleteStartValuePresenter.ApplySelectionAction(SelectOption.DeleteSourceNotDefined);
      }
   }

   internal class When_deleting_selected_start_values_from_building_block : When_deleting_start_values_from_building_block
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.SelectedStartValues).Returns(new[]
         {
            new MoleculeStartValueDTO(_startValue1, _moleculeStartValueBuildingBlock),
            new MoleculeStartValueDTO(_startValue2, _moleculeStartValueBuildingBlock)
         });
      }

      protected override void Because()
      {
         _deleteStartValuePresenter.ApplySelectionAction(SelectOption.DeleteSelected);
      }
   }

   internal class When_changing_name_of_start_value_not_in_building_block : concern_for_MoleculeStartValuesPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_moleculeStartValueBuildingBlock);
      }

      protected override void Because()
      {
         sut.UpdateStartValueName(new MoleculeStartValueDTO(new InitialCondition { Path = new ObjectPath("one", "two", "C1") }, new InitialConditionsBuildingBlock()), "C2");
      }

      [Observation]
      public void results_in_call_to_task_add_new_start_value_to_building_block()
      {
         A.CallTo(() => _moleculeStartValueTask.AddStartValueToBuildingBlock(
            _moleculeStartValueBuildingBlock, A<InitialCondition>.That.Matches(x => x.Path.Last().Equals("C2")))).MustHaveHappened();
      }
   }

   internal class When_changing_the_value_description_of_a_start_value : concern_for_MoleculeStartValuesPresenter
   {
      private InitialCondition _startValue;
      private ValueOrigin _valueOrigin;

      protected override void Context()
      {
         base.Context();
         _valueOrigin = new ValueOrigin();
         _startValue = new InitialCondition { Name = "startValue" };
         _moleculeStartValueBuildingBlock.Add(_startValue);
         sut.Edit(_moleculeStartValueBuildingBlock);
      }

      protected override void Because()
      {
         sut.SetValueOrigin(new MoleculeStartValueDTO(_startValue, _moleculeStartValueBuildingBlock), _valueOrigin);
      }

      [Observation]
      public void should_updae_the_value_description_in_the_start_value()
      {
         A.CallTo(() => _moleculeStartValueTask.SetValueOrigin(_moleculeStartValueBuildingBlock, _valueOrigin, _startValue)).MustHaveHappened();
      }
   }

   internal class When_GetFormulas_is_called_at_msv_presenter : concern_for_MoleculeStartValuesPresenter
   {
      private IEnumerable<ValueFormulaDTO> _resultFormulas;
      private IFormula _explicitFormula;

      protected override void Because()
      {
         _explicitFormula = new ExplicitFormula("1+2").WithName("Test");
         _moleculeStartValueBuildingBlock.AddFormula(_explicitFormula);
         sut.Edit(_moleculeStartValueBuildingBlock);
         _resultFormulas = sut.AllFormulas();
      }

      [Observation]
      public void The_returned_list_Should_contain_null_formula()
      {
         var nullFormula =
            _resultFormulas.Where(dto => dto.Formula == null)
               .Where(dto => dto.FormulaString == AppConstants.Captions.FormulaNotAvailable)
               .FirstOrDefault();
         nullFormula.ShouldNotBeNull();
      }

      [Observation]
      public void The_returned_list_Should_contain_explizit_formula_from_cache()
      {
         var explicitFormula = _resultFormulas.Where(dto => _explicitFormula.Equals(dto.Formula)).FirstOrDefault();
         explicitFormula.ShouldNotBeNull();
      }
   }

   public class When_setting_all_molecule_start_values_to_is_present : concern_for_MoleculeStartValuesPresenter
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _command = A.Fake<IMoBiCommand>();
         A.CallTo(() => _moleculeStartValueTask.SetIsPresent(_moleculeStartValueBuildingBlock, A<IEnumerable<InitialCondition>>._, true)).Returns(_command);
         sut.Edit(_moleculeStartValueBuildingBlock);
      }

      protected override void Because()
      {
         _isPresentSelectionPresenter.ApplySelectionAction(SelectOption.AllPresent);
      }

      [Observation]
      public void should_add_the_resulting_command_to_the_command_manager()
      {
         A.CallTo(() => _commandCollector.AddCommand(_command)).MustHaveHappened();
      }
   }

   public class When_setting_all_molecule_start_values_to_not_is_present : concern_for_MoleculeStartValuesPresenter
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _command = A.Fake<IMoBiCommand>();
         A.CallTo(() => _moleculeStartValueTask.SetIsPresent(_moleculeStartValueBuildingBlock, A<IEnumerable<InitialCondition>>._, false)).Returns(_command);
         sut.Edit(_moleculeStartValueBuildingBlock);
      }

      protected override void Because()
      {
         _isPresentSelectionPresenter.ApplySelectionAction(SelectOption.AllNotPresent);
      }

      [Observation]
      public void should_add_the_resulting_command_to_the_command_manager()
      {
         A.CallTo(() => _commandCollector.AddCommand(_command)).MustHaveHappened();
      }
   }

   public class When_performing_a_bulk_update_of_molecule_start_values : concern_for_MoleculeStartValuesPresenter
   {
      private InitialCondition _msv1;
      private MoleculeStartValueDTO _msv1DTOFirst;
      private MoleculeStartValueDTO _msv1DTOSecond;

      protected override void Context()
      {
         base.Context();
         _msv1 = new InitialCondition();
         _msv1DTOFirst = new MoleculeStartValueDTO(_msv1, _moleculeStartValueBuildingBlock);
         _msv1DTOSecond = new MoleculeStartValueDTO(_msv1, _moleculeStartValueBuildingBlock);
         A.CallTo(() => _mapper.MapFrom(_msv1, _moleculeStartValueBuildingBlock))
            .ReturnsNextFromSequence(_msv1DTOFirst, _msv1DTOSecond);

         _moleculeStartValueBuildingBlock.Add(_msv1);
         sut.Edit(_moleculeStartValueBuildingBlock);
         _view.FocusedStartValue = _msv1DTOFirst;
      }

      protected override void Because()
      {
         sut.Handle(new BulkUpdateStartedEvent());
         sut.Handle(new BulkUpdateFinishedEvent());
      }

      [Observation]
      public void should_save_the_currently_focused_start_values_and_set_it_back_to_the_view()
      {
         _view.FocusedStartValue.ShouldBeEqualTo(_msv1DTOSecond);
      }
   }
}