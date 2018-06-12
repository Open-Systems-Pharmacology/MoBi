using OSPSuite.Core.Domain;

namespace MoBi.Core.Events
{
   public class UserDefinedSelectedEvent : IObjectBaseEvent
   {
      public IObjectBase ObjectBase { get; }

      public UserDefinedSelectedEvent(IObjectBase objectBase)
      {
         ObjectBase = objectBase;
      }
   }
}