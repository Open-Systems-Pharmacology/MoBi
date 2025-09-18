using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class LoadModuleFromSnapshotUICommand : PKSimStarterSnapshotUICommand<Module>
   {
      private readonly IInteractionTasksForModule _interactionTasksForModule;

      public LoadModuleFromSnapshotUICommand(
         IInteractionTasksForModule interactionTasksForModule,
         IMoBiProjectRetriever projectRetriever,
         IHeavyWorkManager heavyWorkManager,
         IMoBiContext context) : base(projectRetriever, heavyWorkManager, context)
      {
         _interactionTasksForModule = interactionTasksForModule;
      }

      protected override IMoBiCommand AddToProject(Module transfer) => 
         _interactionTasksForModule.AddTo(transfer, _projectRetriever.Current, null);

      protected override Module LoadFromSnapshot() => 
         _interactionTasksForModule.LoadFromSnapshot(Subject.Snapshot);
   }
}