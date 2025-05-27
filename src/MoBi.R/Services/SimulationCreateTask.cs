using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using System.Collections.Generic;
using OSPSuite.Core.Domain;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;
using SimulationConfiguration = MoBi.R.Domain.SimulationConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationCreateTask
   {
      Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration);
      SimulationConfiguration CreateConfiguration(string SimulationName, List<ModuleConfiguration> ModuleConfigurations = null, 
         List<ExpressionProfileBuildingBlock> ExpressionProfiles = null, 
         IndividualBuildingBlock Individual = null);
      ModuleConfiguration CreateModuleConfiguration(Module Module, 
         ParameterValuesBuildingBlock SelectedParameterValue = null, 
         InitialConditionsBuildingBlock SelectedInitialCondition = null);
   }

   public class SimulationCreateTask : ISimulationCreateTask
   {
      private readonly ISimulationFactory _simulationFactory;

      public SimulationCreateTask(ISimulationFactory simulationFactory)
      {
         _simulationFactory = simulationFactory;
      }

      public SimulationConfiguration CreateConfiguration(string SimulationName, List<ModuleConfiguration> ModuleConfigurations = null, 
         List<ExpressionProfileBuildingBlock> ExpressionProfiles = null, 
         IndividualBuildingBlock Individual = null) =>
         new SimulationConfiguration
         {
            SimulationName = SimulationName,
            ModuleConfigurations = ModuleConfigurations,
            ExpressionProfiles = ExpressionProfiles,
            Individual = Individual
         };

      public ModuleConfiguration CreateModuleConfiguration(Module Module, 
         ParameterValuesBuildingBlock SelectedParameterValue = null, 
         InitialConditionsBuildingBlock SelectedInitialCondition = null) => 
            new ModuleConfiguration
            {
               Module = Module,
               SelectedParameterValue = SelectedParameterValue,
               SelectedInitialCondition = SelectedInitialCondition
            };


      public Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration) =>
         _simulationFactory.CreateSimulation(simulationConfiguration);
   }
}