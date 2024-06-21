using System;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ModuleExplorerView : BaseExplorerView, IModuleExplorerView
   {
      private IModuleExplorerPresenter _moduleExplorerPresenter;

      public ModuleExplorerView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
         InitializeComponent();
         treeView.CompareNodeValues += compareNodeValues;
         treeView.DragDrop += HandleDragDrop;
      }

      public void AttachPresenter(IModuleExplorerPresenter presenter)
      {
         _moduleExplorerPresenter = presenter;
         base.AttachPresenter(presenter);
      }

      private void compareNodeValues(object sender, CompareNodeValuesEventArgs e)
      {
         e.Result = _moduleExplorerPresenter.OrderingComparisonFor(e.Node1.Tag as ITreeNode<IWithName>, e.Node2.Tag as ITreeNode<IWithName>);
      }

      private void HandleDragDrop(object sender, DragEventArgs e)
      {

         var data = e.Data.GetData(typeof(ITreeNode));
         var data2 = e.Data.GetData(typeof(ITreeNode<IBuildingBlock>));
         var data3 = e.Data.GetData(typeof(DragDropInfo));
         var target = data3 as DragDropInfo;
         var subject = target.Subject;
         var tv = this.TreeView as UxImageTreeView;
         var nodes = tv.Nodes;
      }

   }
}