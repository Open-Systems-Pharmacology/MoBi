using NHibernate;

namespace MoBi.Core.Serialization.ORM.MetaData
{
   public interface IUpdatableFrom<T>
   {
      void UpdateFrom(T source, ISession session);
   }
}