using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ICreateSimulationPresenter : IWizardPresenter, IPresenter<ICreateSimulationView>
   {
      IMoBiSimulation Create();
   }

   public class CreateSimulationPresenter : ConfigureSimulationPresenterBase<ICreateSimulationView, ICreateSimulationPresenter>, ICreateSimulationPresenter
   {
      private readonly IModelConstructor _modelConstructor;
      private readonly IDimensionValidator _dimensionValidator;
      private readonly ISimulationFactory _simulationFactory;
      private ObjectBaseDTO _simulationDTO;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IForbiddenNamesRetriever _forbiddenNamesRetriever;
      private readonly IModuleConfigurationDTOToModuleConfigurationMapper _moduleConfigurationMapper;

      private readonly ICloneManagerForBuildingBlock _cloneManager;
      public IMoBiSimulation Simulation { get; private set; }

      public CreateSimulationPresenter(
         ICreateSimulationView view,
         IMoBiContext context,
         IModelConstructor modelConstructor,
         IDimensionValidator dimensionValidator,
         ISimulationFactory simulationFactory,
         IHeavyWorkManager heavyWorkManager,
         ISubPresenterItemManager<ISimulationConfigurationItemPresenter> subPresenterManager,
         IDialogCreator dialogCreator,
         IForbiddenNamesRetriever forbiddenNamesRetriever,
         IUserSettings userSettings,
         IModuleConfigurationDTOToModuleConfigurationMapper moduleConfigurationMapper,
         ICloneManagerForBuildingBlock cloneManager)
         : base(view, subPresenterManager, dialogCreator, context, userSettings, SimulationItems.All)
      {
         _simulationFactory = simulationFactory;
         _heavyWorkManager = heavyWorkManager;
         _forbiddenNamesRetriever = forbiddenNamesRetriever;
         _moduleConfigurationMapper = moduleConfigurationMapper;

         _dimensionValidator = dimensionValidator;
         _modelConstructor = modelConstructor;
         _cloneManager = cloneManager;
      }

      public IMoBiSimulation Create()
      {
         var moBiSimulation = _simulationFactory.Create();
         edit(moBiSimulation);
         UpdateControls();
         _view.Display();
         if (_view.Canceled)
            return null;

         finish();

         return Simulation;
      }

      private void edit(IMoBiSimulation simulation)
      {
         Simulation = simulation;
         _simulationDTO = new ObjectBaseDTO(simulation);
         _simulationDTO.AddUsedNames(nameOfSimulationAlreadyUsed());
         _subPresenterItemManager.AllSubPresenters.Each(x => x.Edit(simulation.Configuration));
         _view.BindTo(_simulationDTO);
      }

      private IEnumerable<string> nameOfSimulationAlreadyUsed()
      {
         return _forbiddenNamesRetriever.For(Simulation);
      }

      public object Subject => Simulation;

      /// <summary>
      ///    Action performed when the wizard finishes.
      /// </summary>
      private void finish()
      {
         updateSimulationConfiguration();
         CreationResult result = null;

         _heavyWorkManager.Start(() => { result = createModel(); }, AppConstants.Captions.CreatingSimulation);

         if (result == null || result.IsInvalid)
            throw new MoBiException(AppConstants.Exceptions.CouldNotCreateSimulation);

         Simulation = createSimulation(result.Model);

         validateDimensions(Simulation.Model, result.SimulationBuilder);
      }

      private void validateDimensions(IModel model, SimulationBuilder simulationBuilder)
      {
         _dimensionValidator.Validate(model, simulationBuilder)
            .SecureContinueWith(t => showWarnings(t.Result));
      }

      private CreationResult createModel()
      {
         Simulation.Configuration.SimulationSettings = _cloneManager.CloneBuildingBlock(_context.CurrentProject.SimulationSettings);
         var result = _modelConstructor.CreateModelFrom(Simulation.Configuration, _simulationDTO.Name);
         if (result == null)
            return null;

         showWarnings(result.ValidationResult);

         return result;
      }

      private void showWarnings(ValidationResult validationResult)
      {
         _context.PublishEvent(new ShowValidationResultsEvent(validationResult));
      }

      private void updateSimulationConfiguration()
      {
         PresenterAt(SimulationItems.ModuleConfiguration).ModuleConfigurationDTOs.MapAllUsing(_moduleConfigurationMapper).Each(Simulation.Configuration.AddModuleConfiguration);

         var individualAndExpressionPresenter = PresenterAt(SimulationItems.IndividualAndExpressionConfiguration);
         if(individualAndExpressionPresenter.SelectedIndividual != null)
            Simulation.Configuration.Individual = _cloneManager.CloneBuildingBlock(individualAndExpressionPresenter.SelectedIndividual);
         individualAndExpressionPresenter.ExpressionProfiles.Each(x => Simulation.Configuration.AddExpressionProfile(_cloneManager.CloneBuildingBlock(x)));

         Simulation.Configuration.ShouldValidate = true;
         Simulation.Configuration.PerformCircularReferenceCheck = _userSettings.CheckCircularReference;
      }
      private IMoBiSimulation createSimulation(IModel model)
      {
         var simulation = _simulationFactory.CreateFrom(Simulation.Configuration, model).WithName(_simulationDTO.Name);
         simulation.HasChanged = true;
         return simulation;
      }
   }
}