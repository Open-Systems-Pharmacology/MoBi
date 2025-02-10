using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeModifierCommand : ContextSpecification<ChangeModifierCommand>
   {
      private MoBiReactionBuildingBlock _reactionBuildingBlock;
      protected ReactionBuilder _reaction;
      private IMoBiReactionDiagramManager _moBiReactionDiagramManager;

      protected override void Context()
      {
         _reactionBuildingBlock = new MoBiReactionBuildingBlock();
         _moBiReactionDiagramManager = A.Fake<IMoBiReactionDiagramManager>();
         _reactionBuildingBlock.DiagramManager = _moBiReactionDiagramManager;
         _reaction = new ReactionBuilder().WithName("R");
         _reaction.AddModifier("old");
         sut = new ChangeModifierCommand("new", "old", _reaction, _reactionBuildingBlock);
      }
   }

   public class When_creating_a_change_modifer_command : concern_for_ChangeModifierCommand
   {
      [Observation]
      public void should_have_a_non_empty_description()
      {
         string.IsNullOrEmpty(sut.Description).ShouldBeFalse();
      }
   }

   public class When_executing_the_change_modifier_command : concern_for_ChangeModifierCommand
   {
      private IMoBiContext _context;
      private string _removed;
      private string _added;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.PublishEvent(A<RemovedReactionModifierEvent>._))
            .Invokes(x => { _removed = x.GetArgument<RemovedReactionModifierEvent>(0).ModifierName; });

         A.CallTo(() => _context.PublishEvent(A<AddedReactionModifierEvent>._))
            .Invokes(x => { _added = x.GetArgument<AddedReactionModifierEvent>(0).ModifierName; });
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_modifer_in_the_reaction()
      {
         _reaction.ModifierNames.ShouldOnlyContain("new");
      }

      [Observation]
      public void should_publish_the_modifer_added_and_removed_event()
      {
         _removed.ShouldBeEqualTo("old");
         _added.ShouldBeEqualTo("new");
      }
   }
}