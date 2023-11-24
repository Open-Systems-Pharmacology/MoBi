using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IExpressionDistributedPathAndValueEntityPresenter :
      IDistributedPathAndValueEntityPresenter<ExpressionParameterDTO, ExpressionProfileBuildingBlock>, IPresenter<IExpressionDistributedPathAndValueEntityView>
   {
   }

   public class ExpressionDistributedPathAndValueEntityPresenter :
      DistributedPathAndValueEntityPresenter<IExpressionDistributedPathAndValueEntityView, IExpressionDistributedPathAndValueEntityPresenter, ExpressionParameterDTO, ExpressionParameter, ExpressionProfileBuildingBlock>,
      IExpressionDistributedPathAndValueEntityPresenter
   {
      public ExpressionDistributedPathAndValueEntityPresenter(IExpressionDistributedPathAndValueEntityView view, 
         IInteractionTasksForExpressionProfileBuildingBlock interactionTasksForExpressionProfile) : 
         base(view, interactionTasksForExpressionProfile)
      {
      }
   }
}