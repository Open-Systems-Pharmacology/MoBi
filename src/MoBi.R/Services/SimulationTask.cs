using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.R.Domain;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;
using SimulationConfiguration = MoBi.R.Domain.SimulationConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationTask
   {
      Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration, string simulationName);

      SimulationConfiguration CreateConfiguration(List<ModuleConfiguration> moduleConfigurations = null,
         List<ExpressionProfileBuildingBlock> expressionProfiles = null,
         IndividualBuildingBlock individual = null);

      ModuleConfiguration CreateModuleConfiguration(Module module,
         string selectedParameterValues = null,
         string selectedInitialConditions = null);
   }

   public class SimulationTask : ISimulationTask
   {
      private readonly ISimulationFactory _simulationFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public SimulationTask(ISimulationFactory simulationFactory, 
         IObjectTypeResolver objectTypeResolver)
      {
         _simulationFactory = simulationFactory;
         _objectTypeResolver = objectTypeResolver;
      }

      public SimulationConfiguration CreateConfiguration(List<ModuleConfiguration> moduleConfigurations = null,
         List<ExpressionProfileBuildingBlock> expressionProfiles = null,
         IndividualBuildingBlock individual = null) =>
         new SimulationConfiguration
         {
            ModuleConfigurations = moduleConfigurations,
            ExpressionProfiles = expressionProfiles,
            Individual = individual
         };

      public ModuleConfiguration CreateModuleConfiguration(Module module,
         string selectedParameterValues = null,
         string selectedInitialConditions = null) =>
         new ModuleConfiguration
         {
            Module = module,
            SelectedParameterValue = selectByName(module.ParameterValuesCollection, selectedParameterValues),
            SelectedInitialCondition = selectByName(module.InitialConditionsCollection, selectedInitialConditions)
         };

      private T selectByName<T>(IReadOnlyList<T> allNamedObjects, string namedObjectToSelect) where T : class, IWithName
      {
         // empty, or null string indicates no selection was required
         if (string.IsNullOrEmpty(namedObjectToSelect))
            return null;

         if (!allNamedObjects.ExistsByName(namedObjectToSelect))
            throw new InvalidArgumentException(AppConstants.Exceptions.CannotFindObjectWithName(namedObjectToSelect, allNamedObjects.AllNames(), _objectTypeResolver.TypeFor<T>().SplitToUpperCase()));

         return allNamedObjects.FindByName(namedObjectToSelect);
      }

      public Simulation CreateSimulationFrom(SimulationConfiguration simulationConfiguration, string simulationName)
      {
         return _simulationFactory.CreateSimulation(simulationConfiguration, simulationName);
      }
   }
}