using System.Linq;
using FluentNHibernate.Conventions;
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
         var buildConfiguration = new SimulationConfiguration();
         var moduleConfiguration = new ModuleConfiguration(new Module());
         buildConfiguration.AddModuleConfiguration(moduleConfiguration);


         moduleConfiguration.Module.SpatialStructure = CreateDefaultSpatialStructure();
         moduleConfiguration.Module.Reactions = CreateDefaultReactions();
         buildConfiguration.SimulationSettings = CreateDefaultSimulationSettings();
         moduleConfiguration.Module.Molecules = CreateDefaultMolecules();
         moduleConfiguration.Module.PassiveTransports = CreateDefaultPassiveTransports();
         moduleConfiguration.Module.EventGroups = CreateDefaultEventGroups();
         moduleConfiguration.Module.Observers = CreateDefaultObservers();

         return buildConfiguration;
      }

      public static MoBiSpatialStructure CreateDefaultSpatialStructure(string buildingBlockName = "Spatial Structure")
      {
         var spatialStructureFactory = IoC.Resolve<IMoBiSpatialStructureFactory>();
         return spatialStructureFactory.CreateDefault(buildingBlockName);
      }

      public static MoBiReactionBuildingBlock CreateDefaultReactions(string buildingBlockName = "Reactions")
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

      public static PassiveTransportBuildingBlock CreateDefaultPassiveTransports(string buildingBlockName = "PassiveTransports") => createBuildingBlock<PassiveTransportBuildingBlock>(buildingBlockName);

      public static EventGroupBuildingBlock CreateDefaultEventGroups(string buildingBlockName = "EventGroups") => createBuildingBlock<EventGroupBuildingBlock>(buildingBlockName);

      public static ObserverBuildingBlock CreateDefaultObservers(string buildingBlockName = "Observers") => createBuildingBlock<ObserverBuildingBlock>(buildingBlockName);

      private static T createBuildingBlock<T>(string buildingBlockName) where T : IBuildingBlock => IoC.Resolve<T>().WithName(buildingBlockName);

      public static IMoBiSimulation CreateSimulationFor(SimulationConfiguration buildConfiguration, string simulationName = "Simulation")
      {
         var createResult = CreateModelFor(buildConfiguration, simulationName);
         var simulationFactory = IoC.Resolve<ISimulationFactory>();
         return simulationFactory.CreateFrom(buildConfiguration, createResult.Model).WithName(simulationName);
      }

      public static CreationResult CreateModelFor(SimulationConfiguration buildConfiguration, string simulationName)
      {
         if (buildConfiguration.ModuleConfigurations.IsEmpty())
            buildConfiguration.AddModuleConfiguration(new ModuleConfiguration(new Module()));

         var moduleConfiguration = buildConfiguration.ModuleConfigurations.First();
         if (moduleConfiguration.Module.MoleculeStartValuesCollection.IsEmpty())
         {
            moduleConfiguration.Module.AddMoleculeStartValueBlock(CreateMoleculeStartValuesFor(buildConfiguration));
            moduleConfiguration.SelectedMoleculeStartValues = moduleConfiguration.Module.MoleculeStartValuesCollection.First();
         }

         if (moduleConfiguration.Module.ParameterStartValuesCollection.IsEmpty())
         {
            moduleConfiguration.Module.AddParameterStartValueBlock(CreateParameterStartValuesFor(buildConfiguration));
            moduleConfiguration.SelectedParameterStartValues = moduleConfiguration.Module.ParameterStartValuesCollection.First();
         }

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
         return startValuesCreator.CreateFrom(buildConfiguration.All<SpatialStructure>().First(), buildConfiguration.All<MoleculeBuildingBlock>().First());
      }

      public static IDimension AmountDimension => DimensionByName(Constants.Dimension.MOLAR_AMOUNT);

      public static IDimension DimensionByName(string dimensionName) => IoC.Resolve<IDimensionFactory>().Dimension(dimensionName);
   }
}