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
   public interface IReactionProductsPresenter : IReactionPartnerPresenter<ReactionPartnerBuilderDTO>
   {
   }

   public class ReactionProductsPresenter : ReactionPartnerPresenter<ReactionPartnerBuilderDTO>, IReactionProductsPresenter
   {
      public ReactionProductsPresenter(IReactionPartnerView view, IMoBiContext context, IViewItemContextMenuFactory viewItemContextMenuFactory,
         IInteractionTasksForReactionBuilder reactionTask) : base(view, context, viewItemContextMenuFactory, reactionTask)
      {
      }

      protected override IReadOnlyList<ReactionPartnerBuilderDTO> PartnerBuilders()
      {
         return _reactionBuilderDTO.Products;
      }

      public override string PartnerType => AppConstants.Captions.Products;

      protected override IEnumerable<string> ExistingPartners()
      {
         return _reactionBuilderDTO.ReactionBuilder.Products.Select(x => x.MoleculeName);
      }

      protected override ICommand<IMoBiContext> AddCommandFor(string moleculeName)
      {
         return new AddReactionPartnerToProductCollection(ReactionBuildingBlock, new ReactionPartnerBuilder(moleculeName, 1.0), _reactionBuilderDTO.ReactionBuilder);
      }

      protected override ICommand<IMoBiContext> RemoveCommandFor(ReactionPartnerBuilderDTO reactionPartnerDTO)
      {
         return new RemoveReactionPartnerFromProductCollection(_reactionBuilderDTO.ReactionBuilder, reactionPartnerDTO.PartnerBuilder, ReactionBuildingBlock);
      }
   }
}