using OSPSuite.Core.Services;

namespace MoBi.R.Services
{
   /// <summary>
   ///    Minimal <see cref="IStartableProcessFactory" /> for the MoBi.R container. The real OSPSuite.Core registration
   ///    relies on Castle Windsor's typed-factory feature, which the Autofac container used by MoBi.R does not synthesize.
   ///    Providing an explicit implementation lets <c>PKSimStarter</c> (and therefore the snapshot mappers) resolve so
   ///    snapshots that contain PK-Sim modules can be re-built in R workflows.
   /// </summary>
   public class StartableProcessFactory : IStartableProcessFactory
   {
      public StartableProcess CreateStartableProcess(string applicationPath, params string[] arguments) =>
         new StartableProcess(applicationPath, arguments);
   }
}
