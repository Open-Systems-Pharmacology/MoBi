using MoBi.CLI.Core.RunOptions;
using MoBi.CLI.Core.Services;
using OSPSuite.Core;
using OSPSuite.Utility.Container;

namespace MoBi.CLI.Core
{
   public class CLIRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(x =>
         {
            x.AssemblyContainingType<CLIRegister>();

            //Register services
            x.IncludeNamespaceContainingType<SnapshotRunner>();
            x.WithConvention<OSPSuiteRegistrationConvention>();
         });

         container.Register<IBatchRunner<SnapshotRunOptions>, SnapshotRunner>();
      }
   }
}