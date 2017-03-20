using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSimulationSettingsPresenter : ISingleStartPresenter<ISimulationSettings>
   {
   }

   public class EditSimulationSettingsPresenter : SingleStartContainerPresenter<IEditSimulationSettingsView, IEditSimulationSettingsPresenter, ISimulationSettings, ISimulationSettingsItemPresenter>, IEditSimulationSettingsPresenter
   {
      private ISimulationSettings _simulationSettings;

      public EditSimulationSettingsPresenter(IEditSimulationSettingsView view, ISubPresenterItemManager<ISimulationSettingsItemPresenter> subPresenterSubjectManager)
         : base(view, subPresenterSubjectManager, SimulationSettingsItems.All)
      {
      }

      protected override void UpdateCaption()
      {
         View.Caption = AppConstants.Captions.SimulationSettingsBuildingBlockCaption(_simulationSettings.Name);
      }

      public override object Subject
      {
         get { return _simulationSettings; }
      }

      public override void Edit(ISimulationSettings simulationSettings)
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
   }
}