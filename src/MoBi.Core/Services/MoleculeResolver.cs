using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IMoleculeResolver
   {
      IMoleculeBuilder Resolve(ObjectPath containerPath, string moleculeName, ISpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock);
   }

   public class MoleculeResolver : IMoleculeResolver
   {
      private static bool canResolvePhysicalContainer(ObjectPath containerPath, ISpatialStructure spatialStructure)
      {
         return spatialStructure
            .Select(containerPath.TryResolve<IContainer>)
            .FirstOrDefault(container => container != null && (container.Mode == ContainerMode.Physical)) != null;
      }

      private bool canResolveMolecule(MoleculeBuildingBlock moleculeBuildingBlock, string moleculeName)
      {
         return moleculeBuildingBlock[moleculeName] != null;
      }

      public IMoleculeBuilder Resolve(ObjectPath containerPath, string moleculeName, ISpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock)
      {
         if (!canResolveMolecule(moleculeBuildingBlock, moleculeName) || !canResolvePhysicalContainer(containerPath, spatialStructure))
            return null;

         return moleculeBuildingBlock[moleculeName];
      }
   }
}