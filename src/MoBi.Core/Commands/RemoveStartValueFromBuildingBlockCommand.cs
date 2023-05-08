using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveStartValueFromBuildingBlockCommand<T> : BuildingBlockChangeCommandBase<PathAndValueEntityBuildingBlock<T>> where T : PathAndValueEntity
   {
      private readonly T _originalStartValue;

      public RemoveStartValueFromBuildingBlockCommand(PathAndValueEntityBuildingBlock<T> parent, ObjectPath path) : base(parent)
      {
         CommandType = AppConstants.Commands.DeleteCommand;

         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         _originalStartValue = _buildingBlock[path];
         Description = AppConstants.Commands.RemoveStartValue(_originalStartValue, parent.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);

         if (_originalStartValue == null) return;

         _buildingBlock.Remove(_originalStartValue);
         context.PublishEvent(new StartValuesBuildingBlockChangedEvent(_buildingBlock));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddStartValueToBuildingBlockCommand<T>(_buildingBlock, _originalStartValue)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }

   public class RemoveInitialConditionFromBuildingBlockCommand : RemoveStartValueFromBuildingBlockCommand<InitialCondition>
   {
      public RemoveInitialConditionFromBuildingBlockCommand(PathAndValueEntityBuildingBlock<InitialCondition> parent, ObjectPath path)
         : base(parent, path)
      {
      }
   }

   public class RemoveParameterValueFromBuildingBlockCommand : RemoveStartValueFromBuildingBlockCommand<ParameterValue>
   {
      public RemoveParameterValueFromBuildingBlockCommand(PathAndValueEntityBuildingBlock<ParameterValue> parent, ObjectPath path) : base(parent, path)
      {
      }
   }
}