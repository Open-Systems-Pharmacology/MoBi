using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using System;
using System.IO;
using static MoBi.R.Tests.DomainHelperForSpecs;

namespace MoBi.R.Tests
{
   public static class HelperForSpecs
   {
      public static string TestFileFullPath(string fileName)
      {
         var dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
         return Path.Combine(dataFolder, fileName);
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