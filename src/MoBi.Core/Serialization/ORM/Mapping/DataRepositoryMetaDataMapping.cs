using FluentNHibernate.Mapping;
using MoBi.Core.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.Mapping
{
   public class DataRepositoryMetaDataMapping: ClassMap<DataRepositoryMetaData>
   {
      public DataRepositoryMetaDataMapping()
      {
         Table("DATA_REPOSITORIES");
         Not.LazyLoad();
         Id(x => x.Id).GeneratedBy.Assigned();

         References(x => x.Content)
            .Not.LazyLoad()
            .Column("ContentId")
            .Cascade.All()
            .ForeignKey("fk_DataRepository_Content");
      }
   }
}