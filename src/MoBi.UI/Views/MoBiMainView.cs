using System;
using System.ComponentModel;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Utility.Container;
using DevExpress.XtraBars;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.UI.Views
{
   public partial class MoBiMainView : BaseShell, IMoBiMainView
   {
      private IMoBiMainViewPresenter _presenter;

      public MoBiMainView(IContainer container)
      {
         InitializeComponent();
         container.RegisterImplementationOf(ribbonControl.Manager);
         container.RegisterImplementationOf(ribbonControl.Manager as BarManager);
         container.RegisterImplementationOf(applicationMenu);
         container.RegisterImplementationOf(defaultLookAndFeel);
         container.RegisterImplementationOf(defaultLookAndFeel.LookAndFeel);
         container.RegisterImplementationOf(panelControlAppMenuFileLabels);
         container.RegisterImplementationOf(dockManager);
      }

      public void AttachPresenter(IMoBiMainViewPresenter presenter)
      {
         _presenter = presenter;
         base.AttachPresenter(presenter);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.MoBi;
      }

      protected override void OnLoad(EventArgs e)
      {
         SuspendLayout();
         WindowState = FormWindowState.Maximized;
         base.OnLoad(e);
         _presenter.RestoreLayout();
         ResumeLayout();
         _presenter.Run();
         Refresh();
      }

      public override void Initialize()
      {
         base.Initialize();
         InitializeDockManager(dockManager);
         InitializeRibbon(ribbonControl, applicationMenu, rightPaneAppMenu);
         var imageListRetriever = IoC.Resolve<IImageListRetriever>();
         InitializeImages(imageListRetriever);
      }

      protected override void RegisterRegions()
      {
         base.RegisterRegions();
         RegisterRegion(_panelBuildingBlockExplorer, RegionNames.BuildingBlockExplorer);
         RegisterRegion(_panelModuleExplorer, RegionNames.ModuleExplorer);
         RegisterRegion(_panelSimulationExplorer, RegionNames.SimulationExplorer);
         RegisterRegion(_panelHistoryBrowser, RegionNames.History);
         RegisterRegion(_panelSearch, RegionNames.Search);
         RegisterRegion(_panelWarningList, RegionNames.NotificationList);
         RegisterRegion(_panelComparison, RegionNames.Comparison);
         RegisterRegion(_panelJournal, RegionNames.Journal);
         RegisterRegion(_panelJournalDiagram, RegionNames.JournalDiagram);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         Closing += mainViewFormClosing;
      }

      private void mainViewFormClosing(object sender, CancelEventArgs e)
      {
         e.Cancel = _presenter.FormClosing();
      }
   }
}