using System;
using System.Linq.Expressions;
using OSPSuite.Utility.Reflection;

namespace MoBi.Core.Helper
{
   public static class MoBiReflectionHelper
   {
      public static string PropertyName<TObject>(Expression<Func<TObject, object>> property)
      {
         return ReflectionHelper.PropertyFor(property).Name;
      }

      public static string PropertyName<T>(this T obj, Expression<Func<T, object>> property)
      {
         return ReflectionHelper.PropertyFor(property).Name;
      }

      public static Action<T,U> SetterFrom<T,U>( Expression<Func<T, U>> getter)
      {
         var member = (MemberExpression)getter.Body;
         var param = Expression.Parameter(typeof(U), "value");
         var set = Expression.Lambda<Action<T, U>>(
             Expression.Assign(member, param), getter.Parameters[0], param);

         return set.Compile();
      }
   }
}