﻿using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraTreeList;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class EditModuleConfigurationsView : BaseUserControl, IEditModuleConfigurationsView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private IEditModuleConfigurationsPresenter _presenter;
      private readonly ScreenBinder<ModuleConfigurationDTO> _screenBinder = new ScreenBinder<ModuleConfigurationDTO>();
      private readonly TreeNodeExplorerViewDragDropBinder _treeNodeExplorerViewDragDropBinder;

      public EditModuleConfigurationsView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _imageListRetriever = imageListRetriever;
         _treeNodeExplorerViewDragDropBinder = new TreeNodeExplorerViewDragDropBinder(selectedModuleTreeView);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.ConfigureModules;
         ApplicationIcon = ApplicationIcons.Module;
         layoutItemBtnAdd.AsAddButton();
         layoutItemBtnRemove.AsRemoveButton();
         startValuesSelectionGroup.Text = AppConstants.Captions.SelectStartValues;
         layoutItemMSVSelection.Text = AppConstants.Captions.MoleculeStartValues;
         layoutItemPSVSelection.Text = AppConstants.Captions.ParameterStartValues;
         moduleSelectionTreeView.TreeView.StateImageList = _imageListRetriever.AllImages16x16;
         selectedModuleTreeView.StateImageList = _imageListRetriever.AllImages16x16;
         layoutGroupSelectedModules.Text = AppConstants.Captions.SimulationModules;
         layoutGroupModuleSelection.Text = AppConstants.Captions.ProjectModules;
         EnableRemove = false;
         EnableAdd = false;
         selectedModuleTreeView.ShouldExpandAddedNode = false;
      }

      protected virtual void TreeMouseMove(MouseEventArgs e)
      {
         _treeNodeExplorerViewDragDropBinder.TreeMouseMove(e);
      }

      protected virtual void TreeMouseDown(MouseEventArgs e)
      {
         _treeNodeExplorerViewDragDropBinder.TreeMouseDown(e);
      }

      public override void InitializeBinding()
      {
         btnAdd.Click += (o, e) => OnEvent(() => _presenter.AddModuleConfiguration(moduleSelectionTreeView.TreeView.SelectedNode));
         btnRemove.Click += (o, e) => OnEvent(() => _presenter.RemoveModuleConfiguration(selectedModuleTreeView.SelectedNode));
         
         selectedModuleTreeView.SelectedNodeChanged += selectedNode => OnEvent(() => _presenter.SelectedModuleConfigurationNodeChanged(selectedNode));
         moduleSelectionTreeView.TreeView.SelectedNodeChanged += selectedNode => OnEvent(() => _presenter.SelectedModuleNodeChanged(selectedNode));

         _screenBinder.Bind(x => x.SelectedParameterStartValues).To(cbParameterStartValuesSelection).WithValues(x => getParameterStartValues()).Changed += () => OnEvent(refreshStartValues);
         _screenBinder.Bind(x => x.SelectedMoleculeStartValues).To(cbMoleculeStartValuesSelection).WithValues(x => getMoleculeStartValues()).Changed += () => OnEvent(refreshStartValues);

         moduleSelectionTreeView.TreeView.Columns[0].SortOrder = SortOrder.Ascending;
         clearStartValueSelectors();

         selectedModuleTreeView.MouseDown += (o, e) => OnEvent(TreeMouseDown, e);
         selectedModuleTreeView.MouseMove += (o, e) => OnEvent(TreeMouseMove, e);
         selectedModuleTreeView.CompareNodeValues += compareNodeValues;
         selectedModuleTreeView.DataColumn.SortMode = ColumnSortMode.Custom;
         selectedModuleTreeView.DataColumn.SortOrder = SortOrder.Ascending;
      }

      private void refreshStartValues()
      {
         _presenter.UpdateStartValuesFor(selectedModuleTreeView.SelectedNode);
      }

      private void compareNodeValues(object sender, CompareNodeValuesEventArgs e)
      {
         e.Result = _presenter.CompareSelectedNodes(e.Node1.Tag as ITreeNode, e.Node2.Tag as ITreeNode);
      }

      private IReadOnlyList<InitialConditionsBuildingBlock> getMoleculeStartValues()
      {
         return _presenter.InitialConditionsCollectionFor(selectedModuleTreeView.SelectedNode);
      }

      private IReadOnlyList<ParameterValuesBuildingBlock> getParameterStartValues()
      {
         return _presenter.ParameterStartValuesCollectionFor(selectedModuleTreeView.SelectedNode);
      }

      public void AttachPresenter(IEditModuleConfigurationsPresenter presenter)
      {
         _presenter = presenter;
         _treeNodeExplorerViewDragDropBinder.InitializeDragAndDrop(_presenter);
      }

      public void AddModuleNode(ITreeNode moduleNode)
      {
         moduleSelectionTreeView.TreeView.AddNode(moduleNode);
         moduleSelectionTreeView.TreeView.SelectNode(moduleNode);
      }


      public void AddModuleConfigurationNode(ITreeNode nodeToAdd)
      {
         AddNodeToSelectedModuleConfigurations(nodeToAdd);
         selectedModuleTreeView.SelectNode(nodeToAdd);
      }

      public void AddNodeToSelectedModuleConfigurations(ITreeNode nodeToAdd)
      {
         AddSelectedStartValue(nodeToAdd);
      }

      public void UnbindModuleConfiguration()
      {
         _screenBinder.DeleteBinding();
         clearStartValueSelectors();
      }

      public bool EnableAdd
      {
         get => btnAdd.Enabled;
         set => btnAdd.Enabled = value;
      }

      public void SortSelectedModules()
      {
         selectedModuleTreeView.Sort();
      }

      public void AddSelectedStartValue(ITreeNode startValueNode)
      {
         selectedModuleTreeView.AddNode(startValueNode);
         selectedModuleTreeView.ExpandNode(startValueNode);
      }

      public bool EnableRemove
      {
         get => btnRemove.Enabled;
         set => btnRemove.Enabled = value;
      }

      private void clearStartValueSelectors()
      {
         clearComboBox(cbParameterStartValuesSelection);
         clearComboBox(cbMoleculeStartValuesSelection);
         startValuesSelectionGroup.Enabled = false;
      }

      private static void clearComboBox(UxComboBoxEdit comboBox)
      {
         comboBox.Properties.Items.Clear();
         comboBox.SelectedItem = null;
      }

      public void BindModuleConfiguration(ModuleConfigurationDTO moduleConfiguration)
      {
         _screenBinder.BindToSource(moduleConfiguration);
         startValuesSelectionGroup.Enabled = true;
      }

      public void RemoveNodeFromSelectionView(ITreeNode selectedModule)
      {
         moduleSelectionTreeView.TreeView.RemoveNode(selectedModule);
      }

      public void RemoveNodeFromSelectedView(ITreeNode selectedModuleToRemove)
      {
         selectedModuleTreeView.RemoveNode(selectedModuleToRemove);
      }

      private void disposeBinders()
      {
         _screenBinder.Dispose();
      }
   }
}
