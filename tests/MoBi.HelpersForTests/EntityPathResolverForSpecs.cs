using OSPSuite.Core.Domain.Services;

namespace MoBi.HelpersForTests
{
   public class EntityPathResolverForSpecs : EntityPathResolver
   {
      public EntityPathResolverForSpecs() : base(new ObjectPathFactoryForSpecs())
      {
      }
   }
}