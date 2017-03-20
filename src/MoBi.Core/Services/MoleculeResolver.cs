using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IMoleculeResolver
   {
      IMoleculeBuilder Resolve(IObjectPath containerPath, string moleculeName, ISpatialStructure spatialStructure, IMoleculeBuildingBlock moleculeBuildingBlock);
   }

   public class MoleculeResolver : IMoleculeResolver
   {
      private static bool canResolvePhysicalContainer(IObjectPath containerPath, ISpatialStructure spatialStructure)
      {
         return spatialStructure
            .Select(containerPath.TryResolve<IContainer>)
            .FirstOrDefault(container => container != null && (container.Mode == ContainerMode.Physical)) != null;
      }

      private bool canResolveMolecule(IMoleculeBuildingBlock moleculeBuildingBlock, string moleculeName)
      {
         return moleculeBuildingBlock[moleculeName] != null;
      }

      public IMoleculeBuilder Resolve(IObjectPath containerPath, string moleculeName, ISpatialStructure spatialStructure, IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         if (!canResolveMolecule(moleculeBuildingBlock, moleculeName) || !canResolvePhysicalContainer(containerPath, spatialStructure))
            return null;

         return moleculeBuildingBlock[moleculeName];
      }
   }
}