using MoBi.Engine.Sbml;
using OSPSuite.Core;
using OSPSuite.Utility.Container;

namespace MoBi.Engine
{
   public class EngineRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(x =>
         {
            x.AssemblyContainingType<EngineRegister>();
            x.ExcludeType<UnitDefinitionImporter>();
            x.ExcludeType<FunctionDefinitionImporter>();
            x.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });

         //No idea why this is required explicitly. Maybe because the class only has yield operators?
         container.Register<SBMLImporterRepository, SBMLImporterRepository>();
         container.Register<IUnitDefinitionImporter, UnitDefinitionImporter>(LifeStyle.Singleton);
         container.Register<IFunctionDefinitionImporter, FunctionDefinitionImporter>(LifeStyle.Singleton);
      }
   }
}