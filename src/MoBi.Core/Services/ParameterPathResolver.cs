using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Core.Services
{
   public interface IParameterPathResolver
   {
      /// <summary>
      ///    Resolves the absolute <paramref name="parameterPath" /> in the building blocks of the project
      /// </summary>
      /// <param name="parameterPath">The absolute path of the parameter, as it would be in a simulation</param>
      /// <param name="module">The module whose building blocks are searched before the building blocks of other modules</param>
      /// <returns>The parameter or path and value entity defined at <paramref name="parameterPath" /> if found, otherwise null</returns>
      IWithDisplayUnit Resolve(ObjectPath parameterPath, Module module);
   }

   public class ParameterPathResolver : IParameterPathResolver
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public ParameterPathResolver(IBuildingBlockRepository buildingBlockRepository)
      {
         _buildingBlockRepository = buildingBlockRepository;
      }

      public IWithDisplayUnit Resolve(ObjectPath parameterPath, Module module)
      {
         if (parameterPath.Count < 2)
            return null;

         return resolveIn(allContainers(module), parameterPath) ??
                resolveInEventGroups(parameterPath, module) ??
                resolveLocalMoleculeParameter(parameterPath, module) ??
                resolveMoleculePropertiesParameter(parameterPath, module) ??
                findIn(_buildingBlockRepository.IndividualsCollection, parameterPath) ??
                findIn(_buildingBlockRepository.ExpressionProfileCollection, parameterPath);
      }

      private IEnumerable<IContainer> allContainers(Module module) =>
         spatialStructureContainers(module)
            .Concat(molecules(module))
            .Concat(_buildingBlockRepository.ReactionBlockCollection.ModuleFirst(module).SelectMany(x => x));

      private IEnumerable<IContainer> spatialStructureContainers(Module module) => _buildingBlockRepository.SpatialStructureCollection.ModuleFirst(module).SelectMany(x => x);

      private IEnumerable<MoleculeBuilder> molecules(Module module) => _buildingBlockRepository.MoleculeBlockCollection.ModuleFirst(module).SelectMany(x => x);

      private IParameter resolveInEventGroups(ObjectPath parameterPath, Module module)
      {
         if (!string.Equals(parameterPath.First(), Constants.EVENTS))
            return null;

         var pathInEventGroup = parameterPath.Clone<ObjectPath>();
         pathInEventGroup.RemoveFirst();
         return resolveIn(_buildingBlockRepository.EventBlockCollection.ModuleFirst(module).SelectMany(x => x), pathInEventGroup);
      }

      private IParameter resolveLocalMoleculeParameter(ObjectPath parameterPath, Module module)
      {
         var containerPath = new ObjectPath(parameterPath.Take(parameterPath.Count - 2));
         if (!spatialStructureContainers(module).Any(x => containerPath.TryResolve<IContainer>(x) != null))
            return null;

         var moleculeParameterPath = new ObjectPath(parameterPath.Skip(parameterPath.Count - 2));
         return resolveIn(molecules(module), moleculeParameterPath);
      }

      private IParameter resolveMoleculePropertiesParameter(ObjectPath parameterPath, Module module)
      {
         var moleculePropertiesPath = parameterPath.Clone<ObjectPath>();
         moleculePropertiesPath[moleculePropertiesPath.Count - 2] = Constants.MOLECULE_PROPERTIES;
         return resolveIn(spatialStructureContainers(module), moleculePropertiesPath);
      }

      private static IParameter resolveIn(IEnumerable<IContainer> containers, ObjectPath path) =>
         containers.Select(path.TryResolve<IParameter>).FirstOrDefault(x => x != null);

      private static IWithDisplayUnit findIn<T>(IEnumerable<PathAndValueEntityBuildingBlock<T>> buildingBlocks, ObjectPath path) where T : PathAndValueEntity =>
         buildingBlocks.Select(x => x[path]).FirstOrDefault(x => x != null);
   }
}
