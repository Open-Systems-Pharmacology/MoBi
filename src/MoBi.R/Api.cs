using System;
using MoBi.CLI.Core.Services;
using MoBi.R.Bootstrap;
using MoBi.R.Services;
using OSPSuite.R;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

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

      public static ISimulationTask GetSimulationTask() => resolveTask<ISimulationTask>();

      public static IModuleTask GetModuleTask() => resolveTask<IModuleTask>();

      public static IIndividualTask GetIndividualTask() => resolveTask<IIndividualTask>();

      public static IParameterValuesTask GetParameterValuesTask() => resolveTask<IParameterValuesTask>();

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