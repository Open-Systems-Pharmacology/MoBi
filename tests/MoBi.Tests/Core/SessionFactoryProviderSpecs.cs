using OSPSuite.BDDHelper;
using MoBi.Core.Serialization.ORM.MetaData;
using OSPSuite.Infrastructure.Serialization.Services;

namespace MoBi.Core
{

   public class When_creating_a_session_factory_for_an_existing_given_data_source : ContextSpecificationWithSerializationDatabase<ISessionFactoryProvider>
   {
      [Observation]
      public void should_create_the_database_schema()
      {
         using (var session = _sessionFactory.OpenSession())
         using (var transaction = session.BeginTransaction())
         {
            var projectMetaData = new ProjectMetaData { Id = 1, Name = "toto" };
            session.Save(projectMetaData);
            transaction.Commit();
         }
      }

      [Observation]
      public void should_be_able_to_open_a_session()
      {
         using (var session = _sessionFactory.OpenSession())
         {
         }
      }
   }

}	