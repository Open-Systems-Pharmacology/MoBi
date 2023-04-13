using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ModuleConfigurationDTO
   {
      private readonly ModuleConfiguration _moduleConfiguration;
      private readonly List<MoleculeStartValuesBuildingBlock> _moleculeStartValuesCollection = new List<MoleculeStartValuesBuildingBlock> { NullMoleculeStartValues };
      private readonly List<ParameterStartValuesBuildingBlock> _parameterStartValuesCollection = new List<ParameterStartValuesBuildingBlock> { NullParameterStartValues };

      public ModuleConfigurationDTO(ModuleConfiguration moduleConfiguration)
      {
         _moduleConfiguration = moduleConfiguration;
         SelectedMoleculeStartValues = moduleConfiguration.SelectedMoleculeStartValues ?? NullMoleculeStartValues;
         SelectedParameterStartValues = moduleConfiguration.SelectedParameterStartValues ?? NullParameterStartValues;
         _moleculeStartValuesCollection.AddRange(moduleConfiguration.Module.MoleculeStartValuesCollection);
         _parameterStartValuesCollection.AddRange(moduleConfiguration.Module.ParameterStartValuesCollection);
      }

      public ParameterStartValuesBuildingBlock SelectedParameterStartValues { get; set; }
      public MoleculeStartValuesBuildingBlock SelectedMoleculeStartValues { get; set; }
      public IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValuesCollection => _moleculeStartValuesCollection;
      public IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValuesCollection => _parameterStartValuesCollection;

      public static MoleculeStartValuesBuildingBlock NullMoleculeStartValues { get; } = new MoleculeStartValuesBuildingBlock().WithName(AppConstants.Captions.NoMoleculeStartValues);
      public static ParameterStartValuesBuildingBlock NullParameterStartValues { get; } = new ParameterStartValuesBuildingBlock().WithName(AppConstants.Captions.NoParameterStartValues);

      public ModuleConfiguration ModuleConfiguration => _moduleConfiguration;

      public bool Uses(Module module)
      {
         return Equals(module, _moduleConfiguration.Module);
      }

      public bool Uses(ModuleConfiguration moduleConfiguration)
      {
         return Equals(_moduleConfiguration, moduleConfiguration);
      }
   }
}