using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using System;

namespace MoBi.R.MinimalImplementations
{
   internal class ExceptionManager : ExceptionManagerBase
   {
      public override void LogException(Exception ex)
      {
         Console.WriteLine(ex.FullMessage());
      }
   }
}
