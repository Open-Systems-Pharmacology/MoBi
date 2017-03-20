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
   public interface IReactionModifiersPresenter : IReactionPartnerPresenter<ReactionModifierBuilderDTO>
   {
   }

   public class ReactionModifiersPresenter : ReactionPartnerPresenter<ReactionModifierBuilderDTO>, IReactionModifiersPresenter
   {
      private List<ReactionModifierBuilderDTO> _modifierDTOs;

      public ReactionModifiersPresenter(IReactionModifierView view, IMoBiContext context, IViewItemContextMenuFactory viewItemContextMenuFactory, 
         IInteractionTasksForReactionBuilder reactionBuilderTask) : base(view, context, viewItemContextMenuFactory, reactionBuilderTask)
      {

      }

      public override void Edit(ReactionBuilderDTO reactionBuilderDTO, IBuildingBlock buildingBlock)
      {
         _modifierDTOs = new List<ReactionModifierBuilderDTO>();
         _modifierDTOs.AddRange(reactionBuilderDTO.ReactionBuilder.ModifierNames.Select(x => new ReactionModifierBuilderDTO(x)));
         base.Edit(reactionBuilderDTO, buildingBlock);
      }

      protected override IReadOnlyList<ReactionModifierBuilderDTO> PartnerBuilders()
      {
         return _modifierDTOs;
      }

      public override string PartnerType => AppConstants.Captions.Modifiers;

      protected override IEnumerable<string> ExistingPartners()
      {
         return PartnerBuilders().Select(x => x.ModiferName);
      }

      protected override ICommand<IMoBiContext> AddCommandFor(string moleculeName)
      {
         return new AddItemToModifierCollectionCommand(ReactionBuildingBlock, moleculeName, _reactionBuilderDTO.ReactionBuilder);
      }

      protected override ICommand<IMoBiContext> RemoveCommandFor(ReactionModifierBuilderDTO reactionDTO)
      {
         return new RemoveItemFromModifierCollectionCommand(_reactionBuilderDTO.ReactionBuilder, reactionDTO.ModiferName, ReactionBuildingBlock);
      }
   }
}