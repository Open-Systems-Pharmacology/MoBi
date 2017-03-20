using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ChangeVariableNameCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private SumFormula _changedFormula;
      public string NewVariableName { get; set; }
      public string OldVariableName { get; set; }
      public string ChangedFormulaId { get; set; }

      public ChangeVariableNameCommand(SumFormula changedFormula, string newVariableName, string oldVariableName, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _changedFormula = changedFormula;
         ChangedFormulaId = changedFormula.Id;
         NewVariableName = newVariableName;
         OldVariableName = oldVariableName;
         ObjectType = ObjectTypes.SumFormula;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.VariableName, oldVariableName, newVariableName, _changedFormula.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return CommandExtensions.AsInverseFor(new ChangeVariableNameCommand(_changedFormula, OldVariableName, NewVariableName, _buildingBlock), this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _changedFormula = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _changedFormula.Variable = NewVariableName;
         _changedFormula.FormulaString = _changedFormula.VariablePattern;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _changedFormula = context.Get<SumFormula>(ChangedFormulaId);
      }
   }
}