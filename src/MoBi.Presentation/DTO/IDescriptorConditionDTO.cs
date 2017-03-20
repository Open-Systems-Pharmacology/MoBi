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
         get { return string.Empty; }
         set { }
      }

      public TagType TagType => TagType.MatchAll;

      public string TagDescription => AppConstants.MatchAll;
   }

   internal class MatchConnditionDTO : IDescriptorConditionDTO
   {
      public string Tag { get; set; }

      public TagType TagType => TagType.Match;

      public string TagDescription => AppConstants.Match;
   }

   internal class NotMatchConnditionDTO : IDescriptorConditionDTO
   {
      public string Tag { get; set; }

      public TagType TagType => TagType.NotMatch;

      public string TagDescription => AppConstants.NotMatch;
   }

   public class ContainerDescriptorRootItem : IRootViewItem<IDescriptorConditionDTO>
   {
   }
}