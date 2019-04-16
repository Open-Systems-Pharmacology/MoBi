using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class SumFormulaDTO : FormulaBuilderDTO
   {
      public string Variable { get; set; }
      public IEnumerable<IDescriptorConditionDTO> VariableCriteria { get; set; }

      public string VariablePattern { get; set; }
   }
}