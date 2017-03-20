using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Repository
{
   public interface IBuildingBlockRepository : IRepository<IBuildingBlock>
   {
      IEnumerable<TBuildingBlock> All<TBuildingBlock>() where TBuildingBlock : IBuildingBlock;
   }

   public class BuildingBlockRepository : IBuildingBlockRepository
   {
      private readonly IMoBiContext _context;

      public BuildingBlockRepository(IMoBiContext context)
      {
         _context = context;
      }

      public IEnumerable<IBuildingBlock> All()
      {
         return All<IBuildingBlock>();
      }

      public IEnumerable<TBuildingBlock> All<TBuildingBlock>() where TBuildingBlock : IBuildingBlock
      {
         if (_context.CurrentProject == null)
            return new List<TBuildingBlock>();

         return _context.CurrentProject.AllBuildingBlocks().OfType<TBuildingBlock>();
      }
   }
}