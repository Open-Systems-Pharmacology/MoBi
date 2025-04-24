using FakeItEasy;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditReactionInSimulationPresenter : ContextSpecification<EditReactionInSimulationPresenter>
   {
      private IFormulaPresenterCache _formulaPresenterCache;
      private IReactionToReactionDTOMapper _reactionToReactionDTOMapper;
      protected IEditParametersInContainerPresenter _editParametersInContainerPresenter;
      private IEditReactionInSimulationView _view;

      protected override void Context()
      {
         _formulaPresenterCache = A.Fake<IFormulaPresenterCache>();
         _reactionToReactionDTOMapper = A.Fake<IReactionToReactionDTOMapper>();
         _editParametersInContainerPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _view = A.Fake<IEditReactionInSimulationView>();

         sut = new EditReactionInSimulationPresenter(_view, _editParametersInContainerPresenter, _reactionToReactionDTOMapper, _formulaPresenterCache);
      }
   }

   public class When_enabling_simulation_tracking_in_reaction : concern_for_EditReactionInSimulationPresenter
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