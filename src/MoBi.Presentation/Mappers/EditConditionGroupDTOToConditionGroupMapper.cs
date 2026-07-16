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
      private readonly ITagTask _tagTask;

      public EditConditionGroupDTOToConditionGroupMapper(ITagTask tagTask)
      {
         _tagTask = tagTask;
      }

      public ConditionGroup MapFrom(EditConditionGroupDTO dto)
      {
         var criteria = new ConditionGroup { Operator = dto.Operator };
         foreach (var operand in dto.Operands)
            criteria.Add(_tagTask.CreateCondition(operand.Tag, operand.TagType));
         return criteria;
      }
   }
}
