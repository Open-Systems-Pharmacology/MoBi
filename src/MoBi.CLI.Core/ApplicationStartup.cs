using Castle.Facilities.TypedFactory;
using MoBi.Assets;
using MoBi.CLI.Core.MinimalImplementations;
using MoBi.Core;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Presentation.Serialization;
using MoBi.Presentation.Serialization.Xml.Serializer;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.FileLocker;
using OSPSuite.Utility.Format;
using System.Threading;
using OSPSuite.Core.Domain.Data;
using CoreRegister = MoBi.Core.CoreRegister;
using IContainer = OSPSuite.Utility.Container.IContainer;
using ICoreUserSettings = MoBi.Core.ICoreUserSettings;

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
         container.Register<ICoreUserSettings, UserSettings>();
         container.Register<ISpatialStructureDiagramManager, SpatialStructureDiagramManager>();
         container.Register<IMoBiReactionDiagramManager, MoBiReactionDiagramManager>();
         container.Register<ISimulationDiagramManager, SimulationDiagramManager>();

         register.PerformMappingForSerializerIn(container);
         initializeDimensions(container);
         InitCalculationMethodRepository(container);
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
         setupDimensionMerging(dimensionFactory);

         // var molarConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         // var massConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_CONCENTRATION);
         // var amountDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         // var massDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_AMOUNT);
         // var aucMolarDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AUC);
         // var aucMassDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_AUC);
         //
         // dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(molarConcentrationDimension, massConcentrationDimension));
         // dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(massConcentrationDimension, molarConcentrationDimension));
         // dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(amountDimension, massDimension));
         // dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(massDimension, amountDimension));
         // dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(aucMolarDimension, aucMassDimension));
         // dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(aucMassDimension, aucMolarDimension));
      }

      private static void setupDimensionMerging(IDimensionFactory factory)
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
   }
}