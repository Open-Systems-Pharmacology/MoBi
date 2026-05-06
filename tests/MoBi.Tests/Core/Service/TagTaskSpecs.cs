using System;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Service
{
   public abstract class concern_for_TagTask : ContextSpecification<ITagTask>
   {
      private IMoBiContext _context;
      protected IBuildingBlock _buildingBlock;
      protected ObserverBuilder _observerBuilder;
      protected TagConditionCommandParameters<ObserverBuilder> _commandParameters;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _observerBuilder = new ContainerObserverBuilder();
         sut = new TagTask(_context);
         _commandParameters = new TagConditionCommandParameters<ObserverBuilder>
         {
            BuildingBlock = _buildingBlock,
            DescriptorCriteriaRetriever = x => x.ContainerCriteria,
            TaggedObject = _observerBuilder
         };
      }
   }

   public class When_adding_a_match_tag_to_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;

      protected override void Because()
      {
         _command = sut.AddTagCondition("toto", TagType.Match, _commandParameters);
      }

      [Observation]
      public void should_add_the_tag_to_the_object_and_return_the_expected_command()
      {
         _command.ShouldBeAnInstanceOf<AddMatchTagConditionCommand<ObserverBuilder>>();
      }
   }

   public class When_adding_a_not_match_tag_to_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;

      protected override void Because()
      {
         _command = sut.AddTagCondition("toto", TagType.NotMatch, _commandParameters);
      }

      [Observation]
      public void should_add_the_tag_to_the_object_and_return_the_expected_command()
      {
         _command.ShouldBeAnInstanceOf<AddNotMatchTagConditionCommand<ObserverBuilder>>();
      }
   }

   public class When_adding_a_match_all_tag_to_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;

      protected override void Because()
      {
         _command = sut.AddTagCondition("toto", TagType.MatchAll, _commandParameters);
      }

      [Observation]
      public void should_add_the_tag_to_the_object_and_return_the_expected_command()
      {
         _command.ShouldBeAnInstanceOf<AddMatchAllConditionCommand<ObserverBuilder>>();
      }
   }

   public class When_adding_a_condition_group_to_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;
      private ConditionGroup _conditionGroup;

      protected override void Context()
      {
         base.Context();
         _conditionGroup = new ConditionGroup { Operator = CriteriaOperator.And };
         _conditionGroup.Add(new MatchTagCondition("A"));
         _conditionGroup.Add(new MatchTagCondition("B"));
      }

      protected override void Because()
      {
         _command = sut.AddConditionGroup(_conditionGroup, _commandParameters);
      }

      [Observation]
      public void should_return_an_add_condition_group_command()
      {
         _command.ShouldBeAnInstanceOf<AddConditionGroupCommand<ObserverBuilder>>();
      }
   }

   public class When_removing_a_condition_group_from_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;
      private ConditionGroup _conditionGroup;

      protected override void Context()
      {
         base.Context();
         _conditionGroup = new ConditionGroup { Operator = CriteriaOperator.And };
         _conditionGroup.Add(new MatchTagCondition("A"));
         _conditionGroup.Add(new MatchTagCondition("B"));
      }

      protected override void Because()
      {
         _command = sut.RemoveConditionGroup(_conditionGroup, _commandParameters);
      }

      [Observation]
      public void should_return_a_remove_condition_group_command()
      {
         _command.ShouldBeAnInstanceOf<RemoveConditionGroupCommand<ObserverBuilder>>();
      }
   }

   public class When_removing_a_not_match_tag_criteria_from_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _observerBuilder.ContainerCriteria = Create.Criteria(x => x.Not("TOTO"));
      }

      protected override void Because()
      {
         _command = sut.RemoveTagCondition("TOTO", TagType.NotMatch, _commandParameters);
      }

      [Observation]
      public void should_remove_the_tag_from_the_object_and_return_the_expected_command()
      {
         _command.ShouldBeAnInstanceOf<RemoveNotMatchTagConditionCommand<ObserverBuilder>>();
      }
   }

   public class When_removing_a_match_tag_criteria_from_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _observerBuilder.ContainerCriteria = Create.Criteria(x => x.With("TOTO"));
      }

      protected override void Because()
      {
         _command = sut.RemoveTagCondition("TOTO", TagType.Match, _commandParameters);
      }

      [Observation]
      public void should_remove_the_tag_from_the_object_and_return_the_expected_command()
      {
         _command.ShouldBeAnInstanceOf<RemoveMatchTagConditionCommand<ObserverBuilder>>();
      }
   }

   public class When_editing_a_tag_criteria_from_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _observerBuilder.ContainerCriteria = Create.Criteria(x => x.With("TOTO"));
      }

      protected override void Because()
      {
         _command = sut.EditTag("TATA", "TOTO", _commandParameters);
      }

      [Observation]
      public void should_remove_the_tag_from_the_object_and_return_the_expected_command()
      {
         _command.ShouldBeAnInstanceOf<EditTagCommand<ObserverBuilder>>();
      }
   }

   public class When_editing_the_operator_from_a_taggable_object : concern_for_TagTask
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _observerBuilder.ContainerCriteria = Create.Criteria(x => x.With("TOTO").With(CriteriaOperator.And));
      }

      protected override void Because()
      {
         _command = sut.EditOperator(CriteriaOperator.Or, _commandParameters);
      }

      [Observation]
      public void should_update_the_criteria_operator()
      {
         _command.ShouldBeAnInstanceOf<EditOperatorCommand<ObserverBuilder>>();
         _observerBuilder.ContainerCriteria.Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }
   }

   public class When_getting_the_display_name_for_each_known_tag_type : concern_for_TagTask
   {
      [Observation]
      public void should_return_the_app_constant_label_for_each_tag_type()
      {
         sut.DisplayNameFor(TagType.Match).ShouldBeEqualTo(AppConstants.Match);
         sut.DisplayNameFor(TagType.NotMatch).ShouldBeEqualTo(AppConstants.NotMatch);
         sut.DisplayNameFor(TagType.MatchAll).ShouldBeEqualTo(AppConstants.MatchAll);
         sut.DisplayNameFor(TagType.InContainer).ShouldBeEqualTo(AppConstants.InContainer);
         sut.DisplayNameFor(TagType.NotInContainer).ShouldBeEqualTo(AppConstants.NotInContainer);
         sut.DisplayNameFor(TagType.InParent).ShouldBeEqualTo(AppConstants.InParent);
         sut.DisplayNameFor(TagType.InChildren).ShouldBeEqualTo(AppConstants.InChildren);
         sut.DisplayNameFor(TagType.ConditionGroup).ShouldBeEqualTo(AppConstants.ConditionGroup);
      }
   }

   public class When_getting_the_display_name_for_an_unknown_tag_type : concern_for_TagTask
   {
      [Observation]
      public void should_throw_an_argument_out_of_range_exception()
      {
         The.Action(() => sut.DisplayNameFor((TagType) 999)).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }

   public class When_creating_a_condition_for_each_supported_tag_type : concern_for_TagTask
   {
      [Observation]
      public void should_return_the_matching_concrete_condition_type()
      {
         sut.CreateCondition("Liver", TagType.Match).ShouldBeAnInstanceOf<MatchTagCondition>();
         sut.CreateCondition("Liver", TagType.NotMatch).ShouldBeAnInstanceOf<NotMatchTagCondition>();
         sut.CreateCondition(null, TagType.MatchAll).ShouldBeAnInstanceOf<MatchAllCondition>();
         sut.CreateCondition("Liver", TagType.InContainer).ShouldBeAnInstanceOf<InContainerCondition>();
         sut.CreateCondition("Liver", TagType.NotInContainer).ShouldBeAnInstanceOf<NotInContainerCondition>();
         sut.CreateCondition(null, TagType.InParent).ShouldBeAnInstanceOf<InParentCondition>();
         sut.CreateCondition(null, TagType.InChildren).ShouldBeAnInstanceOf<InChildrenCondition>();
      }

      [Observation]
      public void should_coerce_a_null_tag_to_empty_for_tag_bearing_types()
      {
         sut.CreateCondition(null, TagType.Match).Tag.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_creating_a_condition_for_an_unsupported_tag_type : concern_for_TagTask
   {
      [Observation]
      public void should_throw_for_condition_group_which_is_not_a_leaf_operand()
      {
         The.Action(() => sut.CreateCondition("Liver", TagType.ConditionGroup)).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }
}
