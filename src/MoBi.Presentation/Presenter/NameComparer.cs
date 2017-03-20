using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public class NameComparer<T> : IEqualityComparer<T> where T : IObjectBase
   {
      public bool Equals(T x, T y)
      {
         if (x.Name == null) return false;
         if (y.Name == null) return false;
         return x.Name.Equals(y.Name);
      }

      public int GetHashCode(T obj)
      {
         if (obj.Name == null) return 0;
         return obj.Name.GetHashCode();
      }
   }
}