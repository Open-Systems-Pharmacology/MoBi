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
            x.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
            x.ExcludeType<UnitDefinitionImporter>();
            x.ExcludeType<FunctionDefinitionImporter>();
         });

         container.Register<SBMLImporterRepository, SBMLImporterRepository>();
         container.Register<ASTHandler, ASTHandler>();
         container.Register<IUnitDefinitionImporter, UnitDefinitionImporter>(LifeStyle.Singleton);
         container.Register<IFunctionDefinitionImporter, FunctionDefinitionImporter>(LifeStyle.Singleton);
      }
   }
}