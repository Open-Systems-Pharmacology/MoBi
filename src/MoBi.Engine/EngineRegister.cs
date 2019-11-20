using MoBi.Engine.Sbml;
using OSPSuite.Core;
using OSPSuite.Utility.Container;

namespace MoBi.Engine
{
   public class EngineRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {

         container.AddRegister(x => x.FromType<SBMLImportRegister>());

         container.AddScanner(x =>
         {
            x.AssemblyContainingType<EngineRegister>();
            x.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });
      }
   }
}