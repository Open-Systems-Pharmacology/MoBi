using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.MetaData
{
   public abstract class MetaDataWithContent<TKey> : MetaData<TKey>
   {
      /// <summary>
      /// Serialization of the entire Entity
      /// </summary>
      public virtual MetaDataContent Content { get; set; }

      public virtual bool IsLoaded
      {
         get { return Content.Data != null; }
      }

      protected MetaDataWithContent()
      {
         Content = new MetaDataContent();
      }

      protected virtual void UpdateContentFrom(MetaDataWithContent<TKey> source)
      {
         if (!(source.IsLoaded)) return;
         Content.Data = source.Content.Data;
      }
   }
}