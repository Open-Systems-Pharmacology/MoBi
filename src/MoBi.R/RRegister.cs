using MoBi.CLI.Core;
using MoBi.CLI.Core.MinimalImplementations;
using MoBi.Core;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.Services;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Serializer;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.R.Services;
using OSPSuite.CLI.Core.MinimalImplementations;
using OSPSuite.Core;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.FileLocker;
using IContainer = OSPSuite.Utility.Container.IContainer;
using ICoreUserSettings = MoBi.Core.ICoreUserSettings;
using ObjectTypeResolver = MoBi.Core.Helper.ObjectTypeResolver;

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
            // Registered explicitly below so MoBi.R uses the headless PK-Sim snapshot converter.
            scan.ExcludeType<PKSimSnapshotConverter>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         // Headless snapshot conversion via PKSim.R.dll (no PK-Sim desktop install / PKSim.Starter.dll).
         container.Register<IPKSimSnapshotConverter, PKSimSnapshotConverter>(LifeStyle.Singleton);

         registerMinimalTypes(container);
      }

      private static void registerMinimalTypes(IContainer container)
      {
         container.Register<ICoreSerializationTask, CoreSerializationTask>(LifeStyle.Singleton);
         container.Register<IXmlSerializationService, XmlSerializationService>();
         container.Register<IContextPersistor, ContextPersistor>();
         container.Register<IObjectTypeResolver, ObjectTypeResolver>();
         container.Register<IProjectConverterLogger, ProjectConverterLogger>();
         container.Register<IPostSerializationStepsMaker, PostSerializationStepsMaker>();
         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<ISpatialStructureDiagramManager, SpatialStructureDiagramManager>();
         container.Register<IDiagramModelToXmlMapper, DiagramModelToXmlMapper>();
         container.Register<IMoBiReactionDiagramManager, MoBiReactionDiagramManager>();
         container.Register<ISimulationDiagramManager, SimulationDiagramManager>();
         container.Register<IMoBiConfiguration, IApplicationConfiguration, MoBiConfiguration>(LifeStyle.Singleton);
         container.Register<IMoBiXmlSerializerRepository, MoBiCoreXmlSerializerRepository>(LifeStyle.Singleton);
         container.Register<IHistoryManagerFactory, HistoryManagerFactory>(LifeStyle.Singleton);
         container.Register<IDiagramManagerFactory, DiagramManagerFactory>(LifeStyle.Singleton);
         container.Register<IDimensionValidator, DimensionValidator>(LifeStyle.Singleton);
         container.Register<ICoreUserSettings, OSPSuite.Core.ICoreUserSettings, CoreUserSettings>(LifeStyle.Singleton);
      }
   }
}