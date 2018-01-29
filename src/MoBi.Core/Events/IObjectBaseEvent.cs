using OSPSuite.Core.Domain;

namespace MoBi.Core.Events
{
   public interface IObjectBaseEvent
   {
      IObjectBase ObjectBase { get; }
   }
}