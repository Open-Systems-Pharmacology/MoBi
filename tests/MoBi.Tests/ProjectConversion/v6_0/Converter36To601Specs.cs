using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.ProjectConversion.v6_0
{
   public class When_converting_a_project_from_3_6_to_6_0 : ContextWithLoadedProject
   {
      private IMoBiProject _project;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _project = LoadProject("EB_LV");
      }

      [Observation]
      public void should_be_able_to_load_a_project_with_display_dimension_map_using_a_per_Time_attribute()
      {
         _project.DisplayUnits.AllDisplayUnits.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_converting_a_pkml_from_3_6_to_6_0 : ContextWithLoadedProject
   {
      private SimulationTransfer _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKML<SimulationTransfer>("run", ReactionDimensionMode.ConcentrationBased);
      }

      [Observation]
      public void should_be_able_to_load_a_project_with_display_dimension_map_using_a_per_Time_attribute()
      {
         _simulation.ShouldNotBeNull();
      }
   }
}