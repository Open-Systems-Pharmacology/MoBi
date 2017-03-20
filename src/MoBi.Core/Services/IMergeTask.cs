using System;
using System.Collections.Generic;
using System.Threading;
using MoBi.Core.Commands;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IMergeTask
   {
      /// <summary>
      /// Merge the building <paramref name="buildingBlockToMerge"/> in the <paramref name="targetBuildingBlock"/>
      /// </summary>
      /// <returns>The command resulting from the merge action</returns>
      IMoBiCommand MergeBuildingBlock(IBuildingBlock buildingBlockToMerge, IBuildingBlock targetBuildingBlock);


      /// <summary>
      /// Merge all the the <paramref name="buildingBlocksToMerge"/> in the corresponding <paramref name="targetBuildingBlocks"/>
      /// Corresponding is seen as building block having the same index in the list of targer building blocks
      /// </summary>
      /// <returns>The command resulting from the merge action</returns>
      /// <exception cref="ArgumentException">is thrown if the two list do not have the same size</exception>
      IMoBiCommand MergeBuildingBlocks(IList<IBuildingBlock> buildingBlocksToMerge, IList<IBuildingBlock> targetBuildingBlocks, CancellationToken cancellationToken);
   }
}