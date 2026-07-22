using OSPSuite.CLI.Core.RunOptions;

namespace MoBi.CLI.Core.RunOptions
{
   public class MoBiQualificationRunOptions : QualificationRunOptions, IProvidePKSimPath
   {
      public string PKSimPath { get; set; }
   }
}
