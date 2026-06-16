using MoBi.Core.Services;

namespace MoBi.Presentation.DTO;

public class EditConditionGroupOperandDTO
{
   public TagType TagType { get; set; } = TagType.Match;

   public string Tag { get; set; } = string.Empty;

   public bool IsTagless =>
      TagType == TagType.MatchAll || TagType == TagType.InParent || TagType == TagType.InChildren;
}