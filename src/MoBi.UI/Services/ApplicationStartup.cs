using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Castle.Facilities.TypedFactory;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Serialization;
using MoBi.Presentation.Serialization.Xml;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using MoBi.UI.Diagram;
using MoBi.UI.Settings;
using MoBi.UI.Views;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Services;
using OSPSuite.UI;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.FileLocker;
using CoreRegister = OSPSuite.Core.CoreRegister;
using IApplicationSettings = MoBi.Core.IApplicationSettings;
using IContainer = OSPSuite.Utility.Container.IContainer;

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
         var progressManager = IoC.Resolve<IProgressManager>();
         var container = IoC.Container;
         using (var progress = progressManager.Create())
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
         container.Register<IUserSettings, IPresentationUserSettings, ICoreUserSettings, UserSettings>(LifeStyle.Singleton);
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
         setupDimensionMergings(dimFactory);
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
         var calculationMethodsRepositoryPersistor = container.Resolve<ICalculationMethodsRepositoryPersistor>();
         calculationMethodsRepositoryPersistor.Load();
         //Add Empty CM'S to use in non PK-Sim Models
         var rep = container.Resolve<ICoreCalculationMethodRepository>();
         var objectBaseFactory = container.Resolve<IObjectBaseFactory>();
         rep.GetAllCategoriesDefault().Each(cm => rep.AddCalculationMethod(createDefaultCalculationMethodForCategory(cm.Category, objectBaseFactory)));
      }

      private static ICoreCalculationMethod createDefaultCalculationMethodForCategory(string category, IObjectBaseFactory objectBaseFactory)
      {
         var cm = objectBaseFactory.Create<ICoreCalculationMethod>()
            .WithName(AppConstants.DefaultNames.EmptyCalculationMethod)
            .WithDescription(AppConstants.DefaultNames.EmptyCalculationMethodDescription);

         cm.Category = category;
         return cm;
      }

      private static void setupDimensionMergings(IDimensionFactory factory)
      {
         var concentrationDimension = factory.Dimension(Constants.Dimension.MASS_CONCENTRATION);
         var molarConcentrationDimension = factory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);

         factory.AddMergingInformation(new MoBiDimensionMergingInformation<IQuantity>(concentrationDimension, molarConcentrationDimension,
            new MolWeightDimensionConverterForFormulaUsable(concentrationDimension, molarConcentrationDimension)));

         factory.AddMergingInformation(new MoBiDimensionMergingInformation<DataColumn>(concentrationDimension, molarConcentrationDimension,
            new ConcentrationToMolarConcentrationConverterForDataColumn(concentrationDimension, molarConcentrationDimension)));

         factory.AddMergingInformation(new MoBiDimensionMergingInformation<DataColumn>(molarConcentrationDimension, concentrationDimension,
            new MolarConcentrationToConcentrationConverterForDataColumn(molarConcentrationDimension, concentrationDimension)));
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

         container.RegisterImplementationOf(getCurrentContext());

         container.AddRegister(x => x.FromType<PresenterRegister>());
         container.AddRegister(x => x.FromType<UIRegister>());

         // Global Singleton Objects
         container.Register<IExceptionManager, ExceptionManager>(LifeStyle.Singleton);

         container.RegisterImplementationOf(this);

         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<IOSPSuiteLogger, MoBiLogger>(LifeStyle.Singleton);
         container.Register<ISplashScreen, SplashScreen>();
         container.Register<ISplashScreenPresenter, SplashScreenPresenter>();
         container.Register<IProgressUpdater, ProgressUpdater>();
         container.RegisterFactory<IProgressManager>();

         container.Register<IMoBiConfiguration, IApplicationConfiguration, MoBiConfiguration>(LifeStyle.Singleton);

         registerRunOptionsIn(container);

         EnvironmentHelper.ApplicationName = () => "mobi";

         registrationAction?.Invoke(container);
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
         container.AddRegister(x => x.FromType<InfrastructureRegister>());
      }

      private static void registerImport(IContainer container)
      {
         var mobiDataImporterSettings = new DataImporterSettings {IconName = ApplicationIcons.MoBi.IconName, Caption = "MoBi Data Import"};
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

      private static SynchronizationContext getCurrentContext()
      {
         var context = SynchronizationContext.Current;
         if (context == null)
         {
            context = new WindowsFormsSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(context);
         }

         return SynchronizationContext.Current;
      }
   }
}