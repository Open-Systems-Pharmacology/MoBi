using System;
using MoBi.R.Bootstrap;
using OSPSuite.R;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;
using IProjectTask = MoBi.R.Services.IProjectTask;

namespace MoBi.R
{
   public static class Api
   {
      private static IContainer container => OSPSuite.R.Api.Container;

      public static void InitializeOnce(ApiConfig apiConfig)
      {
         if (container != null)
            return;

         ApplicationStartup.Initialize(apiConfig);
      }

      public static IProjectTask GetProjectTask() => resolveTask<IProjectTask>();

      private static T resolveTask<T>()
      {
         try
         {
            return container.Resolve<T>();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.FullMessage());
            throw;
         }
      }
   }
}