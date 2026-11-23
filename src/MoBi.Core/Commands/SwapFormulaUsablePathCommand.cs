using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class SwapFormulaUsablePathCommand : FormulaChangeCommandBase<IFormula>
   {
      private readonly FormulaUsablePath _oldPath;
      private readonly FormulaUsablePath _newPath;

      public SwapFormulaUsablePathCommand(IFormula formula, FormulaUsablePath oldPath, FormulaUsablePath newPath, IBuildingBlock buildingBlock) : base(formula, buildingBlock)
      {
         _oldPath = oldPath;
         _newPath = newPath;
         Description = AppConstants.Commands.ChangeFormulaUsablePath;
         ObjectType = ObjectTypes.Formula;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _formula.RemoveObjectPath(_oldPath);
         context.PublishEvent(new RemovedFormulaUsablePathEvent(_formula, _oldPath));
         _formula.AddObjectPath(_newPath);
         context.PublishEvent(new AddedFormulaUsablePathEvent(_formula, _newPath));
         context.PublishEvent(new FormulaChangedEvent(_formula));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SwapFormulaUsablePathCommand(_formula, _newPath, _oldPath, _buildingBlock).AsInverseFor(this);
      }
   }
}
