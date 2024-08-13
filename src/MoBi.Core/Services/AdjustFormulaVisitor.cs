using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Services
{
   public interface IAdjustFormulasVisitor
   {
      /// <summary>
      ///    Adjusts the formula usage for the given objectBase in the buildingBlock.
      ///    The method looks if in the buildingBlocks formula cache the same formula (same name,paths,etc, not id)  is already
      ///    used.
      ///    If it is, the already existing formula instance is used instead of the original one.
      ///    If the name is the same but other properties are different, the user is asked to rename the formula and then it's
      ///    added to the formula cache
      ///    In other cases the new formula is added unchanged to the Formula cache
      /// </summary>
      /// <param name="objectBase">The object base.</param>
      /// <param name="buildingBlock">The building block.</param>
      /// <returns></returns>
      (IReadOnlyList<IMoBiCommand> addedFormulaCommand, bool canceled) AdjustFormulasIn(IObjectBase objectBase, IBuildingBlock buildingBlock);
   }

   public class AdjustFormulasVisitor : IVisitor<MoleculeBuilder>, IVisitor<IUsingFormula>, IAdjustFormulasVisitor, IVisitor<IParameter>
   {
      private IFormulaCache _formulaCache;
      private IBuildingBlock _buildingBlock;
      private readonly IMoBiContext _context;
      private readonly INameCorrector _nameCorrector;
      private List<IMoBiCommand> _allCommands;
      private bool _canceled;

      public AdjustFormulasVisitor(IMoBiContext context, INameCorrector nameCorrector)
      {
         _context = context;
         _nameCorrector = nameCorrector;
      }

      public (IReadOnlyList<IMoBiCommand> addedFormulaCommand, bool canceled) AdjustFormulasIn(IObjectBase objectBase, IBuildingBlock buildingBlock)
      {
         _buildingBlock = buildingBlock;
         _formulaCache = _buildingBlock.FormulaCache;
         _allCommands = new List<IMoBiCommand>();
         _canceled = false;
         try
         {
            _context.PublishEvent(new BulkUpdateStartedEvent());
            objectBase.AcceptVisitor(this);
            return (_allCommands, _canceled);
         }
         finally
         {
            _context.PublishEvent(new BulkUpdateFinishedEvent());
            _buildingBlock = null;
            _formulaCache = null;
            _allCommands = null;
         }
      }

      public void Visit(MoleculeBuilder moleculeBuilder)
      {
         if (_canceled) return;
         moleculeBuilder.DefaultStartFormula = checkFormula(_formulaCache, moleculeBuilder.DefaultStartFormula);
      }

      public void Visit(IUsingFormula usingFormula)
      {
         if (_canceled) return;
         usingFormula.Formula = checkFormula(_formulaCache, usingFormula.Formula);
      }

      public void Visit(IParameter parameter)
      {
         if (_canceled) return;

         parameter.Formula = checkFormula(_formulaCache, parameter.Formula);
         if (parameter.RHSFormula != null)
            parameter.RHSFormula = checkFormula(_formulaCache, parameter.RHSFormula);
      }

      private IFormula checkFormula(IFormulaCache formulaCache, IFormula formula)
      {
         if (formula.IsExplicit())
            return checkFormulaByType(formulaCache, (ExplicitFormula) formula, AreEqualExplicitFormula);

         if (formula.IsBlackBox())
            return checkFormulaByType(formulaCache, (BlackBoxFormula) formula, AreEqualBlackBoxFormula);

         if (formula.IsTable())
            return checkFormulaByType(formulaCache, (TableFormula) formula, AreEqualTableFormula);

         if (formula.IsTableWithOffSet())
            return checkFormulaByType(formulaCache, (TableFormulaWithOffset) formula, AreEqualTableFormulaWithOffset);

         if (formula.IsDynamic())
            return checkFormulaByType(formulaCache, (SumFormula) formula, AreEqualSumFormula);

         return formula;
      }

      private T checkFormulaByType<T>(IFormulaCache formulaCache, T formula, Func<T, T, bool> areEqualFormula) where T : class, IFormula
      {
         var alreadyUsedFormula = lookForSimilarFormula(formulaCache, formula, areEqualFormula);
         if (alreadyUsedFormula != null)
            return alreadyUsedFormula;
         if (formulaCache.ExistsByName(formula.Name))
         {
            correctName(formulaCache, formula);
         }

         checkId(formula);
         //Run is required here so that the next time the same formula is found, it will be replaced automatically instead of being 
         //added again to the cache
         _allCommands.Add(new AddFormulaToFormulaCacheCommand(_buildingBlock, formula).Run(_context));
         return formula;
      }

      private T lookForSimilarFormula<T>(IEnumerable<IFormula> formulaCache, T formula, Func<T, T, bool> areEqualFormula) where T : class, IFormula
      {
         return formulaCache.OfType<T>().FirstOrDefault(f => areEqualFormula(f, formula));
      }

      private static T getFormulaFromCache<T>(IFormulaCache formulaCache, string formulaName) where T : class, IFormula
      {
         return formulaCache.FindByName(formulaName) as T;
      }

      private void checkId(IFormula formula)
      {
         if (_canceled) return;
         if (!_context.ObjectRepository.ContainsObjectWithId(formula.Id)) return;

         formula.Id = Guid.NewGuid().ToString();
      }

      private void correctName(IFormulaCache formulaCache, IFormula formula)
      {
         _canceled = !_nameCorrector.CorrectName(formulaCache, formula);
      }

      public bool AreEqualBlackBoxFormula(BlackBoxFormula usedFormula, BlackBoxFormula alreadyUsedFormula)
      {
         return alreadyUsedFormula != null && usedFormula != null;
      }

      public bool AreEqualTableFormula(TableFormula formula, TableFormula alreadyUsedFormula)
      {
         var valuePoints = alreadyUsedFormula.AllPoints;
         if (!valuePoints.Count().Equals(formula.AllPoints.Count()))
            return false;

         foreach (var point in formula.AllPoints)
         {
            if (valuePoints.FirstOrDefault(p => p.X.Equals(point.X) && p.Y.Equals(point.Y)) == null)
               return false;
         }

         return true;
      }

      public bool AreEqualTableFormulaWithOffset(TableFormulaWithOffset formula, TableFormulaWithOffset alreadyUsedFormula)
      {
         return areEqualObjectPathCollection(formula.ObjectPaths, alreadyUsedFormula.ObjectPaths);
      }

      public bool AreEqualSumFormula(SumFormula formula, SumFormula alreadyUsedFormula)
      {
         return alreadyUsedFormula.Criteria.Equals(formula.Criteria);
      }

      public bool AreEqualExplicitFormula(ExplicitFormula explicitFormula, ExplicitFormula otherExplicitFormula)
      {
         if (explicitFormula == null || otherExplicitFormula == null)
            return false;

         bool equal = string.Equals(explicitFormula.FormulaString, otherExplicitFormula.FormulaString);
         if (!equal)
            return false;

         return areEqualObjectPathCollection(explicitFormula.ObjectPaths, otherExplicitFormula.ObjectPaths);
      }

      private bool areEqualObjectPathCollection(IReadOnlyList<FormulaUsablePath> objectPaths, IReadOnlyList<FormulaUsablePath> otherObjectPaths)
      {
         if (!objectPaths.Count.Equals(otherObjectPaths.Count)) return false;
         var equal = true;
         foreach (var path in objectPaths)
         {
            var otherPath = otherObjectPaths.FirstOrDefault(objPath => string.Equals(objPath.Alias, path.Alias));
            if (otherPath == null) return false;
            equal = equal && path.Equals(otherPath);
         }

         return equal;
      }
   }
}