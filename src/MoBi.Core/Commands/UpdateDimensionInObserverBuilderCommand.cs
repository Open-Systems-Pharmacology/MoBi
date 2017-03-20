using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateDimensionInObserverBuilderCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IObserverBuilder _observerBuilder;
      private readonly IDimension _newDimension;
      private readonly IDimension _oldDimension;
      private readonly string _observerBuilderId;

      public UpdateDimensionInObserverBuilderCommand(IObserverBuilder observerBuilder, IDimension newDimension, IBuildingBlock observerBuildingBlock)
         : base(observerBuildingBlock)
      {
         _newDimension = newDimension;
         _oldDimension = observerBuilder.Dimension;
         _observerBuilder = observerBuilder;
         _observerBuilderId = observerBuilder.Id;
         ObjectType = ObjectTypes.ObserverBuilder;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.UpdateDimensions(observerBuilder.Name, ObjectType,_oldDimension, newDimension, observerBuildingBlock.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _observerBuilder.Dimension = _newDimension;
         if (_observerBuilder.Formula != null)
            _observerBuilder.Formula.Dimension = _newDimension;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _observerBuilder = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _observerBuilder = context.Get<IObserverBuilder>(_observerBuilderId);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateDimensionInObserverBuilderCommand(_observerBuilder, _oldDimension, _buildingBlock).AsInverseFor(this);
      }
   }
}