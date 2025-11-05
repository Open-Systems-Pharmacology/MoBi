using Castle.Facilities.TypedFactory;
using MoBi.Assets;
using MoBi.CLI.Core.MinimalImplementations;
using MoBi.Core;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Extensions;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Presentation.Serialization;
using MoBi.Presentation.Serialization.Xml;
using MoBi.Presentation.Serialization.Xml.Serializer;
using MoBi.Presentation.Settings;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.FileLocker;
using OSPSuite.Utility.Format;
using System.Threading;
using ApplicationSettings = MoBi.Core.ApplicationSettings;
using CoreRegister = MoBi.Core.CoreRegister;
using IApplicationSettings = MoBi.Core.IApplicationSettings;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.CLI.Core
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

         var register = new SerializerRegister();
         using (container.OptimizeDependencyResolution())
         {
            container.RegisterImplementationOf(NumericFormatterOptions.Instance);
            container.AddRegister(x => x.FromType<CoreRegister>());
            container.AddRegister(x => x.FromType<OSPSuite.Core.CoreRegister>());
            container.AddRegister(x => x.FromType<InfrastructureRegister>());
            container.AddRegister(x => x.FromType<CLIRegister>());
            container.AddRegister(x => x.FromType<OSPSuite.CLI.Core.CLIRegister>());
            container.AddRegister(x => x.FromInstance(register));
         }

         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.RegisterImplementationOf(new SynchronizationContext());

         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<IMoBiConfiguration, IApplicationConfiguration, MoBiConfiguration>(LifeStyle.Singleton);
         container.Register<IMoBiXmlSerializerRepository, MoBiXmlSerializerRepository>(LifeStyle.Singleton);
         container.Register<IUserSettings, IPresentationUserSettings, OSPSuite.Core.ICoreUserSettings, MoBi.Core.ICoreUserSettings, UserSettings>(LifeStyle.Singleton);
         container.Register<IApplicationSettings, OSPSuite.Core.IApplicationSettings, ApplicationSettings>(LifeStyle.Singleton);
         container.Register<ISpatialStructureDiagramManager, SpatialStructureDiagramManager>();
         container.Register<IMoBiReactionDiagramManager, MoBiReactionDiagramManager>();
         container.Register<ISimulationDiagramManager, SimulationDiagramManager>();
         container.Register<ISettingsPersistor<IUserSettings>, UserSettingsPersistor>();
         container.Register<ISettingsPersistor<IApplicationSettings>, ApplicationSettingsPersistor>();

         

         register.PerformMappingForSerializerIn(container);
         initializeDimensions(container);
         initCalculationMethodRepository(container);

         var userSettingsPersistor = container.Resolve<ISettingsPersistor<IUserSettings>>();
         userSettingsPersistor.Load();

         var applicationSettingsPersistor = container.Resolve<ISettingsPersistor<IApplicationSettings>>();
         applicationSettingsPersistor.Load();
      }

      private static void initCalculationMethodRepository(IContainer container)
      {
         var calculationMethodsRepositoryPersistor = container.Resolve<ICalculationMethodsRepositoryPersistor>();
         calculationMethodsRepositoryPersistor.Load();
         //Add Empty CM'S to use in non PK-Sim Models
         var rep = container.Resolve<ICoreCalculationMethodRepository>();
         var objectBaseFactory = container.Resolve<IObjectBaseFactory>();
         rep.GetAllCategoriesDefault().Each(cm => rep.AddCalculationMethod(createDefaultCalculationMethodForCategory(cm.Category, objectBaseFactory)));
      }

      private static CoreCalculationMethod createDefaultCalculationMethodForCategory(string category, IObjectBaseFactory objectBaseFactory)
      {
         var cm = objectBaseFactory.Create<CoreCalculationMethod>()
            .WithName(AppConstants.DefaultNames.EmptyCalculationMethod)
            .WithDescription(AppConstants.DefaultNames.EmptyCalculationMethodDescription);

         cm.Category = category;
         return cm;
      }

      private static void initializeDimensions(IContainer container)
      {
         var applicationConfiguration = container.Resolve<IApplicationConfiguration>();
         var dimensionFactory = container.Resolve<IDimensionFactory>();
         var persistor = container.Resolve<IDimensionFactoryPersistor>();
         persistor.Load(dimensionFactory, applicationConfiguration.DimensionFilePath);
         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);
         dimensionFactory.SetupDimensionMerging();
      }
   }
}