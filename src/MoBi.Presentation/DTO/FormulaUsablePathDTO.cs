using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class FormulaUsablePathDTO : DxValidatableDTO, IViewItem
   {
      public IFormulaUsablePath FormulaUsablePath { get; }
      private readonly IFormula _formula;
      public string Alias => FormulaUsablePath.Alias;
      public IDimension Dimension => FormulaUsablePath.Dimension;
      public string Path => FormulaUsablePath.PathAsString;

      public FormulaUsablePathDTO(IFormulaUsablePath formulaUsablePath, IFormula formula)
      {
         FormulaUsablePath = formulaUsablePath;
         _formula = formula;
         Rules.AddRange(AllRules.AllDefault());
         if (_formula.ObjectPaths.Contains(FormulaUsablePath))
            Rules.Add(AllRules.UniqueNameRule);
      }

      private bool isAliasUnique(string alias)
      {
         return _formula.ObjectPaths.Where(x => !ReferenceEquals(x, FormulaUsablePath))
            .All(usablePath => !string.Equals(usablePath.Alias, alias));
      }
         
      private static class AllRules
      {
         private static IBusinessRule notEmptyNameRule { get; } = GenericRules.NonEmptyRule<FormulaUsablePathDTO>(x => x.Alias, AppConstants.Validation.EmptyAlias);

         private static IBusinessRule notEmptyPathRule { get; } = GenericRules.NonEmptyRule<FormulaUsablePathDTO>(x => x.Path, AppConstants.Validation.EmptyPath);

         public static IBusinessRule UniqueNameRule { get; } =
            CreateRule.For<FormulaUsablePathDTO>()
               .Property(x => x.Alias)
               .WithRule((dto, alias) => dto.isAliasUnique(alias))
               .WithError(AppConstants.Validation.AliasAllreadyUsed);

         public static IEnumerable<IBusinessRule> AllDefault()
         {
            yield return notEmptyNameRule;
            yield return notEmptyPathRule;
         }
      }
   }
}