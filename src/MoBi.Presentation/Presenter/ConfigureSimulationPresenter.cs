using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.Simulation;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IConfigureSimulationPresenter : IWizardPresenter
   {
      IMoBiCommand CreateBuildConfigurationBaseOn(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock);
      IMoBiBuildConfiguration BuildConfiguration { get; }
   }

   public class ConfigureSimulationPresenter : ConfigureSimulationPresenterBase<IConfigureSimulationView, IConfigureSimulationPresenter>, IConfigureSimulationPresenter
   {
      private readonly IDiagramManagerFactory _diagramManagerFactory;
      public IMoBiBuildConfiguration BuildConfiguration { get; private set; }

      public ConfigureSimulationPresenter(IConfigureSimulationView view, ISubPresenterItemManager<ISimulationConfigurationItemPresenter> subPresenterSubjectManager, IDialogCreator dialogCreator, IBuildConfigurationFactory buildConfigurationFactory, IMoBiContext context, IDiagramManagerFactory diagramManagerFactory)
         : base(view, subPresenterSubjectManager, dialogCreator, buildConfigurationFactory, context, SimulationItems.AllConfigure)
      {
         _diagramManagerFactory = diagramManagerFactory;
      }

      public IMoBiCommand CreateBuildConfigurationBaseOn(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         //we create a build configuration where all current building block are referencing template building blocks
         BuildConfiguration = _buildConfigurationFactory.CreateFromReferencesUsedIn(simulation.MoBiBuildConfiguration, templateBuildingBlock);
         var tmpSimulation = new MoBiSimulation()
         {
            DiagramManager = _diagramManagerFactory.Create<ISimulationDiagramManager>(),
            BuildConfiguration = BuildConfiguration,
            Creation = simulation.Creation,
            Name = simulation.Name,
         };

         edit(tmpSimulation);
         _view.Caption = AppConstants.Captions.ConfigureSimulation(simulation.Name);
         _view.Display();
         if (_view.Canceled)
            return new MoBiEmptyCommand();

         //Set the selected MSV AND PSV as per user inputs
         UpdateStartValueInfo<IMoleculeStartValuesBuildingBlock, IMoleculeStartValue>(BuildConfiguration.MoleculeStartValuesInfo, SelectedMoleculeStartValues);
         UpdateStartValueInfo<IParameterStartValuesBuildingBlock, IParameterStartValue>(BuildConfiguration.ParameterStartValuesInfo, SelectedParameterStartValues);


         return _commands;
      }

      private void edit(IMoBiSimulation simulation)
      {
         _subPresenterItemManager.AllSubPresenters.Each(x => x.Edit(simulation));
      }
   }
}