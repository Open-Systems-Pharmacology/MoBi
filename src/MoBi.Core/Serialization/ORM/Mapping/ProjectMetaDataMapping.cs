using FluentNHibernate.Mapping;
using MoBi.Core.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.Mapping
{
   public class ProjectMetaDataMapping : ClassMap<ProjectMetaData>
   {
      public ProjectMetaDataMapping()
      {
         Table("PROJECTS");
         Not.LazyLoad();
         Id(x => x.Id).GeneratedBy.Native();
         Map(x => x.Version);

         HasMany(x => x.Children)
            .Not.LazyLoad()
            .Fetch.Join()
            .Cascade.AllDeleteOrphan()
            .KeyColumn("ProjectId")
            .AsSet();

         References(x => x.Content)
            .Not.LazyLoad()
            .Column("ContentId")
            .Cascade.All()
            .ForeignKey("fk_Project_Content");
      }
  
   }
}