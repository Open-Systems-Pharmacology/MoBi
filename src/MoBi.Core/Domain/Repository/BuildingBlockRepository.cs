using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Repository
{
   public interface IBuildingBlockRepository : IRepository<IBuildingBlock>
   {
      IEnumerable<TBuildingBlock> All<TBuildingBlock>() where TBuildingBlock : IBuildingBlock;
   }

   public class BuildingBlockRepository : IBuildingBlockRepository
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      
      public BuildingBlockRepository(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public IEnumerable<IBuildingBlock> All()
      {
         return All<IBuildingBlock>();
      }

      public IEnumerable<TBuildingBlock> All<TBuildingBlock>() where TBuildingBlock : IBuildingBlock
      {
         var currentProject = _projectRetriever.Current;
         if (currentProject == null)
            return new List<TBuildingBlock>();

         return currentProject.AllBuildingBlocks().OfType<TBuildingBlock>().Concat(moduleBuildingBlocks<TBuildingBlock>(currentProject));
      }

      private static IEnumerable<TBuildingBlock> moduleBuildingBlocks<TBuildingBlock>(MoBiProject currentProject) where TBuildingBlock : IBuildingBlock
      {
         return currentProject.Modules.SelectMany(x => x.BuildingBlocks.OfType<TBuildingBlock>());
      }
   }
}