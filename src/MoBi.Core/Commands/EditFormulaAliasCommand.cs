using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class EditFormulaAliasCommand : FormulaChangeCommandBase<IFormula>
   {
      private readonly string _oldAlias;
      private readonly string _newAlias;

      public EditFormulaAliasCommand(IFormula formula, string newAlias, string oldAlias, IBuildingBlock buildingBlock) : base(formula, buildingBlock)
      {
         _newAlias = newAlias;
         _oldAlias = oldAlias;

         ObjectType = new ObjectTypeResolver().TypeFor(formula);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.ChangeFormulaAlias(formula.Name, _oldAlias, _newAlias, buildingBlock.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);

         _formula.FormulaUsablePathBy(_oldAlias).Alias = _newAlias;

         context.PublishEvent(new FormulaChangedEvent(_formula));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditFormulaAliasCommand(_formula, _oldAlias, _newAlias, _buildingBlock).AsInverseFor(this);
      }
   }
}
