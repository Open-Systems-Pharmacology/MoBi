using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ModuleConfigurationDTO
   {
      private readonly ModuleConfiguration _moduleConfiguration;
      private readonly List<InitialConditionsBuildingBlock> _InitialConditionsCollection = new List<InitialConditionsBuildingBlock> { NullStartValues.NullMoleculeStartValues };
      private readonly List<ParameterStartValuesBuildingBlock> _parameterStartValuesCollection = new List<ParameterStartValuesBuildingBlock> { NullStartValues.NullParameterStartValues };

      public ModuleConfigurationDTO(ModuleConfiguration moduleConfiguration)
      {
         _moduleConfiguration = moduleConfiguration;
         SelectedMoleculeStartValues = moduleConfiguration.SelectedInitialConditions ?? NullStartValues.NullMoleculeStartValues;
         SelectedParameterStartValues = moduleConfiguration.SelectedParameterStartValues ?? NullStartValues.NullParameterStartValues;
         _InitialConditionsCollection.AddRange(moduleConfiguration.Module.InitialConditionsCollection);
         _parameterStartValuesCollection.AddRange(moduleConfiguration.Module.ParameterStartValuesCollection);
      }

      public ParameterStartValuesBuildingBlock SelectedParameterStartValues { get; set; }
      public InitialConditionsBuildingBlock SelectedMoleculeStartValues { get; set; }
      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionsCollection => _InitialConditionsCollection;
      public IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValuesCollection => _parameterStartValuesCollection;

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