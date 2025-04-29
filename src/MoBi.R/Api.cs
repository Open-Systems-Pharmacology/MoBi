using MoBi.R.Bootstrap;
using MoBi.R.Services;
using OSPSuite.R.Services;
using OSPSuite.Utility.Extensions;
using System;
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
      public static ISimulationRunner GetSimulationRunner() => resolveTask<ISimulationRunner>();
      public static ISimulationResultsTask GetSimulationResultsTask() => resolveTask<ISimulationResultsTask>();
      public static ISimulationPersister GetSimulationPersister() => resolveTask<ISimulationPersister>();

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