using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using OSPSuite.Core.Domain;
using System.Linq;

namespace MoBi.R.Services
{
   public interface IModuleTask
   {
      IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditionsFromModule(Module module);
      IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValuesFromModule(Module module);
      InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name);
      ParameterValuesBuildingBlock ParameterValueBuildingBlockByName(Module module, string name);
   }

   public class ModuleTask : IModuleTask
   {
      public InitialConditionsBuildingBlock InitialConditionBuildingBlockByName(Module module, string name) =>
         module.InitialConditionsCollection.FindByName(name);

      public IReadOnlyList<InitialConditionsBuildingBlock> AllInitialConditionsFromModule(Module module) =>
         module.InitialConditionsCollection.ToList();

      public ParameterValuesBuildingBlock ParameterValueBuildingBlockByName(Module module, string name) =>
         module.ParameterValuesCollection.FindByName(name);

      public IReadOnlyList<ParameterValuesBuildingBlock> AllParameterValuesFromModule(Module module) =>
         module.ParameterValuesCollection;

      public string[] AllInitialConditionsBuildingBlockNames(Module module) => AllInitialConditionsFromModule(module).AllNames().ToArray();

      public string[] AllParameterValueBuildingBlockNames(Module module) => AllParameterValuesFromModule(module).AllNames().ToArray();
   }
}