using System.Drawing;
using System.IO;
using System.Linq;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Presentation.Presenter.SpaceDiagram
{
   public interface ISpatialStructureDiagramPresenter : IMoBiBaseDiagramPresenter<IMoBiSpatialStructure>, IPresenter<ISpatialStructureDiagramView>
   {

   }

   public class SpatialStructureDiagramPresenter : MoBiBaseDiagramPresenter<ISpatialStructureDiagramView, ISpatialStructureDiagramPresenter, IMoBiSpatialStructure>, ISpatialStructureDiagramPresenter
   {
      private readonly IMoBiConfiguration _configuration;

      public SpatialStructureDiagramPresenter(ISpatialStructureDiagramView view, IContainerBaseLayouter layouter, IUserSettings userSettings, 
         IMoBiContext context, IDialogCreator dialogCreator, IMoBiConfiguration configuration, IDiagramTask diagramTask, IStartOptions runOptions, 
         IDiagramModelFactory diagramModelFactory, OSPSuite.Utility.Container.IContainer container)
         : base(view, layouter, dialogCreator,diagramModelFactory, userSettings, context, diagramTask, runOptions, container)
      {
         _configuration = configuration;
         _diagramPopupMenu = new PopupMenuSpaceDiagram(this, runOptions, container);
         _containerPopupMenu = new PopupMenuFullContainerWithParametersNode(this, _context, runOptions, container);
         _neighborhoodPopupMenu = new PopupMenuFullEntityNode<INeighborhoodBuilder>(this, _context, runOptions, container);
      }

      public override void Edit(IMoBiSpatialStructure spatialStructure)
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
            string organismTemplateFile = _configuration.SpaceOrganismBaseTemplate;
            if (File.Exists(_configuration.SpaceOrganismUserTemplate))
               organismTemplateFile = _configuration.SpaceOrganismUserTemplate;

            foreach (var topContainerNode in DiagramModel.GetDirectChildren<IContainerNode>())
            {
               var topContainer = _context.Get<IContainer>(topContainerNode.Id);
               if (topContainer != null && topContainer.ContainerType == ContainerType.Organism)
                  ApplyLayoutTemplate(topContainerNode, organismTemplateFile, false);
            }
         }
         Refresh();

         //to avoid scrollbar error
         ResetViewSize();
      }

      private IContainer firstTopContainerWithNode(IMoBiSpatialStructure spatialStructure)
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
         if (!node1.IsAnImplementationOf<IContainerNode>() || !node2.IsAnImplementationOf<IContainerNode>()) return;

         var container1 = _context.Get<IContainer>(node1.Id);
         var container2 = _context.Get<IContainer>(node2.Id);

         var addNeighborhoodTask = IoC.Resolve<IInteractionTasksForNeighborhood>();
         AddCommand(addNeighborhoodTask.Add(container1, container2));
         //because cannot undo this action, reset undo stack
         DiagramModel.ClearUndoStack(); 
      }

      protected override void Unlink(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         //because cannot undo this action, reset undo stack
         DiagramModel.ClearUndoStack();
      }

   }
}