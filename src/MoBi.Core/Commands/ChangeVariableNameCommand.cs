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
      private SumFormula _changedFormula;
      public string NewVariableName { get; set; }
      public string OldVariableName { get; set; }
      public string ChangedFormulaId { get; set; }

      public ChangeVariableNameCommand(SumFormula changedFormula, string newVariableName, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _changedFormula = changedFormula;
         ChangedFormulaId = changedFormula.Id;
         NewVariableName = newVariableName;
         OldVariableName = changedFormula.Variable;
         ObjectType = ObjectTypes.SumFormula;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.VariableName, OldVariableName, newVariableName, _changedFormula.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeVariableNameCommand(_changedFormula, OldVariableName, _buildingBlock).AsInverseFor(this);
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
         context.PublishEvent(new FormulaChangedEvent(_changedFormula));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _changedFormula = context.Get<SumFormula>(ChangedFormulaId);
      }
   }
}