using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.UI;
using OSPSuite.UI.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList.Nodes;
using MoBi.Assets;
using MoBi.Core.Domain;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

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
         _treeView.SelectedNodeChanged += selctionChanged;
         rgReferenceType.Properties.Items.AddRange(getReferenceTypesForRadioGroup());
         
      }

      private void selctionChanged(ITreeNode treeNode)
      {
         _presenter.SelectionChanged(treeNode);
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_ReactionKinetics;

      public override void InitializeResources()
      {
          base.InitializeResources();
          groupControl1.Text = AppConstants.Captions.SelectReferencesView;
          
          btEditSelectLocalisation.ToolTip = ToolTips.ReferenceSelector.SetLocalReferencePoint;
          rgReferenceType.ToolTip = String.Format("{0}\n{1}", 
                                                  ToolTips.ReferenceSelector.AbsolutePath,
                                                  ToolTips.ReferenceSelector.RelativePath);
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
                                   AppConstants.PathType(Enum.GetName(typeof (ObjectPathType), pathType)));
      }

      public ObjectPathType ObjectPathType
      {
         get { return (ObjectPathType) rgReferenceType.Properties.Items[rgReferenceType.SelectedIndex].Value; }
         set { rgReferenceType.SelectedIndex = rgReferenceType.Properties.Items.GetItemIndexByValue(value); }
      }

      private void onMouseDown(object sender, MouseEventArgs e)
      {
         this.DoWithinExceptionHandler(() =>
                                          {
                                             var hitInfo = _treeView.CalcHitInfo(e.Location);
                                             if (hitInfo == null) return;
                                             if (hitInfo.Node == null) return;
                                             var treeNode = _treeView.NodeFrom(hitInfo.Node);
                                             if (treeNode.IsAnImplementationOf<HierarchicalStructureNode>() && e.Button.Equals(MouseButtons.Left))
                                             {
                                                HierarchicalStructureNode node = (HierarchicalStructureNode) treeNode;

                                                var dragItem = _presenter.GetReferenceObjectFrom(node.Tag);
                                                if (dragItem != null)
                                                {
                                                   DoDragDrop(dragItem, DragDropEffects.Copy);
                                                }
                                             }
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
         _treeView.DoWithinBatchUpdate(()=>{
                                                if (clear)
                                                   _treeView.Clear();
                                                nodes.Each(_treeView.AddNode);
                                             });
      }

      public IObjectBaseDTO SelectedDTO
      {
         get
         {
            if (_treeView.SelectedNode == null) return null;
            return _treeView.SelectedNode.TagAsObject as IObjectBaseDTO; }
      }

      private void btEditSelectLocalisation_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         _presenter.GetLocalisationReferences();
      }

      public string Localisation
      {
         get { return btEditSelectLocalisation.Text; }
         set { btEditSelectLocalisation.Text = value; }
      }

      public bool ChangeLocalisationAllowed
      {
         get { return _changeLocalisationAllowed; }
         set
         {
            _changeLocalisationAllowed = value;
            btEditSelectLocalisation.Properties.Buttons[0].Enabled = _changeLocalisationAllowed;
            btEditSelectLocalisation.Properties.Buttons[0].Visible = _changeLocalisationAllowed;
         }
      }

      public void AddNodes(IEnumerable<ITreeNode> nodes)
      {
         addNodes(nodes,false);
      }

      public void AddNode(ITreeNode node)
      {
        AddNodes(new[]{node});
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
         treeNodes.Each(treeNode =>
         {
            _treeView.DestroyNode(treeNode);            
         });
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