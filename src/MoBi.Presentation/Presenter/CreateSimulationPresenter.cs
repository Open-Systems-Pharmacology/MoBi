using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

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
      private IMoBiBuildConfiguration _buildConfiguration;
      public IMoBiSimulation Simulation { get; private set; }

      public CreateSimulationPresenter(ICreateSimulationView view,
         IMoBiContext context, IModelConstructor modelConstructor,
         IDimensionValidator dimensionValidator, ISimulationFactory simulationFactory,
         IBuildConfigurationFactory buildConfigurationFactory, IHeavyWorkManager heavyWorkManager,
         ISubPresenterItemManager<ISimulationItemPresenter> subPresenterManager, IDialogCreator dialogCreator,
         IForbiddenNamesRetriever forbiddenNamesRetriever)
         : base(view, subPresenterManager, dialogCreator, buildConfigurationFactory, context, SimulationItems.All)
      {
         _simulationFactory = simulationFactory;
         _heavyWorkManager = heavyWorkManager;
         _forbiddenNamesRetriever = forbiddenNamesRetriever;
         _dimensionValidator = dimensionValidator;
         _modelConstructor = modelConstructor;
      }

      public IMoBiSimulation Create()
      {
         edit(_simulationFactory.Create());
         _view.Display();
         if (_view.Canceled)
            return null;

         finish();

         return Simulation;
      }

      private void edit(IMoBiSimulation simulation)
      {
         Simulation = simulation;
         _buildConfiguration = simulation.MoBiBuildConfiguration;
         _simulationDTO = new ObjectBaseDTO {Name = simulation.Name};
         _simulationDTO.AddUsedNames(nameOfSimulationAlreadyUsed());
         _subPresenterItemManager.AllSubPresenters.Each(x => x.Edit(simulation));
         _view.BindTo(_simulationDTO);
      }

      private IEnumerable<string> nameOfSimulationAlreadyUsed()
      {
         return _forbiddenNamesRetriever.For(Simulation);
      }

      public object Subject
      {
         get { return Simulation; }
      }

      /// <summary>
      ///    Action performed when the wizard finishes.
      /// </summary>
      private void finish()
      {
         SaveBuildConfiguration();
         CreationResult result = null;

         _heavyWorkManager.Start(() => { result = createModel(); }, AppConstants.Captions.CreatingSimulation);

         if (result == null || result.IsInvalid)
            throw new MoBiException(AppConstants.Exceptions.CoundNotCreateSimulation);

         Simulation = createSimulation(result.Model);

         FinalOptionsPresenter.DoFinalActions(Simulation);

         validateDimensions(Simulation.Model);
      }

      private void validateDimensions(IModel model)
      {
         _dimensionValidator.Validate(model, _buildConfiguration)
            .SecureContinueWith(t => showWarnings(t.Result));
      }

      private CreationResult createModel()
      {
         //Create the model using a build configuration referencing the templates building block so that references to template builders are defined properly 
         //we override the _buildConfiguration so that reference to builders are saved
         _buildConfiguration = _buildConfigurationFactory.CreateFromReferencesUsedIn(_buildConfiguration);
         var result = _modelConstructor.CreateModelFrom(_buildConfiguration, _simulationDTO.Name);
         if (result == null)
            return null;

         showWarnings(result.ValidationResult);
         if (result.IsInvalid)
            return result;

         return result;
      }

      private void showWarnings(ValidationResult validationResult)
      {
         _context.PublishEvent(new ShowValidationResultsEvent(validationResult));
      }

      protected void SaveBuildConfiguration()
      {
         ValidateStartValues();

         UpdateStartValueInfo<IMoleculeStartValuesBuildingBlock, IMoleculeStartValue>(_buildConfiguration.MoleculeStartValuesInfo, SelectedMoleculeStartValues);
         UpdateStartValueInfo<IParameterStartValuesBuildingBlock, IParameterStartValue>(_buildConfiguration.ParameterStartValuesInfo, SelectedParameterStartValues);

         _buildConfiguration.ShouldValidate = true;
      }

      private IMoBiSimulation createSimulation(IModel model)
      {
         //update the building block configuration to now use clones
         var simulationBuildConfiguration = _buildConfigurationFactory.CreateFromTemplateClones(_buildConfiguration);
         var simulation = _simulationFactory.CreateFrom(simulationBuildConfiguration, model).WithName(_simulationDTO.Name);
         simulation.HasChanged = true;
         return simulation;
      }

      protected IFinalOptionsPresenter FinalOptionsPresenter
      {
         get { return PresenterAt(SimulationItems.FinalOptions); }
      }
   }
}