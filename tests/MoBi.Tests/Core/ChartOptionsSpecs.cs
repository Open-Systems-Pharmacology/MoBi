using System.Drawing;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Core
{
   public abstract class concern_for_ChartOptions : ContextSpecification<ChartOptions>
   {
      protected override void Context()
      {
         sut = new ChartOptions();
      }
   }

   public class When_cloning_chart_options : concern_for_ChartOptions
   {
      private ChartOptions _clone;

      protected override void Context()
      {
         base.Context();
         sut.SimulationInCurveName = true;
         sut.TopContainerInCurveName = true;
         sut.DefaultLayoutName = "TestLayout";
         sut.DefaultChartYScaling = Scalings.Linear;
         sut.DefaultChartBackColor = Color.Red;
         sut.DefaultChartDiagramBackColor = Color.Blue;
         sut.ColorGroupObservedDataFromSameFolder = true;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_create_a_clone_with_all_properties_matching_the_original()
      {
         _clone.SimulationInCurveName.ShouldBeEqualTo(sut.SimulationInCurveName);
         _clone.TopContainerInCurveName.ShouldBeEqualTo(sut.TopContainerInCurveName);
         _clone.DefaultLayoutName.ShouldBeEqualTo(sut.DefaultLayoutName);
         _clone.DefaultChartYScaling.ShouldBeEqualTo(sut.DefaultChartYScaling);
         _clone.DefaultChartBackColor.ShouldBeEqualTo(sut.DefaultChartBackColor);
         _clone.DefaultChartDiagramBackColor.ShouldBeEqualTo(sut.DefaultChartDiagramBackColor);
         _clone.ColorGroupObservedDataFromSameFolder.ShouldBeEqualTo(sut.ColorGroupObservedDataFromSameFolder);
      }

      [Observation]
      public void should_not_return_the_same_instance()
      {
         _clone.ShouldNotBeEqualTo(sut);
      }
   }
}
