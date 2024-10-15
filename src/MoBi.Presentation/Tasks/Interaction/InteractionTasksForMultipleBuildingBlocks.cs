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
      //Remove the Generic type
      //get the type of BB and based on it , use the correct implementation for removal.
      public interface ITaskForMultipleBuildingBlocks<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
      {
         void RemoveBuildingBlocks(IReadOnlyList<TBuildingBlock> buildingBlocks);
      } 

      public class TaskForMultipleBuildingBlocks<TBuildingBlock> :
         ITaskForMultipleBuildingBlocks<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock

      {
         
         //private readonly IBuildingBlockTaskRetriever _buildingBlockTaskRetriever;
         public void RemoveBuildingBlocks(IReadOnlyList<TBuildingBlock> buildingBlocks)
         {
            throw new NotImplementedException();
         }
      }
   } 
}