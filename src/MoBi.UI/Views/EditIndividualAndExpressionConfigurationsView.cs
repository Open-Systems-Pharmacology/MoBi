using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraTreeList;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.UI.Controls;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Binders;

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

         btnAdd.Click += (o, e) => OnEvent(addSelectedExpression);
         btnRemove.Click += (o, e) => OnEvent(removeSelectedExpression);

         projectExpressionsTree.TreeView.SelectedNodeChanged += selectedNode => OnEvent(() => _presenter.ProjectExpressionSelectionChanged(selectedNode));
         simulationExpressionsTree.SelectedNodeChanged += selectedNode => OnEvent(() => _presenter.SimulationExpressionSelectionChanged(selectedNode));

         simulationExpressionsTree.MouseDown += (o, e) => OnEvent(TreeMouseDown, e);
         simulationExpressionsTree.MouseMove += (o, e) => OnEvent(TreeMouseMove, e);
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

      private void removeSelectedExpression()
      {
         _presenter.RemoveSelectedExpression(simulationExpressionsTree.SelectedNode);
      }

      private void addSelectedExpression()
      {
         _presenter.AddSelectedExpression(projectExpressionsTree.TreeView.SelectedNode);
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
         projectExpressionsTree.TreeView.SelectNode(treeNodeToAdd);
      }

      public void AddUsedExpression(ITreeNode treeNodeToAdd)
      {
         simulationExpressionsTree.AddNode(treeNodeToAdd);
         simulationExpressionsTree.SelectNode(treeNodeToAdd);
      }

      private void disposeBinders()
      {
         _screenBinder.Dispose();
      }
   }
}
