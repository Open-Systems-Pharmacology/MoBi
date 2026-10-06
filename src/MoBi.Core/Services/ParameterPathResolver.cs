using System.Collections.Generic;
using System.Linq;
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
      /// <returns>The parameter or path and value entity defined at <paramref name="parameterPath" /> if found, otherwise null</returns>
      IWithDisplayUnit Resolve(ObjectPath parameterPath);
   }

   public class ParameterPathResolver : IParameterPathResolver
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public ParameterPathResolver(IBuildingBlockRepository buildingBlockRepository)
      {
         _buildingBlockRepository = buildingBlockRepository;
      }

      public IWithDisplayUnit Resolve(ObjectPath parameterPath)
      {
         if (parameterPath.Count < 2)
            return null;

         return resolveIn(allContainers, parameterPath) ??
                resolveInEventGroups(parameterPath) ??
                resolveLocalMoleculeParameter(parameterPath) ??
                resolveMoleculePropertiesParameter(parameterPath) ??
                findIn(_buildingBlockRepository.IndividualsCollection, parameterPath) ??
                findIn(_buildingBlockRepository.ExpressionProfileCollection, parameterPath);
      }

      private IEnumerable<IContainer> allContainers =>
         spatialStructureContainers
            .Concat(_buildingBlockRepository.MoleculeBlockCollection.SelectMany(x => x))
            .Concat(_buildingBlockRepository.ReactionBlockCollection.SelectMany(x => x));

      private IEnumerable<IContainer> spatialStructureContainers => _buildingBlockRepository.SpatialStructureCollection.SelectMany(x => x);

      private IParameter resolveInEventGroups(ObjectPath parameterPath)
      {
         if (!string.Equals(parameterPath.First(), Constants.EVENTS))
            return null;

         var pathInEventGroup = parameterPath.Clone<ObjectPath>();
         pathInEventGroup.RemoveFirst();
         return resolveIn(_buildingBlockRepository.EventBlockCollection.SelectMany(x => x), pathInEventGroup);
      }

      private IParameter resolveLocalMoleculeParameter(ObjectPath parameterPath)
      {
         var moleculeParameterPath = new ObjectPath(parameterPath.Skip(parameterPath.Count - 2));
         return resolveIn(_buildingBlockRepository.MoleculeBlockCollection.SelectMany(x => x), moleculeParameterPath);
      }

      private IParameter resolveMoleculePropertiesParameter(ObjectPath parameterPath)
      {
         var moleculePropertiesPath = parameterPath.Clone<ObjectPath>();
         moleculePropertiesPath[moleculePropertiesPath.Count - 2] = Constants.MOLECULE_PROPERTIES;
         return resolveIn(spatialStructureContainers, moleculePropertiesPath);
      }

      private static IParameter resolveIn(IEnumerable<IContainer> containers, ObjectPath path) =>
         containers.Select(path.TryResolve<IParameter>).FirstOrDefault(x => x != null);

      private static IWithDisplayUnit findIn<T>(IEnumerable<PathAndValueEntityBuildingBlock<T>> buildingBlocks, ObjectPath path) where T : PathAndValueEntity =>
         buildingBlocks.Select(x => x[path]).FirstOrDefault(x => x != null);
   }
}
