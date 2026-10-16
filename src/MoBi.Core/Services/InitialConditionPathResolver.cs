using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Core.Services
{
   public interface IInitialConditionPathResolver
   {
      /// <summary>
      ///    Resolves the molecule of the absolute <paramref name="initialConditionPath" /> in the building blocks of the project
      /// </summary>
      /// <param name="initialConditionPath">The absolute path of the initial condition, as it would be in a simulation</param>
      /// <param name="module">The module whose molecules are searched before the molecules of other modules</param>
      /// <returns>
      ///    The molecule named by the last element of <paramref name="initialConditionPath" /> if the rest of the path is a
      ///    physical container of a spatial structure, otherwise null
      /// </returns>
      MoleculeBuilder Resolve(ObjectPath initialConditionPath, Module module);
   }

   public class InitialConditionPathResolver : IInitialConditionPathResolver
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IObjectPathFactory _objectPathFactory;

      public InitialConditionPathResolver(IBuildingBlockRepository buildingBlockRepository, IObjectPathFactory objectPathFactory)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _objectPathFactory = objectPathFactory;
      }

      public MoleculeBuilder Resolve(ObjectPath initialConditionPath, Module module)
      {
         if (initialConditionPath.Count < 2)
            return null;

         var containerPath = initialConditionPath.Clone<ObjectPath>();
         containerPath.RemoveAt(containerPath.Count - 1);

         if (!physicalContainers.Any(x => Equals(_objectPathFactory.CreateAbsoluteObjectPath(x), containerPath)))
            return null;

         return _buildingBlockRepository.MoleculeBlockCollection.ModuleFirst(module).SelectMany(x => x).FirstOrDefault(x => x.IsNamed(initialConditionPath.Last()));
      }

      private IEnumerable<IContainer> physicalContainers => _buildingBlockRepository.SpatialStructureCollection.SelectMany(x => x.PhysicalContainers);
   }
}
