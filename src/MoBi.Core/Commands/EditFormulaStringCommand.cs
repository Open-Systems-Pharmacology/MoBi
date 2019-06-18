using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class EditFormulaStringCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private readonly string _newFormulaString;
      private readonly string _oldFormulaString;
      private readonly string _formulaId;
      private FormulaWithFormulaString _formula;

      public EditFormulaStringCommand(string newFormulaString, FormulaWithFormulaString formula, IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _formulaId = formula.Id;
         _formula = formula;
         _newFormulaString = newFormulaString;
         _oldFormulaString = formula.FormulaString;

         ObjectType = new ObjectTypeResolver().TypeFor(formula);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.ChangeFormulaString(formula.Name, _newFormulaString, _oldFormulaString, buildingBlock.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditFormulaStringCommand(_oldFormulaString, _formula, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _formula.FormulaString = _newFormulaString;
         context.PublishEvent(new FormulaChangedEvent(_formula));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _formula = context.Get<FormulaWithFormulaString>(_formulaId);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _formula = null;
      }
   }
}