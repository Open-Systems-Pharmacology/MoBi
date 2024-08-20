using System;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditTableFormulaView : BaseUserControl, IEditTableFormulaView
   {
      private IEditTableFormulaPresenter _presenter;

      public EditTableFormulaView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditTableFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddTableView(IView tableView)
      {
         splitContainerControl.Panel1.FillWith(tableView);
      }

      public void AddChartView(IView chartView)
      {
         splitContainerControl.Panel2.FillWith(chartView);
      }

      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);
         splitContainerControl.SplitterPosition = splitContainerControl.Height / 2;
      }

      public bool ReadOnly { get; set; }
   }
}