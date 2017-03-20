using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.ProjectConversion.v3_5
{
   public class When_converting_the_simple_project_341 : ContextWithLoadedProject
   {
      private IMoBiProject _project;
      private IMoBiSimulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _project = LoadProject("SimpleProject_341");
         _simulation = _project.Simulations.First();
      }

      [Observation]
      public void should_have_added_a_simulation_settings_building_block_to_the_template()
      {
         _project.SimulationSettingsCollection.Any().ShouldBeTrue();
      }

      [Observation]
      public void should_have_ensured_that_the_simulation_is_referencing_the_simulation_settings_template()
      {
         _simulation.MoBiBuildConfiguration.SimulationSettingsInfo.TemplateBuildingBlock
            .ShouldBeEqualTo(_project.SimulationSettingsCollection.First());
      }

      [Observation]
      public void should_have_set_the_project_mode_to_amount()
      {
         _project.ReactionDimensionMode.ShouldBeEqualTo(ReactionDimensionMode.AmountBased);
      }

      [Observation]
      public void should_have_added_the_volume_parameter_to_all_physical_containers()
      {
         foreach (var container in _project.SpatialStructureCollection.SelectMany(x => x.PhysicalContainers))
         {
            container.ContainsName(Constants.Parameters.VOLUME).ShouldBeTrue();
         }
      }

      [Observation]
      public void should_have_added_the_concentration_parameter_to_all_molecule_builder()
      {
         foreach (var moleculeBuilder in _project.MoleculeBlockCollection.SelectMany(x => x))
         {
            moleculeBuilder.ContainsName(Constants.Parameters.CONCENTRATION).ShouldBeTrue();
         }
      }
   }
}