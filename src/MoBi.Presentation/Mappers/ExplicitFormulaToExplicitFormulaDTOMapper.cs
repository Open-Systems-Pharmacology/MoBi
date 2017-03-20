using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface IExplicitFormulaToExplicitFormulaDTOMapper
   {
      ExplicitFormulaBuilderDTO MapFrom(ExplicitFormula explicitFormula, IUsingFormula usingFormula);
   }

   public class ExplicitFormulaToExplicitFormulaDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IExplicitFormulaToExplicitFormulaDTOMapper
   {
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathDTOMapper;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IEntityPathResolver _entityPathResolver;

      public ExplicitFormulaToExplicitFormulaDTOMapper(IFormulaUsablePathToFormulaUsablePathDTOMapper formulaUsablePathDTOMapper,
         IObjectPathFactory objectPathFactory, IEntityPathResolver entityPathResolver)
      {
         _formulaUsablePathDTOMapper = formulaUsablePathDTOMapper;
         _objectPathFactory = objectPathFactory;
         _entityPathResolver = entityPathResolver;
      }

      public ExplicitFormulaBuilderDTO MapFrom(ExplicitFormula explicitFormula, IUsingFormula usingFormula)
      {
         var dto = Map<ExplicitFormulaBuilderDTO>(explicitFormula);
         dto.FormulaString = explicitFormula.FormulaString;
         dto.Dimension = explicitFormula.Dimension;
         dto.ObjectPaths = _formulaUsablePathDTOMapper.MapFrom(updateObjectPaths(explicitFormula, usingFormula), explicitFormula);
         return dto;
      }

      private IEnumerable<IFormulaUsablePath> updateObjectPaths(ExplicitFormula explicitFormula, IUsingFormula usingFormula)
      {
         if (shouldUseExistingObjectPaths(explicitFormula, usingFormula))
            return explicitFormula.ObjectPaths;

         return from objectPath in explicitFormula.ObjectPaths
            let referencedObject = objectPath.TryResolve<IFormulaUsable>(usingFormula)
            select objectPathFor(referencedObject, objectPath);
      }

      private static bool shouldUseExistingObjectPaths(ExplicitFormula explicitFormula, IUsingFormula usingFormula)
      {
         return usingFormula == null || !explicitFormula.AreReferencesResolved;
      }

      private IFormulaUsablePath objectPathFor(IFormulaUsable referencedObject, IFormulaUsablePath originalObjectPath)
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