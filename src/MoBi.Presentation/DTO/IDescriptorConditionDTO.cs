using MoBi.Assets;
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

   internal class MatchAllConditionDTO : IDescriptorConditionDTO
   {
      public string Tag
      {
         get => string.Empty;
         set { }
      }

      public TagType TagType => TagType.MatchAll;

      public string TagDescription => AppConstants.MatchAll;
   }

   internal class MatchConditionDTO : IDescriptorConditionDTO
   {
      public string Tag { get; set; }

      public TagType TagType => TagType.Match;

      public string TagDescription => AppConstants.Match;
   }

   internal class NotMatchConditionDTO : IDescriptorConditionDTO
   {
      public string Tag { get; set; }

      public TagType TagType => TagType.NotMatch;

      public string TagDescription => AppConstants.NotMatch;
   }

   internal class InContainerConditionDTO : IDescriptorConditionDTO
   {
      public string Tag { get; set; }

      public TagType TagType => TagType.InContainer;

      public string TagDescription => AppConstants.InContainer;
   }

   public class ContainerDescriptorRootItem : IRootViewItem<IDescriptorConditionDTO>
   {
   }
}