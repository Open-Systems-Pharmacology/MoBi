using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Core.Domain.Model;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForNeighborhoodSpecs : ContextSpecification<IInteractionTasksForNeighborhood>
   {
      private IInteractionTaskContext _interactionContext;
      private IEditTaskFor<NeighborhoodBuilder> _editTask;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         _spatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName("Neighborhoods")
         };
         _interactionContext = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTaskFor<NeighborhoodBuilder>>();
         A.CallTo(() => _interactionContext.Active<MoBiSpatialStructure>()).Returns(_spatialStructure);
         sut = new InteractionTasksForNeighborhood(_interactionContext, _editTask);
      }
   }

   internal class When_creating_command_the_remove_command : concern_for_InteractionTasksForNeighborhoodSpecs
   {
      private NeighborhoodBuilder _neighborhoodBuilder;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _neighborhoodBuilder = new NeighborhoodBuilder().WithName("N1");
         _neighborhoodBuilder.FirstNeighborPath = new ObjectPath("C1");
         _neighborhoodBuilder.SecondNeighborPath = new ObjectPath("C2");
      }

      protected override void Because()
      {
         _command = sut.CreateRemoveCommand(_neighborhoodBuilder, A.Fake<SpatialStructure>());
      }

      [Observation]
      public void should_return_the_correct_command()
      {
         _command.ShouldBeAnInstanceOf<RemoveContainerFromSpatialStructureCommand>();
      }
   }
}