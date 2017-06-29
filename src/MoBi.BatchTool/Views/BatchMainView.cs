using System.Windows.Forms;
using MoBi.Assets;
using MoBi.BatchTool.Presenters;
using OSPSuite.Assets;
using OSPSuite.UI.Views;

namespace MoBi.BatchTool.Views
{
   public partial class BatchMainView : BaseView, IBatchMainView
   {
      private IBatchMainPresenter _presenter;

      public BatchMainView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IBatchMainPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnLoadPkmlFilesFromFolder.Click += (o, e) => OnEvent(_presenter.StartPkmlLoadFromFolder);
         btnLoadProjectFilesFromFolder.Click += (o, e) => OnEvent(_presenter.StartProjectLoadFromFolder);
         btnGenerateProjectOverview.Click += (o, e) => OnEvent(_presenter.StartGenerateProjectOverview);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.PRODUCT_NAME;
         ShowInTaskbar = true;
         StartPosition = FormStartPosition.CenterScreen;
         ApplicationIcon = ApplicationIcons.MoBi;
         Icon = ApplicationIcons.MoBi;
         btnLoadPkmlFilesFromFolder.Text = "Load PKML Files from Folder";
         btnLoadProjectFilesFromFolder.Text = "Load MoBi Project Files from Folder";
         btnGenerateProjectOverview.Text = "Generate MoBi Project Overview from Folder";
      }

   }
}