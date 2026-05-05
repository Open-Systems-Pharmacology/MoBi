using System;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IDescriptorConditionToDescriptorConditionDTOMapper : IMapper<ITagCondition, DescriptorConditionDTO>
   {
   }

   public class DescriptorConditionToDescriptorConditionDTOMapper : IDescriptorConditionToDescriptorConditionDTOMapper
   {
      public DescriptorConditionDTO MapFrom(ITagCondition descriptorCondition)
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
            case InParentCondition _:
               return new DescriptorConditionDTO(string.Empty, TagType.InParent, AppConstants.InParent);
            case InChildrenCondition _:
               return new DescriptorConditionDTO(string.Empty, TagType.InChildren, AppConstants.InChildren);
            case ConditionGroup conditionGroup:
               return new ConditionGroupDTO(conditionGroup, conditionGroup.Select(MapFrom).ToList());
            default:
               throw new ArgumentException($"Cannot create descriptor condition for {descriptorCondition.GetType().Name}");
         }
      }
   }
}
