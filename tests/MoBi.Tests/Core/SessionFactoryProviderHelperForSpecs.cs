using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Serialization.ORM;
using NHibernate;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core
{
   public abstract class ContextSpecificationWithSerializationDatabase<T> : ContextSpecification<T>
   {
      private SessionFactoryProvider _sessionFactoryProvider;
      protected ISessionFactory _sessionFactory;
      protected string _dataBaseFile;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _sessionFactoryProvider = new SessionFactoryProvider();

         _dataBaseFile = FileHelper.GenerateTemporaryFileName();
         _sessionFactory = _sessionFactoryProvider.InitalizeSessionFactoryFor(_dataBaseFile);

         this.LogDebug("Database file {0}", _dataBaseFile);

      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         if (_sessionFactory == null) return;
         _sessionFactory.Close();

         FileHelper.DeleteFile(_dataBaseFile);
      }
   }
}	