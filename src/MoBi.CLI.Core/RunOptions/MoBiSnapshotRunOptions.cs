using OSPSuite.CLI.Core.RunOptions;

namespace MoBi.CLI.Core.RunOptions
{
   public class MoBiSnapshotRunOptions : SnapshotRunOptions, IProvidePKSimPath
   {
      public string PKSimPath { get; set; }
   }
}