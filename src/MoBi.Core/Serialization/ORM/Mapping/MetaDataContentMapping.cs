using FluentNHibernate.Mapping;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.Mapping
{
   public class MetaDataContentMapping : ClassMap<MetaDataContent>
   {
      public MetaDataContentMapping()
      {
         Table("CONTENTS");
         Id(x => x.Id).GeneratedBy.Native();
         Map(x => x.Data).CustomType("BinaryBlob")
            .CustomSqlType("image");
      }
   }
}