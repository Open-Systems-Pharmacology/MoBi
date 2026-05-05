using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveConditionGroupCommand : ContextSpecification<RemoveConditionGroupCommand<ObserverBuilder>>
   {
      protected IMoBiContext _context;
      protected IBuildingBlock _buildingBlock;
      protected ObserverBuilder _observerBuilder;
      protected ConditionGroup _conditionGroup;
      protected TagConditionCommandParameters<ObserverBuilder> _commandParameters;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         //the base BuildingBlockChangeCommandBase.ExecuteWith resolves a version updater; fake it to a no-op
         A.CallTo(() => _context.Resolve<IBuildingBlockVersionUpdater>()).Returns(A.Fake<IBuildingBlockVersionUpdater>());

         _buildingBlock = A.Fake<IBuildingBlock>();

         _conditionGroup = new ConditionGroup { Operator = CriteriaOperator.And };
         _conditionGroup.Add(new MatchTagCondition("VenousBlood"));
         _conditionGroup.Add(new MatchTagCondition("Plasma"));

         //the criteria already contains the group we're about to remove
         _observerBuilder = new ContainerObserverBuilder
         {
            ContainerCriteria = new DescriptorCriteria { Operator = CriteriaOperator.Or }
         };
         _observerBuilder.ContainerCriteria.Add(_conditionGroup);

         //inverse-command path looks the tagged object up by id; fake the lookup so the round-trip works
         A.CallTo(() => _context.Get<ObserverBuilder>(A<string>._)).Returns(_observerBuilder);

         //fake the Serialize/Deserialize round-trip used to snapshot the group for the inverse:
         //Deserialize hands back a structurally-equal clone so the inverse Add inserts an equivalent
         //group back into the criteria.
         A.CallTo(() => _context.Serialize(_conditionGroup)).Returns(new byte[] { 1 });
         A.CallTo(() => _context.Deserialize<ConditionGroup>(A<byte[]>._))
            .ReturnsLazily(() => (ConditionGroup)_conditionGroup.Clone());

         _commandParameters = new TagConditionCommandParameters<ObserverBuilder>
         {
            BuildingBlock = _buildingBlock,
            DescriptorCriteriaRetriever = x => x.ContainerCriteria,
            TaggedObject = _observerBuilder
         };

         sut = new RemoveConditionGroupCommand<ObserverBuilder>(_conditionGroup, _commandParameters);
      }
   }

   public class When_executing_a_remove_condition_group_command : concern_for_RemoveConditionGroupCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_drop_the_group_from_the_tagged_objects_criteria()
      {
         _observerBuilder.ContainerCriteria.ShouldBeEmpty();
      }
   }

   public class When_executing_a_remove_command_for_a_group_that_is_no_longer_in_the_criteria_by_reference : concern_for_RemoveConditionGroupCommand
   {
      private ConditionGroup _equivalentReplacement;

      protected override void Context()
      {
         base.Context();
         //structural equality fallback: replace the live group with a brand-new instance whose contents
         //match — the command should still find and remove it via Equals()
         _observerBuilder.ContainerCriteria.Clear();
         _equivalentReplacement = new ConditionGroup { Operator = CriteriaOperator.And };
         _equivalentReplacement.Add(new MatchTagCondition("VenousBlood"));
         _equivalentReplacement.Add(new MatchTagCondition("Plasma"));
         _observerBuilder.ContainerCriteria.Add(_equivalentReplacement);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_still_remove_the_structurally_equal_group()
      {
         _observerBuilder.ContainerCriteria.ShouldBeEmpty();
      }
   }

   public class When_executing_a_remove_command_against_a_tagged_object_with_no_descriptor_criteria : concern_for_RemoveConditionGroupCommand
   {
      protected override void Context()
      {
         base.Context();
         //retriever returns null — e.g. the criteria collection was never created on this tagged object
         _commandParameters.DescriptorCriteriaRetriever = x => null;
         sut = new RemoveConditionGroupCommand<ObserverBuilder>(_conditionGroup, _commandParameters);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_treat_the_removal_as_a_no_op_and_not_publish_a_remove_event()
      {
         A.CallTo(() => _context.PublishEvent(A<RemoveTagConditionEvent>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_leave_the_command_description_unset_so_the_history_does_not_show_a_removal()
      {
         string.IsNullOrEmpty(sut.Description).ShouldBeTrue();
      }
   }

   public class When_executing_a_remove_command_for_a_group_that_is_not_present_in_the_criteria : concern_for_RemoveConditionGroupCommand
   {
      protected override void Context()
      {
         base.Context();
         //the criteria exists but the target group isn't in it (and isn't structurally equal to anything in it)
         _observerBuilder.ContainerCriteria.Clear();
         _observerBuilder.ContainerCriteria.Add(new MatchTagCondition("Unrelated"));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_not_publish_a_remove_event_when_nothing_was_removed()
      {
         A.CallTo(() => _context.PublishEvent(A<RemoveTagConditionEvent>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_leave_the_existing_criteria_untouched()
      {
         _observerBuilder.ContainerCriteria.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_leave_the_command_description_unset_so_the_history_does_not_show_a_removal()
      {
         string.IsNullOrEmpty(sut.Description).ShouldBeTrue();
      }
   }

   public class When_invoking_the_inverse_of_a_remove_condition_group_command : concern_for_RemoveConditionGroupCommand
   {
      private ICommand<IMoBiContext> _inverse;

      protected override void Because()
      {
         _inverse = sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_put_the_group_back_so_the_criteria_is_restored_to_its_original_state()
      {
         _observerBuilder.ContainerCriteria.Count.ShouldBeEqualTo(1);
         _observerBuilder.ContainerCriteria[0].ShouldBeEqualTo(_conditionGroup);
      }

      [Observation]
      public void should_return_an_add_condition_group_command_as_the_inverse()
      {
         _inverse.ShouldBeAnInstanceOf<AddConditionGroupCommand<ObserverBuilder>>();
      }
   }
}