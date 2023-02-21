using System.Collections.Generic;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.DTO
{
   public class DescriptorCriteriaDTO
   {
      public CriteriaOperator Operator { get; set; } 
      public IReadOnlyList<DescriptorConditionDTO> Conditions { get; set; } = new List<DescriptorConditionDTO>();
   }

   public class DescriptorConditionDTO : IViewItem
   {
      public TagType TagType { get; }
      public string Tag { get; set; }
      public string TagDescription { get; }

      public DescriptorConditionDTO(string tag, TagType tagType, string tagDescription)
      {
         TagType = tagType;
         Tag = tag;
         TagDescription = tagDescription;
      }

      public bool IsReadOnly => TagType.IsOneOf(TagType.MatchAll, TagType.InParent);
   }

   public class ContainerDescriptorRootItem : IRootViewItem<DescriptorConditionDTO>
   {
   }
}