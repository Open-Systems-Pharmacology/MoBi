using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.Views
{
   public interface IEditReactionInSimulationView : IEditProcessInSimulationView<IEditReactionInSimulationPresenter>
   {
      void BindTo(ReactionDTO reactionDTO);
   }
}
