using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Simulation;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public abstract class ConfigureSimulationPresenterBase<TView, TPresenter> : WizardPresenter<TView, TPresenter, ISimulationItemPresenter>
      where TPresenter : IWizardPresenter
      where TView : IWizardView, IModalView<TPresenter>
   {
      protected IMoBiContext _context;
      protected IMoBiMacroCommand _commands;

      protected ConfigureSimulationPresenterBase(TView view, ISubPresenterItemManager<ISimulationItemPresenter> subPresenterManager, IDialogCreator dialogCreator, IMoBiContext context, IReadOnlyList<ISubPresenterItem> subPresenterItems)
         : base(view, subPresenterManager, subPresenterItems, dialogCreator)
      {
         _context = context;
         _commands = new MoBiMacroCommand();

         InitializeWith(_commands);
         AllowQuickFinish = true;
      }

      protected void ValidateStartValues()
      {
         MoleculeStartValuesPresenter.ValidateStartValues();
         ParameterStartValuesPresenter.ValidateStartValues();
      }

      protected override void UpdateControls(int currentIndex)
      {
         bool configReady = BuildConfigurationPresenter.CanClose;

         View.NextEnabled = configReady;
         _subPresenterItems.Each(p => setControlEnabled(p, configReady));
         View.OkEnabled = CanClose;
      }

      public override void InitializeWith(ICommandCollector commandCollector)
      {
         base.InitializeWith(commandCollector);
         BuildConfigurationPresenter.MoleculeStartValuesChangedEvent += (o, e) => MoleculeStartValuesPresenter.Refresh();
         BuildConfigurationPresenter.ParameterStartValuesChangedEvent += (o, e) => ParameterStartValuesPresenter.Refresh();
         BuildConfigurationPresenter.ModuleChangedEvent += (o, e) => refreshStartValues();
         // BuildConfigurationPresenter.SpatialStructureChangedEvent += (o, e) => refreshStartValues();
         // BuildConfigurationPresenter.MoleculeBuildingBlockChangedEvent += (o, e) => refreshStartValues();
      }

      private void refreshStartValues()
      {
         ParameterStartValuesPresenter.Refresh();
         MoleculeStartValuesPresenter.Refresh();
      }

      private void setControlEnabled(ISubPresenterItem subPresenterItem, bool configReady)
      {
         if (subPresenterItem == SimulationItems.BuildConfiguration)
            return;

         View.SetControlEnabled(subPresenterItem, configReady);
      }

      protected IEditSimulationConfigurationPresenter BuildConfigurationPresenter => PresenterAt(SimulationItems.BuildConfiguration);

      protected MoleculeStartValuesBuildingBlock SelectedMoleculeStartValues => MoleculeStartValuesPresenter.StartValues;

      protected ParameterStartValuesBuildingBlock SelectedParameterStartValues => ParameterStartValuesPresenter.StartValues;

      protected ISelectAndEditParameterStartValuesPresenter ParameterStartValuesPresenter => PresenterAt(SimulationItems.ParameterStartValues);

      protected ISelectAndEditMoleculesStartValuesPresenter MoleculeStartValuesPresenter => PresenterAt(SimulationItems.MoleculeStartValues);

      protected void UpdateStartValueInfo<TBuildingBlock, TStartValue>(IBuildingBlockInfo<TBuildingBlock> info, TBuildingBlock selectedBuildingBlock)
         where TBuildingBlock : class, IStartValuesBuildingBlock<TStartValue>
         where TStartValue : class, IStartValue
      {
         info.SimulationChanges += (selectedBuildingBlock.Version - info.TemplateBuildingBlock.Version);
         if (changedDuringCreation(info.TemplateBuildingBlock, selectedBuildingBlock))
            info.SimulationChanges++;

         info.BuildingBlock = selectedBuildingBlock;
      }

      private bool changedDuringCreation<T>(IStartValuesBuildingBlock<T> templateBuildingBlock, IStartValuesBuildingBlock<T> buildingBlock) where T : class, IStartValue
      {
         if (templateBuildingBlock.Count() != buildingBlock.Count())
            return true;

         //a start value is null if it was added on the fly on the bbuilding block
         return buildingBlock.Select(sv => templateBuildingBlock[sv.Path])
            .Any(startValue => startValue == null);
      }
   }
}