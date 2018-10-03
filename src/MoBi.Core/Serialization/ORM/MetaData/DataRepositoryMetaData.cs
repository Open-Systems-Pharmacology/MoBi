using NHibernate;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.MetaData
{
   public class DataRepositoryMetaData : MetaDataWithContent<string>, IUpdatableFrom<DataRepositoryMetaData>
   {
      public void UpdateFrom(DataRepositoryMetaData source, ISession session)
      {
         UpdateContentFrom(source);
      }
   }
}