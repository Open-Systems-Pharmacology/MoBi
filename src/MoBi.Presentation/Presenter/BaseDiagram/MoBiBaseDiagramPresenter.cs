using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views.BaseDiagram;
using Northwoods.Go;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter.BaseDiagram
{
   public interface IMoBiBaseDiagramPresenter : IBaseDiagramPresenter
   {
      IDiagramPopupMenuBase GetPopupMenu(IBaseNode baseNode);

      //Model manipulation
      void ModelSelect(string id);
      void Link(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2);

      void SaveContainerToXml(IContainerBase containerBase, string diagramTemplateXmlFilePath);

      void Undo();

      PointF CurrentInsertLocation { get; set; }

      void ApplyLayoutTemplate(IContainerBase containerBase, string diagramTemplateXmlFilePath, bool recursive);
      void ApplyLayoutTemplateToSelection();

      void ConfigureLayout();
      void RemoveLinks(GoSelection links);
   }

   public interface IMoBiBaseDiagramPresenter<T> : IMoBiBaseDiagramPresenter, IBaseDiagramPresenter<T>,
      IListener<EntitySelectedEvent>,
      IListener<AddedEvent>
      where T : IWithDiagramFor<T>
   {
   }

   /// <summary>
   ///    Base presenter class for Base diagram model with container nodes and neighborhood nodes.
   /// </summary>
   public abstract class MoBiBaseDiagramPresenter<TView, TPresenter, TModel> : BaseDiagramPresenter<TView, TPresenter, TModel>, IMoBiBaseDiagramPresenter<TModel>
      where TView : IMoBiBaseDiagramView, IView<TPresenter>
      where TPresenter : IBaseDiagramPresenter
      where TModel : class, IWithDiagramFor<TModel>
   {
      protected readonly IMoBiContext _context;
      protected readonly IUserSettings _userSettings;

      protected IDiagramPopupMenuBase _containerPopupMenu;
      protected IDiagramPopupMenuBase _neighborhoodPopupMenu;
      protected IDiagramPopupMenuBase _diagramPopupMenu;
      private readonly IDiagramTask _diagramTask;

      protected MoBiBaseDiagramPresenter(TView view,
         IContainerBaseLayouter layouter,
         IDialogCreator dialogCreator,
         IDiagramModelFactory diagramModelFactory,
         IUserSettings userSettings,
         IMoBiContext context,
         IDiagramTask diagramTask,
         IStartOptions runOptions)
         : base(view, layouter, dialogCreator, diagramModelFactory)
      {
         _context = context;
         _diagramTask = diagramTask;
         _userSettings = userSettings;
         LayoutConfiguration = userSettings.ForceLayoutConfigutation;
         _diagramPopupMenu = new DiagramPopupMenuBase(this, runOptions);
         _containerPopupMenu = _diagramPopupMenu;
         _neighborhoodPopupMenu = _diagramPopupMenu;
      }

      protected override IDiagramOptions GetDiagramOptions()
      {
         return _userSettings.DiagramOptions;
      }

      public PointF CurrentInsertLocation
      {
         get { return DiagramManager.CurrentInsertLocation; }
         set { DiagramManager.CurrentInsertLocation = value; }
      }

      /// <summary>
      ///    Handle select event by selecting node.
      /// </summary>
      public void Handle(EntitySelectedEvent eventToHandle)
      {
         if (DiagramManager == null) return;
         if (!DiagramManager.MustHandleExisting(eventToHandle.ObjectBase.Id)) return;

         IBaseNode baseNode = DiagramModel.GetNode(eventToHandle.ObjectBase.Id);
         if (baseNode == null) return;

         IContainerBase parentContainer = baseNode.GetParent();

         // Show node and parents
         baseNode.Hidden = false;
         baseNode.ShowParents();

         _view.ExpandParents(baseNode);

         // Expand parent
         var parentContainerNode = parentContainer as IContainerNode;
         if (parentContainerNode != null)
            Focus(parentContainerNode);

         _view.ClearSelection();
         _view.Select(baseNode);
         _view.CenterAt(baseNode);
      }

      public virtual IDiagramPopupMenuBase GetPopupMenu(IBaseNode baseNode)
      {
         if (baseNode == null) return _diagramPopupMenu;
         if (baseNode.IsAnImplementationOf(typeof(IContainerNode))) return _containerPopupMenu;
         if (baseNode.IsAnImplementationOf(typeof(INeighborhoodNode))) return _neighborhoodPopupMenu;
         return _diagramPopupMenu;
      }

      public void ModelSelect(string id)
      {
         if (_view.IsMoleculeNode(DiagramModel.GetNode(id)))
            return;

         var objectBase = _context.Get<IObjectBase>(id);
         _context.PublishEvent(new EntitySelectedEvent(objectBase, this));
      }

      public abstract void Link(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2);
      protected abstract void Unlink(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2);

      public override void ShowContextMenu(IBaseNode baseNode, Point popupLocation, PointF locationInDiagramView)
      {
         var popupMenu = GetPopupMenu(baseNode);
         IContainerBase containerBase = null;
         if (baseNode == null)
            containerBase = DiagramModel;
         else if (baseNode.IsAnImplementationOf<IContainerNode>())
            containerBase = baseNode as IContainerNode;

         popupMenu?.Show(_view, popupLocation, locationInDiagramView, containerBase, baseNode);
      }

      public void SaveContainerToXml(IContainerBase containerBase, string diagramTemplateXmlFilePath)
      {
         if (string.IsNullOrEmpty(diagramTemplateXmlFilePath))
         {
            diagramTemplateXmlFilePath = _dialogCreator.AskForFileToSave(AppConstants.Captions.SaveLayoutToFile, AppConstants.Filter.MOBI_DIAGRAM_TEMPLATE_FILTER, AppConstants.DirectoryKey.LAYOUT);
         }

         if (string.IsNullOrEmpty(diagramTemplateXmlFilePath)) return;

         _diagramTask.SaveContainerToXml(containerBase, diagramTemplateXmlFilePath);
      }

      public void ApplyLayoutTemplate(IContainerBase containerBase, string diagramTemplateXmlFilePath, bool recursive)
      {
         if (string.IsNullOrEmpty(diagramTemplateXmlFilePath))
         {
            diagramTemplateXmlFilePath = _dialogCreator.AskForFileToOpen(AppConstants.Captions.OpenLayoutFromFile, AppConstants.Filter.MOBI_DIAGRAM_TEMPLATE_FILTER, AppConstants.DirectoryKey.LAYOUT);
         }

         if (string.IsNullOrEmpty(diagramTemplateXmlFilePath))
            return;

         try
         {
            _view.BeginUpdate();
            _diagramTask.ApplyLayoutTemplate(containerBase, diagramTemplateXmlFilePath, DiagramModel, DiagramManager.RefreshFromDiagramOptions, recursive);
            foreach (var container in containerBase.GetAllChildren<IContainerNode>())
            {
               container.IsExpanded = container.IsExpandedByDefault; // set Default Expansion in the container, because subnodes may be collapsed during Layout application, e.g. in application of container layout to diagram
            }
         }
         finally
         {
            _view.EndUpdate();
         }
      }

      protected IDiagramModel LoadDiagramTemplate(string diagramTemplateXmlFilePath)
      {
         return _diagramTask.LoadDiagramTemplate(diagramTemplateXmlFilePath);
      }

      public void ApplyLayoutTemplateToSelection()
      {
         string diagramTemplateXmlFilePath = _dialogCreator.AskForFileToOpen("Open Named LayoutTemplate", AppConstants.Filter.MOBI_DIAGRAM_TEMPLATE_FILTER, AppConstants.DirectoryKey.LAYOUT);
         if (string.IsNullOrEmpty(diagramTemplateXmlFilePath))
            return;

         if (!File.Exists(diagramTemplateXmlFilePath))
            throw new FileNotFoundException("File not found", diagramTemplateXmlFilePath);

         foreach (var node in _view.GetSelectedNodes<IContainerNode>())
         {
            ApplyLayoutTemplate(node, diagramTemplateXmlFilePath, false);
         }
      }

      protected void ResetViewSize()
      {
         DiagramModel.RefreshSize();
         foreach (var node in DiagramModel.GetDirectChildren<IContainerNode>())
         {
            if (node.Visible)
            {
               _view.CenterAt(node);
               break;
            }
         }
         _view.Refresh();
      }

      public void ConfigureLayout()
      {
         var forceLayoutConfigurationPresenter = IoC.Resolve<IForceLayoutConfigurationPresenter>();
         forceLayoutConfigurationPresenter.Edit(LayoutConfiguration);
      }

      public void RemoveLinks(GoSelection links)
      {
         links.ToList().Each(link =>
         {
            if (isBaseLink(link) && isGoLink(link))
               unlinkBaseNodes(link as IBaseLink, link as GoLink);
            else if (link.IsAnImplementationOf<INeighborhoodNode>())
               unlinkNeighborhoodNode(link);
         });
      }

      private static bool isGoLink(GoObject goLink)
      {
         return goLink is GoLink;
      }

      private static bool isBaseLink(GoObject baseLink)
      {
         return baseLink is IBaseLink;
      }

      private void unlinkBaseNodes(IBaseLink baseLink, GoLink goLink)
      {
         Unlink(baseLink.GetFromNode(), baseLink.GetToNode(), goLink.FromPort.UserObject, goLink.ToPort.UserObject);
      }

      private void unlinkNeighborhoodNode(GoObject itemToDelete)
      {
         var neighborhoodNode = (INeighborhoodNode) itemToDelete;
         Unlink(neighborhoodNode.FirstNeighbor, neighborhoodNode.SecondNeighbor, null, null);
      }

      public void Undo()
      {
         DiagramModel.Undo();
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (DiagramManager == null) return;
         var addedNode = _model.DiagramModel.GetNode(eventToHandle.AddedObject.Id);
         if (addedNode == null || addedNode.IsAnImplementationOf<INeighborhoodNode>()) return;

         if (DiagramManager.InsertLocationHasChanged())
         {
            // move node to "free" location
            var freeNodes = new List<IHasLayoutInfo> {addedNode};
            Layout(addedNode.GetParent(), AppConstants.Diagram.Base.LayoutDepthChildren, freeNodes);
         }
         DiagramManager.UpdateInsertLocation();
      }
   }
}