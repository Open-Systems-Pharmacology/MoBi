using MoBi.Assets;
using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSimulationSettingsPresenter : ISingleStartPresenter<SimulationSettings>, IListener<DefaultSimulationSettingsUpdatedEvent>
   {
   }

   public class EditSimulationSettingsPresenter : SingleStartContainerPresenter<IEditSimulationSettingsView, IEditSimulationSettingsPresenter, SimulationSettings, ISimulationSettingsItemPresenter>, IEditSimulationSettingsPresenter
   {
      private SimulationSettings _simulationSettings;

      public EditSimulationSettingsPresenter(IEditSimulationSettingsView view, ISubPresenterItemManager<ISimulationSettingsItemPresenter> subPresenterSubjectManager)
         : base(view, subPresenterSubjectManager, SimulationSettingsItems.All)
      {
      }

      protected override void UpdateCaption()
      {
         View.Caption = AppConstants.Captions.SimulationSettingsBuildingBlockCaption(_simulationSettings.Name);
      }

      public override object Subject => _simulationSettings;

      public override void Edit(SimulationSettings simulationSettings)
      {
         _simulationSettings = simulationSettings;
         _subPresenterItemManager.AllSubPresenters.Each(p => p.Edit(simulationSettings));
         UpdateCaption();
         _view.Display();
      }

      public override void InitializeWith(ICommandCollector commandRegister)
      {
         base.InitializeWith(commandRegister);
         PresenterAt(SimulationSettingsItems.OutputSchema).DowncastTo<IEditOutputSchemaPresenter>().ShowGroupCaption = false;
         PresenterAt(SimulationSettingsItems.Solver).DowncastTo<IEditSolverSettingsPresenter>().ShowGroupCaption = false;
      }

      public void Handle(DefaultSimulationSettingsUpdatedEvent eventToHandle)
      {
         Edit(eventToHandle.NewSimulationSettings);
      }
   }
}