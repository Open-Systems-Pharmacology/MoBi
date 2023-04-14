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
   public interface IReactionEductPartnerContextMenuSpecificationFactory : IContextMenuSpecificationFactory<IViewItem>
   {
   }

   internal class ReactionEductPartnerContextMenuSpecificationFactory : IReactionEductPartnerContextMenuSpecificationFactory
   {
      private readonly IContainer _container;

      public ReactionEductPartnerContextMenuSpecificationFactory(IContainer container)
      {
         _container = container;
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return (viewItem == null || viewItem.IsAnImplementationOf<ReactionPartnerBuilderDTO>()) &&
                presenter.IsAnImplementationOf<IReactionEductsPresenter>();
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return createFor(viewItem.DowncastTo<ReactionPartnerBuilderDTO>(), presenter.DowncastTo<IReactionEductsPresenter>());
      }

      private IContextMenu createFor(ReactionPartnerBuilderDTO reactionPartnerDTO, IReactionEductsPresenter presenter)
      {
         var reactionBuilder = presenter.Subject.DowncastTo<ReactionBuilder>();

         ReactionPartnerBuilder reactionPartnerBuilder = null;
         if(reactionPartnerDTO != null)
            reactionPartnerBuilder = reactionBuilder.Educts.FirstOrDefault(educt => educt.MoleculeName.Equals(reactionPartnerDTO.MoleculeName));
         return new ContextMenuForEductBuilder(reactionBuilder, reactionPartnerBuilder, _container);
      }
   }
}