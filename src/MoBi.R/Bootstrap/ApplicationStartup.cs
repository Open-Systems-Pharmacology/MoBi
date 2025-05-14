using MoBi.Presentation.Serialization;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.R;
using OSPSuite.Utility.Container;
using CoreRegister = MoBi.Core.CoreRegister;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.R.Bootstrap
{
   internal class ApplicationStartup
   {
      public static void Initialize(ApiConfig apiConfig)
      {
         OSPSuite.R.Api.InitializeOnce(apiConfig, registerAction);

         new SerializerRegister().PerformMappingForMoBiSerializerRepository(OSPSuite.R.Api.Container);
      }

      private static void registerAction(IContainer container)
      {
         container.AddRegister(x => x.FromType<CoreRegister>());
         container.AddRegister(x => x.FromType<InfrastructureRegister>());
         container.AddRegister(x => x.FromType<RRegister>());
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