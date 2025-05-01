using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Services
{
   public interface IRenameInSimulationTask
   {
      /// <summary>
      ///    Find building blocks in all simulations with <paramref name="oldName" /> and rename them to match the
      ///    <paramref name="templateBuildingBlock" /> name
      /// </summary>
      void RenameInSimulationUsingTemplateBuildingBlock(string oldName, IBuildingBlock templateBuildingBlock);

      /// <summary>
      ///    Find modules in all simulations with <paramref name="oldModuleName" /> and rename them to match the
      ///    <paramref name="templateModule" /> name
      /// </summary>
      void RenameInSimulationUsingTemplateModule(string oldModuleName, Module templateModule);
   }

   public class RenameInSimulationTask : IRenameInSimulationTask
   {
      private readonly IMoBiProjectRetriever _projectRetriever;

      public RenameInSimulationTask(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public void RenameInSimulationUsingTemplateBuildingBlock(string oldName, IBuildingBlock templateBuildingBlock)
      {
         getBuildingBlocksWithMatchingNameAndType(oldName, templateBuildingBlock).Each(simulationAndBuildingBlock =>
         {
            simulationAndBuildingBlock.Simulation.HasChanged = true;
            simulationAndBuildingBlock.BuildingBlock.Name = templateBuildingBlock.Name;

            // ToList needed because iteration modifies the enumerable
            simulationAndBuildingBlock.Simulation.EntitySources.Where(x => buildingBlockIsTemplateMatchFor(x, oldName, templateBuildingBlock)).ToList()
               .Each(x => updateBuildingBlockNameInEntitySources(templateBuildingBlock.Name, simulationAndBuildingBlock.Simulation.EntitySources, x));
         });
      }

      private void updateBuildingBlockNameInEntitySources(string newBuildingBlockName, SimulationEntitySources sources, SimulationEntitySource simulationEntitySource)
      {
         sources.Add(new SimulationEntitySource(simulationEntitySource.SimulationEntityPath, newBuildingBlockName, simulationEntitySource.BuildingBlockType, simulationEntitySource.ModuleName, simulationEntitySource.SourcePath));
      }

      private bool buildingBlockIsTemplateMatchFor(SimulationEntitySource simulationEntitySource, string oldBuildingBlockName, IBuildingBlock buildingBlock)
      {
         return string.Equals(oldBuildingBlockName, simulationEntitySource.BuildingBlockName) &&
                (buildingBlock.Module == null ? string.IsNullOrEmpty(simulationEntitySource.ModuleName) : string.Equals(buildingBlock.Module.Name, simulationEntitySource.ModuleName)) && 
                string.Equals(buildingBlock.GetType().Name, simulationEntitySource.BuildingBlockType);
      }

      private IEnumerable<(IMoBiSimulation Simulation, IBuildingBlock BuildingBlock)> getBuildingBlocksWithMatchingNameAndType(string nameToMatch, IBuildingBlock templBuildingBlock)
      {
         return _projectRetriever.Current.Simulations.SelectMany(simulation =>
         {
            // We cannot test for in use using the members of simulation. The standard in-use or created-by tests match based on names.
            // Here, the names do not match because the building block has already been renamed
            return simulation.BuildingBlocks().Where(buildingBlock => buildingBlock.IsTemplateMatchFor(templBuildingBlock, nameToMatch))
               .Select(b => (simulation, b));
         });
      }

      public void RenameInSimulationUsingTemplateModule(string oldModuleName, Module templateModule)
      {
         _projectRetriever.Current.Simulations.Each(simulation =>
         {
            // We cannot test for in use using the members of simulation. The standard in-use or created-by tests match based on names.
            // Here, the names do not match because the module has already been renamed
            simulation.Modules.Where(module => module.IsNamed(oldModuleName))
               .Each(module => renameModules(simulation, module, templateModule.Name));

            // ToList needed because iteration modifies the enumerable
            simulation.EntitySources.Where(x => string.Equals(x.ModuleName, oldModuleName)).ToList().
               Each(x => updateModuleNameInEntitySources(templateModule.Name, simulation.EntitySources, x));
         });
      }

      private static void updateModuleNameInEntitySources(string newModuleName, SimulationEntitySources sources, SimulationEntitySource entitySource)
      {
         sources.Add(new SimulationEntitySource(entitySource.SimulationEntityPath, entitySource.BuildingBlockName, entitySource.BuildingBlockType, newModuleName, entitySource.SourcePath));
      }

      private void renameModules(IMoBiSimulation moBiSimulation, Module module, string newModuleName)
      {
         module.Name = newModuleName;
         moBiSimulation.HasChanged = true;
      }
   }
}