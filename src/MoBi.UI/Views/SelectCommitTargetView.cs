using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectCommitTargetView : BaseModalView, ISelectCommitTargetView
   {
      private ISelectCommitTargetPresenter _presenter;
      private ScreenBinder<CommitTargetDTO> _screenBinder;

      public SelectCommitTargetView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISelectCommitTargetPresenter presenter) => _presenter = presenter;

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<CommitTargetDTO>();
         _screenBinder.Bind(x => x.Module).To(cmbModule).WithValues(x => _presenter.AllModules).Changed += () => OnEvent(_presenter.ModuleChanged);
         _screenBinder.Bind(x => x.ParameterValues).To(cmbParameterValues).WithValues(x => _presenter.AllParameterValuesFor(x.Module));
         _screenBinder.Bind(x => x.InitialConditions).To(cmbInitialConditions).WithValues(x => _presenter.AllInitialConditionsFor(x.Module));
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemModule.Text = AppConstants.Captions.Module.FormatForLabel();
         layoutItemParameterValues.Text = ObjectTypes.ParameterValuesBuildingBlock.FormatForLabel();
         layoutItemInitialConditions.Text = ObjectTypes.InitialConditionsBuildingBlock.FormatForLabel();
         descriptionLabel.AsDescription();
         Text = AppConstants.Captions.SelectCommitTarget;
      }

      public void BindTo(CommitTargetDTO commitTargetDTO) => _screenBinder.BindToSource(commitTargetDTO);

      public void SetDescription(string description) => descriptionLabel.Text = description;

      public void HideParameterValuesSelection() => layoutItemParameterValues.Visibility = LayoutVisibility.Never;

      public void HideInitialConditionsSelection() => layoutItemInitialConditions.Visibility = LayoutVisibility.Never;

      private void disposeBinders() => _screenBinder.Dispose();
   }
}
