using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.Presentation;
using MoBi.Presentation.Serialization.Xml.Serializer;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.R.MinimalImplementations;
using OSPSuite.Core;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Presentation;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.FileLocker;
using IContainer = OSPSuite.Utility.Container.IContainer;
using IProjectTask = MoBi.R.Services.IProjectTask;

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
         container.Register<ISerializationTask, SerializationTask>(LifeStyle.Singleton);
         container.Register<IXmlSerializationService, XmlSerializationService>();
         container.Register<IContextPersistor, ContextPersistor>();
         container.Register<IObjectTypeResolver, ObjectTypeResolver>();
         container.Register<IXmlContentSelector, XmlContentSelector>();
         container.Register<IMoBiApplicationController, MoBiApplicationController>();
         container.Register<IProjectConverterLogger, ProjectConverterLogger>();
         container.Register<IMoBiContext, MoBiContext>();
         container.Register<IPostSerializationStepsMaker, PostSerializationStepsMaker>();
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
         container.Register<IPresentationUserSettings, IUserSettings, ICoreUserSettings, UserSettings>(LifeStyle.Transient);
      }
   }
}