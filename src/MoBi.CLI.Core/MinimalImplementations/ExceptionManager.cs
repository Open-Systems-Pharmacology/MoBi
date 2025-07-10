using System;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace MoBi.CLI.Core.MinimalImplementations
{
   internal class ExceptionManager : ExceptionManagerBase
   {
      public override void LogException(Exception ex)
      {
         Console.WriteLine(ex.FullMessage());
      }
   }
}