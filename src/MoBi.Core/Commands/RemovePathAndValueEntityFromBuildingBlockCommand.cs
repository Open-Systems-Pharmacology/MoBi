using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemovePathAndValueEntityFromBuildingBlockCommand<T> : BuildingBlockChangeCommandBase<PathAndValueEntityBuildingBlock<T>> where T : PathAndValueEntity
   {
      private readonly T _originalEntity;

      public RemovePathAndValueEntityFromBuildingBlockCommand(PathAndValueEntityBuildingBlock<T> parent, ObjectPath path) : base(parent)
      {
         CommandType = AppConstants.Commands.DeleteCommand;

         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         _originalEntity = _buildingBlock[path];
         Description = AppConstants.Commands.RemovePathAndValueEntity(_originalEntity, parent.Name, ObjectType);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);

         if (_originalEntity == null) return;

         _buildingBlock.Remove(_originalEntity);
         context.PublishEvent(new PathAndValueEntitiesBuildingBlockChangedEvent(_buildingBlock));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddPathAndValueEntityToBuildingBlockCommand<T>(_buildingBlock, _originalEntity)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }

   public class RemoveInitialConditionFromBuildingBlockCommand : RemovePathAndValueEntityFromBuildingBlockCommand<InitialCondition>
   {
      public RemoveInitialConditionFromBuildingBlockCommand(PathAndValueEntityBuildingBlock<InitialCondition> parent, ObjectPath path)
         : base(parent, path)
      {
      }
   }

   public class RemoveParameterValueFromBuildingBlockCommand : RemovePathAndValueEntityFromBuildingBlockCommand<ParameterValue>
   {
      public RemoveParameterValueFromBuildingBlockCommand(PathAndValueEntityBuildingBlock<ParameterValue> parent, ObjectPath path) : base(parent, path)
      {
      }
   }
}