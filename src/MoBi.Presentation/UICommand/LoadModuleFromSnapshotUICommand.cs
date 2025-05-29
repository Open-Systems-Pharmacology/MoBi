using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadModuleFromSnapshotUICommand : ObjectUICommand<Module>
   {
      private readonly IPKSimStarter _pkSimStarter;
      private readonly IModuleLoader _moduleLoader;
      private readonly IMoBiProjectRetriever _projectRetriever;
      

      public LoadModuleFromSnapshotUICommand(IPKSimStarter pkSimStarter, IModuleLoader moduleLoader, IMoBiProjectRetriever projectRetriever)
      {
         _pkSimStarter = pkSimStarter;
         _moduleLoader = moduleLoader;
         _projectRetriever = projectRetriever;
      }
      protected override void PerformExecute() => 
         _moduleLoader.LoadProjectContentFromSimulationTransfer(_projectRetriever.Current, _pkSimStarter.LoadSimulationTransferFromSnapshot(Subject.Snapshot));
   }
}