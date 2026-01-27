using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.R.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationTask
   {
      SimulationCreationResult CreateSimulationAndValidateFrom(string simulationName, SimulationRequest request);

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
         if (string.IsNullOrEmpty(namedObjectToSelect))
            return null;

         if (!allNamedObjects.ExistsByName(namedObjectToSelect))
            throw new InvalidArgumentException(AppConstants.Exceptions.CannotFindObjectWithName(
               namedObjectToSelect, allNamedObjects.AllNames(), _objectTypeResolver.TypeFor<T>().SplitToUpperCase()));

         return allNamedObjects.FindByName(namedObjectToSelect);
      }

      public SimulationCreationResult CreateSimulationAndValidateFrom(string simulationName, SimulationRequest request)
      {
         var modulesArray = request?.ModuleConfigurations?.ToArray() ?? Array.Empty<ModuleConfiguration>();
         var expressionsArray = request?.ExpressionProfiles?.ToArray() ?? Array.Empty<ExpressionProfileBuildingBlock>();

         return _simulationFactory.CreateSimulationFrom(simulationName, 
            modulesArray, 
            expressionsArray, 
            request?.Individual, 
            request.CreateAllProcessRateParameters, 
            request?.SimulationSettings);
      }
   }
}