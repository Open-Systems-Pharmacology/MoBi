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
      IParameter Resolve(IObjectPath containerPath, string name, ISpatialStructure spatialStructure, IMoleculeBuildingBlock buildingBlock);
   }

   public class ParameterResolver : IParameterResolver
   {
      private ISpatialStructure _spatialStructure;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;

      public IParameter Resolve(IObjectPath containerPath, string name, ISpatialStructure spatialStructure, IMoleculeBuildingBlock buildingBlock)
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

      private IParameter resolveMoleculeParameterInSpatialStructure(IObjectPath containerPath, string name)
      {
         if (!containerPath.Any() || !canResolveMoleculeContainerPath(containerPath))
            return null;

         var templatePath = containerPath.Clone<IObjectPath>();
         templatePath.RemoveAt(getMoleculeNameIndex(containerPath));
         templatePath.Add(Constants.MOLECULE_PROPERTIES);
         return resolveParameterInSpatialStructure(templatePath, name);
      }

      private static int getMoleculeNameIndex(IObjectPath containerPath)
      {
         // Molecule name is always at the end of the path
         return containerPath.Count - 1;
      }

      private bool canResolveMoleculeContainerPath(IObjectPath containerPath)
      {
         if (!containerPath.Any())
            return false;

         var templatePath = containerPath.Clone<IObjectPath>();
         templatePath.RemoveAt(getMoleculeNameIndex(containerPath));

         return resolveContainerPathInSpatialStructure(templatePath) != null;
      }

      /// <summary>
      /// Resolves global parameters in the molecule building block
      /// </summary>
      /// <param name="containerPath">The path being searched</param>
      /// <param name="parameterName">The name of the parameter</param>
      /// <returns>The matching property if found, otherwise null</returns>
      private IParameter resolveGlobalModeInMoleculeBuildingBlock(IObjectPath containerPath, string parameterName)
      {
         return containerPath.Count > 1 ? null : resolveInMoleculeBuildingBlock(containerPath, parameterName, ParameterBuildMode.Global);
      }

      /// <summary>
      /// Resolves local parameters in the molecule building block
      /// </summary>
      /// <param name="containerPath">The path being searched</param>
      /// <param name="parameterName">The name of the parameter</param>
      /// <returns>The matching property if found, otherwise null</returns>
      private IParameter resolveLocalModeInMoleculeBuildingBlock(IObjectPath containerPath, string parameterName)
      {
         return !canResolveMoleculeContainerPath(containerPath) ? null : resolveInMoleculeBuildingBlock(containerPath, parameterName, ParameterBuildMode.Local);
      }

      private IParameter resolveInMoleculeBuildingBlock(IObjectPath containerPath, string parameterName, ParameterBuildMode buildMode)
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
      private IParameter resolveContainerParameterInSpatialStructure(IObjectPath containerPath, string parameterName)
      {
         return resolveParameterInSpatialStructure(containerPath, parameterName);
      }

      private IParameter resolveParameterInSpatialStructure(IObjectPath containerPath, string parameterName)
      {
         var parameterPath = containerPath.Clone<IObjectPath>().AndAdd(parameterName);
         return resolveParameterPathInSpatialStructure(parameterPath);
      }

      private T resolvePathInSpatialStructure<T>(IObjectPath objectPath) where T : class
      {
         return _spatialStructure.Select(objectPath.TryResolve<T>)
            .FirstOrDefault(parameter => parameter != null);
      }

      private IContainer resolveContainerPathInSpatialStructure(IObjectPath templatePath)
      {
         return resolvePathInSpatialStructure<IContainer>(templatePath);
      }

      private IParameter resolveParameterPathInSpatialStructure(IObjectPath parameterPath)
      {
         return resolvePathInSpatialStructure<IParameter>(parameterPath);
      }
   }
}