using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.ProjectConversion.v3_2
{
   public abstract class concern_for_OutputSchemaConversion : ContextWithLoadedProject
   {
      protected IModelCoreSimulation _simulation;
      protected List<OutputInterval> _intervals;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKML<SimulationTransfer>("warnings").Simulation;
      }
   }

   public class When_converting_the_output_schema_in_the_project_warning : concern_for_OutputSchemaConversion
   {
      [Observation]
      public void should_have_converted_the_existing_intervals()
      {
         _intervals = _simulation.BuildConfiguration.SimulationSettings.OutputSchema.Intervals.ToList();
         _intervals.Count.ShouldBeEqualTo(1);
         var interval = _intervals[0];
         interval.GetSingleChildByName<IParameter>(Constants.Parameters.START_TIME).Value.ShouldBeEqualTo(0);
         interval.GetSingleChildByName<IParameter>(Constants.Parameters.END_TIME).Value.ShouldBeEqualTo(1440);
         interval.GetSingleChildByName<IParameter>(Constants.Parameters.RESOLUTION).Value.ShouldBeEqualTo(4.0 / 60.0);
      }
   }
}