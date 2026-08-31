using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.R.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using ICoreUserSettings = MoBi.Core.ICoreUserSettings;
using ModuleConfiguration = MoBi.R.Domain.ModuleConfiguration;

namespace MoBi.R.Services
{
   public interface ISimulationTask
   {
      /// <summary>
      ///    Creates and validates a simulation named after <see cref="SimulationRequest.SimulationName" />.
      /// </summary>
      SimulationCreationResult CreateSimulationAndValidateFrom(SimulationRequest request);

      /// <summary>
      ///    Creates and validates one simulation per request. The simulations are created in parallel and the results are
      ///    returned in the request order.
      /// </summary>
      SimulationCreationResult[] CreateSimulationsAndValidateFrom(params SimulationRequest[] requests);

      ModuleConfiguration CreateModuleConfiguration(Module module,
         string selectedParameterValues = null,
         string selectedInitialConditions = null);
   }

   public class SimulationTask : ISimulationTask
   {
      private readonly ISimulationFactory _simulationFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly ICoreUserSettings _userSettings;

      public SimulationTask(ISimulationFactory simulationFactory, IObjectTypeResolver objectTypeResolver, ICoreUserSettings userSettings)
      {
         _simulationFactory = simulationFactory;
         _objectTypeResolver = objectTypeResolver;
         _userSettings = userSettings;
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

      public SimulationCreationResult CreateSimulationAndValidateFrom(SimulationRequest request)
      {
         if (request == null)
            throw new InvalidArgumentException(AppConstants.Exceptions.SimulationRequestCannotBeNull);

         var modulesArray = request?.ModuleConfigurations?.ToArray() ?? Array.Empty<ModuleConfiguration>();
         var expressionsArray = request?.ExpressionProfiles?.ToArray() ?? Array.Empty<ExpressionProfileBuildingBlock>();

         return _simulationFactory.CreateSimulationFrom(request.SimulationName,
            modulesArray,
            expressionsArray,
            request.Individual,
            request.AllCalculationMethodOverrides(),
            request.CreateAllProcessRateParameters,
            request.SimulationSettings);
      }

      public SimulationCreationResult[] CreateSimulationsAndValidateFrom(params SimulationRequest[] requests)
      {
         if (requests == null)
            throw new InvalidArgumentException(AppConstants.Exceptions.SimulationRequestCannotBeNull);

         var results = new SimulationCreationResult[requests.Length];

         SimulationCreationResult createAt(int index)
         {
            try
            {
               results[index] = CreateSimulationAndValidateFrom(requests[index]);
            }
            catch (Exception e) when (!e.IsOutOfMemory())
            {
               results[index] = new SimulationCreationResult(null, Enumerable.Empty<string>(), new[] { e.Message });
            }

            return results[index];
         }

         //simulations are created sequentially until one succeeds, so that lazily initialized services are
         //warmed up before the remaining simulations are created in parallel
         var warmupCount = 0;
         while (warmupCount < requests.Length)
         {
            var result = createAt(warmupCount);
            warmupCount++;
            if (result.Simulation != null)
               break;
         }

         if (requests.Length > warmupCount)
            Parallel.For(warmupCount, requests.Length, parallelOptions(), index => createAt(index));

         return results;
      }

      private ParallelOptions parallelOptions() => new ParallelOptions
      {
         MaxDegreeOfParallelism = Math.Max(1, _userSettings.MaximumNumberOfCoresToUse)
      };
   }
}