using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram;
using OSPSuite.UI.Diagram.Elements;

namespace MoBi.UI.Diagram.DiagramManagers
{
   public class SpatialStructureDiagramManager : BaseDiagramManager<SimpleContainerNode, SimpleNeighborhoodNode, IMoBiSpatialStructure>, ISpatialStructureDiagramManager
   {
      // complement and update ViewModel from PkModel and couple ViewModel and PkModel
      protected override void UpdateDiagramModel(IMoBiSpatialStructure spatialStructure, IDiagramModel diagramModel, bool coupleAll)
      {
         var unusedNodeIds = new HashSet<string>();
         foreach (var baseNode in diagramModel.GetAllChildren<IBaseNode>())
         {
            unusedNodeIds.Add(baseNode.Id);
         }

         if (spatialStructure != null)
         {
            // create neighborhoodsContainerNode, because entities are only added 
            // for available parentContainerNodes
            var neighborhoodsContainerNode = AddAndCoupleNode<IContainer, SimpleContainerNode>(diagramModel, spatialStructure.NeighborhoodsContainer, coupleAll);
            neighborhoodsContainerNode.IsVisible = false;
            neighborhoodsContainerNode.Visible = false; // to avoid visibility in PrintPreview - seems not to be sufficient
            unusedNodeIds.Remove(spatialStructure.NeighborhoodsContainer.Id);


            foreach (var topContainer in spatialStructure.TopContainers)
            {
               if (containerShouldBeDisplayed(topContainer))
               {
                  AddObjectBase(diagramModel, topContainer, recursive: true, coupleAll: coupleAll);
               }

               foreach (var container in topContainer.GetAllContainersAndSelf<IContainer>())
               {
                  unusedNodeIds.Remove(container.Id);
               }
            }

            foreach (var neighborhoodBuilder in spatialStructure.Neighborhoods)
            {
               AddNeighborhood(neighborhoodBuilder);
               unusedNodeIds.Remove(neighborhoodBuilder.Id);
            }
         }

         // remove all unused container and neighborhood nodes
         foreach (var nodeId in unusedNodeIds)
         {
            diagramModel.RemoveNode(nodeId);
         }

         DiagramModel.ClearUndoStack();
      }

      private static bool containerShouldBeDisplayed(IContainer topContainer)
      {
         return topContainer.ContainerType == ContainerType.Organism
                || topContainer.ContainerType == ContainerType.Organ
                || topContainer.ContainerType == ContainerType.Compartment;
      }

      // removes all eventHandler which are references to this presenter
      protected override void DecoupleModel()
      {
         foreach (var topContainer in PkModel.TopContainers)
         {
            DecoupleObjectBase(topContainer, recursive: true);
         }

         foreach (var neighborhoodBuilder in PkModel.Neighborhoods)
         {
            DecoupleObjectBase(neighborhoodBuilder, recursive: true);
         }

         Decouple<IContainer, IContainerNode>(PkModel.NeighborhoodsContainer);
      }

      protected override bool MustHandleNew(IObjectBase obj)
      {
         if (obj == null || PkModel == null)
            return false;

         if (obj.IsAnImplementationOf<INeighborhoodBuilder>() && PkModel.Contains(((IEntity)obj).RootContainer))
            return true;

         if (obj.GetType() == typeof(Container) && PkModel.Contains(((IEntity)obj).RootContainer))
            return true;

         return false;
      }

      public override IDiagramManager<IMoBiSpatialStructure> Create()
      {
         return new SpatialStructureDiagramManager();
      }
   }
}
