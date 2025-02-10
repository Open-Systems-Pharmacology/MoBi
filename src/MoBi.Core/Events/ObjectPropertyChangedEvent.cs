namespace MoBi.Core.Events
{
   public class ObjectPropertyChangedEvent
   {
      public readonly object ChangedObject;

      public ObjectPropertyChangedEvent(object changedObject)
      {
         ChangedObject = changedObject;
      }
   }
}