using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public interface IEditTransportInSimulationPresenter : IEditInSimulationPresenter, IEditPresenterWithParameters<Transport>
   {
   }

   public class EditTransportInSimulationPresenter
      : EditProcessInSimulationPresenter<IEditTransportInSimulationView, IEditTransportInSimulationPresenter, Transport>,
        IEditTransportInSimulationPresenter
   {
      private readonly ITransportToTransportDTOMapper _transportToTransportDTOMapper;

      public EditTransportInSimulationPresenter(IEditTransportInSimulationView view,
         IEditParametersInContainerPresenter editParametersInContainerPresenter,
         ITransportToTransportDTOMapper transportToTransportDTOMapper,
         IFormulaPresenterCache formulaPresenterCache)
         : base(view, editParametersInContainerPresenter, formulaPresenterCache)
      {
         _transportToTransportDTOMapper = transportToTransportDTOMapper;
      }

      protected override void BindProcessToView(Transport process)
      {
         _view.BindTo(_transportToTransportDTOMapper.MapFrom(process));
      }
   }
}
