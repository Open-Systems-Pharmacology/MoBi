using System.Linq;
using FakeItEasy;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_CreateConditionGroupPresenter : ContextSpecification<CreateConditionGroupPresenter>
   {
      protected ICreateConditionGroupView _view;
      protected ITagVisitor _tagVisitor;
      protected IEditConditionGroupDTOToConditionGroupMapper _criteriaMapper;
      protected EditConditionGroupDTO _capturedDTO;

      protected override void Context()
      {
         _view = A.Fake<ICreateConditionGroupView>();
         _tagVisitor = A.Fake<ITagVisitor>();
         A.CallTo(() => _tagVisitor.AllTags()).Returns(new[] { "Liver", "Plasma", "VenousBlood" });
         //real mapper: it has no external dependencies and exercises the actual DTO -> criteria mapping
         _criteriaMapper = new EditConditionGroupDTOToConditionGroupMapper();
         //capture the DTO the presenter binds to so each spec can inspect/mutate it as the user would
         A.CallTo(() => _view.BindTo(A<EditConditionGroupDTO>._))
            .Invokes((EditConditionGroupDTO dto) => _capturedDTO = dto);
         sut = new CreateConditionGroupPresenter(_view, _tagVisitor, _criteriaMapper);
      }
   }

   public class When_opening_the_create_condition_group_modal : concern_for_CreateConditionGroupPresenter
   {
      protected override void Because()
      {
         sut.CreateConditionGroup();
      }

      [Observation]
      public void should_default_to_two_empty_match_rows()
      {
         _capturedDTO.Operands.Count.ShouldBeEqualTo(2);
         _capturedDTO.Operands.Each(operand =>
         {
            operand.TagType.ShouldBeEqualTo(TagType.Match);
            operand.Tag.ShouldBeEqualTo(string.Empty);
         });
      }

      [Observation]
      public void should_default_the_operator_to_and()
      {
         _capturedDTO.Operator.ShouldBeEqualTo(CriteriaOperator.And);
      }

      [Observation]
      public void should_expose_the_projects_tags_as_autocomplete_suggestions()
      {
         _capturedDTO.AvailableTags.ShouldOnlyContain("Liver", "Plasma", "VenousBlood");
      }
   }

   public class When_canceling_the_create_condition_group_modal : concern_for_CreateConditionGroupPresenter
   {
      private DescriptorCriteria _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.CreateConditionGroup();
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_confirming_the_create_condition_group_modal_after_editing_operands : concern_for_CreateConditionGroupPresenter
   {
      private DescriptorCriteria _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         //simulate the user editing the captured DTO between Display() and the Canceled check
         A.CallTo(() => _view.Display()).Invokes(() =>
         {
            _capturedDTO.Operator = CriteriaOperator.Or;
            _capturedDTO.Operands.Clear();
            _capturedDTO.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.Match, Tag = "Liver" });
            _capturedDTO.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.InParent });
         });
      }

      protected override void Because()
      {
         _result = sut.CreateConditionGroup();
      }

      [Observation]
      public void should_build_inner_criteria_with_the_chosen_operator()
      {
         _result.Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }

      [Observation]
      public void should_build_one_inner_condition_per_operand_row()
      {
         _result.Count.ShouldBeEqualTo(2);
         _result[0].ShouldBeAnInstanceOf<MatchTagCondition>();
         _result[0].Tag.ShouldBeEqualTo("Liver");
         _result[1].ShouldBeAnInstanceOf<InParentCondition>();
      }
   }

   public class When_adding_an_operand_via_the_presenter : concern_for_CreateConditionGroupPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Display()).Invokes(() => sut.AddOperand());
      }

      protected override void Because()
      {
         sut.CreateConditionGroup();
      }

      [Observation]
      public void should_append_a_default_match_operand_to_the_dto()
      {
         _capturedDTO.Operands.Count.ShouldBeEqualTo(3);
         _capturedDTO.Operands.Last().TagType.ShouldBeEqualTo(TagType.Match);
         _capturedDTO.Operands.Last().Tag.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_removing_an_operand_via_the_presenter : concern_for_CreateConditionGroupPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Display()).Invokes(() => sut.RemoveOperand(_capturedDTO.Operands[0]));
      }

      protected override void Because()
      {
         sut.CreateConditionGroup();
      }

      [Observation]
      public void should_drop_the_operand_from_the_dto()
      {
         _capturedDTO.Operands.Count.ShouldBeEqualTo(1);
      }
   }
}
