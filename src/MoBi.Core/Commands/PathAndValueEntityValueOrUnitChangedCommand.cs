using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using System.Linq;

namespace MoBi.Core.Commands
{
   public class PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock> : BuildingBlockChangeCommandBase<TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock<TBuilder>
      where TBuilder : class, IObjectBase, IUsingFormula, IWithDisplayUnit, IWithPath, IWithNullableValue
   {
      protected TBuilder _builder;
      protected Unit _newDisplayUnit;
      protected Unit _oldDisplayUnit;
      protected double? _newBaseValue;
      protected double? _oldBaseValue;
      protected readonly IObjectPath _valuePath;


      public PathAndValueEntityValueOrUnitChangedCommand(TBuilder builder, double? newBaseValue, Unit newDisplayUnit, TBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _builder = builder;
         _newDisplayUnit = newDisplayUnit;
         _oldDisplayUnit = _builder.DisplayUnit;
         _newBaseValue = newBaseValue;
         _oldBaseValue = builder.Value;
         _valuePath = builder.Path;

         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(builder);
         Description = AppConstants.Commands.SetQuantityValueInBuildingBlock(
            ObjectType,
            builder.Dimension.BaseUnitValueToUnitValue(_newDisplayUnit, newBaseValue.GetValueOrDefault(double.NaN)),
            _newDisplayUnit.Name,
            builder.ConvertToDisplayUnit(_oldBaseValue.GetValueOrDefault(double.NaN)),
            _oldDisplayUnit.Name,
            builder.Path.PathAsString, _buildingBlock.Name);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock>(_builder, _oldBaseValue, _oldDisplayUnit, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _builder = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _builder = _buildingBlock.Single(expressionParameter => expressionParameter.Path.Equals(_valuePath));
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _builder.Value = _newBaseValue;
         _builder.DisplayUnit = _newDisplayUnit;
      }
   }
}