using System;
using MoBi.Assets;
using MoBi.Core.Services;
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
               return new DescriptorConditionDTO(inContainerCondition.Tag, TagType.InContainer, AppConstants.InContainer);
            case MatchAllCondition _:
               return new DescriptorConditionDTO(string.Empty, TagType.MatchAll, AppConstants.MatchAll);
            case MatchTagCondition matchTagCondition:
               return new DescriptorConditionDTO(matchTagCondition.Tag, TagType.Match, AppConstants.Match);
            case NotMatchTagCondition notMatchTagCondition:
               return new DescriptorConditionDTO(notMatchTagCondition.Tag, TagType.NotMatch, AppConstants.NotMatch);
            case NotInContainerCondition notInContainerCondition:
               return new DescriptorConditionDTO(notInContainerCondition.Tag, TagType.NotInContainer, AppConstants.NotInContainer);
            default:
               throw new ArgumentException($"Cannot create descriptor condition for {descriptorCondition.GetType().Name}");
         }
      }
   }
}