using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services
{
   public interface IModuleTask
   {
      InitialConditionsBuildingBlock[] AllInitialConditionsBuildingBlocksFromModule(Module module);
      ParameterValuesBuildingBlock[] AllParameterValuesBuildingBlocksFromModule(Module module);
      InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name);
      ParameterValuesBuildingBlock ParameterValuesBuildingBlockByName(Module module, string name);
      Module[] LoadModulesFromFile(string filePath);
      string[] AllInitialConditionsBuildingBlockNames(Module module);
      string[] AllParameterValuesBuildingBlockNames(Module module);
      Module CreateModule(string name, params IBuildingBlock[] buildingBlocks);
      void AddBuildingBlocksToModule(Module module, params IBuildingBlock[] buildingBlocks);
      void RemoveBuildingBlockFromModule(Module module, IBuildingBlock buildingBlock);
   }

   public class ModuleTask : IModuleTask
   {
      private readonly ISerializationTask _serializationTask;
      private readonly IMoBiContext _context;

      public ModuleTask(ISerializationTask serializationTask, IMoBiContext context)
      {
         _serializationTask = serializationTask;
         _context = context;
      }

      public Module[] LoadModulesFromFile(string filePath) =>
         _serializationTask.LoadAll<Module>(filePath).ToArray();

      public InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name) =>
         module.InitialConditionsCollection.FindByName(name);

      public InitialConditionsBuildingBlock[] AllInitialConditionsBuildingBlocksFromModule(Module module) =>
         module.InitialConditionsCollection.ToArray();

      public ParameterValuesBuildingBlock ParameterValuesBuildingBlockByName(Module module, string name) =>
         module.ParameterValuesCollection.FindByName(name);

      public ParameterValuesBuildingBlock[] AllParameterValuesBuildingBlocksFromModule(Module module) =>
         module.ParameterValuesCollection.ToArray();

      public string[] AllInitialConditionsBuildingBlockNames(Module module) => AllInitialConditionsBuildingBlocksFromModule(module).AllNames().ToArray();

      public string[] AllParameterValuesBuildingBlockNames(Module module) => AllParameterValuesBuildingBlocksFromModule(module).AllNames().ToArray();

      public Module CreateModule(string name, params IBuildingBlock[] buildingBlocks)
      {
         var module = _context.Create<Module>().WithName(name);
         AddBuildingBlocksToModule(module, buildingBlocks);
         return module;
      }

      public void AddBuildingBlocksToModule(Module module, params IBuildingBlock[] buildingBlocks) =>
         buildingBlocks.Each(module.Add);

      public void RemoveBuildingBlockFromModule(Module module, IBuildingBlock buildingBlock) =>
         module.Remove(buildingBlock);
   }
}