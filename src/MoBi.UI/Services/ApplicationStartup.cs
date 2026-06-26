using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Castle.Facilities.TypedFactory;
using Microsoft.Extensions.Logging;
using MoBi.Core;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Core.Serialization.Xml;
using MoBi.Presentation.Serialization;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using MoBi.UI.Diagram;
using MoBi.UI.Extensions;
using MoBi.UI.Settings;
using MoBi.UI.Views;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Services;
using OSPSuite.UI;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.FileLocker;
using CoreRegister = OSPSuite.Core.CoreRegister;
using IApplicationSettings = MoBi.Core.IApplicationSettings;
using IContainer = OSPSuite.Utility.Container.IContainer;
using ICoreUserSettings = OSPSuite.Core.ICoreUserSettings;
using IMoBiCoreUserSettings = MoBi.Core.ICoreUserSettings;

namespace MoBi.UI.Services
{
   public class ApplicationStartup
   {
      public static void Initialize(Action<IContainer> registrationAction = null)
      {
         new ApplicationStartup().InitializeForStartup(registrationAction);
      }

      /// <summary>
      ///    Sets up the global configuration that a MoBi Application needs to run
      /// </summary>
      public void Start()
      {
         var container = IoC.Container;
         using (var progress = createSplashProgressUpdater(container))
         {
            progress.Initialize(8);
            using (container.OptimizeDependencyResolution())
            {
               showStatusMessage(progress, "Init Core");
               registerSettings(container);
               registerCoreComponents(container);

               showStatusMessage(progress, "Init Presenter");
               registerPresenter(container);

               showStatusMessage(progress, "Init Views");
               registerView(container);

               showStatusMessage(progress, "Init Tasks");

               showStatusMessage(progress, "Init Serializer");
               registerSerializationDependencies(container);

               registerImport(container);

               showStatusMessage(progress, "Init Chart UI");
               registerUIComponents(container);

               showStatusMessage(progress, "Init Work Context");
               initContext(container);
               initUserProfileFileAppData(container);
            }
         }
      }

      //The splash runs on its own UI thread, so its progress updates must be dispatched to that thread. Bind a
      //dedicated EventPublisher to the splash's own synchronization context and drive a ProgressUpdater through it, so
      //the splash presenter handles the events on the splash thread - updating incrementally and without a cross-thread.
      private static IProgressUpdater createSplashProgressUpdater(IContainer container)
      {
         var splashPresenter = container.Resolve<ISplashScreenPresenter>();
         var splashControl = (Control) splashPresenter.View;

         // Wait until the control is created, up to a maximum of 5 seconds
         SpinWait.SpinUntil(() => splashControl.IsHandleCreated, TimeSpan.FromSeconds(5));
         if (!splashControl.IsHandleCreated)
            return container.Resolve<IProgressManager>().Create();

         var splashContext = splashControl.Invoke(() => SynchronizationContext.Current);
         var splashThread = splashControl.Invoke(() => Thread.CurrentThread);
         var splashPublisher = new EventPublisher(splashContext, splashThread, container.Resolve<IExceptionManager>());
         splashPublisher.AddListener(splashPresenter);
         return new ProgressUpdater(splashPublisher);
      }

      private static void updateGoDiagramKey()
      {
         // This line is patched during creation of setup. Do not modify.
         UIRegister.GoDiagramKey = $"{Environment.GetEnvironmentVariable("GO_DIAGRAM_KEY")}";
      }

      private static void registerUIComponents(IContainer container)
      {
         container.AddRegister(x => x.FromType<DiagramRegister>());
      }
      
      private static void registerSettings(IContainer container)
      {
         container.Register<ICloneableUserSettings, IUserSettings, IPresentationUserSettings, ICoreUserSettings, IMoBiCoreUserSettings, UserSettings>(LifeStyle.Singleton);
      }

      private void initContext(IContainer container)
      {
         InitDimensions(container);

         var userSettingsPersistor = container.Resolve<ISettingsPersistor<IUserSettings>>();
         userSettingsPersistor.Load();

         var applicationSettingsPersistor = container.Resolve<ISettingsPersistor<IApplicationSettings>>();
         applicationSettingsPersistor.Load();

         InitCalculationMethodRepository(container);
         initGroupRepository(container);

         loadPKParameterRepository(container);
      }

      private static void loadPKParameterRepository(IContainer container)
      {
         var pkParameterRepository = container.Resolve<IPKParameterRepository>();
         var pKParameterLoader = container.Resolve<IPKParameterRepositoryLoader>();
         var configuration = container.Resolve<IMoBiConfiguration>();
         pKParameterLoader.Load(pkParameterRepository, configuration.PKParametersFilePath);
      }

      private void initGroupRepository(IContainer container)
      {
         var configuration = container.Resolve<IMoBiConfiguration>();
         var groupRepository = container.Resolve<IGroupRepository>();
         var persister = container.Resolve<IGroupRepositoryPersistor>();
         persister.Load(groupRepository, configuration.GroupRepositoryFile);
      }

