using System;
using System.Collections.Generic;
using System.Threading;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Tasks
{
   public class MergeTask : IMergeTask
   {
      private readonly IBuildingBlockTaskRetriever _buildingBlockTaskRetriever;

      public MergeTask(IBuildingBlockTaskRetriever buildingBlockTaskRetriever)
      {
         _buildingBlockTaskRetriever = buildingBlockTaskRetriever;
      }

      public IMoBiCommand MergeBuildingBlock(IBuildingBlock buildingBlockToMerge, IBuildingBlock targetBuildingBlock)
      {
         var interactionTask = _buildingBlockTaskRetriever.TaskFor(buildingBlockToMerge);
         return interactionTask.Merge(buildingBlockToMerge, targetBuildingBlock);
      }

      public IMoBiCommand MergeBuildingBlocks(IList<IBuildingBlock> buildingBlocksToMerge, IList<IBuildingBlock> targetBuildingBlocks, CancellationToken cancellationToken)
      {
         if (buildingBlocksToMerge.Count != targetBuildingBlocks.Count)
            throw new ArgumentException(AppConstants.Exceptions.MergeBuildingBlocksCountError);
         var mergeCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.MergeCommand,
            ObjectType =ObjectTypes.Project
         };

         for (var i = 0; i < buildingBlocksToMerge.Count; i++)
         {
            cancellationToken.ThrowIfCancellationRequested();
            mergeCommand.Add(MergeBuildingBlock(buildingBlocksToMerge[i], targetBuildingBlocks[i]));
         }
         return mergeCommand;
      }
   }
}