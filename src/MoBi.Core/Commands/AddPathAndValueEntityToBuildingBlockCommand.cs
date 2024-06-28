using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddPathAndValueEntityToBuildingBlockCommand<T> : BuildingBlockChangeCommandBase<ILookupBuildingBlock<T>> where T : PathAndValueEntity
   {
      private T _pathAndValueEntity;
      private readonly ObjectPath _objectPath;
      private byte[] _serializedPathAndValueEntity;

      public AddPathAndValueEntityToBuildingBlockCommand(ILookupBuildingBlock<T> buildingBlock, T pathAndValueEntity)
         : base(buildingBlock)
      {
         _pathAndValueEntity = pathAndValueEntity;
         CommandType = AppConstants.Commands.AddCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(_pathAndValueEntity);
         Description = AppConstants.Commands.AddedPathAndValueEntity(pathAndValueEntity, buildingBlock.Name, ObjectType);
         _objectPath = pathAndValueEntity.Path;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _pathAndValueEntity = context.Deserialize<T>(_serializedPathAndValueEntity);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _pathAndValueEntity = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _serializedPathAndValueEntity = context.Serialize(_pathAndValueEntity);
         _buildingBlock.Add(_pathAndValueEntity);
         if (_pathAndValueEntity.Formula != null)
            _buildingBlock.AddFormula(_pathAndValueEntity.Formula);

         context.PublishEvent(new PathAndValueEntitiesBuildingBlockChangedEvent(_buildingBlock));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemovePathAndValueEntityFromBuildingBlockCommand<T>(_buildingBlock, _objectPath)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }

   public class AddParameterValueToBuildingBlockCommand : AddPathAndValueEntityToBuildingBlockCommand<ParameterValue>
   {
      public AddParameterValueToBuildingBlockCommand(ILookupBuildingBlock<ParameterValue> parameterValuesBuildingBlock, ParameterValue pathAndValueEntity) : base(parameterValuesBuildingBlock, pathAndValueEntity)
      {
      }
   }

   public class AddInitialConditionToBuildingBlockCommand : AddPathAndValueEntityToBuildingBlockCommand<InitialCondition>
   {
      public AddInitialConditionToBuildingBlockCommand(ILookupBuildingBlock<InitialCondition> initialConditionsBuildingBlock, InitialCondition pathAndValueEntity)
         : base(initialConditionsBuildingBlock, pathAndValueEntity)
      {
      }
   }
}