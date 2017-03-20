using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.IntegrationTests;
using OSPSuite.Core.Chart;

namespace MoBi.ProjectConversion.v3_3
{
   public abstract class concern_for_ChartConversion : ContextWithLoadedProject
   {
      protected CurveChart _chart;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _chart = LoadPKML<CurveChart>("Chart");
      }
   }

   internal class When_converting_ab_chart_element : concern_for_ChartConversion
   {
      [Observation]
      public void should_have_loaded_the_chart()
      {
         _chart.ShouldNotBeNull();
      }
   }
}