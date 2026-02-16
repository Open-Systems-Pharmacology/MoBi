using MoBi.Assets;
using MoBi.Core.Exceptions;
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

      var resolvedType = _externalAssembly.GetType(type) ?? throw new MoBiException(AppConstants.PKSim.CouldNotFindTypeInAssembly(type, _externalAssembly.Location));
      return resolvedType.GetMethod(methodName) ?? throw new MoBiException(AppConstants.PKSim.CouldNotFindMethodInAssembly(methodName, type, _externalAssembly.Location));

   }

   protected object ExecuteMethod(MethodInfo method, object[] parameters = null)
   {
      return method.Invoke(null, parameters);
   }
}