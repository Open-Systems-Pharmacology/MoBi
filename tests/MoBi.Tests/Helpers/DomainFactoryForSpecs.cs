﻿using System.Linq;
using FluentNHibernate.Conventions;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Helpers
{
   public static class DomainFactoryForSpecs
   {
      public static SimulationConfiguration CreateDefaultConfiguration()
      {
         var buildConfiguration = new SimulationConfiguration();

         var module = new Module
         {
            CreateDefaultSpatialStructureWithTopContainer(),
            CreateDefaultReactions(),
            CreateDefaultSimulationSettings(),
            CreateDefaultMolecules(),
            CreateDefaultPassiveTransports(),
            CreateDefaultEventGroups(),
            CreateDefaultObservers(),
         };

         var moduleConfiguration = new ModuleConfiguration(module);
         buildConfiguration.SimulationSettings = CreateDefaultSimulationSettings();
         buildConfiguration.AddModuleConfiguration(moduleConfiguration);

         return buildConfiguration;
      }

      public static MoBiSpatialStructure CreateDefaultSpatialStructureWithTopContainer(string buildingBlockName = "Spatial Structure")
      {
         var spatialStructureFactory = IoC.Resolve<IMoBiSpatialStructureFactory>();
         var objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
         var parameterFactory = IoC.Resolve<IParameterFactory>();

         var topContainer = objectBaseFactory.Create<IContainer>()
            .WithName(buildingBlockName)
            .WithMode(ContainerMode.Physical)
            .WithContainerType(ContainerType.Organism);
         
         topContainer.AddChildren(parameterFactory.CreateVolumeParameter());
         
         var moleculeProperties = objectBaseFactory.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Other)
            .WithParentContainer(topContainer);


         var moBiSpatialStructure = spatialStructureFactory.CreateDefault(buildingBlockName);
         moBiSpatialStructure.DiagramManager.AddObjectBase(topContainer);
         moBiSpatialStructure.DiagramManager.AddObjectBase(moleculeProperties);

         return moBiSpatialStructure.WithTopContainer(topContainer);
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
         var module = moduleConfiguration.Module;
         if (module.InitialConditionsCollection.IsEmpty())
         {
            module.Add(CreateInitialConditionsFor(buildConfiguration));
            moduleConfiguration.SelectedInitialConditions = module.InitialConditionsCollection.First();
         }

         if (module.ParameterValuesCollection.IsEmpty())
         {
            module.Add(CreateParameterValuesFor(buildConfiguration));
            moduleConfiguration.SelectedParameterValues = module.ParameterValuesCollection.First();
         }

         var modelCreator = IoC.Resolve<IModelConstructor>();
         return modelCreator.CreateModelFrom(buildConfiguration, simulationName);
      }

      public static ParameterValuesBuildingBlock CreateParameterValuesFor(SimulationConfiguration buildConfiguration)
      {
         var startValuesCreator = IoC.Resolve<IParameterValuesCreator>();
         return new ParameterValuesBuildingBlock();
         // return startValuesCreator.CreateFrom(buildConfiguration.MoBiSpatialStructure, buildConfiguration.Molecules);
      }

      public static InitialConditionsBuildingBlock CreateInitialConditionsFor(SimulationConfiguration buildConfiguration)
      {
         var startValuesCreator = IoC.Resolve<IInitialConditionsCreator>();
         return startValuesCreator.CreateFrom(buildConfiguration.All<SpatialStructure>().First(), buildConfiguration.All<MoleculeBuildingBlock>().First().ToList());
      }

      public static IDimension AmountDimension => DimensionByName(Constants.Dimension.MOLAR_AMOUNT);

      public static IDimension DimensionByName(string dimensionName) => IoC.Resolve<IDimensionFactory>().Dimension(dimensionName);
   }
}