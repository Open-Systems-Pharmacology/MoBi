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
         });
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
            var module = simulation.Modules.FindByName(oldModuleName);
            if (module != null)
               renameModule(simulation, module, templateModule.Name);
         });
      }

      private void renameModule(IMoBiSimulation moBiSimulation, Module module, string newModuleName)
      {
         module.Name = newModuleName;
         moBiSimulation.HasChanged = true;
      }
   }
}