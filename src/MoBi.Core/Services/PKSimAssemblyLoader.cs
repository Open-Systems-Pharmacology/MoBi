using System.Reflection;
using MoBi.Assets;
using MoBi.Core.Exceptions;
using OSPSuite.Utility;

namespace MoBi.Core.Services;

public interface IPKSimAssemblyLoader
{
   void InitializePath(string path);
   void LoadPKSimAssembly();
   MethodInfo GetMethod(string type, string methodName);
   object ExecuteMethod(MethodInfo method, object[] parameters = null);
}

public class PKSimAssemblyLoader : IPKSimAssemblyLoader
{
   private string _assemblyPath;
   private Assembly _externalAssembly;

   public void InitializePath(string path)
   {
      _assemblyPath = path;
   }

   public void LoadPKSimAssembly()
   {
      if (_externalAssembly != null)
         return;

      if (_assemblyPath == null)
         throw new MoBiException(AppConstants.PKSim.PKSimAssemblyLoaderNotInitialized);

      if (!FileHelper.FileExists(_assemblyPath))
         throw new MoBiException(AppConstants.PKSim.CouldNotFindCompatiblePKSimAssemblies(_assemblyPath));

      _externalAssembly = Assembly.LoadFrom(_assemblyPath);
   }

   public MethodInfo GetMethod(string type, string methodName)
   {
      LoadPKSimAssembly();

      var resolvedType = _externalAssembly.GetType(type) ?? throw new MoBiException(AppConstants.PKSim.CouldNotFindTypeInAssembly(type, _externalAssembly.Location));
      return resolvedType.GetMethod(methodName) ?? throw new MoBiException(AppConstants.PKSim.CouldNotFindMethodInAssembly(methodName, type, _externalAssembly.Location));
   }

   public object ExecuteMethod(MethodInfo method, object[] parameters = null)
   {
      return method.Invoke(null, parameters);
   }
}