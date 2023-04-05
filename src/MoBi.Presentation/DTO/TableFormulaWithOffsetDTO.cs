using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class TableFormulaWithOffsetDTO : FormulaBuilderDTO
   {
      public TableFormulaWithOffsetDTO(TableFormulaWithOffset formula) : base(formula)
      {
      }
      public FormulaUsablePathDTO OffsetObjectPath { get; set; }
      public FormulaUsablePathDTO TableObjectPath { get; set; }
   }
}