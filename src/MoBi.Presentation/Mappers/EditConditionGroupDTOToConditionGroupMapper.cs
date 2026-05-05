using System;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IEditConditionGroupDTOToConditionGroupMapper : IMapper<EditConditionGroupDTO, ConditionGroup>
   {
   }

   public class EditConditionGroupDTOToConditionGroupMapper : IEditConditionGroupDTOToConditionGroupMapper
   {
      public ConditionGroup MapFrom(EditConditionGroupDTO dto)
      {
         var criteria = new ConditionGroup { Operator = dto.Operator };
         foreach (var operand in dto.Operands)
            criteria.Add(createCondition(operand));
         return criteria;
      }

      private static ITagCondition createCondition(EditConditionGroupOperandDTO operand)
      {
         var tag = operand.Tag ?? string.Empty;
         switch (operand.TagType)
         {
            case TagType.Match:
               return new MatchTagCondition(tag);
            case TagType.NotMatch:
               return new NotMatchTagCondition(tag);
            case TagType.MatchAll:
               return new MatchAllCondition();
            case TagType.InContainer:
               return new InContainerCondition(tag);
            case TagType.NotInContainer:
               return new NotInContainerCondition(tag);
            case TagType.InParent:
               return new InParentCondition();
            case TagType.InChildren:
               return new InChildrenCondition();
            default:
               throw new ArgumentOutOfRangeException(nameof(operand.TagType), operand.TagType, "Operand type not selectable from the condition group editor.");
         }
      }
   }
}
