using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class EditFormulaStringCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private readonly string _newFormulaString;
      private readonly string _oldFormulaString;
      private readonly string _formulaId;

      public EditFormulaStringCommand(string newFormulaString, string oldFormulaString, ExplicitFormula formula, IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _formulaId = formula.Id;
         _newFormulaString = newFormulaString;
         _oldFormulaString = oldFormulaString;

         ObjectType = new ObjectTypeResolver().TypeFor(formula);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.ChangeFormulaString(formula.Name, _newFormulaString, _oldFormulaString, buildingBlock.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         var formula = context.Get<ExplicitFormula>(_formulaId);
         return new EditFormulaStringCommand(_oldFormulaString, _newFormulaString, formula, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var formula = context.Get<ExplicitFormula>(_formulaId);
         formula.FormulaString = _newFormulaString;
         context.PublishEvent(new FormulaChangedEvent(formula));
      }
   }
}