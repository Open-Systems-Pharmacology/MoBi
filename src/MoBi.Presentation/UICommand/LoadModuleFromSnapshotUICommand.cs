using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadModuleFromSnapshotUICommand : ObjectUICommand<Module>
   {
      private readonly IPKSimStarter _pkSimStarter;
      private readonly IModuleLoader _moduleLoader;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IHeavyWorkManager _heavyWorkManager;

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
         SimulationTransfer transfer = null;
         _heavyWorkManager.Start(() =>
            {
               transfer = _pkSimStarter.LoadSimulationTransferFromSnapshot(Subject.Snapshot);
            }, AppConstants.Captions.Loading.WithEllipsis()
         );
         
         if(transfer != null)
            _moduleLoader.LoadProjectContentFromSimulationTransfer(_projectRetriever.Current, transfer);
      }
   }
}