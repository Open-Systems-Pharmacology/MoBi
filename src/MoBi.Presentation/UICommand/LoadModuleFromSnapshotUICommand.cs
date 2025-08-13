using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadModuleFromSnapshotUICommand : ObjectUICommand<Module>
   {
      private readonly IPKSimStarter _pkSimStarter;
      private readonly IModuleLoader _moduleLoader;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private IHeavyWorkManager _heavyWorkManager;

      public LoadModuleFromSnapshotUICommand(IPKSimStarter pkSimStarter, 
         IModuleLoader moduleLoader, 
         IMoBiProjectRetriever projectRetriever, 
         IHeavyWorkManager heavyWorkManager)
      {
         _pkSimStarter = pkSimStarter;
         _moduleLoader = moduleLoader;
         _projectRetriever = projectRetriever;
         _heavyWorkManager = heavyWorkManager;
      }

      protected override void PerformExecute()
      {
         _heavyWorkManager.Start(() =>
         {
            _moduleLoader.LoadProjectContentFromSimulationTransfer(_projectRetriever.Current, _pkSimStarter.LoadSimulationTransferFromSnapshot(Subject.Snapshot));
         }, AppConstants.Captions.Loading.WithEllipsis());
      }
   }
}