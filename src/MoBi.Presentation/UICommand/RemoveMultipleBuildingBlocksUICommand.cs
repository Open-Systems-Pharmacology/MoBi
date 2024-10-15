using System.Collections.Generic;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Tasks.Interaction.MoBi.Presentation.Tasks.Interaction;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveMultipleBuildingBlocksUICommand<TBuildingBlock> : ObjectUICommand<IReadOnlyList<TBuildingBlock>> where TBuildingBlock : class, IBuildingBlock
   {
      private readonly ITaskForMultipleBuildingBlocks<TBuildingBlock> _task;

      public RemoveMultipleBuildingBlocksUICommand(ITaskForMultipleBuildingBlocks<TBuildingBlock> task)
      {
         _task = task;
      }

      protected override void PerformExecute()
      {
         _task.RemoveBuildingBlocks(Subject);
      }
   }
}