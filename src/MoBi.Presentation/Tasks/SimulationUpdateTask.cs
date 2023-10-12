using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

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
   }

   public class SimulationUpdateTask : ISimulationUpdateTask
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly ISimulationFactory _simulationFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;

      public SimulationUpdateTask(IMoBiContext context,
         IMoBiApplicationController applicationController,
         ISimulationFactory simulationFactory,
         ICloneManagerForBuildingBlock cloneManager,
         ISimulationConfigurationFactory simulationConfigurationFactory)
      {
         _context = context;
         _applicationController = applicationController;
         _simulationFactory = simulationFactory;
         _cloneManager = cloneManager;
         _simulationConfigurationFactory = simulationConfigurationFactory;
      }

      public ICommand UpdateSimulation(IMoBiSimulation simulationToUpdate)
      {
         var simulationConfiguration = _simulationConfigurationFactory.CreateFromProjectTemplatesBasedOn(simulationToUpdate.Configuration);

         return updateSimulation(simulationToUpdate, simulationConfiguration, AppConstants.Captions.UpdatingSimulation);
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

         return updateSimulation(simulationToConfigure, simulationConfiguration);
      }

      private ICommand<IMoBiContext> updateSimulation(
         IMoBiSimulation simulationToUpdate,
         SimulationConfiguration simulationConfigurationReferencingTemplates,
         string message = AppConstants.Captions.ConfiguringSimulation)
      {
         _context.PublishEvent(new ClearNotificationsEvent(MessageOrigin.Simulation));
         //create model using referencing templates
         var model = _simulationFactory.CreateModelAndValidate(simulationConfigurationReferencingTemplates, simulationToUpdate.Model.Name, message);

         var simulationBuildConfiguration = createSimulationConfigurationToUseInSimulation(simulationConfigurationReferencingTemplates);

         var updateSimulationCommand = new UpdateSimulationCommand(simulationToUpdate, model, simulationBuildConfiguration);

         updateSimulationCommand.Run(_context);

         var macro = new MoBiMacroCommand
         {
            Description = updateSimulationCommand.Description,
            CommandType = updateSimulationCommand.CommandType,
            ObjectType = updateSimulationCommand.ObjectType
         };

         macro.Add(updateSimulationCommand);
         return macro;
      }

      private SimulationConfiguration createSimulationConfigurationToUseInSimulation(SimulationConfiguration simulationConfiguration)
      {
         return _cloneManager.Clone(simulationConfiguration);
      }
   }
}