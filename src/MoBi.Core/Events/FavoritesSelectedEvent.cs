using OSPSuite.Core.Domain;

namespace MoBi.Core.Events
{
   public class FavoritesSelectedEvent
   {
      public IObjectBase ObjectBase { get; set; }

      public FavoritesSelectedEvent(IObjectBase objectBase)
      {
         ObjectBase = objectBase;
      }
   }
}