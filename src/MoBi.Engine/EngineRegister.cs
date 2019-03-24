using MoBi.Core;
using MoBi.Engine.Sbml;
using OSPSuite.Core;
using OSPSuite.FuncParser;
using OSPSuite.Utility.Container;
using SimModelNET;

namespace MoBi.Engine
{
   public class EngineRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddRegister(x => x.FromType<OSPSuite.Engine.EngineRegister>());
         var pkSimConfiguration = container.Resolve<IMoBiConfiguration>();
         XMLSchemaCache.InitializeFromFile(pkSimConfiguration.SimModelSchemaFilePath);

         container.Register<IDimensionParser, DimensionParser>();

         container.AddRegister(x => x.FromType<SBMLImportRegister>());

         container.AddScanner(x =>
         {
            x.AssemblyContainingType<EngineRegister>();
            x.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });
      }
   }
}