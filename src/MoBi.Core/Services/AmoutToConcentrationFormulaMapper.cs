using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Services
{
   public interface IAmoutToConcentrationFormulaMapper
   {
      /// <summary>
      ///    Returns <c>true</c> if a mapping for the conversion from amount to concentration exists for the
      ///    <paramref name="explicitFormula" />
      ///    otherwise <c>false</c>
      /// </summary>
      bool HasMappingFor(ExplicitFormula explicitFormula);

      /// <summary>
      ///    Returns the mapped formula string for the <paramref name="explicitFormula" />. An exception is thrown if
      ///    no mapping was found for the given formula string
      /// </summary>
      string MappedFormulaFor(ExplicitFormula explicitFormula);
   }

   public class AmoutToConcentrationFormulaMapper : IAmoutToConcentrationFormulaMapper
   {
      private readonly Cache<string, string> _cache;

      public AmoutToConcentrationFormulaMapper()
      {
         _cache = new Cache<string, string>();
         map("f_cell * V * CLspec * C_cell * K_water_cell", "CLspec * C_cell * K_water_cell");
         map("CP * kcat * V * K_water * C / (Km + K_water * C)", "CP * kcat * K_water * C / (Km + K_water * C)");
         map("CP * kcat * V * K_water * C / (Km*(1 + Inhibitor/Ki) + K_water * C)", "CP * kcat * K_water * C / (Km*(1 + Inhibitor/Ki) + K_water * C)");
         map("CP * kcat * V * K_water * C^alpha / (Km^alpha + K_water * C^alpha)", "CP * kcat * K_water * C^alpha / (Km^alpha + K_water * C^alpha)");
      }

      private void map(string oldFormula, string newFormula)
      {
         _cache.Add(oldFormula, newFormula);
      }

      public bool HasMappingFor(ExplicitFormula explicitFormula)
      {
         return _cache.Contains(trimmedFormulaFor(explicitFormula));
      }

      private string trimmedFormulaFor(ExplicitFormula explicitFormula)
      {
         return explicitFormula.FormulaString.Trim();
      }

      public string MappedFormulaFor(ExplicitFormula explicitFormula)
      {
         return _cache[trimmedFormulaFor(explicitFormula)];
      }
   }
}