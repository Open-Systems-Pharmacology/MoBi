using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   namespace MoBi.Presentation.Tasks.Interaction
   {
      public interface IInteractionTasksForMultipleBuildingBlocks
      {
         void RemoveBuildingBlocks(IReadOnlyList<IBuildingBlock> buildingBlocks);
      } 

      public class InteractionTasksForMultipleBuildingBlocks: 
         IInteractionTasksForMultipleBuildingBlocks
      
      {
         private readonly IInteractionTasksForChildren<Module, BuildingBlock> _interactionTasks;
         private readonly IBuildingBlockTaskRetriever _buildingBlockTaskRetriever;
         public InteractionTasksForMultipleBuildingBlocks(IInteractionTasksForChildren<Module, BuildingBlock> interactionTasks)
         {
            _interactionTasks = interactionTasks;
         }

         public void RemoveBuildingBlocks(IReadOnlyList<IBuildingBlock> buildingBlocks)
         {
            buildingBlocks.Each(x=> _interactionTasks.Remove(x as BuildingBlock, x.Module, x, true));            
         }
      }
   } 
}