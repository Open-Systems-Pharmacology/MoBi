using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using MoBi.BatchTool.Presenters;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.BatchTool.Views
{
   public partial class FileFromFolderRunnerView : BaseView, IFileFromFolderRunnerView
   {
      private IFileFromFolderPresenter _presenter;
      private readonly ScreenBinder<FileFromFolderDTO> _screenBinder;

      public FileFromFolderRunnerView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<FileFromFolderDTO>();
      }

      public void AttachPresenter(IFileFromFolderPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ShowInTaskbar = true;
         StartPosition = FormStartPosition.CenterScreen;
         btnExit.InitWithImage(ApplicationIcons.Exit, "Exit");
         btnCalculate.InitWithImage(ApplicationIcons.Run, "Calculate");
         layoutItemInputFolder.Text = "Input Folder".FormatForLabel();
         layoutItemButtonCalculate.AdjustLargeButtonSize();
         layoutItemButtonExit.AdjustLargeButtonSize();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.InputFolder).To(btnInputFolder);
         RegisterValidationFor(_screenBinder);
         btnExit.Click += (o, e) => OnEvent(_presenter.Exit);
         btnCalculate.Click += (o, e) => OnEvent(_presenter.Run);
         btnInputFolder.ButtonClick += (o, e) => OnEvent(_presenter.SelectInputFolder);
      }

      protected override void OnValidationError(Control control, string error)
      {
         base.OnValidationError(control, error);
         SetOkButtonEnable();
      }

      protected override void OnClearError(Control control)
      {
         base.OnClearError(control);
         SetOkButtonEnable();
      }

      protected virtual void SetOkButtonEnable()
      {
         btnCalculate.Enabled = !_screenBinder.HasError;
      }

      public void Display()
      {
         Show();
      }

      public void AddLogView(IView view)
      {
         panelLog.FillWith(view);
      }

      public bool CalculateEnabled
      {
         set { btnCalculate.Enabled = value; }
      }

      public void BindTo(FileFromFolderDTO dto)
      {
         _screenBinder.BindToSource(dto);
      }
   }
}