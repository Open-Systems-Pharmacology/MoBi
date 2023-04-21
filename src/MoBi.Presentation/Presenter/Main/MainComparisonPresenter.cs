using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views.Comparisons;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter.Main
{
   public class MainComparisonPresenter : OSPSuite.Presentation.Presenters.Comparisons.MainComparisonPresenter,
      IListener<SimulationRemovedEvent>,
      IListener<RemovedEvent>,
      IListener<SimulationUnloadEvent>

   {
      public MainComparisonPresenter(IMainComparisonView view, IRegionResolver regionResolver, IComparisonPresenter comparisonPresenter,
         IComparerSettingsPresenter comparerSettingsPresenter, IPresentationUserSettings presentationUserSettings,
         IDialogCreator dialogCreator, IExportDataTableToExcelTask exportToExcelTask, IMoBiContext context)
         : base(view, regionResolver, comparisonPresenter, comparerSettingsPresenter,
            presentationUserSettings, dialogCreator, exportToExcelTask, context, RegionNames.Comparison)
      {
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         foreach (var removedObject in eventToHandle.RemovedObjects)
         {
            ClearComparisonIfComparing(removedObject);
         }
      }

      public void Handle(SimulationRemovedEvent eventToHandle)
      {
         ClearComparisonIfComparing(eventToHandle.Simulation);
      }

      public void Handle(SimulationUnloadEvent eventToHandle)
      {
         ClearComparisonIfComparing(eventToHandle.Simulation);
         clearAllBuildingBlockComparisons(eventToHandle.Simulation);
      }

      private void clearAllBuildingBlockComparisons(IMoBiSimulation simulation)
      {
         simulation.Configuration.ModuleConfigurations.Each(clearAllBuildingBlockComparisons);
      }

      private void clearAllBuildingBlockComparisons(ModuleConfiguration moduleConfiguration)
      {
         moduleConfiguration.Module.BuildingBlocks.Each(ClearComparisonIfComparing);
      }
   }
}