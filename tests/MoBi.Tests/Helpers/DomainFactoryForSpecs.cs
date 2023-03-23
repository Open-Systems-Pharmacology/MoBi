using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Container;

namespace MoBi.Helpers
{
   public static class DomainFactoryForSpecs
   {
      public static IMoBiBuildConfiguration CreateDefaultConfiguration()
      {
         var buildConfigurationFactory = IoC.Resolve<IBuildConfigurationFactory>();
         var buildConfiguration = buildConfigurationFactory.Create();

         buildConfiguration.SpatialStructure = CreateDefaultSpatialStructure();
         buildConfiguration.Reactions = CreateDefaultReactions();
         buildConfiguration.SimulationSettings = CreateDefaultSimulationSettings();
         buildConfiguration.Molecules = CreateDefaultMolecules();
         buildConfiguration.PassiveTransports = CreateDefaultPassiveTransports();
         buildConfiguration.EventGroups = CreateDefaultEventGroups();
         buildConfiguration.Observers = CreateDefaultObservers();

         return buildConfiguration;
      }

      public static IMoBiSpatialStructure CreateDefaultSpatialStructure(string buildingBlockName = "Spatial Structure")
      {
         var spatialStructrureFactory = IoC.Resolve<IMoBiSpatialStructureFactory>();
         return spatialStructrureFactory.CreateDefault(buildingBlockName);
      }

      public static IMoBiReactionBuildingBlock CreateDefaultReactions(string buildingBlockName = "Reactions")
      {
         var reactionBuildingBlockFactory = IoC.Resolve<IReactionBuildingBlockFactory>();
         return reactionBuildingBlockFactory.Create().WithName(buildingBlockName);
      }

      public static SimulationSettings CreateDefaultSimulationSettings(string buildingBlockName = "Reactions")
      {
         var simulationSettingsFactory = IoC.Resolve<ISimulationSettingsFactory>();
         return simulationSettingsFactory.CreateDefault().WithName(buildingBlockName);
      }

      public static MoleculeBuildingBlock CreateDefaultMolecules(string buildingBlockName = "Molecules") => createBuildingBlock<MoleculeBuildingBlock>(buildingBlockName);

      public static IPassiveTransportBuildingBlock CreateDefaultPassiveTransports(string buildingBlockName = "PassiveTransports") => createBuildingBlock<IPassiveTransportBuildingBlock>(buildingBlockName);

      public static IEventGroupBuildingBlock CreateDefaultEventGroups(string buildingBlockName = "EventGroups") => createBuildingBlock<IEventGroupBuildingBlock>(buildingBlockName);

      public static IObserverBuildingBlock CreateDefaultObservers(string buildingBlockName = "OBservers") => createBuildingBlock<IObserverBuildingBlock>(buildingBlockName);

      private static T createBuildingBlock<T>(string buildingBlockName) where T : IBuildingBlock => IoC.Resolve<T>().WithName(buildingBlockName);

      public static IMoBiSimulation CreateSimulationFor(IMoBiBuildConfiguration buildConfiguration, string simulationName = "Simulation")
      {
         var createResult = CreateModelFor(buildConfiguration, simulationName);
         var simulationFactory = IoC.Resolve<ISimulationFactory>();
         return simulationFactory.CreateFrom(buildConfiguration, createResult.Model).WithName(simulationName);
      }

      public static CreationResult CreateModelFor(IMoBiBuildConfiguration buildConfiguration, string simulationName)
      {
         if (buildConfiguration.MoleculeStartValues == null)
            buildConfiguration.MoleculeStartValues = CreateMoleculeStartValuesFor(buildConfiguration);

         if (buildConfiguration.ParameterStartValues == null)
            buildConfiguration.ParameterStartValues = CreateParameterStartValuesFor(buildConfiguration);

         var modelCreator = IoC.Resolve<IModelConstructor>();
         return modelCreator.CreateModelFrom(buildConfiguration, simulationName);
      }

      public static ParameterStartValuesBuildingBlock CreateParameterStartValuesFor(IMoBiBuildConfiguration buildConfiguration)
      {
         var startValuesCreator = IoC.Resolve<IParameterStartValuesCreator>();
         return startValuesCreator.CreateFrom(buildConfiguration.MoBiSpatialStructure, buildConfiguration.Molecules);
      }

      public static MoleculeStartValuesBuildingBlock CreateMoleculeStartValuesFor(IMoBiBuildConfiguration buildConfiguration)
      {
         var startValuesCreator = IoC.Resolve<IMoleculeStartValuesCreator>();
         return startValuesCreator.CreateFrom(buildConfiguration.MoBiSpatialStructure, buildConfiguration.Molecules);
      }

      public static IDimension AmountDimension => DimensionByName(Constants.Dimension.MOLAR_AMOUNT);

      public static IDimension DimensionByName(string dimensionName) => IoC.Resolve<IDimensionFactory>().Dimension(dimensionName);
   }
}