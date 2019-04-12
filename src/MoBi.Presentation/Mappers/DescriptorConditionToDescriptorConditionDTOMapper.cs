using System;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IDescriptorConditionToDescriptorConditionDTOMapper : IMapper<IDescriptorCondition, IDescriptorConditionDTO>
   {
   }

   internal class DescriptorConditionToDescriptorConditionDTOMapper : IDescriptorConditionToDescriptorConditionDTOMapper
   {
      public IDescriptorConditionDTO MapFrom(IDescriptorCondition descriptorCondition)
      {
         switch (descriptorCondition)
         {
            case InContainerCondition inContainerCondition:
               return new InContainerConditionDTO {Tag = inContainerCondition.Tag};
            case MatchAllCondition _:
               return new MatchAllConditionDTO();
            case MatchTagCondition matchTagCondition:
               return new MatchConditionDTO {Tag = matchTagCondition.Tag};
            case NotMatchTagCondition notMatchTagCondition:
               return new NotMatchConditionDTO {Tag = notMatchTagCondition.Tag};
            default:
               throw new ArgumentException($"Cannot create descriptor condition for {descriptorCondition.GetType().Name}");
         }
      }
   }
}