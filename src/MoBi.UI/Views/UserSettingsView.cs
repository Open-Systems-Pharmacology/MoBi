using MoBi.Assets;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class UserSettingsView : BaseModalView, IUserSettingsView
   {
      private readonly ScreenBinder<IUserSettings> _screenBinder;
      private IUserSettingsPresenter _presenter;

      public UserSettingsView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<IUserSettings>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.RenameDependentObjectsDefault).To(chkRenameDependent);
         _screenBinder.Bind(x => x.MRUListItemCount).To(tbMRUFiles);
         _screenBinder.Bind(x => x.DecimalPlace).To(tbDecimalPlace);
         _screenBinder.Bind(x => x.MaximumNumberOfCoresToUse).To(tbNumberOfProcessors);

         _screenBinder.Bind(x => x.DefaultParameterGroupingModeForPIAndSA)
            .To(cbDefaultParameterGroupingModePISA)
            .WithValues(x => _presenter.AllParameterGroupingMode())
            .AndDisplays(x => _presenter.AllParameterGroupingModeDisplay());

         RegisterValidationFor(_screenBinder);
      }

      public void BindTo(IUserSettings userSettings)
      {
         _screenBinder.BindToSource(userSettings);
      }

      public void SetDiagramOptionsView(IView view)
      {
         tabDiagramOptions.FillWith(view);
      }

      public void SetLayoutView(IView forceLayoutConfigurationView)
      {
         tabFlowLayout.FillWith(forceLayoutConfigurationView);
      }

      public void SetChartOptionsView(IView view)
      {
         tabChartOptions.FillWith(view);
      }

      public void SetValidationOptionsView(IView view)
      {
         pnlValidationOptions.FillWith(view);
      }

      public void SetDisplayUnitsView(IView view)
      {
         tabDisplayUnits.FillWith(view);
      }

      public void SetApplicationSettingsView(IView view)
      {
         tabApplicationSettings.FillWith(view);
      }

      public bool LayoutViewVisible
      {
         get => tabFlowLayout.PageVisible;
         set => tabFlowLayout.PageVisible = value;
      }

      public void AttachPresenter(IUserSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.Options;
         layoutItemDecimalPlace.Text = AppConstants.Captions.DecimalPlace.FormatForLabel();
         layoutGroupValidationItems.Text = AppConstants.Captions.ValidationOptions;
         layoutItemNumberOfRecentProjects.Text = AppConstants.Captions.MRUListItemCount.FormatForLabel();
         CancelVisible = false;
         tabDisplayUnits.Text = AppConstants.Captions.DefaultDisplayUnits;
         tabApplicationSettings.Text = AppConstants.Captions.ApplicationSettings;
         ApplicationIcon = ApplicationIcons.Settings;
         layoutItemNumberOfProcessors.Text = Captions.NumberOfProcessors.FormatForLabel();
         layoutItemParameterLayout.Text = AppConstants.Captions.DefaultParameterLayout.FormatForLabel();
      }

      public override bool HasError => _screenBinder.HasError;
   }
}