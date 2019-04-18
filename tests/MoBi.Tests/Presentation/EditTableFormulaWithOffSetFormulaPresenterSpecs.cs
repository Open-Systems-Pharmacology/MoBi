using System;
using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditTableFormulaWithOffSetFormulaPresenter : ContextSpecification<IEditTableFormulaWithOffsetFormulaPresenter>
   {
      protected IEditTableFormulaWithOffsetFormulaView _view;
      protected ITableFormulaWithOffsetToTableFormulaWithOffsetDTOMapper _mapper;
      protected IMoBiContext _context;
      protected IMoBiFormulaTask _mobiFormulaTask;
      protected IDisplayUnitRetriever _displayUnitRetriever;
      protected IApplicationController _applicationController;
      protected ISelectReferencePresenterFactory _selectReferencePresenterFactory;
      protected TableFormulaWithOffset _formula;
      protected IEntity _usingFormula;
      protected TableFormulaWithOffsetDTO _dto;
      protected IDimension _timeDimension;
      protected ISelectFormulaUsablePathPresenter _selectFormulaUsablePathPresenter;
      protected ISelectReferenceAtParameterPresenter _selectReferenceAtParameterPresenter;
      protected Func<IObjectBase, bool> _predicate;
      protected ICommandCollector _commandCollector;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _view = A.Fake<IEditTableFormulaWithOffsetFormulaView>();
         _mapper = A.Fake<ITableFormulaWithOffsetToTableFormulaWithOffsetDTOMapper>();
         _context = A.Fake<IMoBiContext>();
         _mobiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _applicationController = A.Fake<IApplicationController>();
         _selectReferencePresenterFactory = A.Fake<ISelectReferencePresenterFactory>();
         _timeDimension = DomainHelperForSpecs.TimeDimension;
         A.CallTo(() => _context.DimensionFactory.Dimension(Constants.Dimension.TIME)).Returns(_timeDimension);

         sut = new EditTableFormulaWithOffsetFormulaPresenter(_view, _mapper, _context, _mobiFormulaTask, _displayUnitRetriever, _applicationController, _selectReferencePresenterFactory);

         _commandCollector = A.Fake<ICommandCollector>();
         _buildingBlock = A.Fake<IBuildingBlock>();

         sut.InitializeWith(_commandCollector);
         sut.BuildingBlock = _buildingBlock;
         _formula = new TableFormulaWithOffset();
         _usingFormula = new Parameter();
         _dto = new TableFormulaWithOffsetDTO();

         _selectFormulaUsablePathPresenter = A.Fake<ISelectFormulaUsablePathPresenter>();
         A.CallTo(() => _applicationController.Start<ISelectFormulaUsablePathPresenter>()).Returns(_selectFormulaUsablePathPresenter);

         _selectReferenceAtParameterPresenter = A.Fake<ISelectReferenceAtParameterPresenter>();
         A.CallTo(_selectReferencePresenterFactory).WithReturnType<ISelectReferenceAtParameterPresenter>().Returns(_selectReferenceAtParameterPresenter);

         A.CallTo(() => _selectFormulaUsablePathPresenter.Init(A<Func<IObjectBase, bool>>._, _usingFormula, A<IEnumerable<IObjectBase>>._, A<string>._, _selectReferenceAtParameterPresenter))
            .Invokes(x => _predicate = x.GetArgument<Func<IObjectBase, bool>>(0));
      }
   }

   public class When_the_edit_table_formulat_with_offset_presenter_is_editing_a_table_formula : concern_for_EditTableFormulaWithOffSetFormulaPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _mapper.MapFrom(_formula)).Returns(_dto);
      }

      protected override void Because()
      {
         sut.Edit(_formula, _usingFormula);
      }

      [Observation]
      public void should_create_a_dto_for_the_edited_formula_and_bind_it_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_dto)).MustHaveHappened();
      }
   }

   public class When_the_user_is_selecting_a_new_offset_formula_for_the_edited_table_formula : concern_for_EditTableFormulaWithOffSetFormulaPresenter
   {
      private IObjectBase _timeParameter;
      private IObjectBase _anotherParameter;
      private IObjectBase _notAParameter;
      private IFormulaUsablePath _selectedFormulaUsablePath;

      protected override void Context()
      {
         base.Context();
         _timeParameter = new Parameter().WithDimension(_timeDimension);
         _anotherParameter = new Parameter().WithDimension(DomainHelperForSpecs.ConcentrationDimension);
         _notAParameter = A.Fake<IObjectBase>();
         _selectedFormulaUsablePath = new FormulaUsablePath();
         A.CallTo(() => _selectFormulaUsablePathPresenter.GetSelection()).Returns(_selectedFormulaUsablePath);
         sut.Edit(_formula, _usingFormula);
      }

      protected override void Because()
      {
         sut.SetOffsetFormulaPath();
      }

      [Observation]
      public void should_only_allow_selection_of_parameters_with_the_time_dimension()
      {
         _predicate(_timeParameter).ShouldBeTrue();
         _predicate(_anotherParameter).ShouldBeFalse();
         _predicate(_notAParameter).ShouldBeFalse();
      }

      [Observation]
      public void should_register_a_command_in_the_history()
      {
         A.CallTo(() => _mobiFormulaTask.ChangeOffsetObject(_formula, _selectedFormulaUsablePath, _buildingBlock)).MustHaveHappened();
         A.CallTo(() => _commandCollector.AddCommand(A<ICommand>._)).MustHaveHappened();
      }

      [Observation]
      public void should_rebind_to_the_view()
      {
         A.CallTo(() => _view.BindTo(A<TableFormulaWithOffsetDTO>._)).MustHaveHappenedTwiceExactly();
      }

      public class When_the_user_is_selecting_a_new_offset_formula_for_the_edited_table_formula_and_the_selection_is_canceled : concern_for_EditTableFormulaWithOffSetFormulaPresenter
      {
         protected override void Context()
         {
            base.Context();
            A.CallTo(() => _selectFormulaUsablePathPresenter.GetSelection()).Returns(null);
            sut.Edit(_formula, _usingFormula);
         }

         protected override void Because()
         {
            sut.SetOffsetFormulaPath();
         }

         [Observation]
         public void should_not_add_a_command_to_the_history()
         {
            A.CallTo(() => _commandCollector.AddCommand(A<ICommand>._)).MustNotHaveHappened();
         }
      }

      public class When_the_user_is_selecting_a_new_table_object_for_the_edited_table_formula : concern_for_EditTableFormulaWithOffSetFormulaPresenter
      {
         private IObjectBase _tableParameter;
         private IObjectBase _anotherParameter;
         private IObjectBase _notAParameter;
         private IFormulaUsablePath _selectedFormulaUsablePath;

         protected override void Context()
         {
            base.Context();
            _tableParameter = new Parameter().WithFormula(new TableFormula());
            _anotherParameter = new Parameter().WithFormula(new ConstantFormula());
            _notAParameter = A.Fake<IObjectBase>();
            _selectedFormulaUsablePath = new FormulaUsablePath();
            A.CallTo(() => _selectFormulaUsablePathPresenter.GetSelection()).Returns(_selectedFormulaUsablePath);
            sut.Edit(_formula, _usingFormula);
         }

         protected override void Because()
         {
            sut.SetTableObjectPath();
         }

         [Observation]
         public void should_only_allow_selection_of_parameters_with_the_time_dimension()
         {
            _predicate(_tableParameter).ShouldBeTrue();
            _predicate(_anotherParameter).ShouldBeFalse();
            _predicate(_notAParameter).ShouldBeFalse();
         }

         [Observation]
         public void should_register_a_command_in_the_history()
         {
            A.CallTo(() => _mobiFormulaTask.ChangeTableObject(_formula, _selectedFormulaUsablePath, _buildingBlock)).MustHaveHappened();
            A.CallTo(() => _commandCollector.AddCommand(A<ICommand>._)).MustHaveHappened();
         }

         [Observation]
         public void should_rebind_to_the_view()
         {
            A.CallTo(() => _view.BindTo(A<TableFormulaWithOffsetDTO>._)).MustHaveHappenedTwiceExactly();
         }
      }
   }
}