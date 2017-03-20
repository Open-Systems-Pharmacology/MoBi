using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditBuildConfigurationPresenter : ISimulationConfigurationItemPresenter
   {
      event EventHandler MoleculeStartValuesChangedEvent;
      event EventHandler SpatialStructureChangedEvent;
      event EventHandler MoleculeBuildingBlockChangedEvent;
      event EventHandler ParameterStartValuesChangedEvent;
   }

   internal class EditBuildConfigurationPresenter :
      AbstractSubPresenter<IEditBuildConfigurationView, IEditBuildConfigurationPresenter>,
      IEditBuildConfigurationPresenter,
      IListener<AddedEvent<IMoleculeStartValuesBuildingBlock>>,
      IListener<AddedEvent<IParameterStartValuesBuildingBlock>>
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IBuildingBlockSelectionPresenter<IMoleculeBuildingBlock> _selectMoleculePresenter;
      private readonly IBuildingBlockSelectionPresenter<IMoBiSpatialStructure> _selectSpatialStructure;
      private readonly IBuildingBlockSelectionPresenter<IMoBiReactionBuildingBlock> _selectReactionBlock;
      private readonly IBuildingBlockSelectionPresenter<IEventGroupBuildingBlock> _selectEventBlock;
      private readonly IBuildingBlockSelectionPresenter<IPassiveTransportBuildingBlock> _selectPassiveTransport;
      private readonly IBuildingBlockSelectionPresenter<IObserverBuildingBlock> _selectObserverBlock;
      private readonly IBuildingBlockSelectionPresenter<IParameterStartValuesBuildingBlock> _selectParameterStartValues;
      private readonly IBuildingBlockSelectionPresenter<IMoleculeStartValuesBuildingBlock> _selectMoleculeStartValues;
      private readonly IBuildingBlockSelectionPresenter<ISimulationSettings> _selectSimulationSettingsBlock;
      private readonly IList<IBuildingBlockSelectionPresenter> _allSelectionPresenter;
      private IMoBiBuildConfiguration _buildConfiguration;

      public event EventHandler MoleculeStartValuesChangedEvent = delegate { };
      public event EventHandler ParameterStartValuesChangedEvent = delegate { };
      public event EventHandler MoleculeBuildingBlockChangedEvent = delegate { };
      public event EventHandler SpatialStructureChangedEvent = delegate { };

      public void UpdateMoleculeStartValueBuildingBlock(IMoleculeStartValuesBuildingBlock buildingBlock)
      {
         _selectMoleculeStartValues.UpdateBuildingBlock(buildingBlock);
      }

      public void UpdateParameterStartValueBuildingBlock(IParameterStartValuesBuildingBlock buildingBlock)
      {
         _selectParameterStartValues.UpdateBuildingBlock(buildingBlock);
      }


      public EditBuildConfigurationPresenter(IEditBuildConfigurationView view,  IMoBiApplicationController applicationController) :
         base(view)
      {
         _applicationController = applicationController;
         _allSelectionPresenter = new List<IBuildingBlockSelectionPresenter>();
         _selectSpatialStructure = createSelectionPresenterFor<IMoBiSpatialStructure>(AppConstants.Captions.SpatialStructure, ApplicationIcons.SpatialStructure);
         _selectMoleculePresenter = createSelectionPresenterFor<IMoleculeBuildingBlock>(AppConstants.Captions.Molecules, ApplicationIcons.Drug);
         _selectReactionBlock = createSelectionPresenterFor<IMoBiReactionBuildingBlock>(AppConstants.Captions.Reactions, ApplicationIcons.Reaction);
         _selectPassiveTransport = createSelectionPresenterFor<IPassiveTransportBuildingBlock>(AppConstants.Captions.PassiveTransports, ApplicationIcons.PassiveTransport);
         _selectObserverBlock = createSelectionPresenterFor<IObserverBuildingBlock>(AppConstants.Captions.Observers, ApplicationIcons.Observer);
         _selectEventBlock = createSelectionPresenterFor<IEventGroupBuildingBlock>(AppConstants.Captions.Events, ApplicationIcons.Event);
         _selectSimulationSettingsBlock = createSelectionPresenterFor<ISimulationSettings>(AppConstants.Captions.SimulationSettings, ApplicationIcons.SimulationSettings);
         _selectMoleculeStartValues = createSelectionPresenterFor<IMoleculeStartValuesBuildingBlock>(AppConstants.Captions.MoleculeStartValues, ApplicationIcons.MoleculeStartValuesFolder, true);
         _selectParameterStartValues = createSelectionPresenterFor<IParameterStartValuesBuildingBlock>(AppConstants.Captions.ParameterStartValues, ApplicationIcons.ParameterStartValuesFolder, true);
         _view.AddEmptyPlaceHolder();

         _selectMoleculeStartValues.SelectionChanged += () => MoleculeStartValuesChangedEvent(this, EventArgs.Empty);
         _selectParameterStartValues.SelectionChanged += () => ParameterStartValuesChangedEvent(this, EventArgs.Empty);
         _selectSpatialStructure.SelectionChanged += () => SpatialStructureChangedEvent(this, EventArgs.Empty);
         _selectMoleculePresenter.SelectionChanged += () => MoleculeBuildingBlockChangedEvent(this, EventArgs.Empty);
      }

      private IBuildingBlockSelectionPresenter<T> createSelectionPresenterFor<T>(string caption, ApplicationIcon icon) where T : class, IBuildingBlock
      {
         return createSelectionPresenterFor<T>(caption, icon, false);
      }

      private IBuildingBlockSelectionPresenter<T> createSelectionPresenterFor<T>(string caption, ApplicationIcon icon, bool newEnabled) where T : class, IBuildingBlock
      {
         var presenter = _applicationController.Start<IBuildingBlockSelectionPresenter<T>>();
         presenter.CanCreateBuildingBlock = newEnabled;
         presenter.SelectionChanged += ViewChanged;
         _view.AddSelectionView(presenter.View, caption, icon);
         _allSelectionPresenter.Add(presenter);
         return presenter;
      }

      public override void InitializeWith(ICommandCollector commandRegister)
      {
         base.InitializeWith(commandRegister);
         _allSelectionPresenter.Each(x => x.InitializeWith(commandRegister));
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _allSelectionPresenter.Each(p=>p.ReleaseFrom(eventPublisher));
         _allSelectionPresenter.Clear();
      }

      public void Edit(IMoBiSimulation simulation)
      {
         edit(simulation.MoBiBuildConfiguration);
      }

      private void edit(IMoBiBuildConfiguration buildConfiguration)
      {
         _buildConfiguration = buildConfiguration;
         _selectMoleculePresenter.Edit(buildConfiguration.MoleculesInfo);
         _selectSpatialStructure.Edit(buildConfiguration.SpatialStructureInfo);
         _selectReactionBlock.Edit(buildConfiguration.ReactionsInfo);
         _selectPassiveTransport.Edit(buildConfiguration.PassiveTransportsInfo);
         _selectObserverBlock.Edit(buildConfiguration.ObserversInfo);
         _selectEventBlock.Edit(buildConfiguration.EventGroupsInfo);
         _selectMoleculeStartValues.Edit(buildConfiguration.MoleculeStartValuesInfo);
         _selectParameterStartValues.Edit(buildConfiguration.ParameterStartValuesInfo);
         _selectSimulationSettingsBlock.Edit(buildConfiguration.SimulationSettingsInfo);
         
         OnStatusChanged();
      }

      public override bool CanClose
      {
         get { return _allSelectionPresenter.All(item => item.CanClose); }
      }

      public object Subject
      {
         get { return _buildConfiguration; }
      }

      public void Handle(AddedEvent<IMoleculeStartValuesBuildingBlock> eventToHandle)
      {
         UpdateMoleculeStartValueBuildingBlock(eventToHandle.AddedObject);
      }

      public void Handle(AddedEvent<IParameterStartValuesBuildingBlock> eventToHandle)
      {
         UpdateParameterStartValueBuildingBlock(eventToHandle.AddedObject);
      }
   }
}