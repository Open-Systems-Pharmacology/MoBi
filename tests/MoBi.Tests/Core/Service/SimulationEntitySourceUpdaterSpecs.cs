using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper.Extensions;

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
}