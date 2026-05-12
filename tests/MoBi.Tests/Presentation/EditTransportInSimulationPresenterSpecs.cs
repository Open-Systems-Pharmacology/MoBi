using FakeItEasy;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditTransportInSimulationPresenter : ContextSpecification<EditTransportInSimulationPresenter>
   {
      private IFormulaPresenterCache _formulaPresenterCache;
      private ITransportToTransportDTOMapper _transportToTransportDTOMapper;
      protected IEditParametersInContainerPresenter _editParametersInContainerPresenter;
      private IEditTransportInSimulationView _view;

      protected override void Context()
      {
         _formulaPresenterCache = A.Fake<IFormulaPresenterCache>();
         _transportToTransportDTOMapper = A.Fake<ITransportToTransportDTOMapper>();
         _editParametersInContainerPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _view = A.Fake<IEditTransportInSimulationView>();

         sut = new EditTransportInSimulationPresenter(_view, _editParametersInContainerPresenter, _transportToTransportDTOMapper, _formulaPresenterCache);
      }
   }

   public class When_enabling_simulation_tracking_in_transport : concern_for_EditTransportInSimulationPresenter
   {
      private TrackableSimulation _trackableSimulation;

      protected override void Context()
      {
         base.Context();
         _trackableSimulation = new TrackableSimulation(null, new SimulationEntitySourceReferenceCache());
      }

      protected override void Because()
      {
         sut.TrackableSimulation = _trackableSimulation;
      }

      [Observation]
      public void the_parameters_presenter_has_tracking_enabled()
      {
         A.CallTo(() => _editParametersInContainerPresenter.EnableSimulationTracking(_trackableSimulation)).MustHaveHappened();
      }
   }
}
