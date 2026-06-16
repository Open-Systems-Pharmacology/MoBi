using System.Drawing;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Model.Diagram
{
   public class ChartOptions
   {
      public bool SimulationInCurveName { get; set; }
      public bool TopContainerInCurveName { get; set; }
      public string DefaultLayoutName { get; set; }
      public Scalings DefaultChartYScaling { get; set; }
      public Color DefaultChartBackColor { get; set; }
      public Color DefaultChartDiagramBackColor { get; set; }
      public bool ColorGroupObservedDataFromSameFolder { get; set; }
      public ChartOptions()
      {
         SimulationInCurveName = false;
         TopContainerInCurveName = false;
         DefaultLayoutName = Constants.DEFAULT_CHART_LAYOUT;
         DefaultChartYScaling = Scalings.Log;
         DefaultChartBackColor = Color.White;
         DefaultChartDiagramBackColor = Color.White;
         ColorGroupObservedDataFromSameFolder = false;
      }

      public ChartOptions Clone()
      {
         var clone = new ChartOptions();
         clone.UpdatePropertiesFrom(this);
         return clone;
      }

      public void UpdatePropertiesFrom(ChartOptions source)
      {
         SimulationInCurveName = source.SimulationInCurveName;
         TopContainerInCurveName = source.TopContainerInCurveName;
         DefaultLayoutName = source.DefaultLayoutName;
         DefaultChartYScaling = source.DefaultChartYScaling;
         DefaultChartBackColor = source.DefaultChartBackColor;
         DefaultChartDiagramBackColor = source.DefaultChartDiagramBackColor;
         ColorGroupObservedDataFromSameFolder = source.ColorGroupObservedDataFromSameFolder;
      }
   }
}