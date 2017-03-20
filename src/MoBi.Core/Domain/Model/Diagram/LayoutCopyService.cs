using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Diagram;

namespace MoBi.Core.Domain.Model.Diagram
{
   public class LayoutCopyService
   {
      public static string ContainerNamePlaceHolder
      {
         get { return "XXX"; }
      }

      public void CopyRecursive(IContainerBase source, IContainerBase target)
      {
         foreach (var targetChild in target.GetDirectChildren<IContainerNode>())
            CopyRecursive(source, targetChild);

         Copy(source, target);
      }

      public void Copy(IDiagramModel sourceDiagramModel, IDiagramModel targetDiagramModel)
      {
         if (sourceDiagramModel == null || targetDiagramModel == null) 
            return;

         try
         {
            targetDiagramModel.BeginUpdate();

            Copy(sourceDiagramModel.DowncastTo<IContainerBase>(), targetDiagramModel.DowncastTo<IContainerBase>());
            targetDiagramModel.IsLayouted = sourceDiagramModel.IsLayouted;
         }
         finally { targetDiagramModel.EndUpdate(); }
      }

      public void Copy(IContainerBase sourceTopContainer, IContainerBase targetTopContainer)
      {
         if (sourceTopContainer == null || targetTopContainer == null)
            return;

         bool expanded = true;
         PointF location = targetTopContainer.Location;
         int topLevelsToSkip = 0;

         var targetContainerNode = targetTopContainer as IContainerNode;
         if (targetContainerNode != null)
         {
            expanded = targetContainerNode.IsExpanded;
            location = targetContainerNode.Location;
            targetContainerNode.IsExpanded = true;
            topLevelsToSkip = 1;
         }

         foreach (var targetNode in targetTopContainer.GetDirectChildren<IContainerNode>())
         {
            copyContainerRecursive(sourceTopContainer, targetTopContainer, targetNode, topLevelsToSkip);

            var sourceNode = getNodeByPath(sourceTopContainer, targetTopContainer, targetNode, topLevelsToSkip);
            if (sourceNode == null) continue;

            targetNode.CopyLayoutInfoFrom(sourceNode, location);
         }

         foreach (var targetNode in targetTopContainer.GetDirectChildren<IElementBaseNode>())
         {
            var sourceNode = getNodeByPath(sourceTopContainer, targetTopContainer, targetNode, topLevelsToSkip);
            if (sourceNode == null) continue;

            targetNode.CopyLayoutInfoFrom(sourceNode, location);
         }

         if (targetContainerNode != null)
         {
            targetContainerNode.IsExpanded = expanded;
            targetContainerNode.Location = location;

            var sourceContainerNode = sourceTopContainer as IContainerNode;
            if (sourceContainerNode != null)
               targetContainerNode.IsExpandedByDefault = sourceContainerNode.IsExpandedByDefault;
         }
      }

      private void copyContainerRecursive(IContainerBase sourceTopContainer, IContainerBase targetTopContainer,
                                          IContainerNode targetContainerNode, int topLevelsToSkip)
      {
         bool expanded = targetContainerNode.IsExpanded;
         PointF location = targetContainerNode.Location;
         targetContainerNode.IsExpanded = true;

         foreach (var targetNode in targetContainerNode.GetDirectChildren<IContainerNode>())
         {
            copyContainerRecursive(sourceTopContainer, targetTopContainer, targetNode, topLevelsToSkip);

            var sourceNode = getNodeByPath(sourceTopContainer, targetTopContainer, targetNode, topLevelsToSkip);

            if (sourceNode == null)
            {
               targetNode.Collapse(0);
               continue;
            }

            targetNode.CopyLayoutInfoFrom(sourceNode, location);
         }

         foreach (var targetNode in targetContainerNode.GetDirectChildren<IElementBaseNode>())
         {
            var sourceNode = getNodeByPath(sourceTopContainer, targetTopContainer, targetNode, topLevelsToSkip);
            if (sourceNode == null) continue;
            targetNode.CopyLayoutInfoFrom(sourceNode, location);
         }

         // reset Expansion state and Location of container itself
         targetContainerNode.IsExpanded = expanded;
         targetContainerNode.Location = location;
      }

      /// <summary>
      /// finds node in container with same name, type and parent containers as other node
      /// container can be a ContainerBaseNode (with name) or BaseDiagram (without name)
      /// </summary>
      private IBaseNode getNodeByPath(IContainerBase container, IContainerBase otherContainer, IBaseNode otherNode, int topLevelsToSkip)
      {
         string otherContainerName = otherContainer.IsAnImplementationOf<IContainerNode>() ? ((IContainerNode) otherContainer).Name : "";
         bool nameFound;
         IContainerBase parent = container;
         // fill otherParentNodes with Parent nodes of otherNode
         IList<IContainerNode> otherParentNodes = new List<IContainerNode>();

         var parentNode = otherNode.GetParent() as IContainerNode;
         // find corresponding parent by path in container
         if (parentNode != null) 
         {
            otherParentNodes.Insert(0, parentNode);
            while (parentNode != null && parentNode != otherContainer)
            {
               parentNode = parentNode.GetParent() as IContainerNode;
               if (parentNode != null) otherParentNodes.Insert(0, parentNode);
            }

            // skip/neglect topLevels
            for (int i = 0; i < topLevelsToSkip && otherParentNodes.Count > 0; i++)
               otherParentNodes.RemoveAt(0);

            // navigate down in container using the names of otherParentNodes
            parent = container;
            nameFound = true;
            foreach (var otherParentNode in otherParentNodes)
            {
               nameFound = false;
               // find container with same name as otherParentNode
               foreach (var containerNode in parent.GetDirectChildren<IContainerNode>())
                  if (compareNames(containerNode.Name, otherParentNode.Name, otherContainerName))
                  {
                     parent = containerNode;
                     nameFound = true;
                     break;
                  }
               if (!nameFound) break;
            }
         }
         
         // find node in corresponding container with same name and type as otherNode
         nameFound = false;
         IBaseNode node = null;
         foreach (var baseNode in parent.GetDirectChildren<IBaseNode>())
            if (compareNames(baseNode.Name, otherNode.Name, otherContainerName))
            {
               node = baseNode;
               nameFound = true;
               break;
            }
         if (!nameFound) return null;
         if (!compareTypes(node, otherNode)) return null;

         return node;
      }


      /// <summary>
      /// in template the base container name is replaced by "XXX", especially in the neighborhood node names,
      /// therefore here the comparison is done with "XXX" replaced by the actual base container name in case of occurence.
      /// </summary>
      private bool compareNames(string nodeXXXname, string nodeName, string XXXname)
      {
         if (!nodeXXXname.Contains(ContainerNamePlaceHolder)) return nodeXXXname == nodeName;
         return nodeXXXname.Replace(ContainerNamePlaceHolder, XXXname) == nodeName;
      }

      /// <summary>
      /// true, if node1 is derived from node2 or vice versa; else false
      /// </summary>
      private bool compareTypes(IBaseNode node1, IBaseNode node2)
      {
         Type type1 = node1.GetType();
         Type type2 = node2.GetType();

         if (type1 == type2) return true;
         if (type1.IsAssignableFrom(type2) || type2.IsAssignableFrom(type1)) return true;
         return false;
      }
   }
}