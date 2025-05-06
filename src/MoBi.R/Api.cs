using MoBi.R.Bootstrap;
using OSPSuite.R.Services;
using OSPSuite.Utility.Extensions;
using System;
using MoBi.Presentation.Tasks;
using OSPSuite.R;
using IContainer = OSPSuite.Utility.Container.IContainer;
using IProjectTask = MoBi.R.Services.IProjectTask;

namespace MoBi.R
{
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