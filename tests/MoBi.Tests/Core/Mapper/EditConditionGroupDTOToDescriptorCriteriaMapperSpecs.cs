using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mapper
{
   public abstract class concern_for_EditConditionGroupDTOToDescriptorCriteriaMapper : ContextSpecification<IEditConditionGroupDTOToConditionGroupMapper>
   {
      protected override void Context()
      {
         sut = new EditConditionGroupDTOToConditionGroupMapper();
      }

      //the DTO ctor seeds two default rows for the modal's initial state; clear them so each spec
      //starts from an empty operand list and tests only the rows it explicitly sets up.
      protected static EditConditionGroupDTO emptyDTO(CriteriaOperator @operator = CriteriaOperator.And)
      {
         var dto = new EditConditionGroupDTO(new List<string>()) { Operator = @operator };
         dto.Operands.Clear();
         return dto;
      }
   }

   public class When_mapping_an_empty_dto : concern_for_EditConditionGroupDTOToDescriptorCriteriaMapper
   {
      private EditConditionGroupDTO _dto;
      private DescriptorCriteria _result;

      protected override void Context()
      {
         base.Context();
         _dto = emptyDTO(CriteriaOperator.Or);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dto);
      }

      [Observation]
      public void should_carry_the_operator()
      {
         _result.Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }

      [Observation]
      public void should_be_empty()
      {
         _result.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_mapping_a_dto_with_each_supported_operand_type : concern_for_EditConditionGroupDTOToDescriptorCriteriaMapper
   {
      private EditConditionGroupDTO _dto;
      private DescriptorCriteria _result;

      protected override void Context()
      {
         base.Context();
         _dto = emptyDTO();
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.Match, Tag = "Liver" });
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.NotMatch, Tag = "Liver" });
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.MatchAll });
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.InContainer, Tag = "Liver" });
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.NotInContainer, Tag = "Liver" });
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.InParent });
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.InChildren });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dto);
      }

      [Observation]
      public void should_emit_one_condition_per_operand_row_with_the_correct_runtime_type()
      {
         _result.Count.ShouldBeEqualTo(7);
         _result[0].ShouldBeAnInstanceOf<MatchTagCondition>();
         _result[1].ShouldBeAnInstanceOf<NotMatchTagCondition>();
         _result[2].ShouldBeAnInstanceOf<MatchAllCondition>();
         _result[3].ShouldBeAnInstanceOf<InContainerCondition>();
         _result[4].ShouldBeAnInstanceOf<NotInContainerCondition>();
         _result[5].ShouldBeAnInstanceOf<InParentCondition>();
         _result[6].ShouldBeAnInstanceOf<InChildrenCondition>();
      }

      [Observation]
      public void should_pass_the_tag_through_for_tag_bearing_types()
      {
         _result.OfType<MatchTagCondition>().Single().Tag.ShouldBeEqualTo("Liver");
         _result.OfType<NotMatchTagCondition>().Single().Tag.ShouldBeEqualTo("Liver");
         _result.OfType<InContainerCondition>().Single().Tag.ShouldBeEqualTo("Liver");
         _result.OfType<NotInContainerCondition>().Single().Tag.ShouldBeEqualTo("Liver");
      }
   }

   public class When_mapping_a_match_dto_with_a_null_tag : concern_for_EditConditionGroupDTOToDescriptorCriteriaMapper
   {
      private EditConditionGroupDTO _dto;
      private DescriptorCriteria _result;

      protected override void Context()
      {
         base.Context();
         _dto = emptyDTO();
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.Match, Tag = null });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dto);
      }

      [Observation]
      public void should_coerce_the_null_tag_to_empty()
      {
         _result[0].Tag.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_mapping_a_dto_with_an_unsupported_operand_type : concern_for_EditConditionGroupDTOToDescriptorCriteriaMapper
   {
      private EditConditionGroupDTO _dto;

      protected override void Context()
      {
         base.Context();
         //ConditionGroup is not selectable as an operand from the modal — surface that clearly if it sneaks in
         _dto = emptyDTO();
         _dto.Operands.Add(new EditConditionGroupOperandDTO { TagType = TagType.ConditionGroup });
      }

      [Observation]
      public void should_throw()
      {
         The.Action(() => sut.MapFrom(_dto)).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }

   public class When_constructing_a_dto_with_default_state : concern_for_EditConditionGroupDTOToDescriptorCriteriaMapper
   {
      private EditConditionGroupDTO _dto;

      protected override void Context()
      {
         base.Context();
         _dto = new EditConditionGroupDTO(new[] { "Liver", "Plasma" });
      }

      [Observation]
      public void should_seed_two_default_match_rows()
      {
         _dto.Operands.Count.ShouldBeEqualTo(2);
         _dto.Operands.Each(o =>
         {
            o.TagType.ShouldBeEqualTo(TagType.Match);
            o.Tag.ShouldBeEqualTo(string.Empty);
         });
      }

      [Observation]
      public void should_default_the_operator_to_and()
      {
         _dto.Operator.ShouldBeEqualTo(CriteriaOperator.And);
      }

      [Observation]
      public void should_expose_the_supplied_available_tags()
      {
         _dto.AvailableTags.ShouldOnlyContain("Liver", "Plasma");
      }
   }
}
