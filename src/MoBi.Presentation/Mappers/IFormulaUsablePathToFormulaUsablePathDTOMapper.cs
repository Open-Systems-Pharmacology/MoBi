using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface IFormulaUsablePathToFormulaUsablePathDTOMapper
   {
      FormulaUsablePathDTO MapFrom(FormulaUsablePath formulaUsablePath, IFormula formula);
      IReadOnlyList<FormulaUsablePathDTO> MapFrom(IEnumerable<FormulaUsablePath> formulaUsablePath, IFormula formula);
      IReadOnlyList<FormulaUsablePathDTO> MapFrom(IFormula formula);
      IReadOnlyList<FormulaUsablePathDTO> MapFrom(IFormula formula, IUsingFormula usingFormula);
   }

   public class FormulaUsablePathToFormulaUsablePathDTOMapper : IFormulaUsablePathToFormulaUsablePathDTOMapper
   {
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IObjectPathFactory _objectPathFactory;

      public FormulaUsablePathToFormulaUsablePathDTOMapper(
         IEntityPathResolver entityPathResolver,
         IObjectPathFactory objectPathFactory)
      {
         _entityPathResolver = entityPathResolver;
         _objectPathFactory = objectPathFactory;
      }

      public FormulaUsablePathDTO MapFrom(FormulaUsablePath formulaUsablePath, IFormula formula)
      {
         return new FormulaUsablePathDTO(formulaUsablePath, formula);
      }

      public IReadOnlyList<FormulaUsablePathDTO> MapFrom(IEnumerable<FormulaUsablePath> formulaUsablePath, IFormula formula)
      {
         return formulaUsablePath.Select(x => MapFrom(x, formula)).ToList();
      }

      public IReadOnlyList<FormulaUsablePathDTO> MapFrom(IFormula formula)
      {
         return MapFrom(formula.ObjectPaths, formula);
      }

      public IReadOnlyList<FormulaUsablePathDTO> MapFrom(IFormula formula, IUsingFormula usingFormula)
      {
         return MapFrom(updateObjectPaths(formula, usingFormula), formula);
      }

      private IEnumerable<FormulaUsablePath> updateObjectPaths(IFormula formula, IUsingFormula usingFormula)
      {
         if (shouldUseExistingObjectPaths(formula, usingFormula))
            return formula.ObjectPaths;

         return from objectPath in formula.ObjectPaths
            let referencedObject = objectPath.TryResolve<IFormulaUsable>(usingFormula)
            select objectPathFor(referencedObject, objectPath);
      }

      private static bool shouldUseExistingObjectPaths(IFormula formula, IUsingFormula usingFormula)
      {
         return usingFormula == null || !formula.AreReferencesResolved;
      }

      private FormulaUsablePath objectPathFor(IFormulaUsable referencedObject, FormulaUsablePath originalObjectPath)
      {
         if (referencedObject == null)
            return originalObjectPath;

         var consolidatedPath = _entityPathResolver.ObjectPathFor(referencedObject);
         return _objectPathFactory.CreateFormulaUsablePathFrom(consolidatedPath)
            .WithAlias(originalObjectPath.Alias)
            .WithDimension(originalObjectPath.Dimension);
      }
   }
}