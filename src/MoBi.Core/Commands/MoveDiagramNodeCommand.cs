using System.Drawing;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Diagram;

namespace MoBi.Core.Commands
{
   public class MoveDiagramNodeCommand : BuildingBlockChangeCommandBase<IMoBiReactionBuildingBlock>
   {
      private IBaseNode _destinationNode;
      private IBaseNode _originalNode;
      private readonly PointF _destinationParentLocation;
      private readonly string _targetNodeName;

      public MoveDiagramNodeCommand(IMoBiReactionBuildingBlock targetBuildingBlock, string targetNodeName, IBaseNode destinationNode)
         : base(targetBuildingBlock)
      {
         _destinationNode = destinationNode;
         _destinationParentLocation = destinationNode.GetParent().Location;
         _targetNodeName = targetNodeName;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new MoveDiagramNodeCommand(_buildingBlock, _targetNodeName, _originalNode).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _destinationNode = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var targetBuildingBlock = context.Get<IMoBiReactionBuildingBlock>(_buildingBlockId);

         var targetNode = targetBuildingBlock.DiagramModel?.FindByName(_targetNodeName);

         if (targetNode == null) return;

         _originalNode = targetNode.Copy();

         targetNode.CopyLayoutInfoFrom(_destinationNode, _destinationParentLocation);
      }
   }
}