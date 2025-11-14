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
      MoBiSimulation CreateSimulationFrom(string simulationName, IReadOnlyList<ModuleConfiguration> moduleConfigurations,
         IReadOnlyList<ExpressionProfileBuildingBlock> expressionProfiles = null,
         IndividualBuildingBlock individual = null);

      ModuleConfiguration CreateModuleConfiguration(Module module,
         string selectedParameterValues = null,
         string selectedInitialConditions = null);
   }

   public class SimulationTask : ISimulationTask
   {
      private readonly ISimulationFactory _simulationFactory;

      private readonly IObjectTypeResolver _objectTypeResolver;

      public SimulationTask(ISimulationFactory simulationFactory, IObjectTypeResolver objectTypeResolver)
      {
         _simulationFactory = simulationFactory;
         _objectTypeResolver = objectTypeResolver;
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
         // empty, or null string indicates no selection was required
         if (string.IsNullOrEmpty(namedObjectToSelect))
            return null;

         if (!allNamedObjects.ExistsByName(namedObjectToSelect))
            throw new InvalidArgumentException(AppConstants.Exceptions.CannotFindObjectWithName(namedObjectToSelect, allNamedObjects.AllNames(), _objectTypeResolver.TypeFor<T>().SplitToUpperCase()));

         return allNamedObjects.FindByName(namedObjectToSelect);
      }

      public MoBiSimulation CreateSimulationFrom(string simulationName, IReadOnlyList<ModuleConfiguration> moduleConfigurations,
         IReadOnlyList<ExpressionProfileBuildingBlock> expressionProfiles = null,
         IndividualBuildingBlock individual = null)
      {
         return _simulationFactory.CreateSimulation(simulationName, moduleConfigurations, expressionProfiles, individual);
      }
   }
}