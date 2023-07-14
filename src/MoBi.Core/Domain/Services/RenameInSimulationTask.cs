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
      ///    Find modules in all simulations with <paramref name="oldName" /> and rename them to match the
      ///    <paramref name="templateModule" /> name
      /// </summary>
      void RenameInSimulationUsingTemplateModule(string oldName, Module templateModule);
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
         getBuildingBlocksWithMatchingNameAndType(_projectRetriever.Current.Simulations, oldName, templateBuildingBlock).Each(simulationAndBuildingBlock =>
         {
            simulationAndBuildingBlock.Simulation.HasChanged = true;
            simulationAndBuildingBlock.BuildingBlock.Name = templateBuildingBlock.Name;
         });
      }

      private static IReadOnlyList<(IMoBiSimulation Simulation, IBuildingBlock BuildingBlock)> getBuildingBlocksWithMatchingNameAndType(IReadOnlyList<IMoBiSimulation> simulations, string nameToMatch, IBuildingBlock templBuildingBlock)
      {
         return simulations.SelectMany(simulation =>
         {
            // We cannot test for in use using the members of simulation. The standard in-use or created-by tests match based on names.
            // Here, the names do not match because the building block has already been renamed
            return simulation.BuildingBlocks.Where(buildingBlock => buildingBlock.IsTemplateMatchFor(templBuildingBlock, nameToMatch))
               .Select(b => (simulation, b));
         }).ToList();
      }

      public void RenameInSimulationUsingTemplateModule(string oldName, Module templateModule)
      {
         _projectRetriever.Current.Simulations.Each(simulation =>
         {
            // We cannot test for in use using the members of simulation. The standard in-use or created-by tests match based on names.
            // Here, the names do not match because the module has already been renamed
            simulation.Configuration.ModuleConfigurations.Select(x => x.Module).Where(module => module.IsNamed(oldName))
               .Each(module => renameModules(simulation, module, templateModule.Name));
         });
      }

      private void renameModules(IMoBiSimulation moBiSimulation, Module module, string newModuleName)
      {
         module.Name = newModuleName;
         moBiSimulation.HasChanged = true;
      }
   }
}