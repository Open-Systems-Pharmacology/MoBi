using Castle.Facilities.TypedFactory;
using MoBi.CLI.Core.MinimalImplementations;
using MoBi.Core;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Serialization.Xml.Serializer;
using MoBi.Presentation.Settings;
using OSPSuite.Core;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Presentation;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.FileLocker;
using OSPSuite.Utility.Format;
using ICoreUserSettings = MoBi.Core.ICoreUserSettings;

namespace MoBi.CLI
{
   public static class ApplicationStartup
   {
      public static void Initialize()
      {
         var container = new CastleWindsorContainer();
         IoC.InitializeWith(container);

         container.WindsorContainer.AddFacility<EventRegisterFacility>();

         //required to used abstract factory pattern with container
         container.WindsorContainer.AddFacility<TypedFactoryFacility>();

         //Register container into container to avoid any reference to dependency in code
         container.RegisterImplementationOf(container.DowncastTo<IContainer>());
      }

      public static void Start()
      {
         var container = IoC.Container;

         using (container.OptimizeDependencyResolution())
         {
            container.RegisterImplementationOf(NumericFormatterOptions.Instance);

            registerMinimalTypes(container);
         }
      }

      private static void registerMinimalTypes(IContainer container)
      {
         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<ISpatialStructureDiagramManager, SpatialStructureDiagramManager>();
         container.Register<IMoBiReactionDiagramManager, MoBiReactionDiagramManager>();
         container.Register<ISimulationDiagramManager, SimulationDiagramManager>();
         container.Register<IMoBiConfiguration, IApplicationConfiguration, MoBiConfiguration>(LifeStyle.Singleton);
         container.Register<IMoBiXmlSerializerRepository, MoBiXmlSerializerRepository>(LifeStyle.Singleton);
         container.Register<IDiagramManagerFactory, DiagramManagerFactory>(LifeStyle.Singleton);
         container.Register<IDimensionValidator, DimensionValidator>(LifeStyle.Singleton);
         container.Register<IPresentationUserSettings, IUserSettings, ICoreUserSettings, UserSettings>(LifeStyle.Transient);
      }
   }
}