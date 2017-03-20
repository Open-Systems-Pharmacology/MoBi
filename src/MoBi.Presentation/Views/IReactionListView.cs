using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IReactionListView : IView<IReactionsListSubPresenter>
   {
      void Show(IEnumerable<ReactionInfoDTO> reactions);
   }
}