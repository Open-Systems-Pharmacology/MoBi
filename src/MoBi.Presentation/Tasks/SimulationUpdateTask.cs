using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

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

      ICommand UpdateSimulation(IMoBiSimulation subject);
   }

   public class SimulationUpdateTask : ISimulationUpdateTask
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly ISimulationFactory _simulationFactory;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IInteractionTasksForSimulation _interactionTasksForSimulation;
      private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;

      public SimulationUpdateTask(IMoBiContext context,
         IMoBiApplicationController applicationController,
         IEntityPathResolver entityPathResolver,
         ISimulationFactory simulationFactory,
         ICloneManagerForBuildingBlock cloneManager,
         IInteractionTasksForSimulation interactionTasksForSimulation,
         ISimulationConfigurationFactory simulationConfigurationFactory)
      {
         _context = context;
         _applicationController = applicationController;
         _entityPathResolver = entityPathResolver;
         _simulationFactory = simulationFactory;
         _cloneManager = cloneManager;
         _interactionTasksForSimulation = interactionTasksForSimulation;
         _simulationConfigurationFactory = simulationConfigurationFactory;
      }

      public ICommand UpdateSimulation(IMoBiSimulation simulationToUpdate)
      {
         var simulationConfiguration = _simulationConfigurationFactory.Create();
         
         simulationConfiguration.CopyPropertiesFrom(simulationToUpdate.Configuration);
         simulationConfiguration.SimulationSettings = _cloneManager.Clone(simulationToUpdate.Configuration.SimulationSettings);
         
         simulationToUpdate.Configuration.ModuleConfigurations.Each(moduleConfiguration => { simulationConfiguration.AddModuleConfiguration(templateModuleConfigurationFor(moduleConfiguration)); });

         simulationConfiguration.Individual = templateBuildingBlockFor(simulationToUpdate.Configuration.Individual);

         simulationToUpdate.Configuration.ExpressionProfiles.Each(x => { simulationConfiguration.AddExpressionProfile(templateBuildingBlockFor(x)); });

         return updateSimulation(simulationToUpdate, simulationConfiguration, new MoBiMacroCommand(), AppConstants.Captions.UpdatingSimulation);
      }

      private ModuleConfiguration templateModuleConfigurationFor(ModuleConfiguration moduleConfiguration)
      {
         return new ModuleConfiguration(_interactionTasksForSimulation.TemplateModuleFor(moduleConfiguration.Module),
            templateBuildingBlockFor(moduleConfiguration.SelectedInitialConditions),
            templateBuildingBlockFor(moduleConfiguration.SelectedParameterValues));
      }

      private TBuildingBlock templateBuildingBlockFor<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : class, IBuildingBlock
      {
         return _interactionTasksForSimulation.TemplateBuildingBlockFor(buildingBlock) as TBuildingBlock;
      }

      public ICommand ConfigureSimulation(IMoBiSimulation simulationToConfigure)
      {
         SimulationConfiguration simulationConfiguration;
         using (var presenter = _applicationController.Start<ICreateSimulationConfigurationPresenter>())
         {
            simulationConfiguration = presenter.CreateBasedOn(simulationToConfigure, allowNaming: false);
         }

         if (simulationConfiguration == null)
            return new MoBiEmptyCommand();

         return updateSimulation(simulationToConfigure, simulationConfiguration, new MoBiMacroCommand());
      }

      private ICommand<IMoBiContext> updateSimulation(
         IMoBiSimulation simulationToUpdate,
         SimulationConfiguration simulationConfigurationReferencingTemplates,
         IMoBiCommand configurationCommands,
         string message = AppConstants.Captions.ConfiguringSimulation)
      {
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

         macro.Add(configurationCommands);
         macro.Add(updateSimulationCommand);
         return macro;
      }

      private SimulationConfiguration createSimulationConfigurationToUseInSimulation(SimulationConfiguration simulationConfiguration)
      {
         return _cloneManager.Clone(simulationConfiguration);
      }
   }
}