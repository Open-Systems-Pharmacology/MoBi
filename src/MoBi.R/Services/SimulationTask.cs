using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;
using SimulationConfiguration = MoBi.R.Domain.SimulationConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationTask
   {
      Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration, string simulationName);

      SimulationConfiguration CreateConfiguration(IReadOnlyList<ModuleConfiguration> moduleConfigurations = null,
         IReadOnlyList<ExpressionProfileBuildingBlock> expressionProfiles = null,
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

      public SimulationConfiguration CreateConfiguration(IReadOnlyList<ModuleConfiguration> moduleConfigurations = null,
         IReadOnlyList<ExpressionProfileBuildingBlock> expressionProfiles = null,
         IndividualBuildingBlock individual = null) =>
         new SimulationConfiguration
         {
            ModuleConfigurations = (moduleConfigurations ?? Enumerable.Empty<ModuleConfiguration>()).ToArray(),
            ExpressionProfiles = (expressionProfiles ?? Enumerable.Empty<ExpressionProfileBuildingBlock>()).ToArray(),
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

      public Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration, string simulationName) => 
         _simulationFactory.CreateSimulation(simulationConfiguration, simulationName);
   }
}