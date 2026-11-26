using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class UpdateDimensionOfFormulaUsablePathCommand : FormulaChangeCommandBase<IFormula>
   {
      private readonly string _alias;
      private readonly string _newDimension;
      private readonly string _oldDimension;

      public UpdateDimensionOfFormulaUsablePathCommand(IDimension newDimension, IFormula formula, string alias, IBuildingBlock buildingBlock) : base(formula, buildingBlock)
      {
         _alias = alias;
         _oldDimension = formula.FormulaUsablePathBy(_alias).Dimension.ToString();
         _newDimension = newDimension.ToString();

         ObjectType = new ObjectTypeResolver().TypeFor(formula);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.ChangeFormulaPathDimension(formula.Name, _oldDimension, _newDimension, buildingBlock.Name, alias);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _formula.FormulaUsablePathBy(_alias).Dimension = context.DimensionFactory.Dimension(_newDimension);
         context.PublishEvent(new FormulaChangedEvent(_formula));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateDimensionOfFormulaUsablePathCommand(context.DimensionFactory.Dimension(_oldDimension), _formula, _alias, _buildingBlock).AsInverseFor(this);
      }
   }
}
