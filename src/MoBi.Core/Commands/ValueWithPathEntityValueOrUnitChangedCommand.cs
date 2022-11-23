using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class ValueWithPathEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock> : BuildingBlockChangeCommandBase<TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock<TBuilder> 
      where TBuilder : class, IObjectBase, IUsingFormula, IWithDisplayUnit
   {
      protected TBuilder _builder;
      protected Unit _newDisplayUnit;
      protected Unit _oldDisplayUnit;
      protected double? _newBaseValue;
      protected double? _oldBaseValue;
      protected readonly IObjectPath _valuePath;

      protected abstract double? GetOldValue();

      protected ValueWithPathEntityValueOrUnitChangedCommand(TBuilder builder, double? newBaseValue, Unit newDisplayUnit, TBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _builder = builder;
         _newDisplayUnit = newDisplayUnit;
         _oldDisplayUnit = _builder.DisplayUnit;
         _newBaseValue = newBaseValue;
         _oldBaseValue = GetOldValue();
         _valuePath = GetPath();

         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(builder);
         Description = AppConstants.Commands.SetQuantityValueInBuildingBlock(
            ObjectType,
            builder.Dimension.BaseUnitValueToUnitValue(_newDisplayUnit, newBaseValue.GetValueOrDefault(double.NaN)),
            _newDisplayUnit.Name,
            builder.ConvertToDisplayUnit(_oldBaseValue.GetValueOrDefault(double.NaN)),
            _oldDisplayUnit.Name,
            GetPath().PathAsString, _buildingBlock.Name);
      }

      protected abstract IObjectPath GetPath();

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _builder = null;
      }



      protected abstract void SetNewValue(double? newBaseValue);

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         SetNewValue(_newBaseValue);
         _builder.DisplayUnit = _newDisplayUnit;
      }
   }
}