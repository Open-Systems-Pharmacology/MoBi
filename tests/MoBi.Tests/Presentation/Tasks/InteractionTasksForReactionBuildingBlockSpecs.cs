using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForReactionBuildingBlock : ContextSpecification<InteractionTasksForReactionBuildingBlock>
   {
      protected IInteractionTaskContext _interactionTaskContext;
      protected IEditTasksForBuildingBlock<MoBiReactionBuildingBlock> _editTasksForBuildingBlock;
      protected IInteractionTasksForBuilder<ReactionBuilder> _interactionTasksForBuilder;
      protected IDiagramTask _diagramTask;
      protected IReactionBuildingBlockFactory _reactionBuildingBlockFactory;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTasksForBuildingBlock = A.Fake<IEditTasksForBuildingBlock<MoBiReactionBuildingBlock>>();
         _interactionTasksForBuilder = A.Fake<IInteractionTasksForBuilder<ReactionBuilder>>();
         _diagramTask = A.Fake<IDiagramTask>();
         _reactionBuildingBlockFactory = A.Fake<IReactionBuildingBlockFactory>();
         
         sut = new InteractionTasksForReactionBuildingBlock(_interactionTaskContext, _editTasksForBuildingBlock, _interactionTasksForBuilder, _reactionBuildingBlockFactory);
      }
   }

   public class When_creating_a_new_building_block : concern_for_InteractionTasksForReactionBuildingBlock
   {
      protected override void Because()
      {
         sut.CreateNewEntity(new Module());
      }

      [Observation]
      public void the_building_block_factory_must_be_used()
      {
         A.CallTo(() => _reactionBuildingBlockFactory.Create()).MustHaveHappened();
      }
   }
}
