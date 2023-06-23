using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   /// <summary>
   ///    Performs Action necessary to update an existing Simulation when a building block has changed
   /// </summary>
   public interface ISimulationUpdateTask
   {
      /// <summary>
      ///    Updates the simulation according to changes in building Block. All not buildingBlock related changes are kept
      /// </summary>
      /// <param name="simulationToUpdate">The simulation to update.</param>
      /// <param name="templateBuildingBlock">The changed building block.</param>
      /// <returns>Executed command representing the update performed on the simulation</returns>
      ICommand UpdateSimulationFrom(IMoBiSimulation simulationToUpdate, IBuildingBlock templateBuildingBlock);

      /// <summary>
      ///    Configures the simulation (e.g. Allows the user to update the current simulation with other building blocks)
      /// </summary>
      /// <param name="simulationToConfigure"></param>
      /// <returns></returns>
      ICommand ConfigureSimulation(IMoBiSimulation simulationToConfigure);
   }

   public class SimulationUpdateTask : ISimulationUpdateTask
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly ISimulationFactory _simulationFactory;

      public SimulationUpdateTask(IMoBiContext context, IMoBiApplicationController applicationController, IEntityPathResolver entityPathResolver, ISimulationFactory simulationFactory)
      {
         _context = context;
         _applicationController = applicationController;
         _entityPathResolver = entityPathResolver;
         _simulationFactory = simulationFactory;
      }

      public ICommand UpdateSimulationFrom(IMoBiSimulation simulationToUpdate, IBuildingBlock templateBuildingBlock)
      {
         SimulationConfiguration simulationConfigurationReferencingTemplate;
         var configurationCommands = new MoBiMacroCommand();
         var fixedValueQuantities = new PathCache<IQuantity>(_entityPathResolver);

         if (triggersReconfiguration(templateBuildingBlock))
         {
            SimulationConfiguration simulationConfiguration;
            using (var presenter = _applicationController.Start<ICreateSimulationConfigurationPresenter>())
            {
               simulationConfiguration = presenter.CreateBasedOn(simulationToUpdate, allowNaming: false);
            }

            if (simulationConfiguration == null)
               return new MoBiEmptyCommand();

            simulationConfigurationReferencingTemplate = simulationConfiguration;
         }
         else
         {
            simulationConfigurationReferencingTemplate = createBuildConfigurationUsingTemplates(simulationToUpdate, templateBuildingBlock);
            fixedValueQuantities.AddRange(simulationToUpdate.Model.Root.GetAllChildren<IQuantity>(x => x.IsFixedValue));
         }

         return updateSimulation(simulationToUpdate, simulationConfigurationReferencingTemplate, configurationCommands, templateBuildingBlock, fixedValueQuantities);
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
         IBuildingBlock templateBuildingBlock = null,
         PathCache<IQuantity> fixedValueQuantities = null)
      {
         //create model using referencing templates
         var model = _simulationFactory.CreateModelAndValidate(simulationConfigurationReferencingTemplates, simulationToUpdate.Model.Name);

         var simulationBuildConfiguration = createBuildConfigurationToUseInSimulation(simulationConfigurationReferencingTemplates);

         var updateSimulationCommand = templateBuildingBlock == null
            ? // is null when we a simulation is being configured. Otherwise this is the template building block to user
            new UpdateSimulationCommand(simulationToUpdate, model, simulationBuildConfiguration)
            : new UpdateSimulationCommand(simulationToUpdate, model, simulationBuildConfiguration, templateBuildingBlock);

         updateSimulationCommand.Run(_context);

         synchronizeFixedParameterValues(simulationToUpdate, templateBuildingBlock, fixedValueQuantities);

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

      private void synchronizeFixedParameterValues(IMoBiSimulation simulationToUpdate, IBuildingBlock templateBuildingBlock, PathCache<IQuantity> fixedValueQuantities)
      {
         if (fixedValueQuantities == null)
            return;

         var currentQuantities = new PathCache<IQuantity>(_entityPathResolver);
         currentQuantities.AddRange(simulationToUpdate.Model.Root.GetAllChildren<IQuantity>());

         foreach (var fixedValueQuantity in fixedValueQuantities.KeyValues)
         {
            //quantity does not exist anymore after update. Nothing to do 
            var simulationQuantity = currentQuantities[fixedValueQuantity.Key];
            if (simulationQuantity == null)
               continue;

            //building block corresponding to quantity could not be found. That should never happen

            continue;
            //TODO SIMULATION_CONFIGURATION
            // var affectedBuildingBlock = _affectedBuildingBlockRetriever.RetrieveFor(simulationQuantity, simulationToUpdate);
            // if (affectedBuildingBlock?.UntypedTemplateBuildingBlock == null)
            //    continue;
            //
            // //The quantity previously fixed is part of the template building block. We should not reset the changes
            // if (Equals(affectedBuildingBlock.UntypedTemplateBuildingBlock, templateBuildingBlock))
            //    continue;

            synchronizeQuantities(fixedValueQuantity.Value, simulationQuantity);
         }
      }

      private static void synchronizeQuantities(IQuantity fixedValueQuantity, IQuantity simulationQuantity)
      {
         //no need to worry about formula cache here as we are dealing with formula in simulations
         //also formula should be set first and then value
         simulationQuantity.Formula = fixedValueQuantity.Formula;
         simulationQuantity.Value = fixedValueQuantity.Value;
      }

      private bool triggersReconfiguration(IBuildingBlock buildingBlock)
      {
         return buildingBlock.IsAnImplementationOf<MoleculeBuildingBlock>() ||
                buildingBlock.IsAnImplementationOf<SpatialStructure>();
      }

      private SimulationConfiguration createBuildConfigurationUsingTemplates(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         return simulation.Configuration;
      }

      private SimulationConfiguration createBuildConfigurationToUseInSimulation(SimulationConfiguration simulationConfiguration)
      {
         return simulationConfiguration;
      }
   }
}