using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Core.Commands
{
   public class SetParameterDimensionInBuildingBlockCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IParameter _parameter;
      private readonly string _parameterId;
      private readonly IDimension _newDimension;
      private readonly Unit _oldDisplayUnit;
      private readonly IDimension _oldDimension;
      private Unit _newDisplayUnit;

      public SetParameterDimensionInBuildingBlockCommand(IParameter parameter, IDimension newDimension, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _parameter = parameter;
         _parameterId = parameter.Id;
         _newDimension = newDimension;
         _oldDimension = parameter.Dimension;
         _oldDisplayUnit = _parameter.DisplayUnit;
         _newDisplayUnit = null;
         ObjectType = ObjectTypes.Parameter;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var displayUnitRetriever = context.Resolve<IDisplayUnitRetriever>();
         _parameter.Dimension = _newDimension;
         _parameter.DisplayUnit = _newDisplayUnit ?? displayUnitRetriever.PreferredUnitFor(_parameter);

         updateFormulaDimension(_parameter.Formula, () => _newDimension, context);

         updateFormulaDimension(_parameter.RHSFormula, () => context.DimensionFactory.GetOrAddRHSDimensionFor(_newDimension), context);

         Description = AppConstants.Commands.SetParameterDimension(_parameter.EntityPath(), _oldDimension.Name, _newDimension.Name);
         context.PublishEvent(new QuantityChangedEvent(_parameter));
      }

      // Note": We use a function here to ensure that evaluation of RHSDimension is only performed if there is a RHS 
      private void updateFormulaDimension(IFormula formula, Func<IDimension> dimensionRetriever, IMoBiContext context)
      {
         if (formula == null) return;
         formula.Dimension = dimensionRetriever();
         context.PublishEvent(new FormulaChangedEvent(_parameter.Formula));
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _parameter = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _parameter = context.Get<IParameter>(_parameterId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetParameterDimensionInBuildingBlockCommand(_parameter, _oldDimension, _buildingBlock)
         {
            _newDisplayUnit = _oldDisplayUnit
         }.AsInverseFor(this);
      }
   }
}