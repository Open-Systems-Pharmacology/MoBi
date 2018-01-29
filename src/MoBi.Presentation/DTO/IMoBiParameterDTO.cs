using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public interface IMoBiParameterDTO : IParameterDTO
   {
      FormulaBuilderDTO RHSFormula { get; set; }
      FormulaBuilderDTO Formula { get; set; }
   }
}