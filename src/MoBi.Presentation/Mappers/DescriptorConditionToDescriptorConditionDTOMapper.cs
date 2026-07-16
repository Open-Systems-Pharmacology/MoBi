using System;
using System.Linq;
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
      private readonly ITagTask _tagTask;

      public DescriptorConditionToDescriptorConditionDTOMapper(ITagTask tagTask)
      {
         _tagTask = tagTask;
      }

      public DescriptorConditionDTO MapFrom(ITagCondition descriptorCondition)
      {
         switch (descriptorCondition)
         {
            case ConditionGroup conditionGroup:
               return new ConditionGroupDTO(conditionGroup, conditionGroup.Select(MapFrom).ToList());
            case InContainerCondition inContainerCondition:
               return descriptorConditionDTO(inContainerCondition.Tag, TagType.InContainer);
            case MatchAllCondition _:
               return descriptorConditionDTO(string.Empty, TagType.MatchAll);
            case MatchTagCondition matchTagCondition:
               return descriptorConditionDTO(matchTagCondition.Tag, TagType.Match);
            case NotMatchTagCondition notMatchTagCondition:
               return descriptorConditionDTO(notMatchTagCondition.Tag, TagType.NotMatch);
            case NotInContainerCondition notInContainerCondition:
               return descriptorConditionDTO(notInContainerCondition.Tag, TagType.NotInContainer);
            case InParentCondition _:
               return descriptorConditionDTO(string.Empty, TagType.InParent);
            case InChildrenCondition _:
               return descriptorConditionDTO(string.Empty, TagType.InChildren);
            default:
               throw new ArgumentException($"Cannot create descriptor condition for {descriptorCondition.GetType().Name}");
         }
      }

      private DescriptorConditionDTO descriptorConditionDTO(string tag, TagType tagType) =>
         new DescriptorConditionDTO(tag, tagType, _tagTask.DisplayNameFor(tagType));
   }
}
