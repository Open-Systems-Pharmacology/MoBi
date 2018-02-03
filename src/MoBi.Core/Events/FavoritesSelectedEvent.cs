using OSPSuite.Core.Domain;

namespace MoBi.Core.Events
{
   public class FavoritesSelectedEvent : IObjectBaseEvent
   {
      public IObjectBase ObjectBase { get; }

      public FavoritesSelectedEvent(IObjectBase objectBase)
      {
         ObjectBase = objectBase;
      }
   }
}