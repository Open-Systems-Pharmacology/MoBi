using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using OSPSuite.Core.Domain;
using System.Linq;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Serialization;
using OSPSuite.Presentation.Services;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services
{
   public interface IModuleTask
   {
      IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditionsFromModule(Module module);
      IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValuesFromModule(Module module);
      InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name);
      ParameterValuesBuildingBlock ParameterValueBuildingBlockByName(Module module, string name);
      IReadOnlyList<Module> LoadModulesFromFile(string filePath);
   }

   public class ModuleTask : IModuleTask
   {
      private readonly ISerializationTask _serializationTask;

      public ModuleTask(IProjectTask projectTask, ISerializationTask serializationTask)
      {
         _serializationTask = serializationTask;
      }

      public IReadOnlyList<Module> LoadModulesFromFile(string filePath) =>
         _serializationTask.LoadMany<Module>(filePath).ToList();

      public InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name) =>
         module.InitialConditionsCollection.FindByName(name);

      public IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditionsFromModule(Module module) =>
         module.InitialConditionsCollection.ToList();

      public ParameterValuesBuildingBlock ParameterValueBuildingBlockByName(Module module, string name) =>
         module.ParameterValuesCollection.FindByName(name);

      public IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValuesFromModule(Module module) =>
         module.ParameterValuesCollection;

   }
}