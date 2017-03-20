using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class EditFormulaAliasCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private readonly string _oldAlias;
      private readonly string _newAlias;
      private readonly string _formulaId;

      public EditFormulaAliasCommand(IFormula formula, string newAlias, string oldAlias, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _newAlias = newAlias;
         _oldAlias = oldAlias;
         _formulaId = formula.Id;

         ObjectType = new ObjectTypeResolver().TypeFor(formula);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.ChangeFormulaAlias(formula.Name, _oldAlias, _newAlias, buildingBlock.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var formula = context.Get<IFormula>(_formulaId);

         formula.FormulaUsablePathBy(_oldAlias).Alias = _newAlias;

         context.PublishEvent(new FormulaChangedEvent(formula));
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         var formula = context.Get<IFormula>(_formulaId);
         return new EditFormulaAliasCommand(formula, _oldAlias, _newAlias, _buildingBlock).AsInverseFor(this);
      }
   }
}