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
   public class UpdateDimensionOfFormulaUsablePathCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private readonly string _alias;
      private readonly string _newDimension;
      private readonly string _oldDimension;
      private readonly string _formulaId;

      public UpdateDimensionOfFormulaUsablePathCommand(IDimension newDimension, IFormula formula, string alias, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _alias = alias;
         _oldDimension = formula.FormulaUsablePathBy(_alias).Dimension.ToString();
         _newDimension = newDimension.ToString();
         _formulaId = formula.Id;

         ObjectType = new ObjectTypeResolver().TypeFor(formula);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.ChangeFormulaPathDimension(formula.Name, _oldDimension, _newDimension, buildingBlock.Name, alias);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var formula = context.Get<IFormula>(_formulaId);
         formula.FormulaUsablePathBy(_alias).Dimension = context.DimensionFactory.GetDimension(_newDimension);
         context.PublishEvent(new FormulaChangedEvent(formula));
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         var formula = context.Get<IFormula>(_formulaId);
         return new UpdateDimensionOfFormulaUsablePathCommand(context.DimensionFactory.GetDimension(_oldDimension), formula, _alias, _buildingBlock).AsInverseFor(this);
      }
   }
}