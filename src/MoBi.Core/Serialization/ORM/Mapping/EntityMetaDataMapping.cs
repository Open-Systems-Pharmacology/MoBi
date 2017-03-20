using FluentNHibernate.Mapping;
using MoBi.Core.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.Mapping
{
   public class EntityMetaDataMapping : ClassMap<EntityMetaData>
   {
      public EntityMetaDataMapping()
      {
         Table("ENTITIES");
         Not.LazyLoad();
         Id(x => x.Id).GeneratedBy.Assigned();
         Map(x => x.Discriminator).Not.Nullable();

         References(x => x.Content)
            .Not.LazyLoad()
            .Column("ContentId")
            .Cascade.All()
            .ForeignKey("fk_Entity_Content");
      }
   }
}