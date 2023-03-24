using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
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
      private readonly IUserSettings _userSettings;
      private SimulationConfiguration _simulationConfiguration;
      public IMoBiSimulation Simulation { get; private set; }

      public CreateSimulationPresenter(
         ICreateSimulationView view,
         IMoBiContext context,
         IModelConstructor modelConstructor,
         IDimensionValidator dimensionValidator,
         ISimulationFactory simulationFactory,
         IHeavyWorkManager heavyWorkManager,
         ISubPresenterItemManager<ISimulationItemPresenter> subPresenterManager,
         IDialogCreator dialogCreator,
         IForbiddenNamesRetriever forbiddenNamesRetriever,
         IUserSettings userSettings)
         : base(view, subPresenterManager, dialogCreator, context, SimulationItems.All)
      {
         _simulationFactory = simulationFactory;
         _heavyWorkManager = heavyWorkManager;
         _forbiddenNamesRetriever = forbiddenNamesRetriever;
         _userSettings = userSettings;
         _dimensionValidator = dimensionValidator;
         _modelConstructor = modelConstructor;
      }

      public IMoBiSimulation Create()
      {
         var moBiSimulation = _simulationFactory.Create();
         moBiSimulation.Configuration.SimulationSettings = _context.CurrentProject.SimulationSettings;
         edit(moBiSimulation);
         _view.Display();
         if (_view.Canceled)
            return null;

         finish();

         return Simulation;
      }

      private void edit(IMoBiSimulation simulation)
      {
         Simulation = simulation;
         _simulationConfiguration = simulation.Configuration;
         _simulationDTO = new ObjectBaseDTO { Name = simulation.Name };
         _simulationDTO.AddUsedNames(nameOfSimulationAlreadyUsed());
         _subPresenterItemManager.AllSubPresenters.Each(x => x.Edit(simulation));
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
         saveBuildConfiguration();
         CreationResult result = null;

         _heavyWorkManager.Start(() => { result = createModel(); }, AppConstants.Captions.CreatingSimulation);

         if (result == null || result.IsInvalid)
            throw new MoBiException(AppConstants.Exceptions.CouldNotCreateSimulation);

         Simulation = createSimulation(result.Model);

         validateDimensions(Simulation.Model);
      }

      private void validateDimensions(IModel model)
      {
         _dimensionValidator.Validate(model, _simulationConfiguration)
            .SecureContinueWith(t => showWarnings(t.Result));
      }

      private CreationResult createModel()
      {
         //Create the model using a build configuration referencing the templates building block so that references to template builders are defined properly 
         //we override the _buildConfiguration so that reference to builders are saved

         // TODO
         // _simulationConfiguration = _buildConfigurationFactory.CreateFromReferencesUsedIn(_simulationConfiguration);
         var result = _modelConstructor.CreateModelFrom(_simulationConfiguration, _simulationDTO.Name);
         if (result == null)
            return null;

         showWarnings(result.ValidationResult);

         return result;
      }

      private void showWarnings(ValidationResult validationResult)
      {
         _context.PublishEvent(new ShowValidationResultsEvent(validationResult));
      }

      private void saveBuildConfiguration()
      {
         ValidateStartValues();

         // UpdateStartValueInfo<MoleculeStartValuesBuildingBlock, MoleculeStartValue>(_simulationConfiguration.MoleculeStartValuesInfo, SelectedMoleculeStartValues);
         // UpdateStartValueInfo<ParameterStartValuesBuildingBlock, ParameterStartValue>(_simulationConfiguration.ParameterStartValuesInfo, SelectedParameterStartValues);

         _simulationConfiguration.ShouldValidate = true;
         _simulationConfiguration.PerformCircularReferenceCheck = _userSettings.CheckCircularReference;
      }

      private IMoBiSimulation createSimulation(IModel model)
      {
         //update the building block configuration to now use clones
         // var simulationBuildConfiguration = _buildConfigurationFactory.CreateFromTemplateClones(_simulationConfiguration);

         var simulation = _simulationFactory.CreateFrom(_simulationConfiguration, model).WithName(_simulationDTO.Name);
         simulation.HasChanged = true;
         return simulation;
      }

      protected IFinalOptionsPresenter FinalOptionsPresenter => PresenterAt(SimulationItems.FinalOptions);
   }
}