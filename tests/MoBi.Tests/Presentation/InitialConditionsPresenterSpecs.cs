using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_InitialConditionsPresenter : ContextSpecification<InitialConditionsPresenter>
   {
      protected IInitialConditionsView _view;
      protected IInitialConditionToInitialConditionDTOMapper _dtoMapper;
      protected IMoBiContext _context;
      protected IInitialConditionsTask<InitialConditionsBuildingBlock> _initialConditionTask;
      protected ICommandCollector _commandCollector;
      protected InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      protected IInitialConditionsCreator _initialConditionsCreator;
      private IFormulaToValueFormulaDTOMapper _formulaToValueFormulaDTOMapper;
      private IDimensionFactory _dimensionFactory;
      private IInitialConditionsDistributedPathAndValueEntityPresenter _distributedParameterPresenter;
      private IInitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper _buildingBlockMapper;
      private IPathAndValueEntityToDistributedParameterMapper _mapper;
      private IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IDialogCreator _dialogCreator;
      protected IInitialConditionPathResolver _initialConditionPathResolver;
      protected IClipboardManager _clipboardManager;

      protected override void Context()
      {
         _view = A.Fake<IInitialConditionsView>();
         _dtoMapper = A.Fake<IInitialConditionToInitialConditionDTOMapper>();
         _context = A.Fake<IMoBiContext>();
         _initialConditionTask = A.Fake<IInitialConditionsTask<InitialConditionsBuildingBlock>>();
         _commandCollector = A.Fake<ICommandCollector>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _initialConditionsCreator = A.Fake<IInitialConditionsCreator>();
         _distributedParameterPresenter = A.Fake<IInitialConditionsDistributedPathAndValueEntityPresenter>();
         _mapper = A.Fake<IPathAndValueEntityToDistributedParameterMapper>();
         _formulaToValueFormulaDTOMapper = new FormulaToValueFormulaDTOMapper();
         _buildingBlockMapper = new InitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper(_dtoMapper, _mapper);
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _initialConditionPathResolver = A.Fake<IInitialConditionPathResolver>();
         _clipboardManager = A.Fake<IClipboardManager>();
         sut = new InitialConditionsPresenter(
            _view,
            _initialConditionTask,
            _initialConditionsCreator,
            _context,
            _formulaToValueFormulaDTOMapper,
            _dimensionFactory,
            _distributedParameterPresenter,
            _buildingBlockMapper,
            _viewItemContextMenuFactory,
            _dialogCreator,
            _initialConditionPathResolver,
            _clipboardManager);

         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();

         sut.InitializeWith(_commandCollector);
      }
   }

   public class When_setting_negative_values_allowed : concern_for_InitialConditionsPresenter
   {
      private InitialCondition _initialCondition;
      private InitialConditionDTO _dto;

      protected override void Context()
      {
         base.Context();
         _initialCondition = new InitialCondition();
         _initialConditionsBuildingBlock.Add(_initialCondition);
         _dto = new InitialConditionDTO(_initialCondition, _initialConditionsBuildingBlock);

         A.CallTo(() => _dtoMapper.MapFrom(_initialCondition, _initialConditionsBuildingBlock)).Returns(_dto);
         sut.Edit(_initialConditionsBuildingBlock);
      }

      [TestCase(true)]
      [TestCase(false)]
      public void the_negativeValueShouldBeSet(bool allowNegative)
      {
         sut.SetNegativeValuesAllowed(_dto, allowNegative);
         A.CallTo(() => _initialConditionTask.SetNegativeValuesAllowed(_initialConditionsBuildingBlock, A<IEnumerable<InitialCondition>>.That.Contains(_initialCondition), allowNegative)).MustHaveHappened();
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

         A.CallTo(() => _dtoMapper.MapFrom(_startValue1, _initialConditionsBuildingBlock)).Returns(new InitialConditionDTO(_startValue1, _initialConditionsBuildingBlock));
         A.CallTo(() => _dtoMapper.MapFrom(_startValue2, _initialConditionsBuildingBlock)).Returns(new InitialConditionDTO(_startValue2, _initialConditionsBuildingBlock));
         A.CallTo(() => _dtoMapper.MapFrom(_startValue3, _initialConditionsBuildingBlock)).Returns(new InitialConditionDTO(_startValue3, _initialConditionsBuildingBlock));

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

   internal class When_changing_name_of_start_value_not_in_building_block : concern_for_InitialConditionsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_initialConditionsBuildingBlock);
      }

      protected override void Because()
      {
         sut.UpdatePathAndValueEntityName(new InitialConditionDTO(new InitialCondition { Path = new ObjectPath("one", "two", "C1") }, new InitialConditionsBuildingBlock()), "C2");
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
      public void should_update_the_value_description_in_the_start_value()
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
            _resultFormulas
               .Where(dto => dto.Formula == null)
               .FirstOrDefault(dto => dto.FormulaString == AppConstants.Captions.FormulaNotAvailable);
         nullFormula.ShouldNotBeNull();
      }

      [Observation]
      public void The_returned_list_Should_contain_explicit_formula_from_cache()
      {
         var explicitFormula = _resultFormulas.FirstOrDefault(dto => _explicitFormula.Equals(dto.Formula));
         explicitFormula.ShouldNotBeNull();
      }
   }

   public class When_performing_a_bulk_update_of_molecule_start_values : concern_for_InitialConditionsPresenter
   {
      private InitialCondition _msv1;
      private InitialConditionDTO _initialConditionDTO1;
      private InitialConditionDTO _initialConditionDTO2;

      protected override void Context()
      {
         base.Context();
         _msv1 = new InitialCondition();
         _initialConditionDTO1 = new InitialConditionDTO(_msv1, _initialConditionsBuildingBlock);
         _initialConditionDTO2 = new InitialConditionDTO(_msv1, _initialConditionsBuildingBlock);
         A.CallTo(() => _dtoMapper.MapFrom(_msv1, _initialConditionsBuildingBlock))
            .ReturnsNextFromSequence(_initialConditionDTO1, _initialConditionDTO2);

         _initialConditionsBuildingBlock.Add(_msv1);
         sut.Edit(_initialConditionsBuildingBlock);
         _view.FocusedStartValue = _initialConditionDTO1;
      }

      protected override void Because()
      {
         sut.Handle(new BulkUpdateStartedEvent());
         sut.Handle(new BulkUpdateFinishedEvent());
      }

      [Observation]
      public void should_save_the_currently_focused_start_values_and_set_it_back_to_the_view()
      {
         _view.FocusedStartValue.ShouldBeEqualTo(_initialConditionDTO2);
      }
   }

   public class When_adding_new_initial_conditions_from_paths : concern_for_InitialConditionsPresenter
   {
      private ObjectPath _existingPath;
      private ObjectPath _resolvedPath;
      private ObjectPath _unresolvedPath;
      private MoleculeBuilder _molecule;
      private InitialCondition _resolvedInitialCondition;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _existingPath = new ObjectPath("Organism", "Plasma", "Drug");
         _resolvedPath = new ObjectPath("Organism", "Plasma", "Metabolite");
         _unresolvedPath = new ObjectPath("Organism", "Tumor", "Metabolite");
         _molecule = new MoleculeBuilder { Dimension = DimensionFactoryForSpecs.MassDimension };
         _molecule.DisplayUnit = _molecule.Dimension.Unit("mg");
         _initialConditionsBuildingBlock.Add(new InitialCondition { ContainerPath = new ObjectPath("Organism", "Plasma"), Name = "Drug" });
         _module = new Module { _initialConditionsBuildingBlock };
         sut.Edit(_initialConditionsBuildingBlock);

         A.CallTo(() => _initialConditionsCreator.CreateEmptyStartValue(A<IDimension>._)).ReturnsLazily(() => new InitialCondition());
         A.CallTo(() => _initialConditionPathResolver.Resolve(A<ObjectPath>._, A<Module>._)).Returns(null);
         A.CallTo(() => _initialConditionPathResolver.Resolve(_resolvedPath, _module)).Returns(_molecule);
         A.CallTo(() => _initialConditionTask.AddWithFullPathCommand(A<InitialCondition>._, _resolvedPath, _initialConditionsBuildingBlock)).Invokes(x => _resolvedInitialCondition = x.GetArgument<InitialCondition>(0));
         A.CallTo(() => _initialConditionTask.GetNewPaths()).Returns(new[] { _existingPath, _resolvedPath, new ObjectPath("Organism", "Plasma", "Metabolite"), _unresolvedPath });
      }

      protected override void Because()
      {
         sut.AddNewInitialConditions();
      }

      [Observation]
      public void an_initial_condition_should_be_added_once_for_each_new_path()
      {
         A.CallTo(() => _initialConditionTask.AddWithFullPathCommand(A<InitialCondition>._, _resolvedPath, _initialConditionsBuildingBlock)).MustHaveHappenedOnceExactly();
         A.CallTo(() => _initialConditionTask.AddWithFullPathCommand(A<InitialCondition>._, _unresolvedPath, _initialConditionsBuildingBlock)).MustHaveHappenedOnceExactly();
      }

      [Observation]
      public void the_path_already_in_the_building_block_should_be_skipped()
      {
         A.CallTo(() => _initialConditionTask.AddWithFullPathCommand(A<InitialCondition>._, _existingPath, _initialConditionsBuildingBlock)).MustNotHaveHappened();
      }

      [Observation]
      public void the_user_should_be_told_which_paths_were_skipped()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(AppConstants.Captions.InitialConditionsBuildingBlockAlreadyContains(new[] { _existingPath.PathAsString }))).MustHaveHappened();
      }

      [Observation]
      public void the_dimension_and_display_unit_of_the_resolved_molecule_should_be_used()
      {
         _resolvedInitialCondition.Dimension.ShouldBeEqualTo(_molecule.Dimension);
         _resolvedInitialCondition.DisplayUnit.ShouldBeEqualTo(_molecule.DisplayUnit);
      }

      [Observation]
      public void the_user_should_be_told_which_initial_conditions_do_not_match_the_project()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(AppConstants.Warnings.AddedInitialConditionsNotFoundInProject(new[] { _unresolvedPath.PathAsString }))).MustHaveHappened();
      }

      [Observation]
      public void the_initial_conditions_should_be_added_in_one_bulk_update()
      {
         A.CallTo(() => _context.PublishEvent(A<BulkUpdateStartedEvent>._)).MustHaveHappenedOnceExactly();
      }
   }

   internal class When_pasting_initial_conditions_from_the_clipboard : concern_for_InitialConditionsPresenter
   {
      private InitialCondition _pastedInitialCondition;
      private IReadOnlyList<InitialCondition> _extendingInitialConditions;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_initialConditionsBuildingBlock);
         _pastedInitialCondition = new InitialCondition { Path = new ObjectPath("Organism", "Molecule") };
         A.CallTo(() => _clipboardManager.ClipBoardContainsType<InitialCondition>()).Returns(true);
         A.CallTo(() => _clipboardManager.PasteFromClipBoard(A<Action<InitialCondition>>._)).Invokes(x => x.GetArgument<Action<InitialCondition>>(0)(_pastedInitialCondition));
         A.CallTo(() => _initialConditionTask.ExtendBuildingBlockWith(_initialConditionsBuildingBlock, A<IReadOnlyList<InitialCondition>>._)).Invokes(x => _extendingInitialConditions = x.GetArgument<IReadOnlyList<InitialCondition>>(1));
      }

      protected override void Because()
      {
         sut.PasteFromClipBoard();
      }

      [Observation]
      public void the_building_block_should_be_extended_with_the_pasted_initial_conditions()
      {
         _extendingInitialConditions.ShouldOnlyContain(_pastedInitialCondition);
      }
   }
}