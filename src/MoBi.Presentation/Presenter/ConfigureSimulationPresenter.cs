using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IConfigureSimulationPresenter : IWizardPresenter
   {
      IMoBiCommand CreateBuildConfigurationBasedOn(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock);
      IMoBiCommand CreateBuildConfiguration(IMoBiSimulation simulation);
      SimulationConfiguration SimulationConfiguration { get; }
   }

   public class ConfigureSimulationPresenter : ConfigureSimulationPresenterBase<IConfigureSimulationView, IConfigureSimulationPresenter>, IConfigureSimulationPresenter
   {
      private readonly IDiagramManagerFactory _diagramManagerFactory;
      public SimulationConfiguration SimulationConfiguration { get; private set; }

      public ConfigureSimulationPresenter(IConfigureSimulationView view, ISubPresenterItemManager<ISimulationConfigurationItemPresenter> subPresenterSubjectManager, 
         IDialogCreator dialogCreator, IMoBiContext context, IDiagramManagerFactory diagramManagerFactory, IUserSettings userSettings)
         : base(view, subPresenterSubjectManager, dialogCreator, context, userSettings, SimulationItems.All)
      {
         _diagramManagerFactory = diagramManagerFactory;
      }

      public IMoBiCommand CreateBuildConfiguration(IMoBiSimulation simulation) => CreateBuildConfigurationBasedOn(simulation, null);

      public IMoBiCommand CreateBuildConfigurationBasedOn(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         edit(simulation);
         _view.Caption = AppConstants.Captions.ConfigureSimulation(simulation.Name);
         UpdateControls();
         _view.Display();

         if (_view.Canceled)
            return new MoBiEmptyCommand();

         SimulationConfiguration = new SimulationConfiguration();
         // UpdateSimulationConfiguration(SimulationConfiguration);

         _commands.AddCommand(new UpdateSimulationConfigurationCommand(simulation, SimulationConfiguration));

         return _commands;
      }

      private void edit(IMoBiSimulation simulation)
      {
         _subPresenterItemManager.AllSubPresenters.Each(x => x.Edit(simulation.Configuration));
      }
   }
}