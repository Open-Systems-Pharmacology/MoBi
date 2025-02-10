using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IIndividualDistributedPathAndValueEntityPresenter :
      IDistributedPathAndValueEntityPresenter<IndividualParameterDTO, IndividualBuildingBlock>, IPresenter<IIndividualDistributedPathAndValueEntityView>
   {

   }

   public class IndividualDistributedPathAndValueEntityPresenter : 
      DistributedPathAndValueEntityPresenter<IIndividualDistributedPathAndValueEntityView, IIndividualDistributedPathAndValueEntityPresenter, IndividualParameterDTO, IndividualParameter, IndividualBuildingBlock>, 
      IIndividualDistributedPathAndValueEntityPresenter
   {
      public IndividualDistributedPathAndValueEntityPresenter(IIndividualDistributedPathAndValueEntityView view, IInteractionTasksForIndividualBuildingBlock interactionTasks) : base(view, interactionTasks)
      {
      }
   }
}