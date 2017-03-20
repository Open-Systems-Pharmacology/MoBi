using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class EditParameterFormulaInBuildingBlockCommand : EditParameterPropertyInBuildingBlockCommand<IFormula>
   {
      private readonly string _newFormulaId;
      private readonly string _oldFormulaId;

      public EditParameterFormulaInBuildingBlockCommand(IFormula newFormula, IFormula oldFormula, IParameter parameter, IBuildingBlock buildingBlock) 
         : base (newFormula, oldFormula, parameter, buildingBlock)
      {
         _newFormulaId = newFormula.Id;
         _oldFormulaId = oldFormula.Id;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterFormulaInBuildingBlockCommand(_oldValue, _newValue, _parameter, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parameter.Formula = _newValue;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _newValue = null;
         _oldValue = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _newValue = context.Get<IFormula>(_newFormulaId);
         _oldValue = context.Get<IFormula>(_oldFormulaId);
      }

      protected override string GetCommandDescription(IFormula newValue, IFormula oldValue, IParameter parameter, IBuildingBlock buildingBlock)
      {
         return AppConstants.Commands.ChangeParameterFormula(parameter.Name, newValue.Name, oldValue.Name, buildingBlock.Name);
      }
   };
}