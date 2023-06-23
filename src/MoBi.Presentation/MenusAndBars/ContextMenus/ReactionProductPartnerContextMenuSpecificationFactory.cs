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
   public interface IReactionProductContextMenuSpecificationFactory : IContextMenuSpecificationFactory<IViewItem>
   {
   }

   internal class ReactionProductPartnerContextMenuSpecificationFactory : IReactionProductContextMenuSpecificationFactory
   {
      private readonly IContainer _container;

      public ReactionProductPartnerContextMenuSpecificationFactory(IContainer container)
      {
         _container = container;
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return (viewItem == null || viewItem.IsAnImplementationOf<ReactionPartnerBuilderDTO>()) &&
                presenter.IsAnImplementationOf<IReactionProductsPresenter>();
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return createFor(viewItem.DowncastTo<ReactionPartnerBuilderDTO>(), presenter.DowncastTo<IReactionProductsPresenter>());
      }

      private IContextMenu createFor(ReactionPartnerBuilderDTO reactionPartnerDTO, IReactionProductsPresenter presenter)
      {
         var reactionBuilder = presenter.Subject.DowncastTo<ReactionBuilder>();

         ReactionPartnerBuilder reactionPartnerBuilder = null;
         if (reactionPartnerDTO != null)
            reactionPartnerBuilder = reactionBuilder.Products.FirstOrDefault(product => product.MoleculeName.Equals(reactionPartnerDTO.MoleculeName));
         return new ContextMenuForProductBuilder(reactionBuilder, reactionPartnerBuilder, _container);
      }
   }
}