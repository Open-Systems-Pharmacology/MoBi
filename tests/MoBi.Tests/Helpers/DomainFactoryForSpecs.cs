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
      public static SimulationConfiguration CreateDefaultConfiguration()
      {
         // var buildConfigurationFactory = IoC.Resolve<IBuildConfigurationFactory>();
         var buildConfiguration = new SimulationConfiguration { Module = new Module() };

         buildConfiguration.Module.SpatialStructure = CreateDefaultSpatialStructure();
         buildConfiguration.Module.Reaction = CreateDefaultReactions();
         buildConfiguration.SimulationSettings = CreateDefaultSimulationSettings();
         buildConfiguration.Module.Molecule = CreateDefaultMolecules();
         buildConfiguration.Module.PassiveTransport = CreateDefaultPassiveTransports();
         buildConfiguration.Module.EventGroup = CreateDefaultEventGroups();
         buildConfiguration.Module.Observer = CreateDefaultObservers();

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

      public static IMoBiSimulation CreateSimulationFor(SimulationConfiguration buildConfiguration, string simulationName = "Simulation")
      {
         var createResult = CreateModelFor(buildConfiguration, simulationName);
         var simulationFactory = IoC.Resolve<ISimulationFactory>();
         return simulationFactory.CreateFrom(buildConfiguration, createResult.Model).WithName(simulationName);
      }

      public static CreationResult CreateModelFor(SimulationConfiguration buildConfiguration, string simulationName)
      {
         if (buildConfiguration.Module == null)
            buildConfiguration.Module = new Module();

         if (buildConfiguration.MoleculeStartValues == null)
            buildConfiguration.Module.AddMoleculeStartValueBlock(CreateMoleculeStartValuesFor(buildConfiguration));

         if (buildConfiguration.ParameterStartValues == null)
            buildConfiguration.Module.AddParameterStartValueBlock(CreateParameterStartValuesFor(buildConfiguration));

         var modelCreator = IoC.Resolve<IModelConstructor>();
         return modelCreator.CreateModelFrom(buildConfiguration, simulationName);
      }

      public static ParameterStartValuesBuildingBlock CreateParameterStartValuesFor(SimulationConfiguration buildConfiguration)
      {
         var startValuesCreator = IoC.Resolve<IParameterStartValuesCreator>();
         return new ParameterStartValuesBuildingBlock();
         // return startValuesCreator.CreateFrom(buildConfiguration.MoBiSpatialStructure, buildConfiguration.Molecules);
      }

      public static MoleculeStartValuesBuildingBlock CreateMoleculeStartValuesFor(SimulationConfiguration buildConfiguration)
      {
         var startValuesCreator = IoC.Resolve<IMoleculeStartValuesCreator>();
         return startValuesCreator.CreateFrom(buildConfiguration.SpatialStructure, buildConfiguration.Molecules);
      }

      public static IDimension AmountDimension => DimensionByName(Constants.Dimension.MOLAR_AMOUNT);

      public static IDimension DimensionByName(string dimensionName) => IoC.Resolve<IDimensionFactory>().Dimension(dimensionName);
   }
}