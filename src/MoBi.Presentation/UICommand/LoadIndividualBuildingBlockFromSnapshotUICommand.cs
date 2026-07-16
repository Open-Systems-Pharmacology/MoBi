using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class LoadIndividualBuildingBlockFromSnapshotUICommand : PKSimStarterSnapshotUICommand<IndividualBuildingBlock>
   {
      private readonly IInteractionTasksForIndividualBuildingBlock _interactionTasks;

      public LoadIndividualBuildingBlockFromSnapshotUICommand(
         IMoBiProjectRetriever projectRetriever,
         IHeavyWorkManager heavyWorkManager,
         IInteractionTasksForIndividualBuildingBlock interactionTasks,
         IMoBiContext context) : base(projectRetriever, heavyWorkManager, context)
      {
         _interactionTasks = interactionTasks;
      }

      protected override IMoBiCommand AddToProject(IndividualBuildingBlock buildingBlock)
      {
         return _interactionTasks.AddToProject(buildingBlock);
      }

      protected override IndividualBuildingBlock LoadFromSnapshot()
      {
         return _interactionTasks.LoadFromSnapshot(Subject.Snapshot);
      }
   }
}