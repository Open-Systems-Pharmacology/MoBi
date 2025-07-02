using System;
using System.IO;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Events;

namespace MoBi.HelpersForTests
{
   public static class DomainHelperForSpecs
   {
      private static Dimension _concentrationDimension;
      private static Dimension _timeDimension;

      public static string TestFileFullPath(string fileName)
      {
         var dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "TestFiles");
         return Path.Combine(dataFolder, fileName);
      }

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

      static DomainHelperForSpecs()
      {
         TimeDimension.AddUnit(new Unit("seconds", 1 / 60.0, 0.0));
         AmountDimension.AddUnit(new Unit("mmol", 1000.0, 0.0));
      }

      public static IDimension AmountDimension { get; } = new Dimension(new BaseDimensionRepresentation { AmountExponent = 1 }, Constants.Dimension.MOLAR_AMOUNT, "µmol");

      public static ParameterValue ParameterValue { get; } = new ParameterValue { Name = "_name", Value = 1.0, Formula = null, Dimension = AmountDimension };

      public static IDimension ConcentrationDimension { get; } = new Dimension(new BaseDimensionRepresentation { LengthExponent = -3, MassExponent = 1, TimeExponent = -1 }, Constants.Dimension.MOLAR_CONCENTRATION, "µmol/l");

      public static IDimension FractionDimension { get; } = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.FRACTION, "");

      public static IDimension ConcentrationPerTimeDimension { get; } = new Dimension(new BaseDimensionRepresentation { LengthExponent = -3, AmountExponent = 1, TimeExponent = -1 }, Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME, "µmol/l/min");

      public static IDimension AmountPerTimeDimension { get; } = new Dimension(new BaseDimensionRepresentation { AmountExponent = 1, TimeExponent = -1 }, Constants.Dimension.AMOUNT_PER_TIME, "µmol/min");

      public static IDimension TimeDimension { get; } = new Dimension(new BaseDimensionRepresentation { TimeExponent = 1 }, Constants.Dimension.TIME, "min");

      public static QuantityValueInSimulationChangeTracker QuantityValueChangeTracker(IEventPublisher eventPublisher)
      {
         var entityPathResolver = new EntityPathResolver(new ObjectPathFactoryForSpecs());
         return new QuantityValueInSimulationChangeTracker(new QuantityToOriginalQuantityValueMapper(entityPathResolver), eventPublisher);
      }

      public static DataRepository ObservedData(string id = "TestData", IDimension timeDimension = null, IDimension concentrationDimension = null, string obsDataColumnName = null)
      {
         var observedData = new DataRepository(id).WithName(id);
         var baseGrid = new BaseGrid("Time", timeDimension ?? TimeDimension)
         {
            Values = new[] { 1.0f, 2.0f, 3.0f }
         };
         observedData.Add(baseGrid);

         var data = ConcentrationColumnForObservedData(baseGrid, concentrationDimension, obsDataColumnName);
         observedData.Add(data);

         return observedData;
      }

      public static DataColumn ConcentrationColumnForObservedData(BaseGrid baseGrid, IDimension concentrationDimension = null, string obsDataColumnName = null)
      {
         var data = new DataColumn(obsDataColumnName ?? "Col", concentrationDimension ?? ConcentrationDimension, baseGrid)
         {
            Values = new[] { 10f, 20f, 30f },
            DataInfo = { Origin = ColumnOrigins.Observation }
         };
         return data;
      }

      public static IParameter ConstantParameterWithValue(double value = 10, bool isDefault = false, bool visible = true)
      {
         var parameter = new Parameter
         {
            Visible = visible,
            Dimension = AmountDimension,
            IsFixedValue = true,
            IsDefault = isDefault,
            Formula = new ConstantFormula(value).WithId("constantFormulaId")
         };
         return parameter;
      }

      public static MoBiProject NewProject()
      {
         return new MoBiProject { SimulationSettings = new SimulationSettings() };
      }
   }
}