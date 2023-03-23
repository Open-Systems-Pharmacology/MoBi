using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Presenter;
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

   internal class SimulationUpdateTask : ISimulationUpdateTask
   {
      private readonly IModelConstructor _modelConstructor;
      private readonly IBuildConfigurationFactory _buildConfigurationFactory;
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IDimensionValidator _dimensionValidator;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IAffectedBuildingBlockRetriever _affectedBuildingBlockRetriever;

      public SimulationUpdateTask(IModelConstructor modelConstructor, IBuildConfigurationFactory buildConfigurationFactory,
         IMoBiContext context, IMoBiApplicationController applicationController, IDimensionValidator dimensionValidator, IEntityPathResolver entityPathResolver, IAffectedBuildingBlockRetriever affectedBuildingBlockRetriever)
      {
         _modelConstructor = modelConstructor;
         _buildConfigurationFactory = buildConfigurationFactory;
         _context = context;
         _applicationController = applicationController;
         _dimensionValidator = dimensionValidator;
         _entityPathResolver = entityPathResolver;
         _affectedBuildingBlockRetriever = affectedBuildingBlockRetriever;
      }

      public ICommand UpdateSimulationFrom(IMoBiSimulation simulationToUpdate, IBuildingBlock templateBuildingBlock)
      {
         SimulationConfiguration simulationConfigurationReferencingTemplate;
         IMoBiCommand configurationCommands = null;
         var fixedValueQuantities = new PathCache<IQuantity>(_entityPathResolver);

         if (triggersReconfiguration(templateBuildingBlock))
         {
            using (var presenter = _applicationController.Start<IConfigureSimulationPresenter>())
            {
               configurationCommands = presenter.CreateBuildConfigurationBaseOn(simulationToUpdate, templateBuildingBlock);
               if (configurationCommands.IsEmpty())
                  return new MoBiEmptyCommand();

               simulationConfigurationReferencingTemplate = presenter.SimulationConfiguration;
            }
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
         using (var presenter = _applicationController.Start<IConfigureSimulationPresenter>())
         {
            var configurationCommands = presenter.CreateBuildConfiguration(simulationToConfigure);
            if (configurationCommands.IsEmpty())
               return new MoBiEmptyCommand();

            return updateSimulation(simulationToConfigure, presenter.SimulationConfiguration, configurationCommands);
         }
      }

      private ICommand<IMoBiContext> updateSimulation(
         IMoBiSimulation simulationToUpdate,
         SimulationConfiguration simulationConfigurationReferencingTemplates,
         IMoBiCommand configurationCommands,
         IBuildingBlock templateBuildingBlock = null,
         PathCache<IQuantity> fixedValueQuantities = null)
      {
         //create model using referencing templates
         var model = createModelAndValidate(simulationToUpdate.Model.Name, simulationConfigurationReferencingTemplates);

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
            var affectedBuildingBlock = _affectedBuildingBlockRetriever.RetrieveFor(simulationQuantity, simulationToUpdate);
            if (affectedBuildingBlock?.UntypedTemplateBuildingBlock == null)
               continue;

            //The quantity previously fixed is part of the template building block. We should not reset the changes
            if (Equals(affectedBuildingBlock.UntypedTemplateBuildingBlock, templateBuildingBlock))
               continue;

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

      private IModel createModelAndValidate(string modelName, SimulationConfiguration buildConfigurationReferencingTemplate)
      {
         var results = _modelConstructor.CreateModelFrom(buildConfigurationReferencingTemplate, modelName);

         if (results != null)
            showWarnings(results.ValidationResult);

         if (results == null || results.IsInvalid)
            throw new MoBiException(AppConstants.Exceptions.CouldNotCreateSimulation);

         var model = results.Model;

         _dimensionValidator.Validate(model, buildConfigurationReferencingTemplate)
            .SecureContinueWith(t => showWarnings(t.Result));

         return model;
      }

      private void showWarnings(ValidationResult validationResult)
      {
         _context.PublishEvent(new ShowValidationResultsEvent(validationResult));
      }

      private bool triggersReconfiguration(IBuildingBlock buildingBlock)
      {
         return buildingBlock.IsAnImplementationOf<MoleculeBuildingBlock>() ||
                buildingBlock.IsAnImplementationOf<ISpatialStructure>();
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