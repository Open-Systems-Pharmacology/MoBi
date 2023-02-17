using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Events
{
   public abstract class FormulaEvent
   {
      public IFormula Formula { get; private set; }

      protected FormulaEvent(IFormula formula)
      {
         Formula = formula;
      }
   }

   public class AddedFormulaUsablePathEvent : FormulaEvent
   {
      public FormulaUsablePath FormulaUsablePath { get; set; }

      public AddedFormulaUsablePathEvent(IFormula formula, FormulaUsablePath newPath) : base(formula)
      {
         FormulaUsablePath = newPath;
      }
   }

   public class RemovedFormulaUsablePathEvent : FormulaEvent
   {
      public FormulaUsablePath FormulaUsablePath { get; set; }

      public RemovedFormulaUsablePathEvent(IFormula formula, FormulaUsablePath formulaUsablePath) : base(formula)
      {
         FormulaUsablePath = formulaUsablePath;
      }
   }

   public class FormulaChangedEvent : FormulaEvent
   {
      public FormulaChangedEvent(IFormula formula) : base(formula)
      {
      }
   }
}