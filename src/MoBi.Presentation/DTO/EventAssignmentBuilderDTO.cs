using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class EventAssignmentBuilderDTO : ObjectBaseDTO
   {
      public FormulaBuilderDTO NewFormula { get; set; }
      public bool UseAsValue { get; set; }

      public EventAssignmentBuilderDTO()
      {
         Rules.Add(createTargetHasToBeSetRule());
      }

      private IBusinessRule createTargetHasToBeSetRule()
      {
         return CreateRule.For<EventAssignmentBuilderDTO>()
            .Property(x => x.ChangedEntityPath)
            .WithRule((dto, path) => !path.IsNullOrEmpty())
            .WithError(AppConstants.Validation.ChangedEntityNotSet);
      }

      private string _changedEntityPath;

      public string ChangedEntityPath
      {
         get { return _changedEntityPath; }
         set
         {
            _changedEntityPath = value;
            OnPropertyChanged(() => ChangedEntityPath);
         }
      }
   }
}