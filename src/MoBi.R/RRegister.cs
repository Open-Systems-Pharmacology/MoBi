using MoBi.Core;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Presentation.Serialization.Xml.Serializer;
using MoBi.R.MinimalImplementations;
using MoBi.R.Services;
using OSPSuite.Core;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.FileLocker;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.R
{
   public class RRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<RRegister>();
            scan.IncludeNamespaceContainingType<IProjectTask>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         registerMinimalTypes(container);
      }

      private static void registerMinimalTypes(IContainer container)
      {
         //container.Register<IEventPublisher, EventPublisher>();
         //container.Register<IDialogCreator, DialogCreator>();
         //container.RegisterImplementationOf(new SynchronizationContext());
         //container.Register<IExceptionManager, ExceptionManager>(LifeStyle.Singleton);
         //container.Register<IDisplayUnitRetriever, DisplayUnitRetriever>(LifeStyle.Singleton);
         //container.Register<IMoBiContext, MoBiContext>(LifeStyle.Singleton);
         //container.Register<ISimulationRunner, SimulationRunner>();
         //container.Register<ISimulationPersistableUpdater, SimulationPersistableUpdater>();
         //container.Register<IPopulationTask, PopulationTask>();
         //container.Register<IOSPSuiteLogger, RLogger, RLogger>(LifeStyle.Singleton);
         //container.Register<IProgressManager, ProgressManager>(LifeStyle.Singleton);
         //container.RegisterFactory<IHistoryManagerFactory>();

         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<ISpatialStructureDiagramManager, SpatialStructureDiagramManager>();
         container.Register<IDiagramModelToXmlMapper, DiagramModelToXmlMapper>();
         container.Register<IJournalDiagramManagerFactory, JournalDiagramManagerFactory>();
         container.Register<IMoBiReactionDiagramManager, MoBiReactionDiagramManager>();
         container.Register<ISimulationDiagramManager, SimulationDiagramManager>();
         container.Register<IMoBiConfiguration, IApplicationConfiguration, MoBiConfiguration>(LifeStyle.Singleton);
         container.Register<IMoBiXmlSerializerRepository, MoBiXmlSerializerRepository>(LifeStyle.Singleton);
         container.Register<IHistoryManagerFactory, HistoryManagerFactory>(LifeStyle.Singleton);
      }
   }
}