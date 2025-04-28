using System;
using MoBi.R.Bootstrap;
using MoBi.R.Services;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.R
{
   public class ApiConfig
   {
      public string DimensionFilePath { get; set; }
      public string PKParametersFilePath { get; set; }
   }
   public static class Api
   {
      public static IContainer Container { get; private set; }

      public static void InitializeOnce(ApiConfig apiConfig)
      {
         Container = ApplicationStartup.Initialize(apiConfig);
      }

      public static IProjectTask GetProjectTask() => resolveTask<IProjectTask>();

      private static T resolveTask<T>()
      {
         try
         {
            return Container.Resolve<T>();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.FullMessage());
            throw;
         }
      }
   }
}