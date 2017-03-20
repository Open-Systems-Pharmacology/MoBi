using OSPSuite.Core.Domain;

namespace MoBi.Core.Events
{
   public class QuantityChangedEvent
   {
      public IQuantity Quantity { get; private set; }

      public QuantityChangedEvent(IQuantity quantity)
      {
         Quantity = quantity;
      }
   }

   public class QuantityValueChangedEvent : QuantityChangedEvent
   {
      public QuantityValueChangedEvent(IQuantity quantity) : base(quantity)
      {
      }
   }
}