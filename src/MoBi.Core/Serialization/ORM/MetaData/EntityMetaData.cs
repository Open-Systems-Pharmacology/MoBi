using NHibernate;

namespace MoBi.Core.Serialization.ORM.MetaData
{
   public class EntityMetaData : MetaDataWithContent<string>, IUpdatableFrom<EntityMetaData>
   {
      public virtual string Discriminator { get; set; }
    
      /// <summary>
      /// This method is called when updating an entity that was already saved in the database
      /// </summary>
      public virtual void UpdateFrom(EntityMetaData sourceMetaData, ISession session)
      {
         UpdateContentFrom(sourceMetaData);
      }
   }
}