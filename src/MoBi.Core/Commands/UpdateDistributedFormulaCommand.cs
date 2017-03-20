using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;
using Command = OSPSuite.Assets.Command;

namespace MoBi.Core.Commands
{
   public class UpdateDistributedFormulaCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IDistributedParameter _parameter;
      private IDistributionFormula _newFormula;
      private readonly IDistributionFormula _oldFormula;
      private readonly string _parameterId;

      public UpdateDistributedFormulaCommand(IDistributedParameter parameter, IDistributionFormula newFormula, string formulaType, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _parameter = parameter;
         _parameterId = parameter.Id;
         _newFormula = newFormula;
         _oldFormula = _parameter.Formula;
         Description = AppConstants.Commands.UpdateDistributedFormulaCommandDescription(parameter.EntityPath(), formulaType);
         ObjectType = ObjectTypes.DistributedParameter;
         CommandType = Command.CommandTypeEdit;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parameter.Formula = _newFormula;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _parameter = context.Get<IDistributedParameter>(_parameterId);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _parameter = null;
         _newFormula = null;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateDistributedFormulaCommand(_parameter, _oldFormula, "XX", _buildingBlock).AsInverseFor(this);
      }
   }
}