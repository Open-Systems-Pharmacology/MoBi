using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IReactionEductsPresenter : IReactionPartnerPresenter<ReactionPartnerBuilderDTO>
   {
   }

   public class ReactionEductsPresenter : ReactionPartnerPresenter<ReactionPartnerBuilderDTO>, IReactionEductsPresenter
   {
      public ReactionEductsPresenter(IReactionPartnerView view, IMoBiContext context, IViewItemContextMenuFactory viewItemContextMenuFactory,
         IInteractionTasksForReactionBuilder reactionTask) : base(view, context, viewItemContextMenuFactory, reactionTask)
      {
      }

      protected override IReadOnlyList<ReactionPartnerBuilderDTO> PartnerBuilders()
      {
         return _reactionBuilderDTO.Educts;
      }

      public override string PartnerType => AppConstants.Captions.Educts;

      protected override IEnumerable<string> ExistingPartners()
      {
         return _reactionBuilderDTO.ReactionBuilder.Educts.Select(x => x.MoleculeName);
      }

      protected override ICommand<IMoBiContext> AddCommandFor(string moleculeName)
      {
         return new AddReactionPartnerToEductCollection(ReactionBuildingBlock, new ReactionPartnerBuilder(moleculeName, 1.0), _reactionBuilderDTO.ReactionBuilder);
      }

      protected override ICommand<IMoBiContext> RemoveCommandFor(ReactionPartnerBuilderDTO reactionPartnerDTO)
      {
         return new RemoveReactionPartnerFromEductCollection(_reactionBuilderDTO.ReactionBuilder, reactionPartnerDTO.PartnerBuilder, ReactionBuildingBlock);
      }
   }
}