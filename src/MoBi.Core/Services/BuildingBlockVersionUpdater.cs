using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public interface IBuildingBlockVersionUpdater
   {
      void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, uint newVersion, bool shouldConvertPKSimModule);
      void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, bool shouldIncrementVersion, bool shouldConvertPKSimModule);
   }

   public class BuildingBlockVersionUpdater : IBuildingBlockVersionUpdater
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IEventPublisher _eventPublisher;
      private readonly IDialogCreator _dialogCreator;

      public BuildingBlockVersionUpdater(IMoBiProjectRetriever projectRetriever, IEventPublisher eventPublisher, IDialogCreator dialogCreator)
      {
         _projectRetriever = projectRetriever;
         _eventPublisher = eventPublisher;
         _dialogCreator = dialogCreator;
      }

      public virtual void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, uint newVersion, bool shouldConvertPKSimModule)
      {
         if (buildingBlock == null)
            return;

         if (shouldConvertPKSimModule && shouldShowModuleConversionWarning(buildingBlock))
            showModuleConversionWarning(buildingBlock);

         buildingBlock.Version = newVersion;

         if(shouldConvertPKSimModule)
            buildingBlock.Module.IsPKSimModule = false;

         publishSimulationStatusChangedEvents(buildingBlock);
         publishModuleStatusChangedEvents(buildingBlock);
      }

      private void showModuleConversionWarning(IBuildingBlock buildingBlock)
      {
         _dialogCreator.MessageBoxInfo(AppConstants.Captions.TheModuleWillBeConvertedFromPKSimToExtensionModule(buildingBlock.Module.Name));
      }

      private bool shouldShowModuleConversionWarning(IBuildingBlock buildingBlock)
      {
         return buildingBlock.Module != null && buildingBlock.Module.IsPKSimModule;
      }

      protected void publishModuleStatusChangedEvents(IBuildingBlock buildingBlock)
      {
         // Building blocks that are not contained by modules do not need to refresh when modified
         if (!_projectRetriever.Current.Modules.SelectMany(x => x.BuildingBlocks).Contains(buildingBlock))
            return;

         refreshModule(buildingBlock.Module);
      }

      private void refreshModule(Module module)
      {
         _eventPublisher.PublishEvent(new ModuleStatusChangedEvent(module));
      }

      public void UpdateBuildingBlockVersion(IBuildingBlock buildingBlock, bool shouldIncrementVersion, bool shouldConvertPKSimModule)
      {
         if (buildingBlock == null) return;
         var version = buildingBlock.Version;

         if (shouldIncrementVersion)
            version++;
         else
            version--;

         UpdateBuildingBlockVersion(buildingBlock, version, shouldConvertPKSimModule);
      }

      protected void publishSimulationStatusChangedEvents(IBuildingBlock changedBuildingBlock)
      {
         var affectedSimulations = _projectRetriever.Current.SimulationsUsing(changedBuildingBlock);
         affectedSimulations.Each(refreshSimulation);
      }

      private void refreshSimulation(IMoBiSimulation simulation)
      {
         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }
   }
}