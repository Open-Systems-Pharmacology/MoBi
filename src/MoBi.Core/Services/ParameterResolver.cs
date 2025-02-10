using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IParameterResolver
   {
      /// <summary>
      /// Resolves a parameter with name and containerPath inside a spatial structure and molecule building block
      /// </summary>
      /// <param name="containerPath">The container path being searched</param>
      /// <param name="name">The parameter name being searched</param>
      /// <param name="spatialStructure">The spatial structure used to resolve the path</param>
      /// <param name="buildingBlock">The building block used to resolve the path</param>
      /// <returns>The matching parameter if found, otherwise null</returns>
      IParameter Resolve(ObjectPath containerPath, string name, SpatialStructure spatialStructure, MoleculeBuildingBlock buildingBlock);
   }

   public class ParameterResolver : IParameterResolver
   {
      private SpatialStructure _spatialStructure;
      private MoleculeBuildingBlock _moleculeBuildingBlock;

      public IParameter Resolve(ObjectPath containerPath, string name, SpatialStructure spatialStructure, MoleculeBuildingBlock buildingBlock)
      {
         _spatialStructure = spatialStructure;
         _moleculeBuildingBlock = buildingBlock;
         try
         {
            var parameter =
               resolveContainerParameterInSpatialStructure(containerPath, name) ??
               resolveLocalModeInMoleculeBuildingBlock(containerPath, name) ??
               resolveGlobalModeInMoleculeBuildingBlock(containerPath, name) ??
               resolveMoleculePropertiesInSpatialStructure(name) ??
               resolveMoleculeParameterInSpatialStructure(containerPath, name);

            return parameter;
         }
         finally
         {
            _spatialStructure = null;
            _moleculeBuildingBlock = null;
         }

      }

      private IParameter resolveMoleculeParameterInSpatialStructure(ObjectPath containerPath, string name)
      {
         if (!containerPath.Any() || !canResolveMoleculeContainerPath(containerPath))
            return null;

         var templatePath = containerPath.Clone<ObjectPath>();
         templatePath.RemoveAt(getMoleculeNameIndex(containerPath));
         templatePath.Add(Constants.MOLECULE_PROPERTIES);
         return resolveParameterInSpatialStructure(templatePath, name);
      }

      private static int getMoleculeNameIndex(ObjectPath containerPath)
      {
         // Molecule name is always at the end of the path
         return containerPath.Count - 1;
      }

      private bool canResolveMoleculeContainerPath(ObjectPath containerPath)
      {
         if (!containerPath.Any())
            return false;

         var templatePath = containerPath.Clone<ObjectPath>();
         templatePath.RemoveAt(getMoleculeNameIndex(containerPath));

         return resolveContainerPathInSpatialStructure(templatePath) != null;
      }

      /// <summary>
      /// Resolves global parameters in the molecule building block
      /// </summary>
      /// <param name="containerPath">The path being searched</param>
      /// <param name="parameterName">The name of the parameter</param>
      /// <returns>The matching property if found, otherwise null</returns>
      private IParameter resolveGlobalModeInMoleculeBuildingBlock(ObjectPath containerPath, string parameterName)
      {
         return containerPath.Count > 1 ? null : resolveInMoleculeBuildingBlock(containerPath, parameterName, ParameterBuildMode.Global);
      }

      /// <summary>
      /// Resolves local parameters in the molecule building block
      /// </summary>
      /// <param name="containerPath">The path being searched</param>
      /// <param name="parameterName">The name of the parameter</param>
      /// <returns>The matching property if found, otherwise null</returns>
      private IParameter resolveLocalModeInMoleculeBuildingBlock(ObjectPath containerPath, string parameterName)
      {
         return !canResolveMoleculeContainerPath(containerPath) ? null : resolveInMoleculeBuildingBlock(containerPath, parameterName, ParameterBuildMode.Local);
      }

      private IParameter resolveInMoleculeBuildingBlock(ObjectPath containerPath, string parameterName, ParameterBuildMode buildMode)
      {
         if (!containerPath.Any())
            return null;

         var moleculeName = containerPath.Last();
         var molecule = _moleculeBuildingBlock[moleculeName];
         if (molecule == null) return null;

         var parameter = molecule.Parameters.FirstOrDefault(p => p.IsNamed(parameterName) && p.BuildMode == buildMode);
         return parameter;
      }

      /// <summary>
      /// Resolves parameters through the global spatial structure molecule properties
      /// </summary>
      /// <param name="parameterName">The name of the property</param>
      /// <returns>The property if found, otherwise null</returns>
      private IParameter resolveMoleculePropertiesInSpatialStructure(string parameterName)
      {
         return _spatialStructure.GlobalMoleculeDependentProperties.GetSingleChildByName<IParameter>(parameterName);
      }

      /// <summary>
      /// Searches the spatial structure through the entire depth for the matching parameter
      /// </summary>
      /// <param name="containerPath">The path being searched</param>
      /// <param name="parameterName">The name of the property</param>
      /// <returns>The matching property if found, otherwise null</returns>
      private IParameter resolveContainerParameterInSpatialStructure(ObjectPath containerPath, string parameterName)
      {
         return resolveParameterInSpatialStructure(containerPath, parameterName);
      }

      private IParameter resolveParameterInSpatialStructure(ObjectPath containerPath, string parameterName)
      {
         var parameterPath = containerPath.Clone<ObjectPath>().AndAdd(parameterName);
         return resolveParameterPathInSpatialStructure(parameterPath);
      }

      private T resolvePathInSpatialStructure<T>(ObjectPath objectPath) where T : class
      {
         return _spatialStructure.Select(objectPath.TryResolve<T>)
            .FirstOrDefault(parameter => parameter != null);
      }

      private IContainer resolveContainerPathInSpatialStructure(ObjectPath templatePath)
      {
         return resolvePathInSpatialStructure<IContainer>(templatePath);
      }

      private IParameter resolveParameterPathInSpatialStructure(ObjectPath parameterPath)
      {
         return resolvePathInSpatialStructure<IParameter>(parameterPath);
      }
   }
}