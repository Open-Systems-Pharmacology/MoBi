using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.UICommand
{
   public interface IUserSettingsPresenter : IDisposablePresenter
   {
      void Edit(IUserSettings userSettings);
   }

   public class UserSettingsPresenter : MoBiDisposablePresenter<IUserSettingsView, IUserSettingsPresenter>, IUserSettingsPresenter
   {
      private readonly IDiagramOptionsPresenter _diagramOptionsPresenter;
      private readonly IForceLayoutConfigurationPresenter _forceLayoutConfigurationPresenter;
      private readonly IChartOptionsPresenter _chartOptionsPresenter;
      private readonly IValidationOptionsPresenter _validationOptionsPresenter;
      private readonly IDisplayUnitsPresenter _displayUnitsPresenter;
      private readonly IStartOptions _runOptions;
      private readonly IApplicationSettingsPresenter _applicationSettingsPresenter;

      public UserSettingsPresenter(IUserSettingsView view, IDiagramOptionsPresenter diagramOptionsPresenter,
         IForceLayoutConfigurationPresenter forceLayoutConfigurationPresenter, IChartOptionsPresenter chartOptionsPresenter,
         IValidationOptionsPresenter validationOptionsPresenter, IDisplayUnitsPresenter displayUnitsPresenter, 
         IStartOptions runOptions, IApplicationSettingsPresenter applicationSettingsPresenter) : base(view)
      {
         _diagramOptionsPresenter = diagramOptionsPresenter;
         _forceLayoutConfigurationPresenter = forceLayoutConfigurationPresenter;
         _chartOptionsPresenter = chartOptionsPresenter;
         _validationOptionsPresenter = validationOptionsPresenter;
         _displayUnitsPresenter = displayUnitsPresenter;
         _runOptions = runOptions;
         _applicationSettingsPresenter = applicationSettingsPresenter;
         _view.SetDiagramOptionsView(_diagramOptionsPresenter.View);
         _view.SetLayoutView(_forceLayoutConfigurationPresenter.View);
         _view.SetChartOptionsView(_chartOptionsPresenter.View);
         _view.SetValidationOptionsView(validationOptionsPresenter.View);
         _view.SetDisplayUnitsView(_displayUnitsPresenter.View);
         _view.SetApplicationSettingsView(_applicationSettingsPresenter.View);
         AddSubPresenters(_diagramOptionsPresenter, _forceLayoutConfigurationPresenter, _chartOptionsPresenter, _validationOptionsPresenter, _displayUnitsPresenter, _applicationSettingsPresenter);
      }

      public void Edit(IUserSettings userSettings)
      {
         _view.LayoutViewVisible = _runOptions.IsDeveloperMode;
         _diagramOptionsPresenter.Edit(userSettings.DiagramOptions);
         _forceLayoutConfigurationPresenter.Edit(userSettings.ForceLayoutConfigutation);
         _chartOptionsPresenter.Edit(userSettings.ChartOptions);
         _validationOptionsPresenter.Edit(userSettings.ValidationSettings);
         _displayUnitsPresenter.Edit(userSettings.DisplayUnits);
         _view.BindTo(userSettings);
         _view.Display();
      }
   }
}