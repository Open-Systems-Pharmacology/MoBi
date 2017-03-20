using OSPSuite.Utility.Exceptions;

namespace MoBi.Core.Domain.Model
{
   public class InvalidBuildConfigurationException : OSPSuiteException
   {
      public InvalidBuildConfigurationException(string message) : base(message)
      {
      }
   }
}