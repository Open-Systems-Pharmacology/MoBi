using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
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
   }

   public class ModuleTask : IModuleTask
   {
      private readonly ISerializationTask _serializationTask;

      public ModuleTask(IProjectTask projectTask, ISerializationTask serializationTask)
      {
         _serializationTask = serializationTask;
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
   }
}