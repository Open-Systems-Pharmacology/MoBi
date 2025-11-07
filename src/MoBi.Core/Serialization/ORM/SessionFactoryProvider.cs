using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Serialization.Services;

namespace MoBi.Core.Serialization.ORM
{
   public class SessionFactoryProvider : ISessionFactoryProvider
   {
      public ISessionFactory InitializeSessionFactoryFor(string dataSource)
      {
         var cfg = createSqlLiteConfigurationFor(dataSource);
         //Create schema for database
         new SchemaExport(cfg).Execute(useStdOut: false, execute: true, justDrop: false);

         return cfg.BuildSessionFactory();
      }

      public ISessionFactory OpenSessionFactoryFor(string dataSource)
      {
         return createSqlLiteConfigurationFor(dataSource).BuildSessionFactory();
      }

      public SchemaExport GetSchemaExport(string dataSource)
      {
         var cfg = createSqlLiteConfigurationFor(dataSource);
         //Create schema for database
         return new SchemaExport(cfg);
      }

      private Configuration createSqlLiteConfigurationFor(string dataSource)
      {
         var configuration = new Configuration();
         var path = dataSource.ToUNCPath();
         configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
         configuration.SetProperty("connection.driver_class", typeof(NHibernate.Extensions.Sqlite.SqliteDriver).AssemblyQualifiedName);
         configuration.SetProperty("dialect", typeof(NHibernate.Extensions.Sqlite.SqliteDialect).AssemblyQualifiedName);
         configuration.SetProperty("query.substitutions", "true=1;false=0");
         configuration.SetProperty("show_sql", "false");
         configuration.SetProperty("connection.connection_string", $"Data Source={path};Cache=Shared");

         return Fluently.Configure(configuration)
            .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>()).BuildConfiguration();
      }
   }
}