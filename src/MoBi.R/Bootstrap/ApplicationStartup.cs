using MoBi.Core.Services;
using MoBi.Presentation.Serialization;
using OSPSuite.CLI.Core;
using OSPSuite.Infrastructure;
using OSPSuite.R;
using OSPSuite.Utility.Container;
using CoreRegister = MoBi.Core.CoreRegister;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.R.Bootstrap
{
   internal class ApplicationStartup
   {
      public static void Initialize(ApiConfig apiConfig)
      {
         OSPSuite.R.Api.InitializeOnce(apiConfig, registerAction);
         new SerializerRegister().PerformMappingForMoBiSerializerRepository(OSPSuite.R.Api.Container);
         CalculationMethodRepositoryInitialization.Initialize(OSPSuite.R.Api.Container);
      }

      private static void registerAction(IContainer container)
      {
         container.AddRegister(x => x.FromType<CoreRegister>());
         container.AddRegister(x => x.FromType<InfrastructureRegister>());
         container.AddRegister(x => x.FromType<RRegister>());
         container.AddRegister(x => x.FromType<CLIRegister>());
      }
   }
}