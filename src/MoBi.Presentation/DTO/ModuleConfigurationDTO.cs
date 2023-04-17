using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ModuleConfigurationDTO
   {
      private readonly ModuleConfiguration _moduleConfiguration;
      private readonly List<MoleculeStartValuesBuildingBlock> _moleculeStartValuesCollection = new List<MoleculeStartValuesBuildingBlock> { NullStartValues.NullMoleculeStartValues };
      private readonly List<ParameterStartValuesBuildingBlock> _parameterStartValuesCollection = new List<ParameterStartValuesBuildingBlock> { NullStartValues.NullParameterStartValues };

      public ModuleConfigurationDTO(ModuleConfiguration moduleConfiguration)
      {
         _moduleConfiguration = moduleConfiguration;
         SelectedMoleculeStartValues = moduleConfiguration.SelectedMoleculeStartValues ?? NullStartValues.NullMoleculeStartValues;
         SelectedParameterStartValues = moduleConfiguration.SelectedParameterStartValues ?? NullStartValues.NullParameterStartValues;
         _moleculeStartValuesCollection.AddRange(moduleConfiguration.Module.MoleculeStartValuesCollection);
         _parameterStartValuesCollection.AddRange(moduleConfiguration.Module.ParameterStartValuesCollection);
      }

      public ParameterStartValuesBuildingBlock SelectedParameterStartValues { get; set; }
      public MoleculeStartValuesBuildingBlock SelectedMoleculeStartValues { get; set; }
      public IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValuesCollection => _moleculeStartValuesCollection;
      public IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValuesCollection => _parameterStartValuesCollection;

      public ModuleConfiguration ModuleConfiguration => _moduleConfiguration;
      public Module Module => ModuleConfiguration.Module;

      public bool Uses(Module module)
      {
         return Equals(module, _moduleConfiguration.Module);
      }

      public bool Uses(ModuleConfiguration moduleConfiguration)
      {
         return Equals(_moduleConfiguration, moduleConfiguration);
      }

      public bool HasMoleculeStartValues()
      {
         return !NullStartValues.NullMoleculeStartValues.Equals(SelectedMoleculeStartValues);
      }

      public bool HasParameterStartValues()
      {
         return !NullStartValues.NullParameterStartValues.Equals(SelectedParameterStartValues);
      }
   }
}