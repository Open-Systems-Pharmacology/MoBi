using MoBi.CLI.Core;
using MoBi.R.Services;
using OSPSuite.Core;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.R
{
   public class RRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddRegister(x => x.FromType<CLIRegister>());

         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<RRegister>();
            scan.IncludeNamespaceContainingType<IModuleTask>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });
      }
   }
}