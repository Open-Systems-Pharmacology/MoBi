using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{

   public interface IParameterValueDistributedPathAndValueEntityPresenter :
      IDistributedPathAndValueEntityPresenter<ParameterValueDTO, ParameterValuesBuildingBlock>, IPresenter<IParameterValueDistributedPathAndValueEntityView>
   {
   }

   public class ParameterValueDistributedPathAndValueEntityPresenter :
      DistributedPathAndValueEntityPresenter<IParameterValueDistributedPathAndValueEntityView, IParameterValueDistributedPathAndValueEntityPresenter, ParameterValueDTO, ParameterValue, ParameterValuesBuildingBlock>,
      IParameterValueDistributedPathAndValueEntityPresenter
   {
      public ParameterValueDistributedPathAndValueEntityPresenter(IParameterValueDistributedPathAndValueEntityView view, IParameterValuesTask parameterValuesTask) : base(view, parameterValuesTask)
      {
      }
   }
}