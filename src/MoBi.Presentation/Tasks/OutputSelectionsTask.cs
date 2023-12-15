using System.Linq;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public interface IOutputSelectionsTask
   {
      /// <summary>
      /// Adds a new output selection to the <paramref name="simulationSettings"/>. The <paramref name="preSelectedQuantitySelection"/>
      /// is preselected in the tree view
      /// </summary>
      void AddOutputSelection(SimulationSettings simulationSettings, QuantitySelection preSelectedQuantitySelection = null);

      /// <summary>
      /// Edit the <paramref name="selection"/> in the <paramref name="simulationSettings"/> using the interactive editor
      /// </summary>
      void EditOutputSelection(SimulationSettings simulationSettings, QuantitySelection selection);

      /// <summary>
      /// Removes the <paramref name="selection"/> from the <paramref name="simulationSettings"/>
      /// </summary>
      void RemoveOutputSelection(SimulationSettings simulationSettings, QuantitySelection selection);

      /// <summary>
      /// Updates the <paramref name="selection"/> with the <paramref name="newPath"/>
      /// </summary>
      void UpdateOutputSelection(SimulationSettings simulationSettings, QuantitySelection selection, string newPath);
   }

   public class OutputSelectionsTask : IOutputSelectionsTask
   {
      private readonly IOSPSuiteExecutionContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly ISimulationRepository _simulationRepository;
      private readonly IDialogCreator _dialogCreator;

      public OutputSelectionsTask(IOSPSuiteExecutionContext context, IMoBiApplicationController applicationController, ISimulationRepository simulationRepository, IDialogCreator dialogCreator)
      {
         _context = context;
         _applicationController = applicationController;
         _simulationRepository = simulationRepository;
         _dialogCreator = dialogCreator;
      }

      public void AddOutputSelection(SimulationSettings simulationSettings, QuantitySelection preSelectedQuantitySelection = null)
      {
         var pathFromSimulations = selectPathFromSimulationsFor(AppConstants.Captions.AddDefaultCurveForNewSimulations, preSelectedQuantitySelection);
         if(pathFromSimulations == null)
            return;

         if (!validateAndWarnNewPath(pathFromSimulations, simulationSettings))
            return;

         simulationSettings.OutputSelections.AddOutput(new QuantitySelection(pathFromSimulations, QuantityType.Undefined));
         projectHasChanged();
      }

      private void showWarningMessage(ObjectPath pathFromSimulations)
      {
         _dialogCreator.MessageBoxInfo(AppConstants.Validation.ThePathIsAlreadySelectedAsAnOutput(pathFromSimulations));
      }

      private bool alreadyContainsPath(ObjectPath pathFromSimulations, SimulationSettings simulationSettings)
      {
         return simulationSettings.OutputSelections.Any(selection => Equals(selection.Path, pathFromSimulations.ToPathString()));
      }

      private void projectHasChanged()
      {
         _context.Project.HasChanged = true;
      }

      public void EditOutputSelection(SimulationSettings simulationSettings, QuantitySelection selection)
      {
         var newPath = selectPathFromSimulationsFor(AppConstants.Captions.ChangeDefaultCurveForNewSimulations, selection);

         if (noChangesRequired(selection, newPath))
            return;

         if(validateAndUpdate(selection, newPath, simulationSettings))
            projectHasChanged();
      }

      private static bool noChangesRequired(QuantitySelection selection, ObjectPath newPath)
      {
         return newPath == null || Equals(selection.Path, newPath.ToPathString());
      }

      private ObjectPath selectPathFromSimulationsFor(string caption, QuantitySelection preSelectedQuantity = null)
      {
         using (var modalPresenter = _applicationController.Start<IModalPresenter>())
         {
            modalPresenter.Text = caption;
            var selectPathPresenter = _applicationController.Start<IHierarchicalQuantitySelectionPresenter>();
            modalPresenter.Encapsulate(selectPathPresenter);
            selectPathPresenter.SelectPathFrom(_simulationRepository.All().ToList());

            if(preSelectedQuantity != null)
               selectPathPresenter.SelectQuantityFromPath(new ObjectPath(preSelectedQuantity.Path.ToPathArray()));

            if (modalPresenter.Show())
               return selectPathPresenter.SelectedPath;
         }

         return null;
      }

      private bool validateAndWarnNewPath(ObjectPath selectedPath, SimulationSettings simulationSettings)
      {
         if (!alreadyContainsPath(selectedPath, simulationSettings)) 
            return true;
         
         showWarningMessage(selectedPath);
         return false;
      }

      public void UpdateOutputSelection(SimulationSettings simulationSettings, QuantitySelection selection, string newPath)
      {
         if (noChangesRequired(selection, new ObjectPath(newPath.ToPathArray())))
            return;

         if (!validateAndUpdate(selection, newPath, simulationSettings)) 
            return;

         projectHasChanged();
      }

      private bool validateAndUpdate(QuantitySelection selection, string newPath, SimulationSettings simulationSettings)
      {
         if (!validateAndWarnNewPath(new ObjectPath(newPath.ToPathArray()), simulationSettings))
            return false;

         selection.Path = newPath;
         return true;
      }

      public void RemoveOutputSelection(SimulationSettings simulationSettings, QuantitySelection selection)
      {
         simulationSettings.OutputSelections.RemoveOutput(selection);
         projectHasChanged();
      }
   }
}
