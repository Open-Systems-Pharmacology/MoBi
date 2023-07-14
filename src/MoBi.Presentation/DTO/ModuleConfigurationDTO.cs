using System.Collections.Generic;
using System.ComponentModel;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Reflection;

namespace MoBi.Presentation.DTO
{
   public class ModuleConfigurationDTO : Notifier
   {
      private readonly List<InitialConditionsBuildingBlock> _initialConditionsCollection = new List<InitialConditionsBuildingBlock> { NullPathAndValueEntityBuildingBlocks.NullInitialConditions };
      private readonly List<ParameterValuesBuildingBlock> _parameterValuesCollection = new List<ParameterValuesBuildingBlock> { NullPathAndValueEntityBuildingBlocks.NullParameterValues };

      public ModuleConfigurationDTO(ModuleConfiguration moduleConfiguration)
      {
         ModuleConfiguration = moduleConfiguration;
         SelectedInitialConditions = moduleConfiguration.SelectedInitialConditions ?? NullPathAndValueEntityBuildingBlocks.NullInitialConditions;
         SelectedParameterValues = moduleConfiguration.SelectedParameterValues ?? NullPathAndValueEntityBuildingBlocks.NullParameterValues;
         _initialConditionsCollection.AddRange(moduleConfiguration.Module.InitialConditionsCollection);
         _parameterValuesCollection.AddRange(moduleConfiguration.Module.ParameterValuesCollection);
         moduleConfiguration.Module.PropertyChanged += (o,e) => OnPropertyChanged(e.PropertyName);
      }

      public ParameterValuesBuildingBlock SelectedParameterValues { get; set; }
      public InitialConditionsBuildingBlock SelectedInitialConditions { get; set; }
      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionsCollection => _initialConditionsCollection;
      public IReadOnlyList<ParameterValuesBuildingBlock> ParameterValuesCollection => _parameterValuesCollection;

      public ModuleConfiguration ModuleConfiguration { get; }

      public Module Module => ModuleConfiguration.Module;

      public bool Uses(Module module)
      {
         return Equals(module.Name, ModuleConfiguration.Module.Name);
      }

      public bool Uses(ModuleConfiguration moduleConfiguration)
      {
         return Equals(ModuleConfiguration, moduleConfiguration);
      }

      public bool HasInitialConditions => !NullPathAndValueEntityBuildingBlocks.NullInitialConditions.Equals(SelectedInitialConditions);

      public bool HasParameterValues => !NullPathAndValueEntityBuildingBlocks.NullParameterValues.Equals(SelectedParameterValues);
   }
}