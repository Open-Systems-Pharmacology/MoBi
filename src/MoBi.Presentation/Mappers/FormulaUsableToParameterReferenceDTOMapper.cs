using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IFormulaUsableToParameterReferenceDTOMapper
   {
      /// <summary>
      ///    Returns a reference for each formula in the model of <paramref name="simulation" /> where
      ///    <paramref name="target" /> is referenced
      /// </summary>
      IReadOnlyList<ParameterReferenceDTO> MapFrom(IFormulaUsable target, IMoBiSimulation simulation);
   }

   public class FormulaUsableToParameterReferenceDTOMapper : IFormulaUsableToParameterReferenceDTOMapper
   {
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public FormulaUsableToParameterReferenceDTOMapper(IEntityPathResolver entityPathResolver, IObjectTypeResolver objectTypeResolver)
      {
         _entityPathResolver = entityPathResolver;
         _objectTypeResolver = objectTypeResolver;
      }

      public IReadOnlyList<ParameterReferenceDTO> MapFrom(IFormulaUsable target, IMoBiSimulation simulation)
      {
         var references = new List<ParameterReferenceDTO>();
         simulation.Model.Root.GetAllChildren<IUsingFormula>().Each(usingFormula =>
         {
            addReferencesFrom(usingFormula, usingFormula.Formula, AppConstants.Captions.Formula, target, references);
            if (usingFormula is IParameter usingParameter)
               addReferencesFrom(usingFormula, usingParameter.RHSFormula, AppConstants.Captions.RHSFormula, target, references);
         });
         return references.OrderBy(x => x.UsedBy).ToList();
      }

      private void addReferencesFrom(IUsingFormula usingFormula, IFormula formula, string usedAs, IFormulaUsable target, List<ParameterReferenceDTO> references)
      {
         if (formula == null)
            return;

         var referencesToTarget = formula.ObjectReferences.Where(x => Equals(x.Object, target));
         referencesToTarget.Each(objectReference => references.Add(new ParameterReferenceDTO
         {
            UsedByObject = usingFormula,
            UsedBy = _entityPathResolver.PathFor(usingFormula),
            Type = _objectTypeResolver.TypeFor(usingFormula),
            UsedAs = usedAs,
            Alias = objectReference.Alias,
            FormulaName = formula.Name,
            Formula = formulaDisplayFor(formula)
         }));
      }

      private string formulaDisplayFor(IFormula formula)
      {
         if (formula is FormulaWithFormulaString formulaWithFormulaString)
            return formulaWithFormulaString.FormulaString;

         return _objectTypeResolver.TypeFor(formula);
      }
   }
}
