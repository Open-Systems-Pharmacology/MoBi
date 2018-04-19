namespace MoBi.Presentation.DTO
{
   public class TableFormulaWithXArgumentDTO : FormulaBuilderDTO
   {
      public FormulaUsablePathDTO XArgumentObjectPath { get; set; }
      public FormulaUsablePathDTO TableObjectPath { get; set; }
   }
}