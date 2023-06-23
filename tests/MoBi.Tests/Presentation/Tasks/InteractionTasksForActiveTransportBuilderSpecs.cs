using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForActiveTransportBuilder : ContextSpecification<IInteractionTasksForActiveTransportBuilder>
   {
      protected IEditTaskFor<TransportBuilder> _editTask;
      protected IInteractionTaskContext _interactionTaskContext;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTaskFor<TransportBuilder>>();
         sut = new InteractionTasksForActiveTransportBuilder(_interactionTaskContext, _editTask);
      }
   }

   public class When_creating_a_new_active_transport_builder : concern_for_InteractionTasksForActiveTransportBuilder
   {
      private TransportBuilder _result;
      private TransporterMoleculeContainer _transporterMoleculeContainer;
      private IDimension _amountPerTimeDimension;

      protected override void Context()
      {
         base.Context();
         _amountPerTimeDimension = A.Fake<IDimension>();
         _transporterMoleculeContainer = new TransporterMoleculeContainer();
         A.CallTo(() => _interactionTaskContext.DimensionByName(Constants.Dimension.AMOUNT_PER_TIME)).Returns(_amountPerTimeDimension);
      }

      protected override void Because()
      {
         _result = sut.CreateNewEntity(_transporterMoleculeContainer);
      }

      [Observation]
      public void should_return_a_transport_builder_with_the_dimension_set_to_amount_per_time()
      {
         _result.Dimension.ShouldBeEqualTo(_amountPerTimeDimension);
      }
   }
}