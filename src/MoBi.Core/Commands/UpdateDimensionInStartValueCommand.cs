using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class UpdateDimensionInStartValueCommand<T> : BuildingBlockChangeCommandBase<IStartValuesBuildingBlock<T>> where T : class, IStartValue
   {
      private readonly T _startValue;
      private readonly IDimension _oldDimension;
      private readonly IDimension _newDimension;
      private readonly Unit _oldDisplayUnit;
      private readonly Unit _newDisplayUnit;

      public UpdateDimensionInStartValueCommand(T startValue, IDimension newDimension, Unit newDisplayUnit, IStartValuesBuildingBlock<T> parameterStartValuesBuildingBlock)
         : base(parameterStartValuesBuildingBlock)
      {
         _startValue = startValue;
         _oldDimension = startValue.Dimension;
         _oldDisplayUnit = startValue.DisplayUnit;
         _newDimension = newDimension;
         _newDisplayUnit = newDisplayUnit;

         ObjectType = new ObjectTypeResolver().TypeFor(startValue);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.UpdateDimensions(startValue.Path.PathAsString, ObjectType, _oldDimension, newDimension, parameterStartValuesBuildingBlock.Name);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _buildingBlock = context.Get<IStartValuesBuildingBlock<T>>(_buildingBlockId);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateDimensionInStartValueCommand<T>(_startValue, _oldDimension, _oldDisplayUnit, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _startValue.Dimension = _newDimension;
         _startValue.DisplayUnit = _newDisplayUnit;
      }
   }
}