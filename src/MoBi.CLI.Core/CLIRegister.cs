using MoBi.CLI.Core.Services;
using OSPSuite.CLI.Core.RunOptions;
using OSPSuite.CLI.Core.Services;
using OSPSuite.Core;
using OSPSuite.Core.Domain.Mappers;
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
         container.Register<IPathToPathElementsMapper, PathToPathElementsMapper>();
         container.Register<IDataColumnToPathElementsMapper, DataColumnToPathElementsMapper>();
      }
   }
}