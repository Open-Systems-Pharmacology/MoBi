using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using OSPSuite.Core.Domain;
using System.Linq;
using IProjectTask = MoBi.CLI.Core.Services.IProjectTask;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services
{
   public interface IModuleTask
   {
      InitialConditionsBuildingBlock[] AllInitialConditionsFromModule(Module module);
      ParameterValuesBuildingBlock[] AllParameterValuesFromModule(Module module);
      InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name);
      ParameterValuesBuildingBlock ParameterValueBuildingBlockByName(Module module, string name);
      string[] AllInitialConditionsBuildingBlockNames(Module module);
      string[] AllParameterValueBuildingBlockNames(Module module);
      Module[] LoadModulesFromFile(string filePath);
   }

   public class ModuleTask : IModuleTask
   {
      private readonly ISerializationTask _serializationTask;

      public ModuleTask(IProjectTask projectTask, ISerializationTask serializationTask)
      {
         _serializationTask = serializationTask;
      }

      public Module[] LoadModulesFromFile(string filePath) =>
         _serializationTask.LoadMany<Module>(filePath).ToArray();

      public InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name) =>
         module.InitialConditionsCollection.FindByName(name);

      public InitialConditionsBuildingBlock[] AllInitialConditionsFromModule(Module module) =>
         module.InitialConditionsCollection.ToArray();

      public ParameterValuesBuildingBlock ParameterValueBuildingBlockByName(Module module, string name) =>
         module.ParameterValuesCollection.FindByName(name);

      public ParameterValuesBuildingBlock[] AllParameterValuesFromModule(Module module) =>
         module.ParameterValuesCollection.ToArray();

      public string[] AllInitialConditionsBuildingBlockNames(Module module) => AllInitialConditionsFromModule(module).AllNames().ToArray();

      public string[] AllParameterValueBuildingBlockNames(Module module) => AllParameterValuesFromModule(module).AllNames().ToArray();
   }
}