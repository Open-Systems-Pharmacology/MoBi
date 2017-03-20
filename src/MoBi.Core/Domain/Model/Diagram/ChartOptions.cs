using System.Drawing;
using MoBi.Assets;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Model.Diagram
{

   public class ChartOptions
   {
      public ChartOptions()
      {
         SimulationInCurveName = false;
         TopContainerInCurveName = false;
         DimensionInCurveName = false;
         DefaultLayoutName = AppConstants.DefaultChartLayoutName;
         DefaultChartYScaling = Scalings.Log;
         DefaultChartBackColor = Color.White;
         DefaultChartDiagramBackColor = Color.White;
      }

      public bool SimulationInCurveName { get; set; }
      public bool TopContainerInCurveName { get; set; }
      public bool DimensionInCurveName { get; set; }
      public string DefaultLayoutName { get; set; }
      public Scalings DefaultChartYScaling { get; set; }
      public Color DefaultChartBackColor { get; set; }
      public Color DefaultChartDiagramBackColor { get; set; }
   }

}