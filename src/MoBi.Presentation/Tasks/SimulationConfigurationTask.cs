using System.Collections.Generic;
using MoBi.Core.Services;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public interface ISimulationConfigurationTask
   {
      SimulationConfiguration Create();

      void UpdateFrom(SimulationConfiguration configurationToUpdate, 
         IReadOnlyList<ModuleConfiguration> moduleConfigurations, 
         IndividualBuildingBlock selectedIndividual, 
         IReadOnlyList<ExpressionProfileBuildingBlock> selectedExpressions);
   }
   
   public class SimulationConfigurationTask : ISimulationConfigurationTask
   {
      private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public SimulationConfigurationTask(ISimulationConfigurationFactory simulationConfigurationFactory, ICloneManagerForBuildingBlock cloneManager)
      {
         _simulationConfigurationFactory = simulationConfigurationFactory;
         _cloneManager = cloneManager;
      }

      public SimulationConfiguration Create()
      {
         return _simulationConfigurationFactory.Create();
      }

      public void UpdateFrom(SimulationConfiguration simulationConfiguration, 
         IReadOnlyList<ModuleConfiguration> moduleConfigurations, 
         IndividualBuildingBlock selectedIndividual,
         IReadOnlyList<ExpressionProfileBuildingBlock> selectedExpressions)
      {
         moduleConfigurations.Each(moduleConfiguration => simulationConfiguration.AddModuleConfiguration(cloneOf(moduleConfiguration)));
         
         if (selectedIndividual != null)
            simulationConfiguration.Individual = _cloneManager.Clone(selectedIndividual);

         selectedExpressions.Each(x => simulationConfiguration.AddExpressionProfile(_cloneManager.Clone(x)));
      }

      private ModuleConfiguration cloneOf(ModuleConfiguration moduleConfiguration)
      {
         var clonedModule = _cloneManager.Clone(moduleConfiguration.Module);
         var selectedMoleculeStartValues = clonedModule.MoleculeStartValuesCollection.FindByName(moduleConfiguration.SelectedMoleculeStartValues?.Name);
         var selectedParameterStartValues = clonedModule.ParameterStartValuesCollection.FindByName(moduleConfiguration.SelectedParameterStartValues?.Name);
         var clonedConfiguration = new ModuleConfiguration(clonedModule, selectedMoleculeStartValues, selectedParameterStartValues);
         return clonedConfiguration;
      }
   }
}
