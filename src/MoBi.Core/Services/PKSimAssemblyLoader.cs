using System.Reflection;

namespace MoBi.Core.Services;

public abstract class PKSimAssemblyLoader
{
   protected Assembly _externalAssembly;

   protected void LoadPKSimAssembly()
   {
      if (_externalAssembly != null)
         return;

      _externalAssembly = Assembly.LoadFrom(RetrievePKSimAssemblyPath());
   }

   protected abstract string RetrievePKSimAssemblyPath();

   protected MethodInfo GetMethod(string type, string methodName)
   {
      LoadPKSimAssembly();

      return _externalAssembly.GetType(type).GetMethod(methodName);
   }

   protected object ExecuteMethod(MethodInfo method, object[] parameters = null)
   {
      return method.Invoke(null, parameters);
   }
}