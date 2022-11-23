using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using System.Linq;

namespace MoBi.Core.Commands
{
   public class StartValueOrUnitChangedCommand<TBuilder, TBuildingBlock> : ValueWithPathEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock<TBuilder> where TBuilder : class, IStartValue
   {
      public StartValueOrUnitChangedCommand(TBuilder builder, double? newBaseValue, Unit newDisplayUnit, TBuildingBlock buildingBlock)
         : base(builder, newBaseValue, newDisplayUnit, buildingBlock)
      {

      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _builder = _buildingBlock.Single(startValue => startValue.Path.Equals(_valuePath));
      }

      protected override double? GetOldValue()
      {
         return _builder.StartValue;
      }

      protected override IObjectPath GetPath()
      {
         return _builder.Path;
      }

      protected override void SetNewValue(double? newBaseValue)
      {
         _builder.StartValue = newBaseValue;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new StartValueOrUnitChangedCommand<TBuilder, TBuildingBlock>(_builder, _oldBaseValue, _oldDisplayUnit, _buildingBlock).AsInverseFor(this);
      }
   }
}