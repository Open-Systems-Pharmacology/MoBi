using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddConditionGroupCommand : ContextSpecification<AddConditionGroupCommand<ObserverBuilder>>
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
         _observerBuilder = new ContainerObserverBuilder { ContainerCriteria = new DescriptorCriteria { Operator = CriteriaOperator.Or } };

         //inverse-command path looks the tagged object up by id; fake the lookup so the round-trip works
         A.CallTo(() => _context.Get<ObserverBuilder>(A<string>._)).Returns(_observerBuilder);

         _conditionGroup = new ConditionGroup { Operator = CriteriaOperator.And };
         _conditionGroup.Add(new MatchTagCondition("VenousBlood"));
         _conditionGroup.Add(new MatchTagCondition("Plasma"));

         //fake the Serialize/Deserialize round-trip used to snapshot the group for the inverse: the
         //bytes are opaque to the test, and Deserialize hands back a structurally-equal clone
         //(via ConditionGroup.Clone) — the inverse Remove falls back to Equals when the live reference
         //is gone, so an equal-but-distinct instance is enough to round-trip the round-trip.
         A.CallTo(() => _context.Serialize(_conditionGroup)).Returns(new byte[] { 1 });
         A.CallTo(() => _context.Deserialize<ConditionGroup>(A<byte[]>._))
            .ReturnsLazily(() => (ConditionGroup) _conditionGroup.Clone());

         _commandParameters = new TagConditionCommandParameters<ObserverBuilder>
         {
            BuildingBlock = _buildingBlock,
            DescriptorCriteriaRetriever = x => x.ContainerCriteria,
            TaggedObject = _observerBuilder
         };

         sut = new AddConditionGroupCommand<ObserverBuilder>(_conditionGroup, _commandParameters);
      }
   }

   public class When_executing_an_add_condition_group_command : concern_for_AddConditionGroupCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_add_the_group_to_the_tagged_objects_criteria()
      {
         _observerBuilder.ContainerCriteria.Count.ShouldBeEqualTo(1);
         _observerBuilder.ContainerCriteria[0].ShouldBeEqualTo(_conditionGroup);
      }
   }

   public class When_executing_an_add_command_against_a_tagged_object_with_no_existing_criteria : concern_for_AddConditionGroupCommand
   {
      protected override void Context()
      {
         base.Context();
         //fall back path: when the retriever yields null, the creator is invoked to bootstrap a fresh criteria
         _observerBuilder.ContainerCriteria = null;
         _commandParameters.DescriptorCriteriaCreator = builder =>
         {
            builder.ContainerCriteria = new DescriptorCriteria();
            return builder.ContainerCriteria;
         };
         sut = new AddConditionGroupCommand<ObserverBuilder>(_conditionGroup, _commandParameters);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_create_the_criteria_and_add_the_group_to_it()
      {
         _observerBuilder.ContainerCriteria.ShouldNotBeNull();
         _observerBuilder.ContainerCriteria.Count.ShouldBeEqualTo(1);
         _observerBuilder.ContainerCriteria[0].ShouldBeEqualTo(_conditionGroup);
      }
   }

   public class When_invoking_the_inverse_of_an_add_condition_group_command : concern_for_AddConditionGroupCommand
   {
      private OSPSuite.Core.Commands.Core.ICommand<IMoBiContext> _inverse;

      protected override void Because()
      {
         _inverse = sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_remove_the_group_again_so_the_criteria_is_back_to_its_original_state()
      {
         _observerBuilder.ContainerCriteria.ShouldBeEmpty();
      }

      [Observation]
      public void should_return_a_remove_condition_group_command_as_the_inverse()
      {
         _inverse.ShouldBeAnInstanceOf<RemoveConditionGroupCommand<ObserverBuilder>>();
      }
   }
}
