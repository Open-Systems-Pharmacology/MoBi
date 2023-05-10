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
         return _projectRetriever.Current.AllBuildingBlocks();
      }
   }
}