using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class TableFormulaBuilderDTO : FormulaBuilderDTO
   {
      public TableFormulaBuilderDTO(TableFormula tableFormula) : base(tableFormula)
      {
      }

      public IEnumerable<DTOValuePoint> ValuePoints { set; get; }
      public bool UseDerivedValues { get; set; }
      public string XDisplayName { get; set; }
      public string YDisplayName { get; set; }
   }

   public class DTOValuePoint : ValidatableDTO
   {
      private readonly TableFormulaBuilderDTO _tableFormulaDTO;

      public DTOValuePoint(TableFormulaBuilderDTO tableFormulaDTO)
      {
         _tableFormulaDTO = tableFormulaDTO;
         Rules.AddRange(AllRules.All());
      }

      public ValuePointParameterDTO X { get; set; }
      public bool RestartSolver { get; set; }
      public ValuePoint ValuePoint { get; set; }
      public ValuePointParameterDTO Y { get; set; }

      public double XValue
      {
         get { return X.Dimension.BaseUnitValueToUnitValue(X.DisplayUnit, X.Value); }
         set { X.Value = X.Dimension.UnitValueToBaseUnitValue(X.DisplayUnit, value); }
      }

      public double YValue
      {
         get { return Y.Dimension.BaseUnitValueToUnitValue(Y.DisplayUnit, Y.Value); }
         set { Y.Value = Y.Dimension.UnitValueToBaseUnitValue(Y.DisplayUnit, value); }
      }

      private static class AllRules
      {
         public static IEnumerable<IBusinessRule> All()
         {
            yield return CreateRule.For<DTOValuePoint>()
               .Property(x => x.XValue)
               .WithRule(cannotRepeat)
               .WithError((dto, timeValue) => AppConstants.Validation.XDimensionColumnMustNotHaveRepeatedValues(dto._tableFormulaDTO.XDisplayName));
         }

         private static bool cannotRepeat(DTOValuePoint valuePoint, double newValue)
         {
            return valuePoint._tableFormulaDTO.ValuePoints.Except(new[] {valuePoint}).All(x => x.XValue != newValue);
         }
      }
   }

   public class ValuePointParameterDTO : IWithDisplayUnitDTO
   {
      public Unit DisplayUnit { get; set; }
      public IDimension Dimension { get; set; }
      public double Value { get; set; }

      public IEnumerable<Unit> AllUnits
      {
         get { return Dimension.Units; }
         set
         {
            /*nothing to do*/
         }
      }
   }
}