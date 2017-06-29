using MoBi.Assets;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.UICommand
{
   public interface IUserSettingsPresenter : IDisposablePresenter
   {
      void Edit(IUserSettings userSettings);
      void SelectPKSimPath();
   }

   public class UserSettingsPresenter : MoBiDisposablePresenter<IUserSettingsView, IUserSettingsPresenter>, IUserSettingsPresenter
   {
      private readonly IDiagramOptionsPresenter _diagramOptionsPresenter;
      private readonly IForceLayoutConfigurationPresenter _forceLayoutConfigurationPresenter;
      private readonly IChartOptionsPresenter _chartOptionsPresenter;
      private readonly IValidationOptionsPresenter _validationOptionsPresenter;
      private readonly IDisplayUnitsPresenter _displayUnitsPresenter;
      private readonly IStartOptions _runOptions;
      private readonly IDialogCreator _dialogCreator;
      private IUserSettings _userSettings;

      public UserSettingsPresenter(IUserSettingsView view, IDiagramOptionsPresenter diagramOptionsPresenter,
         IForceLayoutConfigurationPresenter forceLayoutConfigurationPresenter, IChartOptionsPresenter chartOptionsPresenter,
         IValidationOptionsPresenter validationOptionsPresenter, IDisplayUnitsPresenter displayUnitsPresenter, IStartOptions runOptions, IDialogCreator dialogCreator) : base(view)
      {
         _diagramOptionsPresenter = diagramOptionsPresenter;
         _forceLayoutConfigurationPresenter = forceLayoutConfigurationPresenter;
         _chartOptionsPresenter = chartOptionsPresenter;
         _validationOptionsPresenter = validationOptionsPresenter;
         _displayUnitsPresenter = displayUnitsPresenter;
         _runOptions = runOptions;
         _dialogCreator = dialogCreator;
         _view.SetDiagramOptionsView(_diagramOptionsPresenter.View);
         _view.SetLayoutView(_forceLayoutConfigurationPresenter.View);
         _view.SetChartOptionsView(_chartOptionsPresenter.View);
         _view.SetValidationOptionsView(validationOptionsPresenter.View);
         _view.SetDisplayUnitsView(_displayUnitsPresenter.View);
         AddSubPresenters(_diagramOptionsPresenter, _forceLayoutConfigurationPresenter, _chartOptionsPresenter, _validationOptionsPresenter, _displayUnitsPresenter);
      }

      public void Edit(IUserSettings userSettings)
      {
         _userSettings = userSettings;
         _view.LayoutViewVisible = _runOptions.IsDeveloperMode;
         _diagramOptionsPresenter.Edit(userSettings.DiagramOptions);
         _forceLayoutConfigurationPresenter.Edit(userSettings.ForceLayoutConfigutation);
         _chartOptionsPresenter.Edit(userSettings.ChartOptions);
         _validationOptionsPresenter.Edit(userSettings.ValidationSettings);
         _displayUnitsPresenter.Edit(userSettings.DisplayUnits);
         _view.BindTo(userSettings);
         _view.Display();
      }

      public void SelectPKSimPath()
      {
         var pkSimPath = _dialogCreator.AskForFileToOpen(AppConstants.Captions.SelectPKSimExecutablePath, AppConstants.Filter.PKSIM_FILE_FILTER, Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(pkSimPath)) return;
         _userSettings.PKSimPath = pkSimPath;
      }
   }
}