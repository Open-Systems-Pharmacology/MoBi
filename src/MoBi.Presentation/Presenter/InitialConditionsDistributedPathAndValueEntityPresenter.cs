using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IInitialConditionsDistributedPathAndValueEntityPresenter :
      IDistributedPathAndValueEntityPresenter<InitialConditionDTO, InitialConditionsBuildingBlock>, IPresenter<IInitialConditionsDistributedPathAndValueEntityView>
   {
   }

   public class InitialConditionsDistributedPathAndValueEntityPresenter :
      DistributedPathAndValueEntityPresenter<IInitialConditionsDistributedPathAndValueEntityView, IInitialConditionsDistributedPathAndValueEntityPresenter, InitialConditionDTO, InitialCondition, InitialConditionsBuildingBlock>,
      IInitialConditionsDistributedPathAndValueEntityPresenter
   {
      public InitialConditionsDistributedPathAndValueEntityPresenter(IInitialConditionsDistributedPathAndValueEntityView view, IInitialConditionsTask<InitialConditionsBuildingBlock> interactionTasks) : base(view, interactionTasks)
      {
      }
   }

   public interface IInitialConditionsDistributedInExpressionProfilePresenter :
      IDistributedPathAndValueEntityPresenter<InitialConditionDTO, ExpressionProfileBuildingBlock>, IPresenter<IInitialConditionsDistributedInExpressionProfileView>
   {
   }

   public class InitialConditionsDistributedInExpressionProfilePresenter :
      DistributedPathAndValueEntityPresenter<IInitialConditionsDistributedInExpressionProfileView, IInitialConditionsDistributedInExpressionProfilePresenter, InitialConditionDTO, InitialCondition, ExpressionProfileBuildingBlock>,
      IInitialConditionsDistributedInExpressionProfilePresenter
   {
      public InitialConditionsDistributedInExpressionProfilePresenter(IInitialConditionsDistributedInExpressionProfileView view, IInitialConditionsTask<ExpressionProfileBuildingBlock> interactionTasks) : base(view, interactionTasks)
      {
      }
   }
}