using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IDistributedPathAndValueEntityView<TPresenter, TPathAndValueEntity, TPathAndValueDTO> : IView<TPresenter> 
      where TPresenter : IPresenter
      where TPathAndValueDTO : PathAndValueEntityDTO<TPathAndValueEntity, TPathAndValueDTO> where TPathAndValueEntity : PathAndValueEntity
   {
      void BindTo(TPathAndValueDTO parameterDTO);
   }

   public interface IIndividualDistributedPathAndValueEntityView : IDistributedPathAndValueEntityView<IIndividualDistributedPathAndValueEntityPresenter, IndividualParameter, IndividualParameterDTO>
   {

   }

   public interface IParameterValueDistributedPathAndValueEntityView : IDistributedPathAndValueEntityView<IParameterValueDistributedPathAndValueEntityPresenter, ParameterValue, ParameterValueDTO>
   {

   }

   public interface IExpressionDistributedPathAndValueEntityView : IDistributedPathAndValueEntityView<IExpressionDistributedPathAndValueEntityPresenter, ExpressionParameter, ExpressionParameterDTO>
   {

   }

   public interface IInitialConditionsDistributedInExpressionProfileView : IDistributedPathAndValueEntityView<IInitialConditionsDistributedInExpressionProfilePresenter, InitialCondition, InitialConditionDTO>
   {

   }

   public interface IInitialConditionsDistributedPathAndValueEntityView : IDistributedPathAndValueEntityView<IInitialConditionsDistributedPathAndValueEntityPresenter, InitialCondition, InitialConditionDTO>
   {

   }
}
