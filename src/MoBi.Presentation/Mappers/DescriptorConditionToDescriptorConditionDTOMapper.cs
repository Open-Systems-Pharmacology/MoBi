using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Presentation.Mappers
{
   public interface IDescriptorConditionToDescriptorConditionDTOMapper : IMapper<IDescriptorCondition, IDescriptorConditionDTO>
   {
   }

   internal class DescriptorConditionToDescriptorConditionDTOMapper : IDescriptorConditionToDescriptorConditionDTOMapper
   {
      public IDescriptorConditionDTO MapFrom(IDescriptorCondition descriptorCondition)
      {
         IDescriptorConditionDTO dto = null;
         if (descriptorCondition.IsAnImplementationOf<MatchTagCondition>())
         {
            dto = new MatchConnditionDTO {Tag = ((MatchTagCondition) descriptorCondition).Tag};
         }
         else
         {
            if (descriptorCondition.IsAnImplementationOf<NotMatchTagCondition>())
            {
               dto = new NotMatchConnditionDTO {Tag = ((NotMatchTagCondition) descriptorCondition).Tag};
            }
            else
            {
               dto = new MatchAllConditionDTO();
            }
         }
         return dto;
      }
   }
}