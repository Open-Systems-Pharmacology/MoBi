using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoBi.Core.Mappers
{
   public static class ColumnIndexes
   {
      public static class InitialConditions
      {
         public const int PATH = 0;
         public const int MOLECULE = 1;
         public const int IS_PRESENT = 2;
         public const int VALUE = 3;
         public const int UNIT = 4;
         public const int SCALE_DIVISOR = 5;
         public const int NEGATIVE_VALUES_ALLOWED = 6;
         //optional dimension column to use
         public const int DIMENSION = 7;
         public const int COLUMNS = 7;
      }

      public static class Parameters
      {
         public const int CONTAINER_PATH = 0;
         public const int NAME = 1;
         public const int VALUE = 2;
         public const int UNIT = 3;
         //optional dimension column
         public const int DIMENSION = 4;
         public const int COLUMNS = 4;
      }
   }
}
