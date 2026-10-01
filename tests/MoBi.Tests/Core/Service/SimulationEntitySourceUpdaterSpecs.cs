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
using OSPSuite.Utility.Collections;

namespace MoBi.Core.Service
{
   public class concern_for_SimulationEntitySourceUpdater : ContextSpecification<SimulationEntitySourceUpdater>
   {
      private IMoBiProjectRetriever _projectRetriever;
      protected MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = new MoBiProject();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         A.CallTo(() => _projectRetriever.Current).Returns(_project);
         sut = new SimulationEntitySourceUpdater(_projectRetriever);
      }
   }

   public class When_updating_sources_for_changes_in_path_and_value_entities : concern_for_SimulationEntitySourceUpdater
   {
      private IMoBiSimulation _simulation;
      private ObjectPath _objectPath;
      private ParameterValuesBuildingBlock _parameterValuesBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock().WithName("PVBB");
         _simulation = new MoBiSimulation();
         _objectPath = new ObjectPath("Root", "Container", "Parameter");
         _simulation.AddEntitySources(new[]
         {
            new SimulationEntitySource("Root|Container|Parameter", "someBuildingBlock", "someType", null, "originalPath")
         });
      }

      protected override void Because()
      {
         sut.UpdateSourcesForNewPathAndValueEntity(_parameterValuesBuildingBlock, _objectPath, _simulation);
      }

      [Observation]
      public void the_existing_entity_source_is_updated()
      {
         _simulation.EntitySources.First().SourcePath.ShouldBeEqualTo(_objectPath);
         _simulation.EntitySources.First().BuildingBlockName.ShouldBeEqualTo(_parameterValuesBuildingBlock.Name);
         _simulation.EntitySources.First().BuildingBlockType.ShouldBeEqualTo(_parameterValuesBuildingBlock.GetType().Name);
      }
   }

   public class When_renaming_a_container_that_is_referenced_by_a_simulation : concern_for_SimulationEntitySourceUpdater
   {
      private ObjectPath _oldPath;
      private ObjectPath _newPath;
      private IMoBiSimulation _simulation1;
      private IndividualBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _simulation1 = new MoBiSimulation();
         _project.AddSimulation(_simulation1);
         _oldPath = new ObjectPath("container", "originalName");
         _newPath = new ObjectPath("container", "new Container");
         _buildingBlock = new IndividualBuildingBlock().WithName("Individual1");
         _simulation1.Configuration = new SimulationConfiguration { Individual = _buildingBlock };
         _simulation1.AddEntitySources(new List<SimulationEntitySource>
         {
            // this source matches building block name, type and object path
            new SimulationEntitySource("path1", _buildingBlock.Name, _buildingBlock.GetType().Name, "", new ObjectPath("container", "originalName", "subContainer", "entity")),
            // this source does not match the building block type
            new SimulationEntitySource("path2", _buildingBlock.Name, new ParameterValuesBuildingBlock().GetType().Name, "", new ObjectPath("container", "originalName", "subContainer", "entity")),
            // this source does not match the building block name
            new SimulationEntitySource("path3", "this is a different building block", _buildingBlock.GetType().Name, "", new ObjectPath("container", "originalName", "subContainer", "entity"))
         });
      }

      protected override void Because()
      {
         sut.UpdateEntitySourcesForContainerRename(_newPath, _oldPath, _buildingBlock);
      }

      [Observation]
      public void the_entity_source_is_updated_with_a_new_path()
      {
         var simulation1EntitySources = _simulation1.EntitySources.ToArray();

         simulation1EntitySources.Length.ShouldBeEqualTo(3);
         simulation1EntitySources[0].SourcePath.ShouldBeEqualTo(new ObjectPath("container", "new Container", "subContainer", "entity"));
         simulation1EntitySources[1].SourcePath.ShouldBeEqualTo(new ObjectPath("container", "originalName", "subContainer", "entity"));
         simulation1EntitySources[2].SourcePath.ShouldBeEqualTo(new ObjectPath("container", "originalName", "subContainer", "entity"));
      }
   }

   public class When_renaming_an_entity_that_is_referenced_by_a_simulation : concern_for_SimulationEntitySourceUpdater
   {
      private ObjectPath _oldPath;
      private ObjectPath _newPath;
      private IMoBiSimulation _simulation1;
      private IndividualBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _simulation1 = new MoBiSimulation();
         _project.AddSimulation(_simulation1);
         _oldPath = new ObjectPath("container", "originalName", "an entity");
         _newPath = new ObjectPath("container", "originalName", "renamed entity");
         _buildingBlock = new IndividualBuildingBlock().WithName("Individual1");
         _simulation1.Configuration = new SimulationConfiguration { Individual = _buildingBlock };
         _simulation1.AddEntitySources(new List<SimulationEntitySource>
         {
            // this source matches building block name, type and object path
            new SimulationEntitySource("path1", _buildingBlock.Name, _buildingBlock.GetType().Name, "", _oldPath),
            // this source does not match the building block name
            new SimulationEntitySource("path2", "this is a different building block", _buildingBlock.GetType().Name, "", _oldPath)
         });
      }

      protected override void Because()
      {
         sut.UpdateEntitySourcesForEntityRename(_newPath, _oldPath, _buildingBlock);
      }

      [Observation]
      public void the_entity_source_is_updated_with_a_new_path()
      {
         var simulation1EntitySources = _simulation1.EntitySources.ToArray();
         simulation1EntitySources.Length.ShouldBeEqualTo(2);
         simulation1EntitySources[0].SourcePath.ShouldBeEqualTo(_newPath);
         simulation1EntitySources[1].SourcePath.ShouldBeEqualTo(_oldPath);
      }
   }

   public class When_updating_sources_for_chained_module_renames : concern_for_SimulationEntitySourceUpdater
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         _simulation.AddEntitySources(new[]
         {
            new SimulationEntitySource("path1", "bb", "atype", "Sim", "sourcePath1"),
            new SimulationEntitySource("path2", "bb", "atype", "Sim 1", "sourcePath2"),
            new SimulationEntitySource("path3", "bb", "atype", "other", "sourcePath3"),
            new SimulationEntitySource("path4", "bb", "atype", null, "sourcePath4")
         });
      }

      protected override void Because()
      {
         sut.UpdateEntitySourcesForModuleRenames(new Cache<string, string> { { "Sim", "Sim 1" }, { "Sim 1", "Sim 1 1" } }, _simulation);
      }

      [Observation]
      public void each_source_should_reference_the_new_name_of_its_own_module()
      {
         _simulation.EntitySources.SourceByPath("path1").ModuleName.ShouldBeEqualTo("Sim 1");
         _simulation.EntitySources.SourceByPath("path2").ModuleName.ShouldBeEqualTo("Sim 1 1");
      }

      [Observation]
      public void sources_of_other_modules_should_not_change()
      {
         _simulation.EntitySources.SourceByPath("path3").ModuleName.ShouldBeEqualTo("other");
         _simulation.EntitySources.SourceByPath("path4").ModuleName.ShouldBeNull();
      }
   }
}