using System;
using System.Collections.Generic;
using System.Linq;
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
         var buildingBlockType = templateBuildingBlock.GetType();
         getBuildingBlocksWithMatchingNameAndType(_projectRetriever.Current.Simulations, oldName, buildingBlockType).Each(simulationAndBuildingBlock =>
         {
            simulationAndBuildingBlock.Simulation.HasChanged = true;
            simulationAndBuildingBlock.BuildingBlock.Name = templateBuildingBlock.Name;
         });
      }

      private static IReadOnlyList<(IMoBiSimulation Simulation, IBuildingBlock BuildingBlock)> getBuildingBlocksWithMatchingNameAndType(IReadOnlyList<IMoBiSimulation> simulations, string nameToMatch, Type typeToMatch)
      {
         return simulations.SelectMany(simulation =>
         {
            return simulationBuildingBlocks(simulation).Where(buildingBlock => buildingBlock.IsNamed(nameToMatch) && buildingBlock.GetType() == typeToMatch)
               .Select(b => (simulation, b));
         }).ToList();
      }

      private static IReadOnlyList<IBuildingBlock> simulationBuildingBlocks(IMoBiSimulation simulation)
      {
         var buildingBlocks = simulation.Configuration.ModuleConfigurations.SelectMany(moduleConfiguration => moduleConfiguration.Module.BuildingBlocks).Concat(simulation.Configuration.ExpressionProfiles).ToList();

         if (simulation.Configuration.Individual != null)
            buildingBlocks.Add(simulation.Configuration.Individual);

         return buildingBlocks;
      }

      public void RenameInSimulationUsingTemplateModule(string oldName, Module templateModule)
      {
         _projectRetriever.Current.Simulations.Each(simulation =>
         {
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