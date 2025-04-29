using OSPSuite.Core.Domain;

namespace MoBi.R.Domain
{
   public class SimulationRunOptions : RunOptions
   {
      /// <summary>
      ///    Specifies whether negative values check is on or off. Default is <c>true</c>
      /// </summary>
      public bool CheckForNegativeValues { get; set; } = true;

      /// <summary>
      ///    Specifies whether progress bar should be shown during simulation run. Default is <c>true</c>
      /// </summary>
      public bool ShowProgress { get; set; } = true;
   }
}