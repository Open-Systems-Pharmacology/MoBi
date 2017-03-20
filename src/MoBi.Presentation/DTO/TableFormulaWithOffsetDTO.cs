namespace MoBi.Presentation.DTO
{
   public class TableFormulaWithOffsetDTO : FormulaBuilderDTO
   {
      public FormulaUsablePathDTO OffsetObjectPath { get; set; }
      public FormulaUsablePathDTO TableObjectPath { get; set; }
   }
}