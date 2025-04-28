using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using MoBi.UI.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;

namespace MoBi.Presentation
{
   internal class concern_for_OutputSelectionsPresenter : ContextSpecification<OutputSelectionsPresenter>
   {
      protected IOutputSelectionsView _view;
      protected IQuantitySelectionPresenter _quantitySelectionPresenter;
      protected ISimulationPersistableUpdater _simulationPersistableUpdater;
      protected IInteractionTasksForSimulationSettings _simulationSettingsTask;
      protected IMoBiProjectRetriever _projectRetriever;
      protected IDefaultOutputSelectionsButtonsView _defaultButtonsView;

      protected override void Context()
      {
         _view = A.Fake<IOutputSelectionsView>();
         _quantitySelectionPresenter = A.Fake<IQuantitySelectionPresenter>();
         _simulationPersistableUpdater = A.Fake<ISimulationPersistableUpdater>();
         _simulationSettingsTask = A.Fake<IInteractionTasksForSimulationSettings>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _defaultButtonsView = new DefaultOutputSelectionsButtonsView();

         sut = new OutputSelectionsPresenter(_view, _quantitySelectionPresenter, _simulationPersistableUpdater, _simulationSettingsTask, _projectRetriever, _defaultButtonsView);
      }

      protected bool ContainsAllElementsFrom(IReadOnlyList<QuantitySelection> argumentList, IReadOnlyList<QuantitySelection> outputSelections)
      {
         return argumentList.All(outputSelections.Contains) && outputSelections.All(argumentList.Contains);
      }
   }

   internal class When_setting_project_defaults_from_output_selection : concern_for_OutputSelectionsPresenter
   {
      private List<QuantitySelection> _outputSelections;

      protected override void Context()
      {
         base.Context();
         _outputSelections = new List<QuantitySelection>
         {
            new QuantitySelection("toto")
         };

         A.CallTo(() => _quantitySelectionPresenter.SelectedQuantities()).Returns(_outputSelections);
      }

      protected override void Because()
      {
         _defaultButtonsView.MakeProjectDefaultsClicked();
      }

      [Observation]
      public void the_project_defaults_should_be_updated_from_the_selected_outputs()
      {
         A.CallTo(() => _simulationSettingsTask.UpdateDefaultOutputSelectionsInProject(A<IReadOnlyList<QuantitySelection>>.That.Matches(x => ContainsAllElementsFrom(x, _outputSelections)))).MustHaveHappened();
      }
   }

   internal class When_loading_project_default_output_selections : concern_for_OutputSelectionsPresenter
   {
      private OutputSelections _outputSelections;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = new MoBiProject();
         _outputSelections = new OutputSelections();
         _outputSelections.AddOutput(new QuantitySelection("toto"));
         _project.SimulationSettings = new SimulationSettings
         {
            OutputSelections = _outputSelections
         };

         A.CallTo(() => _projectRetriever.Current).Returns(_project);
      }

      protected override void Because()
      {
         _defaultButtonsView.LoadProjectDefaultsClicked();
      }

      [Observation]
      public void the_simulation_column_is_hidden()
      {
         _quantitySelectionPresenter.HideSimulationColumn.ShouldBeTrue();
      }

      [Observation]
      public void the_selected_outputs_should_be_updated_from_the_project_defaults()
      {
         A.CallTo(() => _quantitySelectionPresenter.UpdateSelection(A<IReadOnlyList<QuantitySelection>>.That.Matches(x => ContainsAllElementsFrom(x, _outputSelections.ToList())))).MustHaveHappened();
      }
   }
}
