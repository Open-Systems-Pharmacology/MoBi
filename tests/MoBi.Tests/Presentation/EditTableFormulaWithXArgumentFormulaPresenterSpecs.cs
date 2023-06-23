using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditTableFormulaWithXArgumentFormulaPresenter : ContextSpecification<IEditTableFormulaWithXArgumentFormulaPresenter>
   {
      protected IEditTableFormulaWithXArgumentFormulaView _view;
      protected ITableFormulaWithXArgumentToTableFormulaWithXArgumentDTOMapper _mapper;
      protected IMoBiFormulaTask _formulaTask;
      protected IDisplayUnitRetriever _displayUnitRetriever;
      protected IApplicationController _applicationController;
      protected ISelectReferencePresenterFactory _selectReferencePresenterFactory;
      protected IParameter _parameter;

      protected TableFormulaWithXArgument _tableFormulaWithXArgument;
      protected Container _container;
      protected ISelectReferenceAtParameterPresenter _referencePresenter;
      protected ISelectFormulaUsablePathPresenter _selectFormulaUsablePathPresenter;
      protected IBuildingBlock _buildingBlock;
      protected ICommandCollector _commandCollector;

      protected override void Context()
      {
         _view = A.Fake<IEditTableFormulaWithXArgumentFormulaView>();
         _mapper = A.Fake<ITableFormulaWithXArgumentToTableFormulaWithXArgumentDTOMapper>();
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _applicationController = A.Fake<IApplicationController>();
         _selectReferencePresenterFactory = A.Fake<ISelectReferencePresenterFactory>();
         _commandCollector = A.Fake<ICommandCollector>();
         sut = new EditTableFormulaWithXArgumentFormulaPresenter(_view, _mapper, _formulaTask, _displayUnitRetriever, _applicationController, _selectReferencePresenterFactory);

         _tableFormulaWithXArgument = new TableFormulaWithXArgument();
         _parameter = new Parameter().WithName("Parameter");
         _container = new Container().WithName("Parent");
         _container.Add(_parameter);
         _referencePresenter = A.Fake<ISelectReferenceAtParameterPresenter>();
         _selectFormulaUsablePathPresenter = A.Fake<ISelectFormulaUsablePathPresenter>();
         _buildingBlock = A.Fake<IBuildingBlock>();

         A.CallTo(() => _selectReferencePresenterFactory.ReferenceAtParameterFor(_container)).Returns(_referencePresenter);
         A.CallTo(() => _applicationController.Start<ISelectFormulaUsablePathPresenter>()).Returns(_selectFormulaUsablePathPresenter);

         sut.BuildingBlock = _buildingBlock;
         sut.InitializeWith(_commandCollector);
      }
   }

   public class When_the_edit_table_formula_with_x_argument_presenter_is_selecting_a_table_object_path_for_the_edited_table_formula : concern_for_EditTableFormulaWithXArgumentFormulaPresenter
   {
      private FormulaUsablePath _selectedPath;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _command = A.Fake<IMoBiCommand>();
         _selectedPath = A.Fake<FormulaUsablePath>();
         sut.Edit(_tableFormulaWithXArgument, _parameter);
         A.CallTo(() => _selectFormulaUsablePathPresenter.GetSelection()).Returns(_selectedPath);
         A.CallTo(() => _formulaTask.ChangeTableObject(_tableFormulaWithXArgument, _selectedPath, _buildingBlock)).Returns(_command);
      }

      protected override void Because()
      {
         sut.SetTableObjectPath();
      }

      [Observation]
      public void should_let_the_user_select_a_new_path_and_updated_the_selected_path()
      {
         A.CallTo(() => _formulaTask.ChangeTableObject(_tableFormulaWithXArgument, _selectedPath, _buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_command_to_history()
      {
         A.CallTo(() => _commandCollector.AddCommand(_command)).MustHaveHappened();
      }
   }

   public class When_the_edit_table_formula_with_x_argument_presenter_is_selecting_a_x_argument_object_path_for_the_edited_table_formula : concern_for_EditTableFormulaWithXArgumentFormulaPresenter
   {
      private FormulaUsablePath _selectedPath;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _command = A.Fake<IMoBiCommand>();
         _selectedPath = A.Fake<FormulaUsablePath>();
         sut.Edit(_tableFormulaWithXArgument, _parameter);
         A.CallTo(() => _selectFormulaUsablePathPresenter.GetSelection()).Returns(_selectedPath);
         A.CallTo(() => _formulaTask.ChangeXArgumentObject(_tableFormulaWithXArgument, _selectedPath, _buildingBlock)).Returns(_command);
      }

      protected override void Because()
      {
         sut.SetXArgumentFormulaPath();
      }

      [Observation]
      public void should_let_the_user_select_a_new_path_and_updated_the_selected_path()
      {
         A.CallTo(() => _formulaTask.ChangeXArgumentObject(_tableFormulaWithXArgument, _selectedPath, _buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_command_to_history()
      {
         A.CallTo(() => _commandCollector.AddCommand(_command)).MustHaveHappened();
      }
   }
}