using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Exceptions;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.UICommands;
using Northwoods.Go;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.Utility.Extensions;
using ToolTips = MoBi.Assets.ToolTips;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.UI.Presenters
{
   public class ReactionDiagramPresenter : MoBiBaseDiagramPresenter<IReactionDiagramView, IReactionDiagramPresenter, MoBiReactionBuildingBlock>, IReactionDiagramPresenter
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IDiagramPopupMenuBase _moleculePopupMenu;
      private readonly IDiagramPopupMenuBase _reactionPopupMenu;
      private readonly IDiagramLayoutTask _diagramLayoutTask;
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public ReactionDiagramPresenter(
         IReactionDiagramView view, 
         IContainerBaseLayouter layouter, 
         IMoBiContext context, 
         IUserSettings userSettings, 
         IDialogCreator dialogCreator, 
         IMoBiApplicationController applicationController, 
         IDiagramTask diagramTask, 
         IDiagramLayoutTask diagramLayoutTask,
         IStartOptions runOptions, 
         IDiagramModelFactory diagramModelFactory,
         IBuildingBlockRepository buildingBlockRepository) :
         base(view, layouter, dialogCreator, diagramModelFactory, userSettings, context, diagramTask, runOptions)
      {
         _applicationController = applicationController;
         _diagramPopupMenu = new PopupMenuReactionDiagram(this, context, runOptions);
         _moleculePopupMenu = _diagramPopupMenu;
         _reactionPopupMenu = new PopupMenuReactionBuilder(this, context, runOptions);
         _diagramLayoutTask = diagramLayoutTask;
         _buildingBlockRepository = buildingBlockRepository;
      }

      public bool DisplayEductsRight(IBaseNode node)
      {
         if (!IsReactionNode(node))
            return false;

         return (node as ReactionNode).DisplayEductsRight;
      }

      public bool IsReactionNode(IBaseNode baseNode)
      {
         var reactionNode = baseNode as ReactionNode;
         return reactionNode != null;
      }

      public override void Edit(MoBiReactionBuildingBlock reactionBuildingBlock)
      {
         base.Edit(reactionBuildingBlock);
         _view.GridVisible = DiagramManager.DiagramOptions.SnapGridVisible;
         if (reactionBuildingBlock.DiagramModel.IsLayouted)
            return;

         _diagramLayoutTask.LayoutReactionDiagram(DiagramModel);

         Refresh();

         //to avoid scrollbar error
         ResetViewSize();
      }

      public void LayerLayout()
      {
         _diagramLayoutTask.LayoutReactionDiagram(DiagramModel);
      }

      public override IDiagramPopupMenuBase GetPopupMenu(IBaseNode baseNode)
      {
         if (baseNode == null) return _diagramPopupMenu;
         if (baseNode.IsAnImplementationOf(typeof(ReactionNode))) return _reactionPopupMenu;
         if (baseNode.IsAnImplementationOf(typeof(MoleculeNode))) return _moleculePopupMenu;
         return base.GetPopupMenu(baseNode);
      }

      public void AddMoleculeNode()
      {
         using (var presenter = _applicationController.Start<IMultipleStringSelectionPresenter>())
         {
            var moleculeNames = presenter.Show(AppConstants.Captions.AddReactionMolecule,
               AppConstants.Dialog.GetReactionMoleculeName, getMoleculeNames(), ToolTips.AddMoleculeNameToList, canAdd: true);

            var command = new MoBiMacroCommand
            {
               ObjectType = ObjectTypes.Molecule,
               CommandType = AppConstants.Commands.AddCommand,
               Description = AppConstants.Commands.AddManyMoleculesDescription
            };
            moleculeNames.Each(nodeName => addMoleculeNode(nodeName, command));
            AddCommand(command.RunCommand(_context));
         }
      }

      private IEnumerable<string> getMoleculeNames()
      {
         var moleculeBB = _buildingBlockRepository.MoleculeBlockCollection;
         return moleculeBB.SelectMany(bb => bb.Select(molecule => molecule.Name)).Distinct().OrderBy(moleculeName => moleculeName);
      }

      private void addMoleculeNode(string moleculeName, MoBiMacroCommand command)
      {
         if (moleculeName.Equals(string.Empty)) return;

         var moleculeNodes = reactionDiagramManager.GetMoleculeNodes(moleculeName);

         var message = AppConstants.Diagram.MoleculeNodeAlreadyExistsForMolecule(moleculeName);
         if (moleculeNodes.Any() && (_dialogCreator.MessageBoxYesNo(message) == ViewResult.No))
            return;

         command.Add(new AddMoleculeToReactionBuildingBlockCommand(_model, moleculeName));
      }

      public void RemoveMoleculeNode(MoleculeNode moleculeNode)
      {
         if (anyLinkedNodes(moleculeNode))
         {
            promptForRemoveLinks();
            return;
         }

         AddCommand(new RemoveMoleculeFromReactionBuildingBlockCommand(_model, moleculeNode.Id).RunCommand(_context));
      }

      private void promptForRemoveLinks()
      {
         _dialogCreator.MessageBoxInfo(AppConstants.Captions.DeleteLinksFromMoleculesFirst);
      }

      private IEnumerable<ReactionNode> getLinkedReactionsForMoleculeNode(MoleculeNode moleculeNode)
      {
         return moleculeNode.GetLinkedNodes<ReactionNode>();
      }

      public void RemoveSelection(IReadOnlyList<GoObject> objectsToBeRemoved)
      {
         if (allMoleculeNodesUnlinked(objectsToBeRemoved))
            // remove reactions first, then molecules
            objectsToBeRemoved.OrderBy(x => x.IsAnImplementationOf<ReactionNode>() ? 0 : 1).Each(removeItem);
         else
            promptForRemoveLinks();
      }

      public void Select(ReactionBuilder reactionBuilder)
      {
         var reactionNode = reactionDiagramManager?.ReactionNodeFor(reactionBuilder);
         if (reactionNode == null) return;
         _view.ClearSelection();
         _view.Select(reactionNode);
      }

      private bool allMoleculeNodesUnlinked(IReadOnlyList<GoObject> goObjects)
      {
         return !goObjects.OfType<MoleculeNode>().Any(moleculeNode =>
            anyLinkedNodes(moleculeNode) && !moleculeNodeWillBeUnlinkedAfterDelete(moleculeNode, goObjects));
      }

      private bool moleculeNodeWillBeUnlinkedAfterDelete(MoleculeNode moleculeNode, IEnumerable<GoObject> itemsBeingDeleted)
      {
         var linkedReactions = getLinkedReactionsForMoleculeNode(moleculeNode);
         return linkedReactions.All(itemsBeingDeleted.Contains);
      }

      private void removeItem(GoObject itemToDelete)
      {
         if (itemToDelete.IsAnImplementationOf<MoleculeNode>())
            RemoveMoleculeNode((MoleculeNode) itemToDelete);

         else if (itemToDelete.IsAnImplementationOf<ReactionNode>())
            RemoveReactionNode((ReactionNode) itemToDelete);
      }

      private bool anyLinkedNodes(MoleculeNode moleculeNode)
      {
         var reactionNodes = getLinkedReactionsForMoleculeNode(moleculeNode);
         return reactionNodes.Any();
      }

      public void LayerLayout(IContainerBase containerBase)
      {
         _diagramLayoutTask.LayoutReactionDiagram(containerBase);
      }

      public void RemoveReactionNode(ReactionNode reactionNode)
      {
         var removeCommand = _context.Resolve<RemoveCommandFor<MoBiReactionBuildingBlock, ReactionBuilder>>();
         removeCommand.Parent = _model;
         removeCommand.Child = _context.Get<ReactionBuilder>(reactionNode.Id);
         removeCommand.Execute();
      }

      public override void Link(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         getLinkProperties(node1, node2, portObject1, portObject2, out var reactionNode, out var moleculeNode, out var reactionLinkType);
         var reactionBuilder = _context.Get<ReactionBuilder>(reactionNode.Id);

         var addLinkCommand = new AddNamedPartnerUICommand(_context, _model, reactionBuilder, moleculeNode.Name, reactionLinkType);
         addLinkCommand.Execute();

         reactionDiagramManager.UpdateReactionBuilder(reactionBuilder, reactionNode);

         //because cannot undo this action, reset undo stack
         DiagramModel.ClearUndoStack();
      }

      protected override void Unlink(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         ReactionNode reactionNode;
         MoleculeNode moleculeNode;
         ReactionLinkType reactionLinkType;

         getLinkProperties(node1, node2, portObject1, portObject2, out reactionNode, out moleculeNode, out reactionLinkType);

         var reactionBuilder = _context.Get<ReactionBuilder>(reactionNode.Id);

         var removeLinkCommand = new RemoveNamedPartnerUICommand(_context, _model, reactionBuilder, moleculeNode.Name, reactionLinkType);
         removeLinkCommand.Execute();

         reactionDiagramManager.UpdateReactionBuilder(reactionBuilder, reactionNode);
      }

      private void getLinkProperties(
         IBaseNode node1, IBaseNode node2, object portObject1, object portObject2,
         out ReactionNode reactionNode, out MoleculeNode moleculeNode, out ReactionLinkType reactionLinkType)
      {
         var rNode1 = node1 as ReactionNode;
         var rNode2 = node2 as ReactionNode;

         if (rNode1 == null && rNode2 == null)
            throw new MoBiException(AppConstants.Exceptions.ReactionNodeMissingInLink);

         var mNode1 = node1 as MoleculeNode;
         var mNode2 = node2 as MoleculeNode;

         if (mNode1 == null && mNode2 == null)
            throw new MoBiException(AppConstants.Exceptions.MoleculeNodeMissingInLink);

         if (rNode1 != null)
         {
            reactionNode = rNode1;
            moleculeNode = mNode2;
            reactionLinkType = (ReactionLinkType) portObject1;
         }
         else
         {
            reactionNode = rNode2;
            moleculeNode = mNode1;
            reactionLinkType = (ReactionLinkType) portObject2;
         }
      }

      private IMoBiReactionDiagramManager reactionDiagramManager => DiagramManager.DowncastTo<IMoBiReactionDiagramManager>();

      public void SetDisplayEductsRightForDiagramSelection(bool displayEductsRight)
      {
         foreach (var node in _view.GetSelectedNodes<ReactionNode>())
         {
            node.DisplayEductsRight = displayEductsRight;
         }
      }
   }
}