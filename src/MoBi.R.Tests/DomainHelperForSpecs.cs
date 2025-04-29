using OSPSuite.Core.Domain.UnitSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace MoBi.R.Tests
{
   public static class DomainHelperForSpecs
   {
      private static readonly string PATH_TO_DATA = "..\\..\\..\\Data\\";

      private static Dimension _lengthDimension;
      private static Dimension _concentrationDimension;
      private static Dimension _timeDimension;
      private static Dimension _fractionDimension;

      public static IDimension TimeDimensionForSpecs()
      {
         if (_timeDimension == null)
         {
            _timeDimension = new Dimension(new BaseDimensionRepresentation { TimeExponent = 1 }, Constants.Dimension.TIME, "min");
            _timeDimension.AddUnit(new Unit("h", 60, 0));
         }

         return _timeDimension;
      }
      public static IDimension ConcentrationDimensionForSpecs()
      {
         if (_concentrationDimension == null)
         {
            _concentrationDimension = new Dimension(new BaseDimensionRepresentation { AmountExponent = 3, LengthExponent = -1 }, Constants.Dimension.MOLAR_CONCENTRATION, "µmol/l");
            _concentrationDimension.AddUnit(new Unit("mol/l", 1E6, 0));
         }

         return _concentrationDimension;
      }
      public static DataRepository IndividualSimulationDataRepositoryFor(string simulationName)
      {
         var simulationResults = new DataRepository("Results");
         var baseGrid = new BaseGrid("Time", TimeDimensionForSpecs())
         {
            Values = new[] { 1.0f, 2.0f, 3.0f }
         };
         simulationResults.Add(baseGrid);

         var data = ConcentrationColumnForSimulation(simulationName, baseGrid);

         simulationResults.Add(data);

         return simulationResults;
      }


      public static DataColumn ConcentrationColumnForSimulation(string simulationName, BaseGrid baseGrid)
      {
         var data = new DataColumn("Col", ConcentrationDimensionForSpecs(), baseGrid)
         {
            Values = new[] { 10f, 20f, 30f },
            DataInfo = { Origin = ColumnOrigins.Calculation },
            QuantityInfo = new QuantityInfo(new[] { simulationName, "Comp", "Liver", "Cell", "Concentration" }, QuantityType.Drug)
         };
         return data;
      }
   }
}
