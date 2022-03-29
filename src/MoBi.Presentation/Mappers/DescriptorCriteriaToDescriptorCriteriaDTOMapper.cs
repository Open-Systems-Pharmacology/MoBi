using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IDescriptorCriteriaToDescriptorCriteriaDTOMapper : IMapper<DescriptorCriteria, DescriptorCriteriaDTO>
   {
   }

   public class DescriptorCriteriaToDescriptorCriteriaDTOMapper : IDescriptorCriteriaToDescriptorCriteriaDTOMapper
   {
      private readonly IDescriptorConditionToDescriptorConditionDTOMapper _descriptorConditionMapper;

      public DescriptorCriteriaToDescriptorCriteriaDTOMapper(IDescriptorConditionToDescriptorConditionDTOMapper descriptorConditionMapper)
      {
         _descriptorConditionMapper = descriptorConditionMapper;
      }

      public DescriptorCriteriaDTO MapFrom(DescriptorCriteria descriptorCriteria)
      {
         var dto = new DescriptorCriteriaDTO();
         if (descriptorCriteria == null)
            return dto;

         dto.Operator = descriptorCriteria.Operator;
         dto.Conditions = descriptorCriteria.MapAllUsing(_descriptorConditionMapper);

         return dto;
      }
   }
}