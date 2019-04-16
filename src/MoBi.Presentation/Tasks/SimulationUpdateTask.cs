using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

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
      /// <returns>Command to perform the Update</returns>
      ICommand<IMoBiContext> UpdateSimulationFrom(IMoBiSimulation simulationToUpdate, IBuildingBlock templateBuildingBlock);
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

      /// <summary>
      ///    Updates the simulation according to changes in building Block. All changes that are not related to the building
      ///    block are kept
      /// </summary>
      /// <param name="simulationToUpdate">The simulation to update.</param>
      /// <param name="templateBuildingBlock">The building block that will be used to update the simulation.</param>
      /// <returns>
      ///    Command to perform the Update
      /// </returns>
      public ICommand<IMoBiContext> UpdateSimulationFrom(IMoBiSimulation simulationToUpdate, IBuildingBlock templateBuildingBlock)
      {
         IMoBiBuildConfiguration buildConfigurationReferencingTemplate;
         IMoBiCommand configurationCommands = null;
         var fixedValueQuantities = new PathCache<IQuantity>(_entityPathResolver);

         if (triggersReconfiguration(templateBuildingBlock))
         {
            using (var presenter = _applicationController.Start<IConfigureSimulationPresenter>())
            {
               configurationCommands = presenter.CreateBuildConfigurationBaseOn(simulationToUpdate, templateBuildingBlock);
               if (configurationCommands.IsEmpty())
                  return new MoBiEmptyCommand();

               buildConfigurationReferencingTemplate = presenter.BuildConfiguration;
            }
         }
         else
         {
            buildConfigurationReferencingTemplate = createBuildConfigurationUsingTemplates(simulationToUpdate, templateBuildingBlock);
            fixedValueQuantities.AddRange(simulationToUpdate.Model.Root.GetAllChildren<IQuantity>(x => x.IsFixedValue));
         }

         return updateSimulation(simulationToUpdate, templateBuildingBlock, buildConfigurationReferencingTemplate, configurationCommands, fixedValueQuantities);
      }

      private ICommand<IMoBiContext> updateSimulation(IMoBiSimulation simulationToUpdate, IBuildingBlock templateBuildingBlock, IMoBiBuildConfiguration buildConfigurationReferencingTemplates, IMoBiCommand configurationCommands, PathCache<IQuantity> fixedValueQuantities)
      {
         //create model using referencing templates
         var model = createModelAndValidate(simulationToUpdate.Model.Name, buildConfigurationReferencingTemplates);

         var simulationBuildConfiguration = createBuildConfigurationToUseInSimulation(buildConfigurationReferencingTemplates);

         var updateSimulationCommand = new UpdateSimulationCommand(simulationToUpdate, model, simulationBuildConfiguration, templateBuildingBlock).Run(_context);

         synchronizeFixedParameterValues(simulationToUpdate, fixedValueQuantities, templateBuildingBlock);

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

      private void synchronizeFixedParameterValues(IMoBiSimulation simulationToUpdate, PathCache<IQuantity> fixedValueQuantities, IBuildingBlock templateBuildingBlock)
      {
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

      private IModel createModelAndValidate(string modelName, IMoBiBuildConfiguration buildConfigurationReferencingTemplate)
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
         return buildingBlock.IsAnImplementationOf<IMoleculeBuildingBlock>() ||
                buildingBlock.IsAnImplementationOf<ISpatialStructure>();
      }

      private IMoBiBuildConfiguration createBuildConfigurationUsingTemplates(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         return _buildConfigurationFactory.CreateFromReferencesUsedIn(simulation.MoBiBuildConfiguration, templateBuildingBlock);
      }

      private IMoBiBuildConfiguration createBuildConfigurationToUseInSimulation(IMoBiBuildConfiguration buildConfiguration)
      {
         return _buildConfigurationFactory.CreateFromTemplateClones(buildConfiguration);
      }
   }
}