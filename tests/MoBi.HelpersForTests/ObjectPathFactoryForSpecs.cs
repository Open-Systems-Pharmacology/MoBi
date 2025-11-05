using OSPSuite.Core.Domain;

namespace MoBi.HelpersForTests
{
   public class ObjectPathFactoryForSpecs : ObjectPathFactory
   {
      public ObjectPathFactoryForSpecs() : base(new AliasCreator())
      {
      }
   }
}
