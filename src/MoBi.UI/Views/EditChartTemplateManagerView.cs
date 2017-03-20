using MoBi.Assets;
using OSPSuite.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditChartTemplateManagerView : BaseUserControl, IEditChartTemplateManagerView
   {
      private IEditChartTemplateManagerPresenter _presenter;

      public EditChartTemplateManagerView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         editButton.Text = AppConstants.Captions.EnableEditing;
         saveButton.Text = AppConstants.Captions.SaveChanges;
         cancelButton.Text = AppConstants.Captions.Cancel;

         editButtonControlItem.AdjustLargeButtonSize();
         saveButtonControlItem.AdjustLargeButtonSize();
         cancelButtonControlItem.AdjustLargeButtonSize();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         editButton.Click += (o, e) => OnEvent(enableEdits);
         saveButton.Click += (o, e) => OnEvent(saveEdits);
         cancelButton.Click += (o, e) => OnEvent(cancelEdits);
      }

      private void cancelEdits()
      {
         disableEdits();
         _presenter.CancelChanges();
      }

      private void saveEdits()
      {
         disableEdits();
         _presenter.CommitChanges();
      }

      private void enableEdits()
      {
         setEditable(true);
      }

      private void setEditable(bool editable)
      {
         panelControl.Enabled = editable;
         saveButton.Enabled = editable;
         cancelButton.Enabled = editable;
         editButton.Enabled = !editable;
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return _presenter.Icon; }
      }

      public void AttachPresenter(IEditChartTemplateManagerPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetSubView(IView baseView)
      {
         panelControl.FillWith(baseView);
         disableEdits();
      }

      private void disableEdits()
      {
         setEditable(false);
      }
   }
}
