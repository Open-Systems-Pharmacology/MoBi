using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain
{
   public class EntitySourceReference
   {
      public IEntity Source { get; }
      public IBuildingBlock BuildingBlock { get; }
      public Module Module { get; }

      public EntitySourceReference(IEntity source, IBuildingBlock buildingBlock, Module module)
      {
         Source = source;
         BuildingBlock = buildingBlock;
         Module = module;
      }
   }
}