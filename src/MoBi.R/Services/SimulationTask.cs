using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;
using SimulationConfiguration = MoBi.R.Domain.SimulationConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationTask
   {
      Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration);

      SimulationConfiguration CreateConfiguration(string simulationName, IReadOnlyList<ModuleConfiguration> moduleConfigurations = null,
         List<ExpressionProfileBuildingBlock> expressionProfiles = null,
         IndividualBuildingBlock individual = null);

      ModuleConfiguration CreateModuleConfiguration(Module module,
         ParameterValuesBuildingBlock selectedParameterValue = null,
         InitialConditionsBuildingBlock selectedInitialCondition = null);
   }

   public class SimulationTask : ISimulationTask
   {
      private readonly ISimulationFactory _simulationFactory;

      public SimulationTask(ISimulationFactory simulationFactory)
      {
         _simulationFactory = simulationFactory;
      }

      public SimulationConfiguration CreateConfiguration(string simulationName, IReadOnlyList<ModuleConfiguration> moduleConfigurations = null,
         List<ExpressionProfileBuildingBlock> expressionProfiles = null,
         IndividualBuildingBlock individual = null) =>
         new SimulationConfiguration
         {
            SimulationName = simulationName,
            ModuleConfigurations = moduleConfigurations,
            ExpressionProfiles = expressionProfiles,
            Individual = individual
         };

      public ModuleConfiguration CreateModuleConfiguration(Module module,
         ParameterValuesBuildingBlock selectedParameterValue = null,
         InitialConditionsBuildingBlock selectedInitialCondition = null) =>
         new ModuleConfiguration
         {
            Module = module,
            SelectedParameterValue = selectedParameterValue,
            SelectedInitialCondition = selectedInitialCondition
         };

      public Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration) =>
         _simulationFactory.CreateSimulation(simulationConfiguration);
   }
}