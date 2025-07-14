using MoBi.CLI.Core;
using MoBi.CLI.Core.MinimalImplementations;
using MoBi.Core;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Serialization.Xml.Serializer;
using MoBi.Presentation.Settings;
using MoBi.R.Services;
using OSPSuite.CLI.Core.MinimalImplementations;
using OSPSuite.Core;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Presentation;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.FileLocker;
using IContainer = OSPSuite.Utility.Container.IContainer;
using ICoreUserSettings = OSPSuite.Core.ICoreUserSettings;

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

         registerMinimalTypes(container);
      }

      private static void registerMinimalTypes(IContainer container)
      {
         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<ISpatialStructureDiagramManager, SpatialStructureDiagramManager>();
         container.Register<IDiagramModelToXmlMapper, DiagramModelToXmlMapper>();
         container.Register<IJournalDiagramManagerFactory, JournalDiagramManagerFactory>();
         container.Register<IMoBiReactionDiagramManager, MoBiReactionDiagramManager>();
         container.Register<ISimulationDiagramManager, SimulationDiagramManager>();
         container.Register<IMoBiConfiguration, IApplicationConfiguration, MoBiConfiguration>(LifeStyle.Singleton);
         container.Register<IMoBiXmlSerializerRepository, MoBiXmlSerializerRepository>(LifeStyle.Singleton);
         container.Register<IHistoryManagerFactory, HistoryManagerFactory>(LifeStyle.Singleton);
         container.Register<IDiagramManagerFactory, DiagramManagerFactory>(LifeStyle.Singleton);
         container.Register<IDimensionValidator, DimensionValidator>(LifeStyle.Singleton);
         container.Register<IPresentationUserSettings, IUserSettings, ICoreUserSettings, Core.ICoreUserSettings, UserSettings>(LifeStyle.Transient);
      }
   }
}