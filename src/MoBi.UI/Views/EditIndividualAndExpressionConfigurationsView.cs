using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditIndividualAndExpressionConfigurationsView : BaseUserControl, IEditIndividualAndExpressionConfigurationsView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private IEditIndividualAndExpressionConfigurationsPresenter _presenter;
      private readonly ScreenBinder<IndividualSelectionDTO> _screenBinder = new ScreenBinder<IndividualSelectionDTO>();
      private readonly TreeNodeExplorerViewDragDropBinder _treeNodeExplorerViewDragDropBinder;

      public EditIndividualAndExpressionConfigurationsView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _imageListRetriever = imageListRetriever;
         _treeNodeExplorerViewDragDropBinder = new TreeNodeExplorerViewDragDropBinder(simulationExpressionsTree);
      }

      public override void InitializeResources()
      {
         projectExpressionsTree.TreeView.StateImageList = _imageListRetriever.AllImages16x16;
         simulationExpressionsTree.StateImageList = _imageListRetriever.AllImages16x16;
         layoutItemAddButton.AsAddButton();
         layoutItemRemoveButton.AsRemoveButton();
         layoutGroupExpression.Text = AppConstants.Captions.ExpressionProfiles;
         layoutGroupIndividual.Text = AppConstants.Captions.Individuals;
         cbIndividualSelection.SetImages(_imageListRetriever);
         EnableRemove = false;
         EnableAdd = false;
         Caption = AppConstants.Captions.ConfigureIndividualAndExpressions;
         ApplicationIcon = ApplicationIcons.Individual;
         simulationExpressionsTree.DataColumn.SortMode = ColumnSortMode.Custom;
         simulationExpressionsTree.DataColumn.SortOrder = SortOrder.Ascending;
         simulationExpressionsTree.CompareNodeValues += compareNodeValues;

         configureForMultiSelect(simulationExpressionsTree);
         configureForMultiSelect(projectExpressionsTree.TreeView);
      }

      private static void configureForMultiSelect(UxTreeView uxTreeView)
      {
         uxTreeView.OptionsSelection.MultiSelect = true;
         uxTreeView.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.RowSelect;
      }

      private void compareNodeValues(object sender, CompareNodeValuesEventArgs e)
      {
         e.Result = _presenter.CompareSelectedNodes(e.Node1.Tag as ITreeNode, e.Node2.Tag as ITreeNode);
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.SelectedIndividualBuildingBlock)
            .To(cbIndividualSelection)
            .WithImages(individual => _imageListRetriever.ImageIndex(individual.Icon))
            .WithValues(x => x.AllIndividuals);

         btnAdd.Click += (o, e) => OnEvent(addSelectedExpressions);
         btnRemove.Click += (o, e) => OnEvent(removeSelectedExpressions);

         projectExpressionsTree.TreeView.SelectionChanged += (sender, args) =>
            projectExpressionsTree.TreeView.SelectionChanged += (o, e) => OnEvent(projectTreeSelectionChanged);
         simulationExpressionsTree.SelectionChanged += (o, e) => OnEvent(simulationTreeSelectionChanged);

         simulationExpressionsTree.MouseDown += (o, e) => OnEvent(TreeMouseDown, e);
         simulationExpressionsTree.MouseMove += (o, e) => OnEvent(TreeMouseMove, e);
      }

      private void simulationTreeSelectionChanged()
      {
         _presenter.SimulationExpressionSelectionChanged(treeViewSelectionToTreeNodeList(simulationExpressionsTree.Selection));
      }

      private void projectTreeSelectionChanged()
      {
         _presenter.ProjectExpressionSelectionChanged(treeViewSelectionToTreeNodeList(projectExpressionsTree.TreeView.Selection));
      }

      public bool EnableAdd
      {
         get => btnAdd.Enabled;
         set => btnAdd.Enabled = value;
      }

      public bool EnableRemove
      {
         get => btnRemove.Enabled;
         set => btnRemove.Enabled = value;
      }

      public void RemoveUsedExpression(ITreeNode selectedNode)
      {
         simulationExpressionsTree.RemoveNode(selectedNode);
      }

      public void RemoveUnusedExpression(ITreeNode selectedNode)
      {
         projectExpressionsTree.TreeView.RemoveNode(selectedNode);
      }

      public void SortSelectedExpressions()
      {
         simulationExpressionsTree.Sort();
      }

      private void removeSelectedExpressions()
      {
         projectExpressionsTree.TreeView.Selection.Clear();
         _presenter.RemoveSelectedExpressions(treeViewSelectionToTreeNodeList(simulationExpressionsTree.Selection));
      }

      private void addSelectedExpressions()
      {
         simulationExpressionsTree.Selection.Clear();
         _presenter.AddSelectedExpressions(treeViewSelectionToTreeNodeList(projectExpressionsTree.TreeView.Selection));
      }

      private IReadOnlyList<ITreeNode> treeViewSelectionToTreeNodeList(TreeListSelection treeViewSelection)
      {
         return treeViewSelection.All().Where(x => x.Tag is ITreeNode).Select(x => x.Tag as ITreeNode).ToList();
      }

      protected virtual void TreeMouseMove(MouseEventArgs e)
      {
         _treeNodeExplorerViewDragDropBinder.TreeMouseMove(e);
      }

      protected virtual void TreeMouseDown(MouseEventArgs e)
      {
         _treeNodeExplorerViewDragDropBinder.TreeMouseDown(e);
      }

      public void AttachPresenter(IEditIndividualAndExpressionConfigurationsPresenter presenter)
      {
         _presenter = presenter;
         _treeNodeExplorerViewDragDropBinder.InitializeDragAndDrop(_presenter);
      }

      public void BindTo(IndividualSelectionDTO individualSelectionDTO)
      {
         _screenBinder.BindToSource(individualSelectionDTO);
      }

      public void AddUnusedExpression(ITreeNode treeNodeToAdd)
      {
         projectExpressionsTree.TreeView.AddNode(treeNodeToAdd);
         projectExpressionsTree.TreeView.Selection.Add(treeListNodeFor(projectExpressionsTree.TreeView, treeNodeToAdd));
      }

      public void AddUsedExpression(ITreeNode treeNodeToAdd)
      {
         simulationExpressionsTree.AddNode(treeNodeToAdd);
         simulationExpressionsTree.Selection.Add(treeListNodeFor(simulationExpressionsTree, treeNodeToAdd));
      }

      private TreeListNode treeListNodeFor(UxTreeView treeView, ITreeNode treeNodeToAdd)
      {
         return treeView.Nodes.Single(x => Equals(x.Tag, treeNodeToAdd));
      }

      private void disposeBinders()
      {
         _screenBinder.Dispose();
      }
   }
}