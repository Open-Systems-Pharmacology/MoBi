using System.Drawing;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveReactionBuilderCommand : RemoveObjectBaseCommand<IReactionBuilder, IMoBiReactionBuildingBlock>
   {
      private IBaseNode _originalNode;
      private PointF _originalParentLocation;

      public RemoveReactionBuilderCommand(IMoBiReactionBuildingBlock reactionBuildingBlock, IReactionBuilder reactionBuilder)
         : base(reactionBuildingBlock, reactionBuilder, reactionBuildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddReactionBuilderCommand(_parent, _itemToRemove, _originalNode, _originalParentLocation).AsInverseFor(this);
      }

      protected override void RemoveFrom(IReactionBuilder reactionBuilderToRemove, IMoBiReactionBuildingBlock reactionBuildingBlock, IMoBiContext context)
      {
         reactionBuildingBlock.Remove(reactionBuilderToRemove);
         if (reactionBuildingBlock.DiagramModel == null)
            return;

         _originalNode = reactionBuildingBlock.DiagramModel.FindByName(reactionBuilderToRemove.Name);
         if (_originalNode != null)
         {
            _originalParentLocation = new PointF(_originalNode.GetParent().Location.X, _originalNode.GetParent().Location.Y);
            // take a clone because we want to be able to put the node back to it's original place if this action is reverted
            _originalNode = _originalNode.Copy();
         }
         reactionBuildingBlock.DiagramManager.RemoveObjectBase(reactionBuilderToRemove);
      }
   }

   public class AddReactionBuilderCommand : AddObjectBaseCommand<IReactionBuilder, IMoBiReactionBuildingBlock>
   {
      private readonly IBaseNode _destinationNode;
      private readonly PointF _parentLocation;

      public AddReactionBuilderCommand(IMoBiReactionBuildingBlock reactionBuildingBlock, IReactionBuilder reactionBuilder)
         : this(reactionBuildingBlock, reactionBuilder, null, null)
      {
      }

      public AddReactionBuilderCommand(IMoBiReactionBuildingBlock buildingBlock, IReactionBuilder reactionBuilder, IBaseNode destinationNode, PointF? parentLocation)
         : base(buildingBlock, reactionBuilder, buildingBlock)

      {
         _destinationNode = destinationNode;
         if (parentLocation != null) _parentLocation = parentLocation.Value;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveReactionBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }

      protected override void AddTo(IReactionBuilder reactionBuilder, IMoBiReactionBuildingBlock reactionBuildingBlock, IMoBiContext context)
      {
         reactionBuildingBlock.Add(reactionBuilder);
         reactionBuildingBlock.DiagramManager.AddObjectBase(reactionBuilder);

         if (_destinationNode == null)
            return;

         var targetNode = reactionBuildingBlock.DiagramModel.FindByName(reactionBuilder.Name);

         targetNode?.CopyLayoutInfoFrom(_destinationNode, _parentLocation);
      }
   }
}