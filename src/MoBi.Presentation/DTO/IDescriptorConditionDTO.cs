using MoBi.Core.Services;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.DTO
{
   public interface IDescriptorConditionDTO : IViewItem
   {
      string Tag { get; set; }
      TagType TagType { get; }
      string TagDescription { get; }
   }

   public class DescriptorConditionDTO : IDescriptorConditionDTO
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
   }

   public class ContainerDescriptorRootItem : IRootViewItem<IDescriptorConditionDTO>
   {
   }
}