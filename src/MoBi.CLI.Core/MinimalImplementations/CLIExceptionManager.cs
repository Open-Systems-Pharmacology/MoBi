using OSPSuite.Utility.Exceptions;
using System;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class CLIExceptionManager : ExceptionManagerBase
   {
      public override void LogException(Exception ex)
      {
         throw ex;
      }
   }
}
