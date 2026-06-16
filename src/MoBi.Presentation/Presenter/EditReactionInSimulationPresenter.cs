using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public interface IEditReactionInSimulationPresenter : IEditInSimulationPresenter, IEditPresenterWithParameters<Reaction>
   {
   }

   public class EditReactionInSimulationPresenter
      : EditProcessInSimulationPresenter<IEditReactionInSimulationView, IEditReactionInSimulationPresenter, Reaction>,
        IEditReactionInSimulationPresenter
   {
      private readonly IReactionToReactionDTOMapper _reactionToReactionDTOMapper;

      public EditReactionInSimulationPresenter(IEditReactionInSimulationView view,
         IEditParametersInContainerPresenter editParametersInContainerPresenter,
         IReactionToReactionDTOMapper reactionToReactionDTOMapper,
         IFormulaPresenterCache formulaPresenterCache)
         : base(view, editParametersInContainerPresenter, formulaPresenterCache)
      {
         _reactionToReactionDTOMapper = reactionToReactionDTOMapper;
      }

      protected override void BindProcessToView(Reaction process)
      {
         _view.BindTo(_reactionToReactionDTOMapper.MapFrom(process));
      }
   }
}
