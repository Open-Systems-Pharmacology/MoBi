using System.Linq;
using MoBi.Core.Domain.Model;
using SBSuite.Core.Domain;
using SBSuite.Core.Domain.Builder;
using SBSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Serialization.Converter.v3_1
{
   public interface IDimensionRetriever
   {
      IDimension GetDimensionFor(IParameterStartValue psv, IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IMoBiProject project);
      IDimension GetDimensionFor(IParameterStartValue psv, IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IMoBiSimulation simulation);
   }

   internal class DimensionRetriever : IDimensionRetriever
   {
      public IDimension GetDimensionFor(IParameterStartValue psv, IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IMoBiSimulation simulation)
      {
         var parameter = psv.Path.Resolve<IParameter>(simulation.Model.Root);
         return parameter == null ? null : parameter.Dimension;
      }

      public IDimension GetDimensionFor(IParameterStartValue psv, IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IMoBiProject project)
      {
         if (project == null) return null;

         var parameter =
            searchParameterInSimulations(psv, parameterStartValuesBuildingBlock,project) ??
            searchParamterInSpatialStructures(psv, parameterStartValuesBuildingBlock, project) ??
            searchParameterInMolecules(psv, parameterStartValuesBuildingBlock, project);

         return parameter == null ? null : parameter.Dimension;
      }

      private IParameter searchParameterInMolecules(IParameterStartValue psv, IParameterStartValuesBuildingBlock parent, IMoBiProject project)
      {
         var parameterName = psv.Path.Last();
         var tmpPath = psv.Path.Clone<IObjectPath>();
         tmpPath.Remove(tmpPath.Last());
         var moleculeName = tmpPath.Last();

         var parameter = searchParameterInMoleculeBuildingBlock(project.MoleculeBlockCollection.FirstOrDefault(mb => mb.Id.Equals(parent.MoleculeBuildingBlockId)), moleculeName, parameterName);

         foreach (var moleculeBuildingBlock in project.MoleculeBlockCollection)
         {
            parameter = searchParameterInMoleculeBuildingBlock(moleculeBuildingBlock, moleculeName, parameterName);
            if (parameter != null) break;
         }
         return parameter;
      }

      private static IParameter searchParameterInMoleculeBuildingBlock(IMoleculeBuildingBlock moleculeBuildingBlock,
                                                                       string moleculeName, string parameterName)
      {
         IParameter parameter = null;
         if (moleculeBuildingBlock != null)
         {
            var moleculeBuilder = moleculeBuildingBlock[moleculeName];
            if (moleculeBuilder != null)
            {
               parameter = moleculeBuilder
                  .GetChildren<IParameter>(p => !p.BuildMode.Equals(ParameterBuildMode.Property))
                  .FirstOrDefault(p => p.Name.Equals(parameterName));
            }
         }
         return parameter;
      }

      private IParameter searchParamterInSpatialStructures(IParameterStartValue psv, IParameterStartValuesBuildingBlock parent, IMoBiProject project)
      {
         var parameterPath = psv.Path;
         IParameter parameter = searchParamterInSpatialStructures(parameterPath, parent,project);
         if (parameter == null)
         {
            parameterPath = psv.Path.Clone<IObjectPath>();
            var parameterName = parameterPath.Last();
            parameterPath.Remove(parameterName);
            var tmp = parameterPath.Last();
            parameterPath.Remove(tmp);
            parameterPath.Add(Constants.MOLECULE_PROPERTIES);
            parameterPath.Add(parameterName);
            parameter = searchParamterInSpatialStructures(parameterPath, parent,project);
         }
         return parameter;
      }

      private IParameter searchParamterInSpatialStructures(IObjectPath parameterPath, IParameterStartValuesBuildingBlock parent, IMoBiProject project)
      {
         IParameter parameter = searchParameterInContainers(parameterPath, project.SpatialStructureCollection.FirstOrDefault(ss => ss.Id.Equals(parent.SpatialStructureId)));

         if (parameter == null)
         {
            foreach (var spatialStructure in project.SpatialStructureCollection)
            {
               parameter = searchParameterInContainers(parameterPath, spatialStructure);
               if (parameter != null) break;
            }
         }
         return parameter;
      }

      private static IParameter searchParameterInContainers(IObjectPath parameterPath, IMoBiSpatialStructure spatialStructure)
      {
         if (spatialStructure == null) return null;
         IParameter parameter = null;
         foreach (var top in spatialStructure.TopContainers)
         {
            parameter = parameterPath.Resolve<IParameter>(top);
            if (parameter != null) break;
         }
         if (parameter == null)
         {
            parameter = parameterPath.Resolve<IParameter>(spatialStructure.NeighborhoodsContainer);
         }
         return parameter;
      }

      private IParameter searchParameterInSimulations(IParameterStartValue psv, IParameterStartValuesBuildingBlock parent, IMoBiProject project)
      {
         IParameter parameter = null;
         var simulations = project.Simulations.Where(sim => usesParameterStartValuesBB(sim, parent));
         foreach (var simulation in simulations)
         {
            parameter = psv.Path.Resolve<IParameter>(simulation.Model.Root);
            if (parameter != null) break;
         }
         return parameter;
      }

      private bool usesParameterStartValuesBB(IMoBiSimulation moBiSimulation, IParameterStartValuesBuildingBlock parameterStartValueBuildingBlock)
      {
         var templateBuildingBlock = moBiSimulation.MoBiBuildConfiguration.ParameterStartValuesInfo.TemplateBuildingBlock;

         if (Equals(templateBuildingBlock,parameterStartValueBuildingBlock))
            return true;

         var parameterStartValuesBuildingBlock = moBiSimulation.MoBiBuildConfiguration.ParameterStartValues;
         return parameterStartValuesBuildingBlock.Equals(parameterStartValueBuildingBlock) || parameterStartValuesBuildingBlock.Name.Equals(parameterStartValueBuildingBlock.Name);
      }
   }
}