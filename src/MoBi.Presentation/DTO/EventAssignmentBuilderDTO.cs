using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class EventAssignmentBuilderDTO : ObjectBaseDTO
   {
      public FormulaBuilderDTO NewFormula { get; set; }
      public bool UseAsValue { get; set; }
      private string _changedEntityPath;
      private readonly EventAssignmentBuilder _eventAssignmentBuilder;

      public EventAssignmentBuilderDTO(EventAssignmentBuilder eventAssignmentBuilder) : base(eventAssignmentBuilder)
      {
         _eventAssignmentBuilder = eventAssignmentBuilder;
         Rules.Add(createTargetHasToBeSetRule);
      }

      private IBusinessRule createTargetHasToBeSetRule { get; } = CreateRule.For<EventAssignmentBuilderDTO>()
         .Property(x => x.ChangedEntityPath)
         .WithRule((dto, path) => !path.IsNullOrEmpty())
         .WithError(AppConstants.Validation.ChangedEntityNotSet);

      public virtual string ChangedEntityPath
      {
         get => _changedEntityPath;
         set => SetProperty(ref _changedEntityPath, value);
      }

      public IDimension Dimension
      {
         get => _eventAssignmentBuilder.Dimension;
         set => _eventAssignmentBuilder.Dimension = value;
      }
   }
}