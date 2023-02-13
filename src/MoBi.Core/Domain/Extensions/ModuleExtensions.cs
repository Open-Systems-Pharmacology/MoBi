using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Extensions
{
   public static class ModuleExtensions
   {
      public static IReadOnlyList<IBuildingBlock> AllBuildingBlocks(this Module module)
      {
         var buildingBlocks = new List<IBuildingBlock>
         {
            module.SpatialStructure,
            module.EventGroup,
            module.PassiveTransport,
            module.Molecule,
            module.Observer,
            module.Reaction,
         };
         
         buildingBlocks.AddRange(module.ParameterStartValuesCollection);
         buildingBlocks.AddRange(module.MoleculeStartValuesCollection);
         
         return buildingBlocks.Where(x => x != null).ToList();
      }
   }
}