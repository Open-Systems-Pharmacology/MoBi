using System.Linq;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Diagram.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility;

namespace MoBi.Presentation.Presenter.ModelDiagram
{
   public interface ISimulationDiagramPresenter : IMoBiBaseDiagramPresenter<IMoBiSimulation>, IPresenter<ISimulationDiagramView>
   {
      bool ObserverLinksVisible { set; }
      void ApplySpaceReactionLayout();
   }

   public class SimulationDiagramPresenter : MoBiBaseDiagramPresenter<ISimulationDiagramView, ISimulationDiagramPresenter, IMoBiSimulation>, ISimulationDiagramPresenter
   {
      private readonly IMoBiConfiguration _configuration;
      private readonly ILayerLayouter _layerLayouter;
      private readonly IDiagramPopupMenuBase _moleculeAmountPopupMenu;

      public SimulationDiagramPresenter(ISimulationDiagramView view,
         IContainerBaseLayouter layouter,
         IDialogCreator dialogCreator,
         IDiagramModelFactory diagramModelFactory,
         IUserSettings userSettings,
         IMoBiContext context,
         IDiagramTask diagramTask,
         IStartOptions runOptions,
         IMoBiConfiguration configuration,
         ILayerLayouter layerLayouter)
         : base(view, layouter, dialogCreator, diagramModelFactory, userSettings, context, diagramTask, runOptions)
      {
         _configuration = configuration;
         _layerLayouter = layerLayouter;
         _diagramPopupMenu = new PopupMenuModelDiagram(this, context, runOptions, dialogCreator);
         _containerPopupMenu = _diagramPopupMenu;
         _moleculeAmountPopupMenu = new DiagramPopupMenuBaseWithContext(this, _context, runOptions);
      }

      public override void Edit(IMoBiSimulation simulation)
      {
         base.Edit(simulation);

         //Fix Location of first TopContainer
         var firstContainerNode = DiagramModel.GetDirectChildren<IContainerNode>().First();
         if (firstContainerNode != null)
            firstContainerNode.LocationFixed = true;

         if (!_model.DiagramModel.IsLayouted)
            ApplySpaceReactionLayout();

         Refresh();
         //to avoid scrollbar error
         ResetViewSize();
      }

      public void ApplySpaceReactionLayout()
      {
         var copier = new LayoutCopyService();

         // if only one organ is visible, in following Copy steps some wrong locations are calculated
         ShowChildren(DiagramModel);

         try
         {
            _view.BeginUpdate();
            var reactionDiagramModel = getReactionBlockDiagramModel();
            if (reactionDiagramModel != null && reactionDiagramModel.IsLayouted)
               copier.Copy(reactionDiagramModel, DiagramModel);

            var spaceDiagramModel = getSpaceBlockDiagramModel();
            if (spaceDiagramModel != null && spaceDiagramModel.IsLayouted)
               copier.Copy(spaceDiagramModel, DiagramModel);

            DiagramManager.RefreshFromDiagramOptions();
            DiagramModel.IsLayouted = true;
         }
         finally
         {
            _view.EndUpdate();
         }

         Refresh();
         //to avoid scrollbar error
         ResetViewSize();
      }

      private IDiagramModel getSpaceBlockDiagramModel()
      {
         var spaceBlockName = DiagramManager.PkModel.BuildConfiguration.SpatialStructure.Name;
         var project = _context.CurrentProject;
         var spatialStructure = project.SpatialStructureCollection.FindByName(spaceBlockName);
         if (spatialStructure == null)
            return null;


         if (spatialStructure.DiagramModel != null)
            return spatialStructure.DiagramModel;

         initializeDiagramManagerFor(spatialStructure);

         var organismTemplateFile = _configuration.SpaceOrganismBaseTemplate;
         if (FileHelper.FileExists(_configuration.SpaceOrganismUserTemplate))
            organismTemplateFile = _configuration.SpaceOrganismUserTemplate;

         foreach (var topContainerNode in spatialStructure.DiagramModel.GetDirectChildren<IContainerNode>())
         {
            var topContainer = _context.Get<IContainer>(topContainerNode.Id);
            if (topContainer != null && topContainer.ContainerType == ContainerType.Organism)
               ApplyLayoutTemplate(topContainerNode, organismTemplateFile, recursive: false);
         }

         spatialStructure.DiagramModel.IsLayouted = true;
         return spatialStructure.DiagramModel;
      }

      private IDiagramModel getReactionBlockDiagramModel()
      {
         var reactionBlockName = DiagramManager.PkModel.BuildConfiguration.Reactions.Name;
         var project = _context.CurrentProject;
         var reactionBlock = project.ReactionBlockCollection.FindByName(reactionBlockName);

         if (reactionBlock == null)
            return null;

         if (reactionBlock.DiagramModel != null)
            return reactionBlock.DiagramModel;

         initializeDiagramManagerFor(reactionBlock);

         _view.DisplayEductsRight(reactionBlock.DiagramModel);

         _layerLayouter.PerformLayout(reactionBlock.DiagramModel, null);

         reactionBlock.DiagramModel.IsLayouted = true;
         return reactionBlock.DiagramModel;
      }

      private void initializeDiagramManagerFor<T>(T reactionBlock) where T : IWithDiagramFor<T>
      {
         reactionBlock.DiagramModel = CreateDiagramModel();
         reactionBlock.InitializeDiagramManager(_userSettings.DiagramOptions);
      }

      public bool ObserverLinksVisible
      {
         set
         {
            _view.ObserverLinksVisible(DiagramModel, value);
            _view.Refresh();
         }
      }

      public override IDiagramPopupMenuBase GetPopupMenu(IBaseNode baseNode)
      {
         if (baseNode == null)
            return _diagramPopupMenu;

         if (_view.IsMoleculeNode(baseNode))
            return _moleculeAmountPopupMenu;

         return base.GetPopupMenu(baseNode);
      }

      public override void Link(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         //do nothing - modelDiagram does not allow structural changes
      }

      protected override void Unlink(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         //do nothing - modelDiagram does not allow structural changes
      }
   }
}