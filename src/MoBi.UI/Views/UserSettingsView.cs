using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.Utils;
using MoBi.Assets;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class UserSettingsView : BaseModalView, IUserSettingsView
   {
      private ScreenBinder<IUserSettings> _screenBinder;

      public UserSettingsView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<IUserSettings>();
         _screenBinder.Bind(x => x.RenameDependentObjectsDefault).To(chkRenameDependent);
         _screenBinder.Bind(x => x.MRUListItemCount).To(tbMRUFiles);
         _screenBinder.Bind(x => x.DecimalPlace).To(tbDecimalPlace);
         _screenBinder.Bind(x => x.MaximumNumberOfCoresToUse).To(tbNumberOfProcessors);

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

      public bool LayoutViewVisible
      {
         get { return tabFlowLayout.PageVisible; }
         set { tabFlowLayout.PageVisible = value; }
      }

      public void AttachPresenter(IUserSettingsPresenter presenter)
      {
         //nothing to do here
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.UserSettings;
         layoutItemDecimalPlace.Text = AppConstants.Captions.DecimalPlace.FormatForLabel();
         layoutItemValidationItems.Text = AppConstants.Captions.ValidationOptions;
         layoutItemNumberOfRecentProjects.Text = AppConstants.Captions.MRUListItemCount.FormatForLabel();
         layoutItemValidationItems.TextLocation = Locations.Top;
         CancelVisible = false;
         tabDisplayUnits.Text = AppConstants.Captions.DefaultDisplayUnits;
         Icon = ApplicationIcons.Settings;
         layoutItemNumberOfProcessors.Text = Captions.NumberOfProcessors.FormatForLabel();
      }

      public override bool HasError => _screenBinder.HasError;
   }
}