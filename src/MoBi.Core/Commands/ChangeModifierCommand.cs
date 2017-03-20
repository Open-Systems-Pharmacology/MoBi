using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ChangeModifierCommand : BuildingBlockChangeCommandBase<IMoBiReactionBuildingBlock>
   {
      private IReactionBuilder _reaction;
      private readonly string _reactionId;
      private readonly string _oldModifier;
      private readonly string _newModifier;

      public ChangeModifierCommand(string newModifier, string oldModifier, IReactionBuilder reaction, IMoBiReactionBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _newModifier = newModifier;
         _oldModifier = oldModifier;
         _reaction = reaction;
         _reactionId = reaction.Id;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.Reaction;
         Description = AppConstants.Commands.EditDescription(ObjectTypes.Reaction, ObjectTypes.Modifier, _oldModifier, _newModifier, _reaction.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeModifierCommand(_oldModifier, _newModifier, _reaction, _buildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _reaction = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _reaction.RemoveModifier(_oldModifier);
         _reaction.AddModifier(_newModifier);
         var reactionDiagramManager = _buildingBlock.DiagramManager.DowncastTo<IMoBiReactionDiagramManager>();

         if (reactionDiagramManager.IsInitialized)
            reactionDiagramManager.RenameMolecule(_reaction, _oldModifier,_newModifier);

         context.PublishEvent(new RemovedReactionModifierEvent(_reaction, _oldModifier));
         context.PublishEvent(new AddedReactionModifierEvent(_reaction, _newModifier));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _reaction = context.Get<IReactionBuilder>(_reactionId);
      }
   }
}