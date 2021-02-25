using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditUseDerivedValuesCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private TableFormula _tableFormula;

      public bool NewValue { get; set; }
      public bool OldValue { get; set; }
      public string TableFormulaId { get; set; }

      public EditUseDerivedValuesCommand(TableFormula tableFormula, bool newValue, bool oldValue, IBuildingBlock buildingBlock):base(buildingBlock)
      {
         _tableFormula = tableFormula;
         TableFormulaId = _tableFormula.Id;
         ObjectType = ObjectTypes.TableFormula;
         CommandType = AppConstants.Commands.EditCommand;
         NewValue = newValue;
         OldValue = oldValue;
         Description = AppConstants.Commands.SetUseDerivedValues(newValue,tableFormula);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditUseDerivedValuesCommand(_tableFormula, OldValue, NewValue, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _tableFormula = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _tableFormula.UseDerivedValues = NewValue;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _tableFormula = context.Get<TableFormula>(TableFormulaId);
      }
   }
}