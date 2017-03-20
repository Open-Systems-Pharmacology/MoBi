using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ExportQuantitiesSelectionDTO : Notifier, IValidatable
   {
      private readonly IBusinessRuleSet _rules;
      private string _reportFile;

      public ExportQuantitiesSelectionDTO()
      {
         _rules = new BusinessRuleSet(AllRules.PathDefined);
      }

      public string ReportFile
      {
         get { return _reportFile; }
         set
         {
            _reportFile = value;
            OnPropertyChanged(() => ReportFile);
         }
      }

      private static class AllRules
      {
         public static IBusinessRule PathDefined
         {
            get { return GenericRules.NonEmptyRule<ExportQuantitiesSelectionDTO>(x => x.ReportFile); }
         }
      }

      public IBusinessRuleSet Rules
      {
         get { return _rules; }
      }
   }
}