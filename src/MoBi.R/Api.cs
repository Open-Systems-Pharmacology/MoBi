using MoBi.R.Bootstrap;
using OSPSuite.R.Services;
using OSPSuite.Utility.Extensions;
using System;
using MoBi.Presentation.Tasks;
using IContainer = OSPSuite.Utility.Container.IContainer;
using IProjectTask = MoBi.R.Services.IProjectTask;

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
      public static IPopulationTask GetPopulationTask() => resolveTask<IPopulationTask>();
      public static ISimulationResultsTask GetSimulationResultsTask() => resolveTask<ISimulationResultsTask>();
      public static ISimulationPersister GetSimulationPersister() => resolveTask<ISimulationPersister>();
      public static ISerializationTask GetS() => resolveTask<ISerializationTask>();

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