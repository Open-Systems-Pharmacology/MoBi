using System.Linq;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;
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
      private readonly IDiagramLayoutTask _diagramLayoutTask;
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
         IDiagramLayoutTask diagramLayoutTask)
         : base(view, layouter, dialogCreator, diagramModelFactory, userSettings, context, diagramTask, runOptions)
      {
         _configuration = configuration;
         _diagramLayoutTask = diagramLayoutTask;
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
         // if only one organ is visible, in following Copy steps some wrong locations are calculated
         ShowChildren(DiagramModel);

         try
         {
            _view.BeginUpdate();

            applyOrganismLayout();
            applyReactionLayout();

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

      private void applyReactionLayout()
      {
         _diagramLayoutTask.LayoutReactionDiagram(DiagramModel);
      }

      private void applyOrganismLayout()
      {
         var organismTemplateFile = _configuration.SpaceOrganismBaseTemplate;
         if (FileHelper.FileExists(_configuration.SpaceOrganismUserTemplate))
            organismTemplateFile = _configuration.SpaceOrganismUserTemplate;

         ApplyLayoutFromTemplate(organismTemplateFile);
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

      public override void Unlink(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         //do nothing - modelDiagram does not allow structural changes
      }
   }
}