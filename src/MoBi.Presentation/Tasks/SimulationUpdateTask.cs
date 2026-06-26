using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using SimulationConfiguration = OSPSuite.Core.Domain.Builder.SimulationConfiguration;

namespace MoBi.Presentation.Tasks
{
   /// <summary>
   ///    Performs Action necessary to update an existing Simulation when a building block has changed
   /// </summary>
   public interface ISimulationUpdateTask
   {
      /// <summary>
      ///    Configures the simulation (e.g. Allows the user to update the current simulation with other building blocks)
      /// </summary>
      /// <param name="simulationToConfigure"></param>
      /// <returns></returns>
      ICommand ConfigureSimulation(IMoBiSimulation simulationToConfigure);

      ICommand UpdateSimulation(IMoBiSimulation simulationToUpdate);

      Task<IReadOnlyList<ModelCreationAndValidationResult>> ConfigureSimulationsInParallel(
         IReadOnlyList<IMoBiSimulation> simulationsToUpdate, CancellationToken cancellationToken);

      ICommand UpdateSimulationOutputSelections(IMoBiSimulation simulation);

      ICommand UpdateSimulationSolverAndSchema(IMoBiSimulation simulationToUpdate);

      ICommand ConfigureSimulationAndAddToProject(IMoBiSimulation clonedSimulation);
   }

   public class SimulationUpdateTask : ISimulationUpdateTask
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly ISimulationFactory _simulationFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IConcurrencyManager _concurrencyManager;
      private readonly ICoreUserSettings _coreUserSettings;

      public SimulationUpdateTask(IMoBiContext context,
         IMoBiApplicationController applicationController,
         ISimulationFactory simulationFactory,
         ICloneManagerForBuildingBlock cloneManager,
         IHeavyWorkManager heavyWorkManager,
         IConcurrencyManager concurrencyManager,
         ICoreUserSettings coreUserSettings)
      {
         _context = context;
         _applicationController = applicationController;
         _simulationFactory = simulationFactory;
         _cloneManager = cloneManager;
         _heavyWorkManager = heavyWorkManager;
         _concurrencyManager = concurrencyManager;
         _coreUserSettings = coreUserSettings;
      }

      public ICommand UpdateSimulation(IMoBiSimulation simulationToUpdate)
      {
         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = ObjectTypes.Simulation,
            CommandType = AppConstants.Commands.UpdateCommand,
            Description = AppConstants.Commands.ConfigureSimulationsDescription(1)
         };

         _heavyWorkManager.Start(() =>
         {
            var simulationConfiguration = _context.Resolve<ISimulationConfigurationFactory>().CreateFromProjectTemplatesBasedOn(simulationToUpdate.Configuration);
            macroCommand.Add(updateSimulation(simulationToUpdate, simulationConfiguration));
         }, AppConstants.Captions.UpdatingSimulation);

