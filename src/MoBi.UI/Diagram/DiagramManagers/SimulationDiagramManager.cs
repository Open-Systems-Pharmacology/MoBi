using System.Drawing;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Diagram.DiagramManagers
{
   public class SimulationDiagramManager : BaseDiagramManager<MultiPortContainerNode, SimpleNeighborhoodNode, IMoBiSimulation>, ISimulationDiagramManager
   {
      public SimulationDiagramManager()
      {
         RegisterUpdateMethod<Observer>(updateObserver);
         RegisterUpdateMethod<Reaction>(UpdateReaction);
      }

      // complement and update ViewModel from PkModel and couple ViewModel and PkModel
      protected override void UpdateDiagramModel(IMoBiSimulation simulation, IDiagramModel diagramModel, bool coupleAll)
      {
         // create neighborhoodsContainerNode, because entities are only added 
         // for available parentContainerNodes
         IContainerNode neighborhoodsContainerNode = AddAndCoupleNode<IContainer, MultiPortContainerNode>(diagramModel, simulation.Model.Neighborhoods, coupleAll);

         foreach (var baseNode in neighborhoodsContainerNode.GetAllChildren<IBaseNode>())
         {
            baseNode.IsVisible = false;
         }

         neighborhoodsContainerNode.IsVisible = false;

         foreach (var topContainer in simulation.Model.Root.GetChildren<IContainer>())
         {
            if (topContainer.ContainerType == ContainerType.Organism
                || topContainer.ContainerType == ContainerType.Organ
                || topContainer.ContainerType == ContainerType.Compartment)
               AddObjectBase(diagramModel, topContainer, true, coupleAll);
         }

         foreach (var neighborhood in simulation.Model.Neighborhoods.GetAllChildren<Neighborhood>())
         {
            AddNeighborhood(neighborhood);
         }

         DiagramModel.ClearUndoStack();
      }

      // removes all eventHandler (which are references to this presenter)
      protected override void DecoupleModel()
      {
         DecoupleObjectBase(PkModel.Model.Root, recursive: true);

         foreach (var neighborhood in PkModel.Model.Neighborhoods.GetAllChildren<Neighborhood>())
         {
            DecoupleObjectBase(neighborhood, recursive: true);
         }
      }

      protected override bool DecoupleObjectBase(IObjectBase objectBase, bool recursive)
      {
         if (Decouple<MoleculeAmount, MoleculeNode>(objectBase as MoleculeAmount))
            return true;

         if (Decouple<Observer, ObserverNode>(objectBase as Observer))
            return true;

         if (Decouple<Reaction, ReactionNode>(objectBase as Reaction))
            return true;

         if (base.DecoupleObjectBase(objectBase, recursive))
            return true;

         return false; // not removed by this implementation
      }

      public override void RefreshFromDiagramOptions()
      {
         base.RefreshFromDiagramOptions();
         foreach (var observerLink in DiagramModel.GetAllChildren<ObserverLink>())
         {
            observerLink.IsVisible = DiagramOptions.ObserverLinksVisible;
         }

         // do not show unused molecules
         foreach (var moleculeNode in DiagramModel.GetAllChildren<MoleculeNode>())
         {
            if (moleculeNode.GetLinkedNodes<IHasLayoutInfo>().Any())
               moleculeNode.IsVisible = true;
            else
               moleculeNode.IsVisible = DiagramOptions.UnusedMoleculesVisibleInModelDiagram;
         }
      }

      protected override bool MustHandleNew(IObjectBase obj)
      {
         return false;
      }

      protected override IBaseNode AddObjectBase(IContainerBase parent, IObjectBase objectBase, bool recursive, bool coupleAll)
      {
         IElementBaseNode eNode = AddAndCoupleNode<MoleculeAmount, MoleculeNode>(parent, objectBase as MoleculeAmount, coupleAll);
         if (eNode == null)
            eNode = AddAndCoupleNode<Observer, ObserverNode>(parent, objectBase as Observer, coupleAll);

         if (eNode == null)
            eNode = AddAndCoupleNode<Reaction, ReactionNode>(parent, objectBase as Reaction, coupleAll);

         if (eNode != null)
         {
            eNode.CanLink = false;
            return eNode;
         }

         return base.AddObjectBase(parent, objectBase, recursive, coupleAll);
      }

      protected override bool RemoveObjectBase(IObjectBase objectBase, bool recursive)
      {
         if (RemoveAndDecoupleNode<MoleculeAmount, MoleculeNode>(objectBase as MoleculeAmount)) return true;
         if (RemoveAndDecoupleNode<Observer, ObserverNode>(objectBase as Observer)) return true;
         if (RemoveAndDecoupleNode<Reaction, ReactionNode>(objectBase as Reaction)) return true;

         if (base.RemoveObjectBase(objectBase, recursive)) return true;

         return false; // not removed by this implementation
      }

      protected virtual void UpdateReaction(IObjectBase reactionAsEntity, IBaseNode reactionNodeAsBaseNode)
      {
         var reaction = reactionAsEntity.DowncastTo<Reaction>();
         var reactionNode = reactionNodeAsBaseNode.DowncastTo<ReactionNode>();

         reactionNode.ClearLinks();

         var newEductDistance = new PointF(-70, 0);
         var newProductDistance = new PointF(30, 0);
         var newModifierDistance = new PointF(-30, -30);

         foreach (var rp in reaction.Educts)
         {
            createReactionLink(ReactionLinkType.Educt, reactionNode, moleculeNodeFor(rp.Partner.Id, reactionNode, newEductDistance));
         }

         foreach (var rp in reaction.Products)
         {
            createReactionLink(ReactionLinkType.Product, reactionNode, moleculeNodeFor(rp.Partner.Id, reactionNode, newProductDistance));
         }

         foreach (var modifierName in reaction.ModifierNames)
         {
            // modifier are in the same container as reaction (and not in subcontainers)
            var modifier = getModifierNode(reaction, modifierName);
            if (modifier != null)
            {
               createReactionLink(ReactionLinkType.Modifier, reactionNode, moleculeNodeFor(modifier.Id, reactionNode, newModifierDistance));
            }
         }
      }

      private void createReactionLink(ReactionLinkType type, ReactionNode reactionNode, MoleculeNode moleculeNode)
      {
         var reactionLink = new ReactionLink();
         reactionLink.Initialize(type, reactionNode, moleculeNode);
         reactionLink.SetColorFrom(DiagramOptions.DiagramColors);
      }

      private MoleculeNode moleculeNodeFor(string id, ReactionNode reactionNode, PointF distance)
      {
         var location = reactionNode.Location.Plus(distance);
         var moleculeNode = DiagramModel.GetNode<MoleculeNode>(id);

         if (moleculeNode == null)
         {
            moleculeNode = DiagramModel.CreateNode<MoleculeNode>(id, location, DiagramModel);
            moleculeNode.SetColorFrom(DiagramOptions.DiagramColors);
         }
         return moleculeNode;
      }

      private MoleculeAmount getModifierNode(Reaction reaction, string modifierName)
      {
         return reaction.ParentContainer.GetChildren<MoleculeAmount>(mol => mol.Name == modifierName).FirstOrDefault();
      }

      private void updateObserver(IObjectBase observerAsEntity, IBaseNode observerNodeAsBaseNode)
      {
         var observer = observerAsEntity.DowncastTo<Observer>();
         var observerNode = observerNodeAsBaseNode.DowncastTo<ObserverNode>();
         observerNode.ClearLinks();

         foreach (var oRef in observer.Formula.ObjectReferences)
         {
            var refId = oRef.Object.Id;
            var refParentId = oRef.Object.ParentContainer.Id;
            var moleculeNode = DiagramModel.GetNode<MoleculeNode>(refId);
            // if object reference is not molecule amount itself, it could be a child of molecule amount, e.g. a concentration
            if (moleculeNode == null)
               moleculeNode = DiagramModel.GetNode<MoleculeNode>(refParentId);

            if (moleculeNode != null)
               createObserverLink(observerNode, moleculeNode);
         }
      }

      private void createObserverLink(ObserverNode observerNode, MoleculeNode moleculeNode)
      {
         var observerLink = new ObserverLink();
         observerLink.Initialize(observerNode, moleculeNode);
         observerLink.SetColorFrom(DiagramOptions.DiagramColors);
         observerLink.IsVisible = DiagramOptions.ObserverLinksVisible;
      }

      protected override INeighborhoodNode AddNeighborhood(INeighborhoodBase neighborhood)
      {
         var neighborhoodNode = base.AddNeighborhood(neighborhood);
         if (neighborhoodNode == null) return null;

         // add transport nodes 
         // it is required, that the moleculeAmounts already exist, because in transport only source and target amount ids are given,
         // and it cannot be determined, whether a source amount had to be created in first or second container of neighborhood.
         foreach (var transport in neighborhood.GetAllChildren<Transport>())
         {
            var sourceAmountNode = DiagramModel.GetNode<MoleculeNode>(transport.SourceAmount.Id);
            var targetAmountNode = DiagramModel.GetNode<MoleculeNode>(transport.TargetAmount.Id);
            if (sourceAmountNode != null && targetAmountNode != null)
            {
               var sourceContainerNode = sourceAmountNode.GetParent() as IContainerNode;
               var targetContainerNode = targetAmountNode.GetParent() as IContainerNode;
               if (sourceContainerNode != null && targetContainerNode != null)
               {
                  if (neighborhoodNode.FirstNeighbor == sourceContainerNode
                      && neighborhoodNode.SecondNeighbor == targetContainerNode)
                  {
                     createTransportLink(neighborhoodNode.FirstNeighborLink, sourceAmountNode);
                     createTransportLink(neighborhoodNode.SecondNeighborLink, targetAmountNode);
                  }
                  else if (neighborhoodNode.FirstNeighbor == targetContainerNode
                           && neighborhoodNode.SecondNeighbor == sourceContainerNode)
                  {
                     createTransportLink(neighborhoodNode.FirstNeighborLink, targetAmountNode);
                     createTransportLink(neighborhoodNode.SecondNeighborLink, sourceAmountNode);
                  }
               }
            }
         }

         return neighborhoodNode;
      }

      private void createTransportLink(IBaseLink neighborLink, MoleculeNode moleculeNode)
      {
         var transportLink = new TransportLink();
         transportLink.Initialize(neighborLink as NeighborLink, moleculeNode);
         transportLink.SetColorFrom(DiagramOptions.DiagramColors);
      }

      public override IDiagramManager<IMoBiSimulation> Create()
      {
         return new SimulationDiagramManager();
      }
   }
}
