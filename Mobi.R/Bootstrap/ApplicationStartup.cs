using Castle.Facilities.TypedFactory;
using MoBi.Presentation.Serialization;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Utility.Container;
using CoreRegister = MoBi.Core.CoreRegister;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.R.Bootstrap
{
   internal class ApplicationStartup
   {
      private static IContainer _container;

      public static IContainer Initialize(ApiConfig apiConfig)
      {
         if (_container != null)
            return _container;

         _container = new ApplicationStartup().performInitialization(apiConfig);
         return _container;
      }

      private IContainer performInitialization(ApiConfig apiConfig)
      {
         var container = new CastleWindsorContainer();
         container.WindsorContainer.AddFacility<TypedFactoryFacility>();
         container.RegisterImplementationOf((IContainer)container);
         registerDependencies(container);

         initializeConfiguration(container, apiConfig);
         initializeDimensions(container);

         return container;
      }

      private static void registerDependencies(CastleWindsorContainer container)
      {
         container.AddRegister(x => x.FromType<CoreRegister>());
         container.AddRegister(x => x.FromType<RRegister>());
         container.AddRegister(x => x.FromType<OSPSuite.Core.CoreRegister>());
         container.AddRegister(x => x.FromType<InfrastructureRegister>());

         var serializerRegister = new SerializerRegister();
         container.AddRegister(x => x.FromInstance(serializerRegister));
         serializerRegister.PerformMappingForSerializerIn(container);
      }

      private void initializeConfiguration(IContainer container, ApiConfig apiConfig)
      {
         var applicationConfiguration = container.Resolve<IApplicationConfiguration>();
         applicationConfiguration.PKParametersFilePath = apiConfig.PKParametersFilePath;
         applicationConfiguration.DimensionFilePath = apiConfig.DimensionFilePath;
      }

      private static void initializeDimensions(IContainer container)
      {
         var applicationConfiguration = container.Resolve<IApplicationConfiguration>();
         var dimensionFactory = container.Resolve<IDimensionFactory>();
         var persistor = container.Resolve<IDimensionFactoryPersistor>();
         persistor.Load(dimensionFactory, applicationConfiguration.DimensionFilePath);
         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);

         var molarConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         var massConcentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_CONCENTRATION);
         var amountDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         var massDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_AMOUNT);
         var aucMolarDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AUC);
         var aucMassDimension = dimensionFactory.Dimension(Constants.Dimension.MASS_AUC);

         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(molarConcentrationDimension, massConcentrationDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(massConcentrationDimension, molarConcentrationDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(amountDimension, massDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(massDimension, amountDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(aucMolarDimension, aucMassDimension));
         dimensionFactory.AddMergingInformation(new SimpleDimensionMergingInformation(aucMassDimension, aucMolarDimension));
      }
   }
}