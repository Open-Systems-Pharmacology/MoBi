using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Core.Domain;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class SelectReferenceView : BaseUserControl, ISelectReferenceView
   {
      private ISelectReferencePresenter _presenter;
      private readonly UxTreeView _treeView;
      private bool _changeLocalisationAllowed;

      public SelectReferenceView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _treeView = new UxTreeView();
         Controls.Add(_treeView);

         grpEntityTreeView.FillWith(_treeView);
         _treeView.MouseDown += onMouseDown;
         _treeView.ShouldExpandAddedNode = false;
         _treeView.UseLazyLoading = true;
         _treeView.StateImageList = imageListRetriever.AllImages16x16;
         btEditSelectLocalisation.Properties.ReadOnly = true;
         _treeView.SelectedNodeChanged += selectionChanged;
         radioGroupReferenceType.Properties.Items.AddRange(getReferenceTypesForRadioGroup());
      }

      private void selectionChanged(ITreeNode treeNode)
      {
         _presenter.SelectionChanged(treeNode);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         groupControl.Text = AppConstants.Captions.SelectReferencesView;
         btEditSelectLocalisation.ToolTip = ToolTips.ReferenceSelector.SetLocalReferencePoint;
         radioGroupReferenceType.ToolTip = $"{ToolTips.ReferenceSelector.AbsolutePath}\n{ToolTips.ReferenceSelector.RelativePath}";
         lblLocalisation.Text = AppConstants.Captions.LocalReferencePoint;
         grpEntityTreeView.Text = AppConstants.Captions.PossibleReferencedObjects;
      }

      private RadioGroupItem[] getReferenceTypesForRadioGroup()
      {
         return new[]
         {
            createObjectPathRadioGroupItemFor(ObjectPathType.Absolute),
            createObjectPathRadioGroupItemFor(ObjectPathType.Relative)
         };
      }

      private RadioGroupItem createObjectPathRadioGroupItemFor(ObjectPathType pathType)
      {
         return new RadioGroupItem(pathType,
            AppConstants.PathType(Enum.GetName(typeof(ObjectPathType), pathType)));
      }

      public ObjectPathType ObjectPathType
      {
         get => (ObjectPathType) radioGroupReferenceType.Properties.Items[radioGroupReferenceType.SelectedIndex].Value;
         set => radioGroupReferenceType.SelectedIndex = radioGroupReferenceType.Properties.Items.GetItemIndexByValue(value);
      }

      private void onMouseDown(object sender, MouseEventArgs e)
      {
         this.DoWithinExceptionHandler(() =>
         {
            var hitInfo = _treeView.CalcHitInfo(e.Location);
            if (hitInfo?.Node == null)
               return;

            var treeNode = _treeView.NodeFrom(hitInfo.Node);

            if (!treeNode.IsAnImplementationOf<HierarchicalStructureNode>() || !e.Button.Equals(MouseButtons.Left))
               return;

            var node = treeNode.DowncastTo<HierarchicalStructureNode>();

            var dragItem = _presenter.GetReferenceObjectFrom(node.Tag);
            if (dragItem == null)
               return;

            DoDragDrop(dragItem, DragDropEffects.Copy);
         });
      }

      public void AttachPresenter(ISelectReferencePresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(IEnumerable<ITreeNode> nodes)
      {
         addNodes(nodes, true);
      }

      private void addNodes(IEnumerable<ITreeNode> nodes, bool clear)
      {
         _treeView.DoWithinBatchUpdate(() =>
         {
            if (clear)
               _treeView.Clear();

            nodes.Each(_treeView.AddNode);
         });
      }

      public IObjectBaseDTO SelectedDTO => _treeView.SelectedNode?.TagAsObject as IObjectBaseDTO;

      private void btEditSelectLocalisation_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         _presenter.GetLocalisationReferences();
      }

      public string Localisation
      {
         get => btEditSelectLocalisation.Text;
         set => btEditSelectLocalisation.Text = value;
      }

      public bool ChangeLocalisationAllowed
      {
         get => _changeLocalisationAllowed;
         set
         {
            _changeLocalisationAllowed = value;
            btEditSelectLocalisation.Properties.Buttons[0].Enabled = _changeLocalisationAllowed;
            btEditSelectLocalisation.Properties.Buttons[0].Visible = _changeLocalisationAllowed;
         }
      }

      public void AddNodes(IEnumerable<ITreeNode> nodes)
      {
         addNodes(nodes, false);
      }

      public void AddNode(ITreeNode node)
      {
         AddNodes(new[] {node});
      }

      public void Select(IEntity entityToSelect)
      {
         var nodeToSelect = _treeView.NodeById(entityToSelect.Id);
         if (nodeToSelect == null) return;
         _treeView.SelectNode(nodeToSelect);
      }

      public bool Shows(IObjectBase entity)
      {
         if (entity == null) return false;
         return GetNodes(entity).Any();
      }

      public void Remove(IObjectBase removedObject)
      {
         var treeNodes = GetNodes(removedObject);
         treeNodes.Each(treeNode => { _treeView.DestroyNode(treeNode); });
      }

      public IReadOnlyList<ITreeNode> GetNodes(IObjectBase objectBase)
      {
         return containsNodeWithId(objectBase.Id).ToList();
      }

      private IEnumerable<ITreeNode> containsNodeWithId(string id)
      {
         return _treeView.RootNodes.SelectMany(rootNode => rootNode.AllNodes).Where(node => node.TagAsObject.IsAnImplementationOf<IObjectBaseDTO>() && objectIdMatches(node.TagAsObject as IObjectBaseDTO, id));
      }

      private bool objectIdMatches(IObjectBaseDTO objectBaseDTO, string id)
      {
         return string.Equals(objectBaseDTO.Id, id);
      }

      private void rgReferenceType_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (_presenter == null) return;
         this.DoWithinExceptionHandler(() => _presenter.CheckPathCreationConfiguration());
      }
   }
}