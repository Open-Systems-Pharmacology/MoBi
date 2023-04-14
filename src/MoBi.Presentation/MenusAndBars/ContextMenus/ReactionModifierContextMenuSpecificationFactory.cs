using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IReactionModifierContextMenuSpecificationFactory : IContextMenuSpecificationFactory<IViewItem>
   {
   }

   public class ReactionModifierContextMenuSpecificationFactory : IReactionModifierContextMenuSpecificationFactory
   {
      private readonly IContainer _container;

      public ReactionModifierContextMenuSpecificationFactory(IContainer container)
      {
         _container = container;
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return (viewItem == null || viewItem.IsAnImplementationOf<ReactionModifierBuilderDTO>()) &&
                presenter.IsAnImplementationOf<IReactionModifiersPresenter>();
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return createFor(viewItem.DowncastTo<ReactionModifierBuilderDTO>(), presenter.DowncastTo<IReactionModifiersPresenter>());
      }

      private IContextMenu createFor(ReactionModifierBuilderDTO reactionPartnerDTO, IReactionModifiersPresenter presenter)
      {
         var reactionBuilder = presenter.Subject.DowncastTo<ReactionBuilder>();

         var reactionModifier = string.Empty;
         if (reactionPartnerDTO != null)
         {
            reactionModifier = reactionBuilder.ModifierNames.FirstOrDefault(product => Equals(reactionPartnerDTO.ModiferName, product));
         }
         return new ContextMenuForModifierBuilder(reactionBuilder, reactionModifier, _container);
      }

   }
}