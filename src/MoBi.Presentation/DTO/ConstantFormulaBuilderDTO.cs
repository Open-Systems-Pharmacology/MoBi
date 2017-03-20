using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;

namespace MoBi.Presentation.DTO
{
   public class ConstantFormulaBuilderDTO : FormulaBuilderDTO
   {
      public ValueEditDTO Value { get; set; }

      public ConstantFormulaBuilderDTO()
      {
         FormulaType =ObjectTypes.ConstantFormula;
      }

      public override IDimension Dimension
      {
         set
         {
            base.Dimension = value;
            if (Value != null)
               Value.Dimension = value;
         }
      }
   }
}