using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using System.Linq;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock> : BuildingBlockChangeCommandBase<TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock<TBuilder>
      where TBuilder : PathAndValueEntity, IObjectBase, IUsingFormula, IWithDisplayUnit
   {
      protected TBuilder _builder;
      protected Unit _newDisplayUnit;
      protected Unit _oldDisplayUnit;
      protected double? _newBaseValue;
      protected double? _oldBaseValue;
      protected readonly ObjectPath _valuePath;
      private readonly DistributionType? _oldDistributionType;

      public PathAndValueEntityValueOrUnitChangedCommand(TBuilder builder, double? newBaseValue, Unit newDisplayUnit, TBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _builder = builder;
         _newDisplayUnit = newDisplayUnit;
         _oldDisplayUnit = _builder.DisplayUnit;
         _newBaseValue = newBaseValue;
         _oldBaseValue = builder.Value;
         _valuePath = builder.Path;
         _oldDistributionType = builder.DistributionType;

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
         return new PathAndValueEntityValueOrUnitChangedCommandWithDistribution(_builder, _oldBaseValue, _oldDisplayUnit, _buildingBlock, _oldDistributionType).AsInverseFor(this);
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
         _builder.DistributionType = null;
      }

      // This command is used to restore a distribution when a distributed parameter is changed to non-distributed, and then the command is reversed.
      // There is no general way to change between distribution types, so this class is private, so it can only be used with restore functionality
      private class PathAndValueEntityValueOrUnitChangedCommandWithDistribution : PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock>
      {
         private readonly DistributionType? _newDistributionType;

         public PathAndValueEntityValueOrUnitChangedCommandWithDistribution(TBuilder builder, double? newBaseValue, Unit newDisplayUnit, TBuildingBlock buildingBlock, DistributionType? newDistributionType) : base(builder, newBaseValue, newDisplayUnit, buildingBlock)
         {
            _newDistributionType = newDistributionType;
         }

         protected override void ExecuteWith(IMoBiContext context)
         {
            base.ExecuteWith(context);
            _builder.DistributionType = _newDistributionType;
         }
      }
   }
}