         return macroCommand;
      }

      public async Task<IReadOnlyList<ModelCreationAndValidationResult>> ConfigureSimulationsInParallel(
         IReadOnlyList<IMoBiSimulation> simulationsToUpdate, CancellationToken cancellationToken)
      {
         //start with a not-configured result for every simulation so the returned list always has one per simulation
         var results = new ConcurrentDictionary<IMoBiSimulation, ModelCreationAndValidationResult>();
         simulationsToUpdate.Each(simulation => results[simulation] = new ModelCreationAndValidationResult(simulation));
         
         try
         {
            await _concurrencyManager.RunAsync(
               simulationsToUpdate,
               (simulation, token) =>
               {
                  _context.PublishEvent(new SimulationConfigurationStartedEvent(simulation));
                  try
                  {
                     configureSimulation(results[simulation], token);
                  }
                  catch (Exception)
                  {
                     //leave the not-configured result already in place
                  }
                  finally
                  {
                     if (results[simulation].IsValid)
                        _context.PublishEvent(new SimulationConfigurationSucceededEvent(simulation));
                     else
                        _context.PublishEvent(new SimulationConfigurationFailedEvent(simulation));
                  }
               },
               cancellationToken,
               Math.Max(1, _coreUserSettings.MaximumNumberOfCoresToUse));
         }
         catch (OperationCanceledException)
         {
            //cancellation just stops configuring; the simulations that did not run keep their not-configured result
         }

         return simulationsToUpdate.Select(simulation => results[simulation]).ToList();
      }

      private void configureSimulation(ModelCreationAndValidationResult result, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();

         // resolve services per-instance because some services are not stateless
         var configurationFactory = _context.Resolve<ISimulationConfigurationFactory>();
         var simulationFactory = _context.Resolve<ISimulationFactory>();
         var cloneManager = _context.Resolve<ICloneManagerForBuildingBlock>();

         var templateConfiguration = configurationFactory.CreateFromProjectTemplatesBasedOn(result.Simulation.Configuration);
         result.CreationResult = simulationFactory.CreateModelAndValidate(templateConfiguration, result.Simulation.Model.Name, throwOnInvalid: false);
         result.ClonedConfiguration = cloneManager.Clone(templateConfiguration);
      }

      public ICommand UpdateSimulationSolverAndSchema(IMoBiSimulation simulationToUpdate)
      {
         var newSimulationSettings = _cloneManager.Clone(_context.CurrentProject.SimulationSettings);
         return new UpdateSolverAndSchemaInSimulationCommand(simulationToUpdate, newSimulationSettings.Solver, newSimulationSettings.OutputSchema).RunCommand(_context);
      }

      public ICommand ConfigureSimulationAndAddToProject(IMoBiSimulation clonedSimulation)
      {
         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = ObjectTypes.Simulation,
            CommandType = AppConstants.Commands.AddCommand,
            Description = AppConstants.Commands.AddToProjectDescription(ObjectTypes.Simulation, clonedSimulation.Name)
         };
         ConfigureSimulation(clonedSimulation);
         macroCommand.Add(new AddSimulationCommand(clonedSimulation).RunCommand(_context));

         return macroCommand;
      }

      public ICommand UpdateSimulationOutputSelections(IMoBiSimulation simulation)
      {
         return new UpdateOutputSelectionsInSimulationCommand(_cloneManager.Clone(_context.CurrentProject.SimulationSettings).OutputSelections, simulation).RunCommand(_context);
      }

      public ICommand ConfigureSimulation(IMoBiSimulation simulationToConfigure)
      {
         SimulationConfiguration simulationConfiguration;
         using (var presenter = _applicationController.Start<ICreateSimulationConfigurationPresenter>())
         {
            simulationConfiguration = presenter.CreateBasedOn(simulationToConfigure, isNew: false);
         }

         if (simulationConfiguration == null)
            return new MoBiEmptyCommand();

         ICommand command = null;
         _heavyWorkManager.Start(() => { command = updateSimulation(simulationToConfigure, simulationConfiguration); }, AppConstants.Captions.ConfiguringSimulation);

         return command ?? new MoBiEmptyCommand();
      }

      private ICommand<IMoBiContext> updateSimulation(
         IMoBiSimulation simulationToUpdate,
         SimulationConfiguration simulationConfigurationReferencingTemplates)
      {
         CreationResult results = null;
         _context.PublishEvent(new ClearNotificationsEvent(MessageOrigin.Simulation));

         //create model using referencing templates
         results = _simulationFactory.CreateModelAndValidate(simulationConfigurationReferencingTemplates, simulationToUpdate.Model.Name);

         if (results == null)
            return new MoBiEmptyCommand();

         //create a clone then that will be saved in the simulation
         var simulationBuildConfiguration = _cloneManager.Clone(simulationConfigurationReferencingTemplates);

         var updateSimulationCommand = new UpdateSimulationCommand(simulationToUpdate, results.Model, results.SimulationBuilder.EntitySources, simulationBuildConfiguration);

         updateSimulationCommand.RunCommand(_context);

         return updateSimulationCommand;
      }
   }
}