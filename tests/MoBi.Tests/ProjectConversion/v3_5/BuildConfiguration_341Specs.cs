using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.ProjectConversion.v3_5
{
   public class When_loading_a_build_configuration_from_a_523_project : ContextWithLoadedProject
   {
      private BuildConfiguration _buildConfiguration;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _buildConfiguration = LoadPKML<MoBiBuildConfiguration>("523_Export_S1");
      }

      [Observation]
      public void should_have_defined_a_default_simulation_setting()
      {
         _buildConfiguration.SimulationSettings.ShouldNotBeNull();
      }
   }
}