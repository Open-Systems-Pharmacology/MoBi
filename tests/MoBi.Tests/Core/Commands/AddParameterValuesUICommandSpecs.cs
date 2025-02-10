using FakeItEasy;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddParameterValuesUICommand : ContextSpecification<AddParameterValuesUICommand>
   {
      protected IParameterValuesTask _parameterValueTask;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IMoBiApplicationController _applicationController;
      protected IMoBiHistoryManager _moBiHistoryManager;
      protected IParameterValuesPresenter _parameterValuesPresenter;

      protected override void Context()
      {
         _parameterValueTask = A.Fake<IParameterValuesTask>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _moBiHistoryManager = A.Fake<IMoBiHistoryManager>();
         _parameterValuesPresenter = A.Fake<IParameterValuesPresenter>();

         sut = new AddParameterValuesUICommand(
            _parameterValueTask,
            _activeSubjectRetriever,
            _applicationController,
            _moBiHistoryManager,
            _parameterValuesPresenter);
      }
   }

   public class after_successful_execution : concern_for_AddParameterValuesUICommand
   {
      private IEditParameterValuesPresenter _editPresenter;

      protected override void Context()
      {
         base.Context();
         _editPresenter = A.Fake<IEditParameterValuesPresenter>();
         A.CallTo(() => _applicationController.Open<IEditParameterValuesPresenter, ParameterValuesBuildingBlock>(A<ParameterValuesBuildingBlock>._, _moBiHistoryManager))
            .Returns(_editPresenter);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_open_edit_parameter_values_presenter()
      {
         A.CallTo(() => _applicationController.Open<IEditParameterValuesPresenter, ParameterValuesBuildingBlock>(A<ParameterValuesBuildingBlock>._, _moBiHistoryManager)).MustHaveHappened();
      }

      [Observation]
      public void should_add_new_parameter_value()
      {
         A.CallTo(() => _editPresenter.AddNewParameterValues()).MustHaveHappened();
      }
   }
}