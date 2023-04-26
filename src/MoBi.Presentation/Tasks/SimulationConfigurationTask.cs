using System.Collections.Generic;
using MoBi.Core.Services;
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

      public SimulationConfigurationTask(ISimulationConfigurationFactory simulationConfigurationFactory, ICloneManagerForBuildingBlock cloneManager)
      {
         _simulationConfigurationFactory = simulationConfigurationFactory;
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
            simulationConfiguration.Individual = selectedIndividual;

         selectedExpressions.Each(simulationConfiguration.AddExpressionProfile);
      }

      private ModuleConfiguration cloneOf(ModuleConfiguration moduleConfiguration)
      {
         var clonedModule = moduleConfiguration.Module;
         var selectedMoleculeStartValues = clonedModule.MoleculeStartValuesCollection.FindByName(moduleConfiguration.SelectedMoleculeStartValues?.Name);
         var selectedParameterStartValues = clonedModule.ParameterStartValuesCollection.FindByName(moduleConfiguration.SelectedParameterStartValues?.Name);
         var clonedConfiguration = new ModuleConfiguration(clonedModule, selectedMoleculeStartValues, selectedParameterStartValues);
         return clonedConfiguration;
      }
   }
}
