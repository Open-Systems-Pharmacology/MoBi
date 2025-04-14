using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain
{
   public class EntitySourceReference
   {
      public IObjectBase Source { get; }
      public IBuildingBlock BuildingBlock { get; }
      public Module Module { get; }

      public EntitySourceReference(IObjectBase source, IBuildingBlock buildingBlock, Module module)
      {
         Source = source;
         BuildingBlock = buildingBlock;
         Module = module;
      }
   }
}