      public static void InitDimensions(IContainer container)
      {
         var configuration = container.Resolve<IMoBiConfiguration>();
         var dimFactory = container.Resolve<IMoBiDimensionFactory>();
         var persister = container.Resolve<IDimensionFactoryPersistor>();
         persister.Load(dimFactory, configuration.DimensionFilePath);
         dimFactory.AddDimension(Constants.Dimension.NO_DIMENSION);
         container.RegisterImplementationOf<IDimensionFactory>(dimFactory);
         dimFactory.SetupDimensionMerging();
      }

      // because Setup cannot copy into each user profile app data, copy has to be done here
      private void initUserProfileFileAppData(IContainer container)
      {
         var configuration = container.Resolve<IMoBiConfiguration>();

         // template is not necessary in user profile app data, because if not available base template is used,
         // but it may be helpful to support user to store his own template at the right location.
         if (!Directory.Exists(configuration.CurrentUserFolderPath))
            Directory.CreateDirectory(configuration.CurrentUserFolderPath);

         if (!File.Exists(configuration.SpaceOrganismUserTemplate))
            File.Copy(configuration.SpaceOrganismBaseTemplate, configuration.SpaceOrganismUserTemplate);

         if (!Directory.Exists(configuration.ChartLayoutTemplateFolderPath))
            Directory.CreateDirectory(configuration.ChartLayoutTemplateFolderPath);
      }

      public static void InitCalculationMethodRepository(IContainer container)
      {
         CalculationMethodRepositoryInitialization.Initialize(container);
      }

      public void InitializeForStartup(Action<IContainer> registrationAction)
      {
         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
         Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

         updateGoDiagramKey();

         var container = new CastleWindsorContainer();
         IoC.InitializeWith(container);
         IoC.RegisterImplementationOf<IContainer>(container);
         initFacilities(container);
         initializeSynchronizationContext();
         container.RegisterImplementationOf(SynchronizationContext.Current);
         // Register the UI thread so the EventPublisher singleton captures it (via its explicit-thread
         // constructor) as its context thread. Events published from the UI thread then dispatch
         // synchronously (Send) rather than deferred (Post) - which matters when a publish is followed
         // immediately by teardown (e.g. closing the project on exit). Registering it as an instance
         // lets the publisher resolve lazily with its other dependencies, without an early resolve here.
         container.RegisterImplementationOf(Thread.CurrentThread);

         container.AddRegister(x => x.FromType<PresenterRegister>());
         container.AddRegister(x => x.FromType<UIRegister>());
         container.AddRegister(x => x.FromType<InfrastructureRegister>());

         // Global Singleton Objects
         container.Register<IExceptionManager, ExceptionManager>(LifeStyle.Singleton);

         container.RegisterImplementationOf(this);

         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<ISplashScreen, SplashScreen>(LifeStyle.Singleton);
         container.Register<ISplashScreenPresenter, SplashScreenPresenter>(LifeStyle.Singleton);
         container.Register<IProgressUpdater, ProgressUpdater>();
         container.RegisterFactory<IProgressManager>();

         container.Register<IMoBiConfiguration, IApplicationConfiguration, MoBiConfiguration>(LifeStyle.Singleton);

         registerRunOptionsIn(container);

         EnvironmentHelper.ApplicationName = () => "mobi";

         configureLogger(container, LogLevel.Information);

         registrationAction?.Invoke(container);
      }

      private void configureLogger(IContainer container, LogLevel logLevel)
      {
         var loggerCreator = container.Resolve<ILoggerCreator>();

         loggerCreator
            .AddLoggingBuilderConfiguration(builder =>
               builder
                  .SetMinimumLevel(logLevel)
                  .AddDebug()
                  .AddPresenter()
            );
      }

      private static void registerRunOptionsIn(IContainer container)
      {
         container.Register<StartOptions, IStartOptions, StartOptions>(LifeStyle.Singleton);
      }

      private void showStatusMessage(IProgressUpdater progressUpdater, string message)
      {
         progressUpdater.IncrementProgress($"{message}...");
      }

      private static void initFacilities(CastleWindsorContainer container)
      {
         container.WindsorContainer.AddFacility<EventRegisterFacility>();
         container.WindsorContainer.AddFacility<TypedFactoryFacility>();
      }

      private static void registerCoreComponents(IContainer container)
      {
         container.AddRegister(x => x.FromType<CoreRegister>());
         container.AddRegister(x => x.FromType<Core.CoreRegister>());
      }

      private static void registerImport(IContainer container)
      {
         var mobiDataImporterSettings = new DataImporterSettings { IconName = ApplicationIcons.MoBi.IconName, Caption = "MoBi Data Import" };
         container.RegisterImplementationOf(mobiDataImporterSettings);
      }

      private static void registerPresenter(IContainer container)
      {
         container.AddRegister(x => x.FromType<PresentationRegister>());
      }

      private static void registerView(IContainer container)
      {
         container.AddRegister(x => x.FromType<UserInterfaceRegister>());
      }

      private static void registerSerializationDependencies(IContainer container)
      {
         //create serializer repository for xml persistence and register all available serializer
         var register = new SerializerRegister();
         container.AddRegister(x => x.FromInstance(register));
         register.PerformMappingForSerializerIn(container);
      }

      private void initializeSynchronizationContext()
      {
         var context = new WindowsFormsSynchronizationContext();
         SynchronizationContext.SetSynchronizationContext(context);
      }
   }
}