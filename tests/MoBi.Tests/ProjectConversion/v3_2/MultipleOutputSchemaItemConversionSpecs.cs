using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;

namespace MoBi.ProjectConversion.v3_2
{
   public abstract class concern_for_MultipleOutputSchemaItemConversion : ContextWithLoadedProject
   {
      protected IModelCoreSimulation _simulation;
      protected List<OutputInterval> _intervals;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadProject("ManualModel_Sim").Simulations.First();
      }
   }

   public class When_converting_the_output_schema_in_the_project_containing_multiple_intervals : concern_for_MultipleOutputSchemaItemConversion
   {
      [Observation]
      public void should_have_converted_the_existing_intervals()
      {
         _intervals = _simulation.BuildConfiguration.SimulationSettings.OutputSchema.Intervals.ToList();
         _intervals.Count.ShouldBeEqualTo(2);
      }
   }
}