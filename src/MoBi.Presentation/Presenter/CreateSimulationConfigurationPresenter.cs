using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ICreateSimulationConfigurationPresenter : IWizardPresenter, IPresenter<ICreateSimulationConfigurationView>
   {
      SimulationConfiguration CreateBasedOn(IMoBiSimulation simulation, bool allowNaming = true);
      string SimulationName { get; }
   }

   public class CreateSimulationConfigurationPresenter : WizardPresenter<ICreateSimulationConfigurationView, ICreateSimulationConfigurationPresenter, ISimulationConfigurationItemPresenter>, ICreateSimulationConfigurationPresenter
   {
      private ObjectBaseDTO _simulationDTO;
      private readonly IForbiddenNamesRetriever _forbiddenNamesRetriever;
      protected IMoBiContext _context;
      protected readonly IUserSettings _userSettings;
      private readonly ISimulationConfigurationTask _simulationConfigurationTask;
      private readonly IModuleConfigurationDTOToModuleConfigurationMapper _moduleConfigurationMapper;

      public CreateSimulationConfigurationPresenter(
         ICreateSimulationConfigurationView view,
         IMoBiContext context,
         ISubPresenterItemManager<ISimulationConfigurationItemPresenter> subPresenterManager,
         IDialogCreator dialogCreator,
         IForbiddenNamesRetriever forbiddenNamesRetriever,
         IUserSettings userSettings,
         IModuleConfigurationDTOToModuleConfigurationMapper moduleConfigurationMapper,
         ISimulationConfigurationTask simulationConfigurationFactory)
         : base(view, subPresenterManager, SimulationItems.All, dialogCreator)
      {
         _forbiddenNamesRetriever = forbiddenNamesRetriever;
         _context = context;
         IMoBiMacroCommand commands = new MoBiMacroCommand();
         _moduleConfigurationMapper = moduleConfigurationMapper;
         _userSettings = userSettings;
         _simulationConfigurationTask = simulationConfigurationFactory;
         InitializeWith(commands);
         AllowQuickFinish = false;
      }

      public SimulationConfiguration CreateBasedOn(IMoBiSimulation moBiSimulation, bool allowNaming = true)
      {
         edit(moBiSimulation, allowNaming);
         UpdateControls();
         _view.Display();
         if (_view.Canceled)
         {
            return null;
         }

         var simulationConfiguration = _simulationConfigurationTask.Create();
         updateSimulationConfiguration(simulationConfiguration);
         return simulationConfiguration;
      }

      public string SimulationName => _simulationDTO.Name;

      private void edit(IMoBiSimulation simulation, bool allowNaming)
      {
         _simulationDTO = new ObjectBaseDTO(simulation).WithName(simulation.Name);
         if (allowNaming)
            _simulationDTO.AddUsedNames(nameOfSimulationAlreadyUsed(simulation));
         else
            _view.DisableNaming();
         _subPresenterItemManager.AllSubPresenters.Each(x => x.Edit(simulation.Configuration));
         _view.BindTo(_simulationDTO);
      }

      private IEnumerable<string> nameOfSimulationAlreadyUsed(IMoBiSimulation simulation)
      {
         return _forbiddenNamesRetriever.For(simulation);
      }

      protected override void UpdateControls(int currentIndex)
      {
         View.OkEnabled = CanClose;
      }
      
      private void updateSimulationConfiguration(SimulationConfiguration simulationConfiguration)
      {
         var moduleConfigurations = PresenterAt(SimulationItems.ModuleConfiguration).ModuleConfigurationDTOs.MapAllUsing(_moduleConfigurationMapper);
         var individualAndExpressionPresenter = PresenterAt(SimulationItems.IndividualAndExpressionConfiguration);
         var selectedExpressions = individualAndExpressionPresenter.ExpressionProfiles;
         var selectedIndividual = individualAndExpressionPresenter.SelectedIndividual;

         _simulationConfigurationTask.UpdateFrom(simulationConfiguration, moduleConfigurations, selectedIndividual, selectedExpressions);
         
         simulationConfiguration.ShouldValidate = true;
         simulationConfiguration.PerformCircularReferenceCheck = _userSettings.CheckCircularReference;
      }
   }
}