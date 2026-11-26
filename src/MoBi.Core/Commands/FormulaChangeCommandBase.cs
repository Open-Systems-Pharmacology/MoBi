using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class FormulaChangeCommandBase<TFormula> : BuildingBlockChangeCommandBase<IBuildingBlock> where TFormula : class, IFormula
   {
      protected TFormula _formula;
      protected readonly string _formulaId;

      protected FormulaChangeCommandBase(TFormula formula, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _formula = formula;
         _formulaId = formula.Id;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _formula.ResetOrigin();
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _formula = context.Get<TFormula>(_formulaId);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _formula = null;
      }
   }
}
