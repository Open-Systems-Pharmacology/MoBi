using MoBi.Core.Domain.Model;

namespace MoBi.Core.Serialization.ORM
{
   public interface IPersistor<T>
   {
      void Save(T target, IMoBiContext context);
      T Load(IMoBiContext context);
   }
}