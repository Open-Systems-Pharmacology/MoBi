using MoBi.Core.Serialization.ORM;
using NHibernate;
using OSPSuite.BDDHelper;
using OSPSuite.Utility;

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