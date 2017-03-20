using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;

namespace MoBi.ProjectConversion.v3_5
{
   public class When_converting_the_concern_for_ModelCoreSimulation3_0_simulation : ContextWithLoadedProject
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = LoadPKML<IMoBiSimulation>("ModelCoreSimulation3_0");
      }

      [Observation]
      public void should_have_a_simulation_with_a_simulation_settings()
      {
         _simulation.BuildConfiguration.SimulationSettings.ShouldNotBeNull();
      }
   }
}