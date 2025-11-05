using System.Drawing;
using System.IO;
using System.Linq;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter.SpaceDiagram
{
   public interface ISpatialStructureDiagramPresenter : IMoBiBaseDiagramPresenter<MoBiSpatialStructure>, IPresenter<ISpatialStructureDiagramView>
   {
   }

   public class SpatialStructureDiagramPresenter : MoBiBaseDiagramPresenter<ISpatialStructureDiagramView, ISpatialStructureDiagramPresenter, MoBiSpatialStructure>, ISpatialStructureDiagramPresenter
   {
      private readonly IMoBiConfiguration _configuration;

      public SpatialStructureDiagramPresenter(
         ISpatialStructureDiagramView view,
         IContainerBaseLayouter layouter,
         IUserSettings userSettings,
         IMoBiContext context,
         IDialogCreator dialogCreator,
         IMoBiConfiguration configuration,
         IDiagramTask diagramTask,
         IStartOptions runOptions,
         IDiagramModelFactory diagramModelFactory)
         : base(view, layouter, dialogCreator, diagramModelFactory, userSettings, context, diagramTask, runOptions)
      {
         _configuration = configuration;
         _diagramPopupMenu = new PopupMenuSpaceDiagram(this, context, runOptions);
         _containerPopupMenu = new PopupMenuFullContainerWithParametersNode(this, context, runOptions);
         _neighborhoodPopupMenu = new PopupMenuFullEntityNode<NeighborhoodBuilder>(this, context, runOptions);
      }

      public override void Edit(MoBiSpatialStructure spatialStructure)
      {
         base.Edit(spatialStructure);

         _view.SetBackColor(spatialStructure.DiagramManager.DiagramOptions.DiagramColors.DiagramBackground);
         if (spatialStructure.TopContainers.Any())
         {
            var firstTopContainer = firstTopContainerWithNode(spatialStructure);
            if (firstTopContainer == null)
               return;

            fixTopContainerNodeLocation(firstTopContainer.Id);
         }

         if (!DiagramModel.IsLayouted)
         {
            var organismTemplateFile = _configuration.SpaceOrganismBaseTemplate;
            if (File.Exists(_configuration.SpaceOrganismUserTemplate))
               organismTemplateFile = _configuration.SpaceOrganismUserTemplate;

            ApplyLayoutFromTemplate(organismTemplateFile);
         }

         Refresh();

         //to avoid scrollbar error
         ResetViewSize();
      }

      private IContainer firstTopContainerWithNode(MoBiSpatialStructure spatialStructure)
      {
         return spatialStructure.TopContainers.FirstOrDefault(topContainer => DiagramModel.GetAllChildren<IBaseNode>().Any(node => string.Equals(node.Id, topContainer.Id)));
      }

      private void fixTopContainerNodeLocation(string topContainerId)
      {
         var node = DiagramModel.GetNode(topContainerId);
         node.Location = new PointF(20, 20);
         node.LocationFixed = true;
      }

      public override void Link(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         if (!(node1 is IContainerNode containerNode1) || !(node2 is IContainerNode containerNode2))
            return;

         var addNeighborhoodTask = _context.Resolve<IInteractionTasksForNeighborhood>();
         AddCommand(addNeighborhoodTask.Add(objectPathFor(containerNode1), objectPathFor(containerNode2)));
         //because cannot undo this action, reset undo stack
         DiagramModel.ClearUndoStack();
      }

      private ObjectPath objectPathFor(IContainerNode containerNode)
      {
         var container = _context.Get<IContainer>(containerNode.Id);
         return container != null ? _context.ObjectPathFactory.CreateAbsoluteObjectPath(container) : DiagramManager.PathForNodeWithoutEntity(containerNode);
      }

      public override void Unlink(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         //because cannot undo this action, reset undo stack
         DiagramModel.ClearUndoStack();
      }
   }
}