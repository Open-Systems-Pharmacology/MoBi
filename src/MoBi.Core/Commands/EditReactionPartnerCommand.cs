using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditReactionPartnerEvent
   {
      public IReactionBuilder Reaction { get; set; }
      public IReactionPartnerBuilder ReactionPartner { get; set; }

      public EditReactionPartnerEvent(IReactionBuilder reaction, IReactionPartnerBuilder partner)
      {
         Reaction = reaction;
         ReactionPartner = partner;
      }
   }

   public abstract class EditReactionPartnerCommand : BuildingBlockChangeCommandBase<IMoBiReactionBuildingBlock>
   {
      protected IReactionBuilder _reaction;
      protected IReactionPartnerBuilder _reactionPartner;
      protected readonly string _reactionId;
      protected readonly bool _isEduct;

      protected EditReactionPartnerCommand(IReactionBuilder reaction, IReactionPartnerBuilder reactionPartner, IMoBiReactionBuildingBlock buildingBlock) : base(buildingBlock)
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
         _reaction = context.Get<IReactionBuilder>(_reactionId);
      }

      protected IReactionPartnerBuilder RetrievePartner(string moleculeName)
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