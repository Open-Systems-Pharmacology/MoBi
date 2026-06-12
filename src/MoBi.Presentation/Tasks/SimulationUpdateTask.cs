using System.Collections.Generic;
using System.Linq;
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

      ICommand UpdateSimulations(IReadOnlyList<IMoBiSimulation> simulationsToUpdate);

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
      private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;
      private readonly IHeavyWorkManager _heavyWorkManager;

      public SimulationUpdateTask(IMoBiContext context,
         IMoBiApplicationController applicationController,
         ISimulationFactory simulationFactory,
         ICloneManagerForBuildingBlock cloneManager,
         ISimulationConfigurationFactory simulationConfigurationFactory, IHeavyWorkManager heavyWorkManager)
      {
         _context = context;
         _applicationController = applicationController;
         _simulationFactory = simulationFactory;
         _cloneManager = cloneManager;
         _simulationConfigurationFactory = simulationConfigurationFactory;
         _heavyWorkManager = heavyWorkManager;
      }

      public ICommand UpdateSimulations(IReadOnlyList<IMoBiSimulation> simulationsToUpdate)
      {
         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = ObjectTypes.Simulation,
            CommandType = AppConstants.Commands.UpdateCommand,
            Description = AppConstants.Commands.ConfigureSimulationsDescription(simulationsToUpdate.Count)
         };

         _heavyWorkManager.Start(() =>
         {
            macroCommand.AddRange(simulationsToUpdate.Select(x =>
            {
               var simulationConfiguration = _simulationConfigurationFactory.CreateFromProjectTemplatesBasedOn(x.Configuration);
               return updateSimulation(x, simulationConfiguration);
            }));
         }, AppConstants.Captions.UpdatingSimulation);

         return macroCommand;
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

         return command;
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