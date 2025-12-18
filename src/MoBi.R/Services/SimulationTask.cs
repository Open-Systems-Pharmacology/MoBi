using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using MoBiSimulation = MoBi.R.Domain.MoBiSimulation;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationTask
   {
      void AddModuleConfiguration(ModuleConfiguration moduleConfiguration);
      void AddExpressionProfile(ExpressionProfileBuildingBlock expressionProfile);

      MoBiSimulation CreateSimulationFrom(string simulationName, IndividualBuildingBlock individual = null);

      ModuleConfiguration CreateModuleConfiguration(Module module,
         string selectedParameterValues = null,
         string selectedInitialConditions = null);
   }

   public class SimulationTask : ISimulationTask
   {
      private readonly ISimulationFactory _simulationFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;

      private readonly List<ModuleConfiguration> _moduleConfigurations = new();
      private readonly List<ExpressionProfileBuildingBlock> _expressionProfiles = new();

      public SimulationTask(ISimulationFactory simulationFactory, IObjectTypeResolver objectTypeResolver)
      {
         _simulationFactory = simulationFactory;
         _objectTypeResolver = objectTypeResolver;
      }

      public void AddModuleConfiguration(ModuleConfiguration moduleConfiguration)
      {
         if (moduleConfiguration != null)
            _moduleConfigurations.Add(moduleConfiguration);
      }

      public void AddExpressionProfile(ExpressionProfileBuildingBlock expressionProfile)
      {
         if (expressionProfile != null)
            _expressionProfiles.Add(expressionProfile);
      }

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
         if (string.IsNullOrEmpty(namedObjectToSelect))
            return null;

         if (!allNamedObjects.ExistsByName(namedObjectToSelect))
            throw new InvalidArgumentException(AppConstants.Exceptions.CannotFindObjectWithName(
               namedObjectToSelect, allNamedObjects.AllNames(), _objectTypeResolver.TypeFor<T>().SplitToUpperCase()));

         return allNamedObjects.FindByName(namedObjectToSelect);
      }

      
      public MoBiSimulation CreateSimulationFrom(string simulationName, IndividualBuildingBlock individual = null)
      {
         var modulesArray = _moduleConfigurations.ToArray();
         var expressionsArray = _expressionProfiles.Any() ? _expressionProfiles.ToArray() : null;

         var simulation = _simulationFactory.CreateSimulation(simulationName, modulesArray, expressionsArray, individual);

         _moduleConfigurations.Clear();
         _expressionProfiles.Clear();

         return simulation;
      }
   }
}