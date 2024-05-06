using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IOutputSelectionsPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Starts the output selection view and returns a new instance of the <see cref="OutputSelections" /> based on the
      ///    quantities
      ///    available in <paramref name="simulation" />. Returns null if the operation was cancelled
      /// </summary>
      OutputSelections StartSelection(IMoBiSimulation simulation);

      void LoadSelectionFromDefaults();
      void MakeCurrentSelectionDefault();
   }

   public class OutputSelectionsPresenter : MoBiDisposablePresenter<IOutputSelectionsView, IOutputSelectionsPresenter>, IOutputSelectionsPresenter
   {
      private readonly IQuantitySelectionPresenter _quantitySelectionPresenter;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private readonly IInteractionTasksForSimulationSettings _simulationSettingsTask;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private IMoBiSimulation _simulation;
      private OutputSelections _editedSelection;

      public OutputSelectionsPresenter(IOutputSelectionsView view, IQuantitySelectionPresenter quantitySelectionPresenter,
         ISimulationPersistableUpdater simulationPersistableUpdater, IInteractionTasksForSimulationSettings simulationSettingsTask, IMoBiProjectRetriever projectRetriever)
         : base(view)
      {
         _quantitySelectionPresenter = quantitySelectionPresenter;
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _simulationSettingsTask = simulationSettingsTask;
         _projectRetriever = projectRetriever;
         _quantitySelectionPresenter.AutomaticallyHideEmptyColumns = true;
         _quantitySelectionPresenter.StatusChanged += (o, e) => refreshView();
         _view.AddSettingsView(_quantitySelectionPresenter.BaseView);
         _quantitySelectionPresenter.Description = AppConstants.Captions.SimulationSettingsDescription.FormatForDescription();
         AddSubPresenters( _quantitySelectionPresenter);
      }

      private void refreshView()
      {
         View.OkEnabled = _quantitySelectionPresenter.HasSelection;
         View.ExtraEnabled = _quantitySelectionPresenter.HasSelection;
         int numberOfSelectedMolecules = _quantitySelectionPresenter.NumberOfSelectedQuantities;
         _quantitySelectionPresenter.Info = AppConstants.Captions.NumberOfGeneratedCurves(numberOfSelectedMolecules);
      }

      public OutputSelections StartSelection(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _editedSelection = defaultSettingsFrom(simulation);
         _quantitySelectionPresenter.Edit(_simulationPersistableUpdater.AllSelectableIn(_simulation), _editedSelection.AllOutputs);

         refreshView();
         _view.Display();
         if (_view.Canceled)
            return null;

         updateSettingsFromSelection();
         return _editedSelection;
      }

      public void LoadSelectionFromDefaults()
      {
         _quantitySelectionPresenter.UpdateSelection(_projectRetriever.Current.SimulationSettings.OutputSelections.ToList());
      }

      public void MakeCurrentSelectionDefault()
      {
         _simulationSettingsTask.UpdateDefaultOutputSelectionsInProject(_quantitySelectionPresenter.SelectedQuantities().ToList());
      }

      private OutputSelections defaultSettingsFrom(IMoBiSimulation simulation)
      {
         return simulation.OutputSelections.Clone();
      }

      private void updateSettingsFromSelection()
      {
         _editedSelection.Clear();
         foreach (var quantitySelection in _quantitySelectionPresenter.SelectedQuantities())
         {
            _editedSelection.AddOutput(quantitySelection);
         }
      }
   }
}