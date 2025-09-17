using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Service
{
   public abstract class concern_for_RenameInSimulationTask : ContextSpecification<RenameInSimulationTask>
   {
      private IMoBiProjectRetriever _projectRetriever;
      protected MoBiProject _project;
      private ISimulationEntitySourceUpdater _simulationEntitySourceUpdater;

      protected override void Context()
      {
         _project = new MoBiProject();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _simulationEntitySourceUpdater = new SimulationEntitySourceUpdater(_projectRetriever);
         A.CallTo(() => _projectRetriever.Current).Returns(_project);
         sut = new RenameInSimulationTask(_projectRetriever, _simulationEntitySourceUpdater);
      }
   }

   public class When_renaming_building_blocks_that_were_used_in_multiple_simulation : concern_for_RenameInSimulationTask
   {
      private MoBiSimulation _simulation1;
      private MoBiSimulation _simulation2;
      private SpatialStructure _templateBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _simulation1 = new MoBiSimulation();
         _simulation2 = new MoBiSimulation();
         var simulation1Configuration = new SimulationConfiguration();
         var simulation2Configuration = new SimulationConfiguration();

         simulation1Configuration.AddModuleConfiguration(createModuleConfiguration());
         simulation2Configuration.AddModuleConfiguration(createModuleConfiguration());

         _simulation1.Configuration = simulation1Configuration;
         _simulation2.Configuration = simulation2Configuration;

         _project.AddSimulation(_simulation1);
         _project.AddSimulation(_simulation2);
         _templateBuildingBlock = new SpatialStructure().WithName("SpatialStructure");
         _templateBuildingBlock.Module = new Module().WithName("moduleName");

         _simulation1.AddEntitySources(new List<SimulationEntitySource> { new SimulationEntitySource("", "buildingBlock", nameof(SpatialStructure), "moduleName", "") });
      }

      private static ModuleConfiguration createModuleConfiguration()
      {
         var configuration = new ModuleConfiguration(new Module
         {
            Name = "moduleName",
         });

         configuration.Module.Add(new SpatialStructure().WithName("buildingBlock"));
         configuration.Module.Add(new EventGroupBuildingBlock().WithName("buildingBlock"));
         return configuration;
      }

      protected override void Because()
      {
         sut.RenameInSimulationUsingTemplateBuildingBlock("buildingBlock", _templateBuildingBlock);
      }

      [Observation]
      public void the_entity_source_is_renamed()
      {
         _simulation1.EntitySources.First().BuildingBlockName.ShouldBeEqualTo("SpatialStructure");
      }

      [Observation]
      public void like_named_building_blocks_of_another_type_are_not_renamed()
      {
         _simulation1.Configuration.ModuleConfigurations.Single().Module.EventGroups.Name.ShouldBeEqualTo("buildingBlock");
         _simulation2.Configuration.ModuleConfigurations.Single().Module.EventGroups.Name.ShouldBeEqualTo("buildingBlock");
      }

      [Observation]
      public void the_correct_building_blocks_are_renamed_in_all_simulations()
      {
         _simulation1.Configuration.ModuleConfigurations.Single().Module.SpatialStructure.Name.ShouldBeEqualTo("SpatialStructure");
         _simulation2.Configuration.ModuleConfigurations.Single().Module.SpatialStructure.Name.ShouldBeEqualTo("SpatialStructure");
      }
   }

   public class When_renaming_individual_that_was_used_in_multiple_simulation : concern_for_RenameInSimulationTask
   {
      private MoBiSimulation _simulation1;
      private MoBiSimulation _simulation2;
      private IndividualBuildingBlock _templateBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _simulation1 = new MoBiSimulation();
         _simulation2 = new MoBiSimulation();
         var simulation1Configuration = new SimulationConfiguration();
         var simulation2Configuration = new SimulationConfiguration();

         simulation1Configuration.AddModuleConfiguration(createModuleConfiguration());
         simulation2Configuration.AddModuleConfiguration(createModuleConfiguration());
         simulation1Configuration.Individual = new IndividualBuildingBlock().WithName("buildingBlock");
         simulation2Configuration.Individual = new IndividualBuildingBlock().WithName("buildingBlock");

         _simulation1.Configuration = simulation1Configuration;
         _simulation2.Configuration = simulation2Configuration;

         _project.AddSimulation(_simulation1);
         _project.AddSimulation(_simulation2);
         _templateBuildingBlock = new IndividualBuildingBlock().WithName("Individual");

         _simulation1.AddEntitySources(new List<SimulationEntitySource> { new SimulationEntitySource("", "buildingBlock", simulation1Configuration.Individual.GetType().Name, null, "") });
      }

      private static ModuleConfiguration createModuleConfiguration()
      {
         var configuration = new ModuleConfiguration(new Module
         {
            Name = "moduleName",
         });

         configuration.Module.Add(new SpatialStructure().WithName("buildingBlock"));
         configuration.Module.Add(new EventGroupBuildingBlock().WithName("buildingBlock"));
         return configuration;
      }

      protected override void Because()
      {
         sut.RenameInSimulationUsingTemplateBuildingBlock("buildingBlock", _templateBuildingBlock);
      }

      [Observation]
      public void like_named_building_blocks_of_another_type_are_not_renamed()
      {
         _simulation1.Configuration.ModuleConfigurations.Single().Module.EventGroups.Name.ShouldBeEqualTo("buildingBlock");
         _simulation2.Configuration.ModuleConfigurations.Single().Module.EventGroups.Name.ShouldBeEqualTo("buildingBlock");
         _simulation1.Configuration.ModuleConfigurations.Single().Module.SpatialStructure.Name.ShouldBeEqualTo("buildingBlock");
         _simulation2.Configuration.ModuleConfigurations.Single().Module.SpatialStructure.Name.ShouldBeEqualTo("buildingBlock");
      }

      [Observation]
      public void the_correct_building_blocks_are_renamed_in_all_simulations()
      {
         _simulation1.Configuration.Individual.Name.ShouldBeEqualTo("Individual");
         _simulation2.Configuration.Individual.Name.ShouldBeEqualTo("Individual");
      }

      [Observation]
      public void the_entity_source_is_renamed()
      {
         _simulation1.EntitySources.First().BuildingBlockName.ShouldBeEqualTo("Individual");
      }
   }

   public class When_renaming_a_module_that_was_used_in_multiple_simulation : concern_for_RenameInSimulationTask
   {
      private Module _templateModule;
      private MoBiSimulation _simulation1;
      private MoBiSimulation _simulation2;

      protected override void Context()
      {
         base.Context();

         _simulation1 = new MoBiSimulation();
         _simulation2 = new MoBiSimulation();
         var simulation1Configuration = new SimulationConfiguration();
         var simulation2Configuration = new SimulationConfiguration();

         simulation1Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module
         {
            Name = "oldName"
         }));
         simulation2Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module
         {
            Name = "oldName"
         }));
         _simulation1.Configuration = simulation1Configuration;
         _simulation2.Configuration = simulation2Configuration;

         _project.AddSimulation(_simulation1);
         _project.AddSimulation(_simulation2);
         _templateModule = new Module { Name = "newName" };

         var entitySources = new List<SimulationEntitySource>
         {
            new SimulationEntitySource("", "bb", "atype", "oldName", "")
         };
         _simulation1.AddEntitySources(entitySources);
      }

      protected override void Because()
      {
         sut.RenameInSimulationUsingTemplateModule("oldName", _templateModule);
      }

      [Observation]
      public void the_entity_source_is_renamed()
      {
         _simulation1.EntitySources.First().ModuleName.ShouldBeEqualTo("newName");
      }

      [Observation]
      public void the_simulations_are_marked_as_having_changed()
      {
         _simulation1.HasChanged.ShouldBeTrue();
         _simulation2.HasChanged.ShouldBeTrue();
      }

      [Observation]
      public void the_module_should_be_renamed_in_all_simulations()
      {
         _simulation1.Configuration.ModuleConfigurations.Single().Module.Name.ShouldBeEqualTo("newName");
         _simulation2.Configuration.ModuleConfigurations.Single().Module.Name.ShouldBeEqualTo("newName");
      }
   }
}