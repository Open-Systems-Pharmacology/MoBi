using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class QuantityDTO : ObjectBaseDTO
   {
      public FormulaBuilderDTO Formula { set; get; }
      public ValueEditDTO Value { get; set; }

      public IDimension Dimension
      {
         get { return Value.Dimension; }
         set { Value.Dimension = value; }
      }
   }
}