using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSimulationConfigurationPresenter : ISimulationConfigurationItemPresenter
   {
      event EventHandler MoleculeStartValuesChangedEvent;

      // event EventHandler SpatialStructureChangedEvent;
      // event EventHandler MoleculeBuildingBlockChangedEvent;
      event EventHandler ParameterStartValuesChangedEvent;
      event EventHandler ModuleChangedEvent;
   }

   internal class EditSimulationConfigurationPresenter :
      AbstractSubPresenter<IEditSimulationConfigurationView, IEditSimulationConfigurationPresenter>,
      IEditSimulationConfigurationPresenter
   {
      private readonly IMoBiApplicationController _applicationController;

      private SimulationConfiguration _simulationConfiguration;
      private readonly List<ICommandCollectorPresenter> _allSelectionPresenter;
      private readonly IBuildingBlockSelectionPresenter<ExpressionProfileBuildingBlock> _selectExpressionProfile;
      private readonly IBuildingBlockSelectionPresenter<IndividualBuildingBlock> _selectIndividual;
      private readonly IModuleSelectionPresenter _selectModule;
      private readonly IStartValuesSelectionPresenter<MoleculeStartValuesBuildingBlock> _selectMoleculeStartValues;
      private readonly IStartValuesSelectionPresenter<ParameterStartValuesBuildingBlock> _selectParameterStartValues;

      public object Subject => _simulationConfiguration;
      public event EventHandler MoleculeStartValuesChangedEvent = delegate { };
      public event EventHandler ParameterStartValuesChangedEvent = delegate { };

      public event EventHandler ModuleChangedEvent = delegate { };
      // public event EventHandler MoleculeBuildingBlockChangedEvent = delegate { };
      // public event EventHandler SpatialStructureChangedEvent = delegate { };

      public EditSimulationConfigurationPresenter(IEditSimulationConfigurationView view, IMoBiApplicationController applicationController) :
         base(view)
      {
         _applicationController = applicationController;
         _allSelectionPresenter = new List<ICommandCollectorPresenter>();
         _selectExpressionProfile = createSelectionPresenterFor<ExpressionProfileBuildingBlock>(AppConstants.Captions.ExpressionProfile, ApplicationIcons.ExpressionProfile);
         _selectIndividual = createSelectionPresenterFor<IndividualBuildingBlock>(AppConstants.Captions.Individual, ApplicationIcons.Individual);
         _selectModule = createModuleSelectionPresenter(AppConstants.Captions.Module, ApplicationIcons.Module);
         _selectParameterStartValues = createStartValuesPresenter<ParameterStartValuesBuildingBlock>(AppConstants.Captions.ParameterStartValues, ApplicationIcons.ParameterStartValues);
         _selectMoleculeStartValues = createStartValuesPresenter<MoleculeStartValuesBuildingBlock>(AppConstants.Captions.MoleculeStartValues, ApplicationIcons.MoleculeStartValues);

         _view.AddEmptyPlaceHolder();

         _selectMoleculeStartValues.SelectionChanged += () => MoleculeStartValuesChangedEvent(this, EventArgs.Empty);
         _selectParameterStartValues.SelectionChanged += () => ParameterStartValuesChangedEvent(this, EventArgs.Empty);
         _selectModule.SelectionChanged += () => ModuleChangedEvent(this, EventArgs.Empty);
      }

      private IStartValuesSelectionPresenter<T> createStartValuesPresenter<T>(string caption, ApplicationIcon applicationIcon) where T : class, IBuildingBlock
      {
         var presenter = _applicationController.Start<IStartValuesSelectionPresenter<T>>();
         presenter.CanCreateBuildingBlock = false;
         _view.AddSelectionView(presenter.View, caption, applicationIcon);
         presenter.SelectionChanged += updateSimulationConfiguration;
         _allSelectionPresenter.Add(presenter);
         return presenter;
      }

      private IModuleSelectionPresenter createModuleSelectionPresenter(string caption, ApplicationIcon applicationIcon)
      {
         var presenter = _applicationController.Start<IModuleSelectionPresenter>();
         presenter.CanCreateBuildingBlock = false;
         _view.AddSelectionView(presenter.View, caption, applicationIcon);
         presenter.SelectionChanged += moduleChanged;
         _allSelectionPresenter.Add(presenter);
         return presenter;
      }

      private void moduleChanged()
      {
         _selectMoleculeStartValues.SetAvailableStartValueBuildingBlocks(_selectModule.SelectedModule.MoleculeStartValuesCollection);
         _selectParameterStartValues.SetAvailableStartValueBuildingBlocks(_selectModule.SelectedModule.ParameterStartValuesCollection);
         updateSimulationConfiguration();
      }

      private void updateSimulationConfiguration()
      {
         _simulationConfiguration.Module = _selectModule.SelectedModule;
         _simulationConfiguration.AddExpressionProfile(_selectExpressionProfile.SelectedBuildingBlock);
         _simulationConfiguration.Individual = _selectIndividual.SelectedBuildingBlock;

         ViewChanged();
         // TODO these are not assignable atm SIMULATION_CONFIGURATION
         // _simulationConfiguration.ParameterStartValues = _selectParameterStartValues.SelectedBuildingBlock;
         // _simulationConfiguration.MoleculeStartValues = _selectParameterStartValues.SelectedBuildingBlock;
      }

      private IBuildingBlockSelectionPresenter<T> createSelectionPresenterFor<T>(string caption, ApplicationIcon icon, bool newEnabled = false) where T : ObjectBase, IBuildingBlock
      {
         var presenter = _applicationController.Start<IBuildingBlockSelectionPresenter<T>>();
         presenter.CanCreateBuildingBlock = newEnabled;
         presenter.SelectionChanged += updateSimulationConfiguration;
         _view.AddSelectionView(presenter.View, caption, icon);
         _allSelectionPresenter.Add(presenter);
         return presenter;
      }

      public void Edit(IMoBiSimulation simulation)
      {
         _simulationConfiguration = simulation.Configuration;
         _selectExpressionProfile.Edit(_simulationConfiguration.ExpressionProfiles.FirstOrDefault());
         _selectIndividual.Edit(_simulationConfiguration.Individual);
         _selectModule.Edit(_simulationConfiguration.Module);
         moduleChanged();
         _selectMoleculeStartValues.Edit(_simulationConfiguration.MoleculeStartValues);
         _selectParameterStartValues.Edit(_simulationConfiguration.ParameterStartValues);


         OnStatusChanged();
      }

      public override void InitializeWith(ICommandCollector commandRegister)
      {
         base.InitializeWith(commandRegister);
         _allSelectionPresenter.Each(x => x.InitializeWith(commandRegister));
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _allSelectionPresenter.Each(p => p.ReleaseFrom(eventPublisher));
         _allSelectionPresenter.Clear();
      }

      public override bool CanClose
      {
         get { return _allSelectionPresenter.All(item => item.CanClose); }
      }
   }
}