using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditOutputSelectionsPresenter : ISimulationSettingsItemPresenter
   {
      /// <summary>
      /// Removes the <paramref name="selection"/> from the simulation settings
      /// </summary>
      void RemoveOutputSelection(QuantitySelection selection);

      /// <summary>
      /// Edit the <paramref name="selection"/> in the simulation settings using the interactive editor
      /// </summary>
      void EditOutputSelection(QuantitySelection selection);

      /// <summary>
      /// Adds a new output selection to the simulation settings. The <paramref name="preSelectedQuantitySelection"/>
      /// is preselected in the tree view
      /// </summary>
      void AddOutputSelection(QuantitySelection preSelectedQuantitySelection);

      /// <summary>
      /// Updates the <paramref name="selection"/> with the <paramref name="newSelectionPath"/>
      /// </summary>
      void UpdateOutputSelection(QuantitySelection selection, string newSelectionPath);
   }

   public class EditOutputSelectionsPresenter : AbstractSubPresenter<IEditOutputSelectionsView, IEditOutputSelectionsPresenter>, IEditOutputSelectionsPresenter
   {
      private readonly IOutputSelectionsTask _outputSelectionsTask;
      private SimulationSettings _simulationSettings;

      public EditOutputSelectionsPresenter(IEditOutputSelectionsView view, IOutputSelectionsTask outputSelectionsTask) : base(view)
      {
         _outputSelectionsTask = outputSelectionsTask;
      }

      public void Edit(SimulationSettings simulationSettings)
      {
         _simulationSettings = simulationSettings;
         _view.BindTo(_simulationSettings.OutputSelections.AllOutputs);
      }

      public void RemoveOutputSelection(QuantitySelection selection)
      {
         _outputSelectionsTask.RemoveOutputSelection(_simulationSettings, selection);
      }

      public void EditOutputSelection(QuantitySelection selection)
      {
         _outputSelectionsTask.EditOutputSelection(_simulationSettings, selection);
      }

      public void AddOutputSelection(QuantitySelection preSelectedQuantitySelection)
      {
         _outputSelectionsTask.AddOutputSelection(_simulationSettings, preSelectedQuantitySelection);
      }

      public void UpdateOutputSelection(QuantitySelection selection, string newSelectionPath)
      {
         _outputSelectionsTask.UpdateOutputSelection(_simulationSettings, selection, newSelectionPath);
      }
   }
}