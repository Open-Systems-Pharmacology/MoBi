using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class EventAssignmentBuilderDTO : ObjectBaseDTO
   {
      public FormulaBuilderDTO NewFormula { get; set; }
      public bool UseAsValue { get; set; }

      public EventAssignmentBuilderDTO(EventAssignmentBuilder eventAssignmentBuilder) : base(eventAssignmentBuilder)
      {
         Rules.Add(createTargetHasToBeSetRule);
      }

      private IBusinessRule createTargetHasToBeSetRule { get; } = CreateRule.For<EventAssignmentBuilderDTO>()
         .Property(x => x.ChangedEntityPath)
         .WithRule((dto, path) => !path.IsNullOrEmpty())
         .WithError(AppConstants.Validation.ChangedEntityNotSet);

      private string _changedEntityPath;

      public virtual string ChangedEntityPath
      {
         get => _changedEntityPath;
         set => SetProperty(ref _changedEntityPath, value);
      }
   }
}