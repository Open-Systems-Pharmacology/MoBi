using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class CommitSimulationChangesToBuildingBlockUICommand : IUICommand
   {
      private IBuildingBlock _templateBuildingBlock;
      private IMoBiSimulation _simulation;
      private readonly ICreateCommitChangesToBuildingBlockCommandTaskRetriever _commitTaskRetriever;
      private readonly IMoBiContext _context;
      private ICreateCommitChangesToBuildingBlockCommandTask _commitTask;
      private readonly IPendingChangesChecker _pendingChangesChecker;

      public CommitSimulationChangesToBuildingBlockUICommand(ICreateCommitChangesToBuildingBlockCommandTaskRetriever commitTaskRetriever,
         IMoBiContext context, IPendingChangesChecker pendingChangesChecker)
      {
         _commitTaskRetriever = commitTaskRetriever;
         _context = context;
         _pendingChangesChecker = pendingChangesChecker;
      }

      public CommitSimulationChangesToBuildingBlockUICommand Initialize(IBuildingBlock templateBuildingBlock, IMoBiSimulation simulation)
      {
         _templateBuildingBlock = templateBuildingBlock;
         _simulation = simulation;
         _commitTask = _commitTaskRetriever.TaskFor(templateBuildingBlock);
         return this;
      }

      public void Execute()
      {
         try
         {
            var commitToBuildingBlockCommand = _commitTask.CreateCommitToBuildingBlockCommand(_simulation, _templateBuildingBlock);
            if (commitToBuildingBlockCommand.IsEmpty())
               return;

            _context.AddToHistory(commitToBuildingBlockCommand.Run(_context));
            // _pendingChangesChecker.CheckForBuildingBlockChanges(_simulation.MoBiBuildConfiguration.BuildingInfoForTemplate(_templateBuildingBlock), _templateBuildingBlock);
         }
         finally
         {
            _templateBuildingBlock = null;
            _simulation = null;
         }
      }
   }
}