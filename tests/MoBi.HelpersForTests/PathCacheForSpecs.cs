using OSPSuite.Core.Domain;

namespace MoBi.HelpersForTests
{
   public class PathCacheForSpecs<T> : PathCache<T> where T : class, IEntity
   {
      public PathCacheForSpecs() : base(new EntityPathResolverForSpecs())
      {
      }
   }
}