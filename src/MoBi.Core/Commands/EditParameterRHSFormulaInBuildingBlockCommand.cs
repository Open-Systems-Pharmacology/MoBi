using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class EditParameterRHSFormulaInBuildingBlockCommand : EditParameterPropertyInBuildingBlockCommand<IFormula>
   {
      private readonly string _newFormulaId;
      private readonly string _oldFormulaId;

      public EditParameterRHSFormulaInBuildingBlockCommand(IFormula newFormula, IFormula oldFormula, IParameter parameter, IBuildingBlock buildingBlock) 
         : base (newFormula, oldFormula, parameter, buildingBlock)
      {
         _newFormulaId = newFormula != null ? newFormula.Id : string.Empty;
         _oldFormulaId = oldFormula != null ? oldFormula.Id : string.Empty;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parameter.RHSFormula = _newValue;
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

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterRHSFormulaInBuildingBlockCommand(_oldValue, _newValue, _parameter, _buildingBlock).AsInverseFor(this);
      }

      protected override string GetCommandDescription(IFormula newFormula, IFormula oldFormula, IParameter parameter, IBuildingBlock buildingBlock)
      {
         var newFormulaName = newFormula != null ? newFormula.Name : AppConstants.Undefined;
         var oldFormulaName = oldFormula != null ? oldFormula.Name : AppConstants.Undefined;

         return AppConstants.Commands.ChangeParameterRHSFormula(parameter.Name, newFormulaName, oldFormulaName, buildingBlock.Name);
      }
   };
}