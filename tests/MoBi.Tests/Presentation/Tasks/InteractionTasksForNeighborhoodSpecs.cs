using System.Linq;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForNeighborhoodSpecs : ContextSpecification<IInteractionTasksForNeighborhood>
   {
      private IInteractionTaskContext _interactionContext;
      private IEditTaskFor<INeighborhoodBuilder> _editTask;

      protected override void Context()
      {
         _interactionContext = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTaskFor<INeighborhoodBuilder>>();
         sut = new InteractionTasksForNeighborhood(_interactionContext,_editTask);

      }
   }

   class When_creating_command_the_remove_command : concern_for_InteractionTasksForNeighborhoodSpecs
   {
      private INeighborhoodBuilder _neighborhoodBuilder;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _neighborhoodBuilder =
            new NeighborhoodBuilder().WithName("N1")
               .WithFirstNeighbor(new Container().WithName("C1"))
               .WithSecondNeighbor(new Container().WithName("C2"));
      }

      protected override void Because()
      {
        _command = sut.CreateRemoveCommand(_neighborhoodBuilder,A.Fake<ISpatialStructure>());
      }

      [Observation]
      public void should_return_the_correct_command()
      {
         
         _command.ShouldBeAnInstanceOf<RemoveContainerFromSpatialStructureCommand>();
      }
   }
}	