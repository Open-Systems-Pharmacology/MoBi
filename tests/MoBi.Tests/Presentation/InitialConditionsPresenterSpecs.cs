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
   public abstract class concern_for_InitialConditionsPresenter : ContextSpecification<InitialConditionsPresenter>
   {
      protected IInitialConditionsView _view;
      protected IInitialConditionToInitialConditionDTOMapper _mapper;
      private IMoBiContext _context;
      protected IMoleculeIsPresentSelectionPresenter _isPresentSelectionPresenter;
      protected IInitialConditionsTask _initialConditionTask;
      protected ICommandCollector _commandCollector;
      protected InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      protected IDeleteStartValuePresenter _deleteStartValuePresenter;
      private IMoleculeNegativeValuesAllowedSelectionPresenter _negativeStartValuesAllowedSelectionPresenter;
      protected IInitialConditionsCreator _initialConditionsCreator;
      private IFormulaToValueFormulaDTOMapper _formulaToValueFormulaDTOMapper;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _view = A.Fake<IInitialConditionsView>();
         _mapper = A.Fake<IInitialConditionToInitialConditionDTOMapper>();
         _context = A.Fake<IMoBiContext>();
         _isPresentSelectionPresenter = A.Fake<IMoleculeIsPresentSelectionPresenter>();
         _negativeStartValuesAllowedSelectionPresenter = A.Fake<IMoleculeNegativeValuesAllowedSelectionPresenter>();
         _initialConditionTask = A.Fake<IInitialConditionsTask>();
         _commandCollector = A.Fake<ICommandCollector>();
         _deleteStartValuePresenter = A.Fake<IDeleteStartValuePresenter>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _initialConditionsCreator = A.Fake<IInitialConditionsCreator>();
         _formulaToValueFormulaDTOMapper = new FormulaToValueFormulaDTOMapper();
         sut = new InitialConditionsPresenter(
            _view, _mapper, _isPresentSelectionPresenter, _negativeStartValuesAllowedSelectionPresenter, _initialConditionTask,
            _initialConditionsCreator, _context, _deleteStartValuePresenter, _formulaToValueFormulaDTOMapper, _dimensionFactory);
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();

         sut.InitializeWith(_commandCollector);
      }
   }


   public class When_adding_a_new_empty_start_value : concern_for_InitialConditionsPresenter
   {
      private IDimension _defaultDimension;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_initialConditionsBuildingBlock);
         _defaultDimension = A.Fake<IDimension>();
         A.CallTo(() => _initialConditionTask.GetDefaultDimension()).Returns(_defaultDimension);
      }

      protected override void Because()
      {
         sut.AddNewEmptyStartValue();
      }

      [Observation]
      public void the_start_value_creator_should_create_the_empty_start_value()
      {
         A.CallTo(() => _initialConditionsCreator.CreateEmptyStartValue(_initialConditionTask.GetDefaultDimension())).MustHaveHappened();
      }
   }

   internal abstract class When_deleting_start_values_from_building_block : concern_for_InitialConditionsPresenter
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
         _initialConditionsBuildingBlock.Add(_startValue1);
         _initialConditionsBuildingBlock.Add(_startValue2);
         _initialConditionsBuildingBlock.Add(_startValue3);

         A.CallTo(() => _mapper.MapFrom(_startValue1, _initialConditionsBuildingBlock)).Returns(new InitialConditionDTO(_startValue1, _initialConditionsBuildingBlock));
         A.CallTo(() => _mapper.MapFrom(_startValue2, _initialConditionsBuildingBlock)).Returns(new InitialConditionDTO(_startValue2, _initialConditionsBuildingBlock));
         A.CallTo(() => _mapper.MapFrom(_startValue3, _initialConditionsBuildingBlock)).Returns(new InitialConditionDTO(_startValue3, _initialConditionsBuildingBlock));

         sut.Edit(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void should_remove_the_selected_start_values_from_the_building_block()
      {
         A.CallTo(() => _initialConditionTask.RemovePathAndValueEntityFromBuildingBlockCommand(_startValue1, _initialConditionsBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _initialConditionTask.RemovePathAndValueEntityFromBuildingBlockCommand(_startValue2, _initialConditionsBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _initialConditionTask.RemovePathAndValueEntityFromBuildingBlockCommand(_startValue3, _initialConditionsBuildingBlock)).MustNotHaveHappened();
      }
   }

   internal class When_deleting_selected_start_values_from_building_block : When_deleting_start_values_from_building_block
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.SelectedStartValues).Returns(new[]
         {
            new InitialConditionDTO(_startValue1, _initialConditionsBuildingBlock),
            new InitialConditionDTO(_startValue2, _initialConditionsBuildingBlock)
         });
      }

      protected override void Because()
      {
         _deleteStartValuePresenter.ApplySelectionAction(SelectOption.DeleteSelected);
      }
   }

   internal class When_changing_name_of_start_value_not_in_building_block : concern_for_InitialConditionsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_initialConditionsBuildingBlock);
      }

      protected override void Because()
      {
         sut.UpdateStartValueName(new InitialConditionDTO(new InitialCondition { Path = new ObjectPath("one", "two", "C1") }, new InitialConditionsBuildingBlock()), "C2");
      }

      [Observation]
      public void results_in_call_to_task_add_new_start_value_to_building_block()
      {
         A.CallTo(() => _initialConditionTask.AddPathAndValueEntityToBuildingBlock(
            _initialConditionsBuildingBlock, A<InitialCondition>.That.Matches(x => x.Path.Last().Equals("C2")))).MustHaveHappened();
      }
   }

   internal class When_changing_the_value_description_of_a_start_value : concern_for_InitialConditionsPresenter
   {
      private InitialCondition _startValue;
      private ValueOrigin _valueOrigin;

      protected override void Context()
      {
         base.Context();
         _valueOrigin = new ValueOrigin();
         _startValue = new InitialCondition { Name = "pathAndValueEntity" };
         _initialConditionsBuildingBlock.Add(_startValue);
         sut.Edit(_initialConditionsBuildingBlock);
      }

      protected override void Because()
      {
         sut.SetValueOrigin(new InitialConditionDTO(_startValue, _initialConditionsBuildingBlock), _valueOrigin);
      }

      [Observation]
      public void should_updae_the_value_description_in_the_start_value()
      {
         A.CallTo(() => _initialConditionTask.SetValueOrigin(_initialConditionsBuildingBlock, _valueOrigin, _startValue)).MustHaveHappened();
      }
   }

   internal class When_GetFormulas_is_called_at_msv_presenter : concern_for_InitialConditionsPresenter
   {
      private IEnumerable<ValueFormulaDTO> _resultFormulas;
      private IFormula _explicitFormula;

      protected override void Because()
      {
         _explicitFormula = new ExplicitFormula("1+2").WithName("Test");
         _initialConditionsBuildingBlock.AddFormula(_explicitFormula);
         sut.Edit(_initialConditionsBuildingBlock);
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

   public class When_setting_all_molecule_start_values_to_is_present : concern_for_InitialConditionsPresenter
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _command = A.Fake<IMoBiCommand>();
         A.CallTo(() => _initialConditionTask.SetIsPresent(_initialConditionsBuildingBlock, A<IEnumerable<InitialCondition>>._, true)).Returns(_command);
         sut.Edit(_initialConditionsBuildingBlock);
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

   public class When_setting_all_molecule_start_values_to_not_is_present : concern_for_InitialConditionsPresenter
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _command = A.Fake<IMoBiCommand>();
         A.CallTo(() => _initialConditionTask.SetIsPresent(_initialConditionsBuildingBlock, A<IEnumerable<InitialCondition>>._, false)).Returns(_command);
         sut.Edit(_initialConditionsBuildingBlock);
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

   public class When_performing_a_bulk_update_of_molecule_start_values : concern_for_InitialConditionsPresenter
   {
      private InitialCondition _msv1;
      private InitialConditionDTO _msv1DTOFirst;
      private InitialConditionDTO _msv1DTOSecond;

      protected override void Context()
      {
         base.Context();
         _msv1 = new InitialCondition();
         _msv1DTOFirst = new InitialConditionDTO(_msv1, _initialConditionsBuildingBlock);
         _msv1DTOSecond = new InitialConditionDTO(_msv1, _initialConditionsBuildingBlock);
         A.CallTo(() => _mapper.MapFrom(_msv1, _initialConditionsBuildingBlock))
            .ReturnsNextFromSequence(_msv1DTOFirst, _msv1DTOSecond);

         _initialConditionsBuildingBlock.Add(_msv1);
         sut.Edit(_initialConditionsBuildingBlock);
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