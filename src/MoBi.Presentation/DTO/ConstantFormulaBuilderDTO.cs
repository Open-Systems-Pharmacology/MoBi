using OSPSuite.Assets;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class ConstantFormulaBuilderDTO : FormulaBuilderDTO
   {
      public ValueEditDTO Value { get; set; }

      public ConstantFormulaBuilderDTO(ConstantFormula constantFormula) : base(constantFormula)
      {
         FormulaType = ObjectTypes.ConstantFormula;
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