using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetQuantityDimensionCommand : EditQuantityInBuildingBlockCommand<IQuantity>
   {
      protected readonly IDimension _dimension;
      protected readonly IDimension _oldDimension;

      public SetQuantityDimensionCommand(IQuantity quantity, IDimension dimension, IBuildingBlock buildingBlock)
         : base(quantity, buildingBlock)
      {
         _dimension = dimension;
         _oldDimension = quantity.Dimension;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetQuantityDimensionCommand(_quantity, _oldDimension, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _quantity.Dimension = _dimension;
         _quantity.DisplayUnit = _dimension.DefaultUnit;
         Description = AppConstants.Commands.EditDescription(ObjectType, "Dimension", _oldDimension.Name, _dimension.ToString(), _quantity.Name);
      }
   }

   public class SetDistributedParameterDimensionCommand : SetQuantityDimensionCommand
   {
      public SetDistributedParameterDimensionCommand(IDistributedParameter quantity, IDimension dimension, IBuildingBlock buildingBlock)
         : base(quantity, dimension, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetDistributedParameterDimensionCommand(distributedParameter, _oldDimension, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         trySetDimensionIn(Constants.Distribution.MINIMUM, context);
         trySetDimensionIn(Constants.Distribution.MAXIMUM, context);
         trySetDimensionIn(Constants.Distribution.MEAN, context);
         trySetDimensionIn(Constants.Distribution.DEVIATION, context);
      }

      private void trySetDimensionIn(string parameterName, IMoBiContext context)
      {
         var parameter = distributedParameter.GetSingleChildByName<IParameter>(parameterName);
         if (parameter == null) return;
         new SetQuantityDimensionCommand(parameter, _dimension,_buildingBlock).Execute(context);
      }

      private IDistributedParameter distributedParameter
      {
         get { return _quantity.DowncastTo<IDistributedParameter>(); }
      }
   }
}