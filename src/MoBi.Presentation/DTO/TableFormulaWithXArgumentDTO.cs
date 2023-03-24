using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class TableFormulaWithXArgumentDTO : FormulaBuilderDTO
   {
      public TableFormulaWithXArgumentDTO(TableFormulaWithXArgument formula) : base(formula)
      {
      }
      public FormulaUsablePathDTO XArgumentObjectPath { get; set; }
      public FormulaUsablePathDTO TableObjectPath { get; set; }
   }
}