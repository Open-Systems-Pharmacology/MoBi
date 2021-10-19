using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class StartValueValueOrUnitChangedCommand<TStartValue, TStartValueBuildingBlock> : BuildingBlockChangeCommandBase<TStartValueBuildingBlock>
      where TStartValueBuildingBlock : class, IStartValuesBuildingBlock<TStartValue> where TStartValue : class, IStartValue
   {
      protected TStartValue _startValue;
      protected Unit _newDisplayUnit;
      protected Unit _oldDisplayUnit;
      protected double? _newBaseValue;
      protected double? _oldBaseValue;
      private readonly IObjectPath _startValuePath;

      public StartValueValueOrUnitChangedCommand(TStartValue startValue, double? newBaseValue, Unit newDisplayUnit, TStartValueBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _startValue = startValue;
         _newDisplayUnit = newDisplayUnit;
         _oldDisplayUnit = _startValue.DisplayUnit;

        _newBaseValue = newBaseValue;
        _oldBaseValue = startValue.StartValue;

         _startValuePath = _startValue.Path;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(startValue);
         Description = AppConstants.Commands.SetQuantityValueInBuildingBlock(
            ObjectType, 
            startValue.Dimension.BaseUnitValueToUnitValue(_newDisplayUnit, newBaseValue.GetValueOrDefault(double.NaN)), 
            _newDisplayUnit.Name, 
            startValue.ConvertToDisplayUnit(_oldBaseValue.GetValueOrDefault(double.NaN)), 
            _oldDisplayUnit.Name, 
            startValue.Path.PathAsString, _buildingBlock.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _startValue = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _startValue = _buildingBlock[_startValuePath];
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _startValue.StartValue = _newBaseValue;
         _startValue.DisplayUnit = _newDisplayUnit;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new StartValueValueOrUnitChangedCommand<TStartValue, TStartValueBuildingBlock>(_startValue, _oldBaseValue, _oldDisplayUnit, _buildingBlock).AsInverseFor(this);
      }
   }
}