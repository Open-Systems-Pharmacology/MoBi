using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mappers
{
   public interface IFormulaUsablePathToFormulaUsablePathDTOMapper
   {
      FormulaUsablePathDTO MapFrom(IFormulaUsablePath formulaUsablePath, IFormula formula);
      IReadOnlyList<FormulaUsablePathDTO> MapFrom(IEnumerable<IFormulaUsablePath> formulaUsablePath, IFormula formula);
      IReadOnlyList<FormulaUsablePathDTO> MapFrom(IFormula formula);
   }

   public class FormulaUsablePathToFormulaUsablePathDTOMapper : IFormulaUsablePathToFormulaUsablePathDTOMapper
   {
      public FormulaUsablePathDTO MapFrom(IFormulaUsablePath formulaUsablePath, IFormula formula)
      {
         return new FormulaUsablePathDTO(formulaUsablePath, formula);
      }

      public IReadOnlyList<FormulaUsablePathDTO> MapFrom(IEnumerable<IFormulaUsablePath> formulaUsablePath, IFormula formula)
      {
         return formulaUsablePath.Select(x => MapFrom(x, formula)).ToList();
      }

      public IReadOnlyList<FormulaUsablePathDTO> MapFrom(IFormula formula)
      {
         return MapFrom(formula.ObjectPaths, formula);
      }
   }
}