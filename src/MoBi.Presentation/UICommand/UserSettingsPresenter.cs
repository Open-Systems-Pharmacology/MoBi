using System.Collections.Generic;
using MoBi.Core;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.UICommand
{
   public interface IUserSettingsPresenter : IDisposablePresenter
   {
      void Edit(ICloneableUserSettings userSettings);
      IReadOnlyList<ParameterGroupingModeForParameterAnalyzable> AllParameterGroupingMode();
   }

   public class UserSettingsPresenter : MoBiDisposablePresenter<IUserSettingsView, IUserSettingsPresenter>, IUserSettingsPresenter
   {
      private readonly IDiagramOptionsPresenter _diagramOptionsPresenter;
      private readonly IChartOptionsPresenter _chartOptionsPresenter;
      private readonly IValidationOptionsPresenter _validationOptionsPresenter;
      private readonly IDisplayUnitsPresenter _displayUnitsPresenter;
      private readonly IApplicationSettingsPresenter _applicationSettingsPresenter;
      private readonly IApplicationSettings _applicationSettings;

      public UserSettingsPresenter(IUserSettingsView view, IDiagramOptionsPresenter diagramOptionsPresenter,
         IChartOptionsPresenter chartOptionsPresenter,
         IValidationOptionsPresenter validationOptionsPresenter, IDisplayUnitsPresenter displayUnitsPresenter,
         IApplicationSettingsPresenter applicationSettingsPresenter, IApplicationSettings applicationSettings) : base(view)
      {
         _diagramOptionsPresenter = diagramOptionsPresenter;
         _chartOptionsPresenter = chartOptionsPresenter;
         _validationOptionsPresenter = validationOptionsPresenter;
         _displayUnitsPresenter = displayUnitsPresenter;
         _applicationSettingsPresenter = applicationSettingsPresenter;
         _applicationSettings = applicationSettings;
         _view.SetDiagramOptionsView(_diagramOptionsPresenter.View);
         _view.SetChartOptionsView(_chartOptionsPresenter.View);
         _view.SetValidationOptionsView(validationOptionsPresenter.View);
         _view.SetDisplayUnitsView(_displayUnitsPresenter.View);
         _view.SetApplicationSettingsView(_applicationSettingsPresenter.View);
         AddSubPresenters(_diagramOptionsPresenter, _chartOptionsPresenter, _validationOptionsPresenter, _displayUnitsPresenter, _applicationSettingsPresenter);
      }

      public void Edit(ICloneableUserSettings userSettings)
      {
         var clone = userSettings.Clone();
         var applicationSettingsDTO = new ApplicationSettingsDTO(_applicationSettings);

         _diagramOptionsPresenter.Edit(clone.DiagramOptions);
         _chartOptionsPresenter.Edit(clone.ChartOptions);
         _validationOptionsPresenter.Edit(clone.ValidationSettings);
         _displayUnitsPresenter.Edit(clone.DisplayUnits);
         _applicationSettingsPresenter.Edit(applicationSettingsDTO);
         _view.BindTo(clone);
         _view.Display();

         if (_view.Canceled)
            return;

         userSettings.UpdatePropertiesFrom(clone);
         applicationSettingsDTO.UpdateApplicationSettings(_applicationSettings);
      }

      public IReadOnlyList<ParameterGroupingModeForParameterAnalyzable> AllParameterGroupingMode() => ParameterGroupingModesForParameterAnalyzable.All();
   }
}
