using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Core.Serialization.ORM
{
   public class ConversionException : OSPSuiteException
   {
      public ConversionException(string projectName, IEnumerable<string> errorMessages)
         : base(AppConstants.ProjectUpdateMessages.UpdateErrors(projectName, errorMessages))
      {
      }

      public ConversionException(IEnumerable<string> errorMessages) : base(AppConstants.ProjectUpdateMessages.UpdateErrors(errorMessages))
      {
      }
   }
}