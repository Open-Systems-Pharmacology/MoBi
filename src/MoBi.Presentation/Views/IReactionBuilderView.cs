using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IReactionBuilderView<TReactionPartnerBuilder> : IView<IReactionPartnerPresenter<TReactionPartnerBuilder>>
   {
      void BindTo(IReadOnlyList<TReactionPartnerBuilder> partnerBuilders);
   }

   public interface IReactionPartnerView : IReactionBuilderView<ReactionPartnerBuilderDTO>
   {

   }

   public interface IReactionModifierView : IReactionBuilderView<ReactionModifierBuilderDTO>
   {

   }
}