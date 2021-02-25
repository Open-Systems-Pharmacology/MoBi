using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class ChangeVariableNameCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private SumFormula _sumFormula;
      private readonly string _newVariableName;
      private readonly string _oldVariableName;
      private readonly string _sumFormulaId;

      public ChangeVariableNameCommand(SumFormula sumFormula, string newVariableName, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _sumFormula = sumFormula;
         _sumFormulaId = sumFormula.Id;
         _newVariableName = newVariableName;
         _oldVariableName = sumFormula.Variable;
         ObjectType = ObjectTypes.SumFormula;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.VariableName, _oldVariableName, newVariableName, _sumFormula.Name);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeVariableNameCommand(_sumFormula, _oldVariableName, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _sumFormula = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _sumFormula.Variable = _newVariableName;
         context.PublishEvent(new FormulaChangedEvent(_sumFormula));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _sumFormula = context.Get<SumFormula>(_sumFormulaId);
      }
   }
}