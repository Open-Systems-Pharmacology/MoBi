using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ModuleConfigurationDTO
   {
      private readonly ModuleConfiguration _moduleConfiguration;
      private readonly List<InitialConditionsBuildingBlock> _InitialConditionsCollection = new List<InitialConditionsBuildingBlock> { NullStartValues.NullMoleculeStartValues };
      private readonly List<ParameterValuesBuildingBlock> _parameterStartValuesCollection = new List<ParameterValuesBuildingBlock> { NullStartValues.NullParameterStartValues };

      public ModuleConfigurationDTO(ModuleConfiguration moduleConfiguration)
      {
         _moduleConfiguration = moduleConfiguration;
         SelectedMoleculeStartValues = moduleConfiguration.SelectedInitialConditions ?? NullStartValues.NullMoleculeStartValues;
         SelectedParameterStartValues = moduleConfiguration.SelectedParameterValues ?? NullStartValues.NullParameterStartValues;
         _InitialConditionsCollection.AddRange(moduleConfiguration.Module.InitialConditionsCollection);
         _parameterStartValuesCollection.AddRange(moduleConfiguration.Module.ParameterValuesCollection);
      }

      public ParameterValuesBuildingBlock SelectedParameterStartValues { get; set; }
      public InitialConditionsBuildingBlock SelectedMoleculeStartValues { get; set; }
      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionsCollection => _InitialConditionsCollection;
      public IReadOnlyList<ParameterValuesBuildingBlock> ParameterValuesCollection => _parameterStartValuesCollection;

      public ModuleConfiguration ModuleConfiguration => _moduleConfiguration;
      public Module Module => ModuleConfiguration.Module;

      public bool Uses(Module module)
      {
         return Equals(module.Name, _moduleConfiguration.Module.Name);
      }

      public bool Uses(ModuleConfiguration moduleConfiguration)
      {
         return Equals(_moduleConfiguration, moduleConfiguration);
      }

      public bool HasMoleculeStartValues => !NullStartValues.NullMoleculeStartValues.Equals(SelectedMoleculeStartValues);

      public bool HasParameterStartValues => !NullStartValues.NullParameterStartValues.Equals(SelectedParameterStartValues);
   }
}