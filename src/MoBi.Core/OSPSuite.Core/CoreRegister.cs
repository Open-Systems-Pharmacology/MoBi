using OSPSuite.Core.Comparison;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   // Temporary register of Core components while they reside in MoBi
    public class MoBiRegisterOfCore : Register
    {
       public override void RegisterInContainer(IContainer container)
       {
          registerComparers(container);
       }

       private static void registerComparers(IContainer container)
       {
          container.AddScanner(scan =>
          {
             scan.AssemblyContainingType<MoBiRegisterOfCore>();
             scan.IncludeNamespaceContainingType<ExpressionProfileBuildingBlockDiffBuilder>();

             scan.WithConvention<RegisterTypeConvention<IDiffBuilder>>();
          });
       }
   }
}
