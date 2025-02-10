using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Serialization.Converter
{
   public interface IModuleConfigurationBuilder
   {
      void BuildSimulationModuleConfigurations(MoBiProject project);
   }

   public class ModuleConfigurationBuilder : IModuleConfigurationBuilder
   {
      private readonly IModuleFactory _moduleFactory;

      public ModuleConfigurationBuilder(IModuleFactory moduleFactory)
      {
         _moduleFactory = moduleFactory;
      }

      public void BuildSimulationModuleConfigurations(MoBiProject project)
      {
         if (project.Modules.Count() != 1)
            project.Simulations.Each(rebuildModuleConfigurationsFrom);
         else
            project.Simulations.Each(x => renameSimulationModules(x, project.Modules.Single().Name));
      }

      private void rebuildModuleConfigurationsFrom(IMoBiSimulation simulation)
      {
         // ToList() is needed here because module configurations will be removed by replaceConfiguration
         simulation.Configuration.ModuleConfigurations.ToList().Each(x => replaceConfiguration(x, simulation));
      }

      private void replaceConfiguration(ModuleConfiguration moduleConfiguration, IMoBiSimulation simulation)
      {
         simulation.Configuration.RemoveModuleConfiguration(moduleConfiguration);

         var module = moduleConfiguration.Module;
         var buildingBlocks = module.BuildingBlocks.ToList();
         buildingBlocks.Each(x =>
         {
            module.Remove(x);
            var newModule = _moduleFactory.CreateDedicatedModuleFor(x);
            simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(newModule));
         });
      }

      private void renameSimulationModules(IMoBiSimulation simulation, string newModuleName) =>
       simulation.Configuration.ModuleConfigurations.Each(x => { x.Module.Name = newModuleName; });

   }
}