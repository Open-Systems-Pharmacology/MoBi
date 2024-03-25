using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IMoleculeResolver
   {
      MoleculeBuilder Resolve(ObjectPath containerPath, string moleculeName, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock);
      MoleculeBuilder Resolve(ObjectPath containerPath, string moleculeName, SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules);
   }

   public class MoleculeResolver : IMoleculeResolver
   {
      private static bool canResolvePhysicalContainer(ObjectPath containerPath, SpatialStructure spatialStructure)
      {
         return spatialStructure
            .Select(containerPath.TryResolve<IContainer>)
            .FirstOrDefault(container => container != null && (container.Mode == ContainerMode.Physical)) != null;
      }

      public MoleculeBuilder Resolve(ObjectPath containerPath, string moleculeName, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock)
      {
         return Resolve(containerPath, moleculeName, spatialStructure, moleculeBuildingBlock.ToList());
      }

      public MoleculeBuilder Resolve(ObjectPath containerPath, string moleculeName, SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules)
      {
         return !canResolvePhysicalContainer(containerPath, spatialStructure) ? null : molecules.FindByName(moleculeName);
      }
   }
}