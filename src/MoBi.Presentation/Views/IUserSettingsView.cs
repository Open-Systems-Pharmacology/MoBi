using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IUserSettingsView : IModalView<IUserSettingsPresenter>
   {
      void BindTo(IUserSettings userSettings);
      void SetDiagramOptionsView(IView view);
      void SetLayoutView(IView forceLayoutConfigurationView);
      void SetChartOptionsView(IView view);
      void SetValidationOptionsView(IView view);
      void SetDisplayUnitsView(IView view);
      void SetApplicationSettingsView(IView view);
      bool LayoutViewVisible { get; set; }
   }
}