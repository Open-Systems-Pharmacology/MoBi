using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.Simulation;
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

      public ConfigureSimulationPresenter(IConfigureSimulationView view, ISubPresenterItemManager<ISimulationConfigurationItemPresenter> subPresenterSubjectManager, IDialogCreator dialogCreator, IMoBiContext context, IDiagramManagerFactory diagramManagerFactory)
         : base(view, subPresenterSubjectManager, dialogCreator, context, SimulationItems.All)
      {
         _diagramManagerFactory = diagramManagerFactory;
      }

      public IMoBiCommand CreateBuildConfiguration(IMoBiSimulation simulation) => CreateBuildConfigurationBasedOn(simulation, null);

      public IMoBiCommand CreateBuildConfigurationBasedOn(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         //we create a build configuration where all current building blocks are referencing template building blocks
         // SimulationConfiguration = _buildConfigurationFactory.CreateFromReferencesUsedIn(simulation.Configuration, templateBuildingBlock);

         // TODO should this be a clone? SIMULATION_CONFIGURATION
         SimulationConfiguration = simulation.Configuration;

         var tmpSimulation = new MoBiSimulation()
         {
            DiagramManager = _diagramManagerFactory.Create<ISimulationDiagramManager>(),

            Configuration = SimulationConfiguration,
            Creation = simulation.Creation,
            Name = simulation.Name,
         };

         edit(tmpSimulation);
         _view.Caption = AppConstants.Captions.ConfigureSimulation(simulation.Name);
         _view.Display();
         if (_view.Canceled)
            return new MoBiEmptyCommand();

         //Set the selected MSV AND PSV as per user inputs
         // UpdateStartValueInfo<MoleculeStartValuesBuildingBlock, MoleculeStartValue>(SimulationConfiguration.MoleculeStartValuesInfo, SelectedMoleculeStartValues);
         // UpdateStartValueInfo<ParameterStartValuesBuildingBlock, ParameterStartValue>(SimulationConfiguration.ParameterStartValuesInfo, SelectedParameterStartValues);


         return _commands;
      }

      private void edit(IMoBiSimulation simulation)
      {
         _subPresenterItemManager.AllSubPresenters.Each(x => x.Edit(simulation.Configuration));
      }
   }
}