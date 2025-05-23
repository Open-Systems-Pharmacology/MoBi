using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Services
{
   public interface ISimulationEntitySourceUpdater
   {
      void UpdateEntitySourcesForContainerRename(ObjectPath newPath, ObjectPath originalPath, IBuildingBlock buildingBlock);

      void UpdateEntitySourcesForEntityRename(ObjectPath newPath, ObjectPath originalPath, IBuildingBlock buildingBlock);

      void UpdateEntitySourcesForModuleRename(string oldModuleName, string newModuleName);

      void UpdateEntitySourcesForBuildingBlockRename(string oldName, IBuildingBlock renamedBuildingBlock);

      void UpdateEntitySourcesForModuleRename(string oldModuleName, string newModuleName, IMoBiSimulation simulation);

      void UpdateEntitySourcesForBuildingBlockRename(string oldName, IBuildingBlock renamedBuildingBlock, IMoBiSimulation simulation);

      void UpdateSourcesForNewPathAndValueEntity<TPathAndValueEntity>(PathAndValueEntityBuildingBlock<TPathAndValueEntity> buildingBlock, ObjectPath objectPath, IMoBiSimulation simulation) where TPathAndValueEntity : PathAndValueEntity;
   }

   public class SimulationEntitySourceUpdater : ISimulationEntitySourceUpdater
   {
      private readonly IMoBiProjectRetriever _projectRetriever;

      public SimulationEntitySourceUpdater(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public void UpdateEntitySourcesForEntityRename(ObjectPath newPath, ObjectPath originalPath, IBuildingBlock buildingBlock) =>
         _projectRetriever.Current.Simulations.Where(x => x.Uses(buildingBlock)).Each(simulation => updateEntitySourcePathsInSimulation(newPath, originalPath, simulation, buildingBlock));

      public void UpdateEntitySourcesForModuleRename(string oldModuleName, string newModuleName) =>
         _projectRetriever.Current.Simulations.Each(simulation => UpdateEntitySourcesForModuleRename(oldModuleName, newModuleName, simulation));

      public void UpdateEntitySourcesForBuildingBlockRename(string oldName, IBuildingBlock renamedBuildingBlock) =>
         _projectRetriever.Current.Simulations.Each(simulation => UpdateEntitySourcesForBuildingBlockRename(oldName, renamedBuildingBlock, simulation));

      public void UpdateEntitySourcesForContainerRename(ObjectPath newPath, ObjectPath originalPath, IBuildingBlock buildingBlock) =>
         _projectRetriever.Current.Simulations.Where(x => x.Uses(buildingBlock)).Each(simulation => updateEntitySourceContainerPathsInSimulation(newPath, originalPath, simulation, buildingBlock));

      public void UpdateEntitySourcesForModuleRename(string oldModuleName, string newModuleName, IMoBiSimulation simulation)
      {
         // ToList needed because iteration modifies the enumerable
         simulation.EntitySources.Where(x => string.Equals(x.ModuleName, oldModuleName)).ToList().Each(x => updateModuleNameInEntitySources(newModuleName, simulation.EntitySources, x));
      }

      public void UpdateEntitySourcesForBuildingBlockRename(string oldName, IBuildingBlock renamedBuildingBlock, IMoBiSimulation simulation)
      {
         // ToList needed because iteration modifies the enumerable
         simulation.EntitySources.Where(x => buildingBlockIsTemplateMatchFor(x, oldName, renamedBuildingBlock)).ToList()
            .Each(x => updateBuildingBlockNameInEntitySources(renamedBuildingBlock.Name, simulation.EntitySources, x));
      }

      public void UpdateSourcesForNewPathAndValueEntity<TPathAndValueEntity>(PathAndValueEntityBuildingBlock<TPathAndValueEntity> buildingBlock, ObjectPath objectPath, IMoBiSimulation simulation) where TPathAndValueEntity : PathAndValueEntity
      {
         simulation.EntitySources.Where(x => x.SimulationEntityPath.Equals(objectPath.PathAsString)).ToList().Each(source =>
            updateBuildingBlockInEntitySources(buildingBlock.Module?.Name, buildingBlock.Name, simulation.EntitySources, source.SimulationEntityPath, buildingBlock.GetType().Name, objectPath));
      }

      private static void updateBuildingBlockNameInEntitySources(string newBuildingBlockName, SimulationEntitySources sources, SimulationEntitySource simulationEntitySource) =>
         updateBuildingBlockInEntitySources(simulationEntitySource.ModuleName, newBuildingBlockName, sources, simulationEntitySource.SimulationEntityPath, simulationEntitySource.BuildingBlockType, simulationEntitySource.SourcePath);

      private static void updateBuildingBlockInEntitySources(string moduleName, string newBuildingBlockName, SimulationEntitySources sources, string simulationEntityPath, string buildingBlockType, string sourcePath)
      {
         sources.Add(new SimulationEntitySource(simulationEntityPath, newBuildingBlockName, buildingBlockType, moduleName, sourcePath));
      }

      private static bool buildingBlockIsTemplateMatchFor(SimulationEntitySource simulationEntitySource, string buildingBlockName, IBuildingBlock buildingBlock)
      {
         return string.Equals(buildingBlockName, simulationEntitySource.BuildingBlockName) &&
                (buildingBlock.Module == null ? string.IsNullOrEmpty(simulationEntitySource.ModuleName) : string.Equals(buildingBlock.Module.Name, simulationEntitySource.ModuleName)) &&
                string.Equals(buildingBlock.GetType().Name, simulationEntitySource.BuildingBlockType);
      }

      private static void updateEntitySourcePathsInSimulation(ObjectPath newPath, ObjectPath originalPath, IMoBiSimulation simulation, IBuildingBlock buildingBlock) =>
         simulation.EntitySources.Where(x => isTemplateMatchForEntity(originalPath, x, buildingBlock)).ToList().Each(source => updateEntitySourcePath(newPath, simulation, source));

      private static bool isTemplateMatchForEntity(ObjectPath originalPath, SimulationEntitySource entitySource, IBuildingBlock buildingBlock) =>
         buildingBlockIsTemplateMatchFor(entitySource, buildingBlock.Name, buildingBlock) && entitySource.SourcePath.Equals(originalPath.PathAsString);

      private static void updateEntitySourceContainerPathsInSimulation(ObjectPath newPath, ObjectPath originalPath, IMoBiSimulation simulation, IBuildingBlock buildingBlock) =>
         simulation.EntitySources.Where(x => isTemplateMatchForContainer(originalPath, x, buildingBlock)).ToList().Each(source => { updateEntitySourcePath(source.SourcePath.Replace(originalPath, newPath).ToObjectPath(), simulation, source); });

      private static bool isTemplateMatchForContainer(ObjectPath originalPath, SimulationEntitySource entitySource, IBuildingBlock buildingBlock) =>
         buildingBlockIsTemplateMatchFor(entitySource, buildingBlock.Name, buildingBlock) && entitySource.SourcePath.Contains(originalPath.PathAsString);

      private static void updateEntitySourcePath(ObjectPath newPath, IMoBiSimulation simulation, SimulationEntitySource source) =>
         simulation.EntitySources.Add(new SimulationEntitySource(source.SimulationEntityPath, source.BuildingBlockName, source.BuildingBlockType, source.ModuleName, newPath.PathAsString));

      private static void updateModuleNameInEntitySources(string newModuleName, SimulationEntitySources sources, SimulationEntitySource entitySource) =>
         sources.Add(new SimulationEntitySource(entitySource.SimulationEntityPath, entitySource.BuildingBlockName, entitySource.BuildingBlockType, newModuleName, entitySource.SourcePath));
   }
}