using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class SumFormulaDTO : FormulaBuilderDTO
   {
      private string _variablePattern;
      public string Variable { get; set; }
      public IEnumerable<IDescriptorConditionDTO> VariableCriteria { get; set; }

      public string VariablePattern
      {
         get => _variablePattern;
         set
         {
            _variablePattern = value;
            OnPropertyChanged(() => FormulaString);
         }
      }
   }
}