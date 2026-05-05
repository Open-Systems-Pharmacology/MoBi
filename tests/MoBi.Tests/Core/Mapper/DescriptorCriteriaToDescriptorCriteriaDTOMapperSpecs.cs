using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mapper
{
   public abstract class concern_for_DescriptorConditionToDescriptorConditionDTOMapper : ContextSpecification<IDescriptorCriteriaToDescriptorCriteriaDTOMapper>
   {
      protected override void Context()
      {
         sut = new DescriptorCriteriaToDescriptorCriteriaDTOMapper(new DescriptorConditionToDescriptorConditionDTOMapper());
      }
   }

   public class When_mapping_a_flat_descriptor_criteria : concern_for_DescriptorConditionToDescriptorConditionDTOMapper
   {
      private DescriptorCriteria _criteria;
      private DescriptorCriteriaDTO _result;

      protected override void Context()
      {
         base.Context();
         _criteria = new DescriptorCriteria { Operator = CriteriaOperator.Or };
         _criteria.Add(new MatchTagCondition("Liver"));
         _criteria.Add(new InContainerCondition("Plasma"));
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_criteria);
      }

      [Observation]
      public void should_carry_the_outer_operator()
      {
         _result.Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }

      [Observation]
      public void should_emit_one_dto_per_condition()
      {
         _result.Conditions.Count.ShouldBeEqualTo(2);
         _result.Conditions[0].TagType.ShouldBeEqualTo(TagType.Match);
         _result.Conditions[0].Tag.ShouldBeEqualTo("Liver");
         _result.Conditions[1].TagType.ShouldBeEqualTo(TagType.InContainer);
         _result.Conditions[1].Tag.ShouldBeEqualTo("Plasma");
      }
   }

   public class When_mapping_a_descriptor_criteria_containing_a_condition_group : concern_for_DescriptorConditionToDescriptorConditionDTOMapper
   {
      private DescriptorCriteria _criteria;
      private ConditionGroup _conditionGroup;
      private DescriptorCriteriaDTO _result;

      protected override void Context()
      {
         base.Context();
         _conditionGroup = new ConditionGroup { Operator = CriteriaOperator.And };
         _conditionGroup.Add(new MatchTagCondition("VenousBlood"));
         _conditionGroup.Add(new MatchTagCondition("Plasma"));

         _criteria = new DescriptorCriteria { Operator = CriteriaOperator.Or };
         _criteria.Add(_conditionGroup);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_criteria);
      }

      [Observation]
      public void should_emit_a_condition_group_dto()
      {
         _result.Conditions.Count.ShouldBeEqualTo(1);
         _result.Conditions[0].ShouldBeAnInstanceOf<ConditionGroupDTO>();
      }

      [Observation]
      public void should_carry_a_reference_to_the_underlying_condition_group()
      {
         var groupDTO = _result.Conditions[0].DowncastTo<ConditionGroupDTO>();
         groupDTO.ConditionGroup.ShouldBeEqualTo(_conditionGroup);
         groupDTO.InnerOperator.ShouldBeEqualTo(CriteriaOperator.And);
      }

      [Observation]
      public void should_recursively_map_the_inner_conditions()
      {
         var groupDTO = _result.Conditions[0].DowncastTo<ConditionGroupDTO>();
         groupDTO.ConditionDTOs.Count.ShouldBeEqualTo(2);
         groupDTO.ConditionDTOs[0].TagType.ShouldBeEqualTo(TagType.Match);
         groupDTO.ConditionDTOs[0].Tag.ShouldBeEqualTo("VenousBlood");
         groupDTO.ConditionDTOs[1].TagType.ShouldBeEqualTo(TagType.Match);
         groupDTO.ConditionDTOs[1].Tag.ShouldBeEqualTo("Plasma");
      }

      [Observation]
      public void group_dto_should_be_read_only()
      {
         _result.Conditions[0].IsReadOnly.ShouldBeTrue();
      }

      [Observation]
      public void group_dto_tag_should_show_the_rendered_expression()
      {
         _result.Conditions[0].Tag.ShouldBeEqualTo("(VenousBlood AND Plasma)");
      }
   }
}
