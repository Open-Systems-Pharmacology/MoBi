using System;
using System.Reflection;
using MoBi.Assets;
using MoBi.Core.Exceptions;
using OSPSuite.Utility;

namespace MoBi.Core.Services;

public interface IPKSimAssemblyLoader
{
   void InitializePath(Func<string> pathRetriever);
   void LoadPKSimAssembly();
   object ExecuteMethod(string type, string methodName, object[] parameters = null);
}

public class PKSimAssemblyLoader : IPKSimAssemblyLoader
{
   private Func<string> _pathRetriever;
   private Assembly _externalAssembly;

   public void InitializePath(Func<string> pathRetriever)
   {
      _pathRetriever = pathRetriever;
   }

   public void LoadPKSimAssembly()
   {
      if (_externalAssembly != null)
         return;

      if (_pathRetriever == null)
         throw new MoBiException(AppConstants.PKSim.PKSimAssemblyLoaderNotInitialized);

      var assemblyPath = _pathRetriever();
      if (!FileHelper.FileExists(assemblyPath))
         throw new MoBiException(AppConstants.PKSim.CouldNotFindCompatiblePKSimAssemblies(assemblyPath));

      _externalAssembly = Assembly.LoadFrom(assemblyPath);
   }

   private MethodInfo getMethod(string type, string methodName)
   {
      LoadPKSimAssembly();

      var resolvedType = _externalAssembly.GetType(type) ?? throw new MoBiException(AppConstants.PKSim.CouldNotFindTypeInAssembly(type, _externalAssembly.Location));
      return resolvedType.GetMethod(methodName) ?? throw new MoBiException(AppConstants.PKSim.CouldNotFindMethodInAssembly(methodName, type, _externalAssembly.Location));
   }

   public object ExecuteMethod(string type, string methodName, object[] parameters = null)
   {
      var method = getMethod(type, methodName);
      return method.Invoke(null, parameters);
   }
}