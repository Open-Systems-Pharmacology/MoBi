using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditReactionPartnerEvent
   {
      public ReactionBuilder Reaction { get; set; }
      public ReactionPartnerBuilder ReactionPartner { get; set; }

      public EditReactionPartnerEvent(ReactionBuilder reaction, ReactionPartnerBuilder partner)
      {
         Reaction = reaction;
         ReactionPartner = partner;
      }
   }

   public abstract class EditReactionPartnerCommand : BuildingBlockChangeCommandBase<MoBiReactionBuildingBlock>
   {
      protected ReactionBuilder _reaction;
      protected ReactionPartnerBuilder _reactionPartner;
      protected readonly string _reactionId;
      protected readonly bool _isEduct;

      protected EditReactionPartnerCommand(ReactionBuilder reaction, ReactionPartnerBuilder reactionPartner, MoBiReactionBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _reaction = reaction;
         _reactionId = reaction.Id;
         _reactionPartner = reactionPartner;
         _isEduct = reaction.Educts.Contains(reactionPartner);
         ObjectType = ObjectTypes.Reaction;
         CommandType = AppConstants.Commands.EditCommand;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _reaction = context.Get<ReactionBuilder>(_reactionId);
      }

      protected ReactionPartnerBuilder RetrievePartner(string moleculeName)
      {
         return _isEduct ? _reaction.EductBy(moleculeName) : _reaction.ProductBy(moleculeName);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _reaction = null;
         _reactionPartner = null;
      }
   }
}