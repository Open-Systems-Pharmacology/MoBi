using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class SumFormulaDTO : FormulaBuilderDTO
   {
      public SumFormulaDTO(SumFormula sumFormula):base(sumFormula)
      {
      }

      public string Variable { get; set; }
      public IEnumerable<DescriptorConditionDTO> VariableCriteria { get; set; }

      public string VariablePattern { get; set; }
   }
}