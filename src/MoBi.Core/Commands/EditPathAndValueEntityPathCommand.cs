using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class EditPathAndValueEntityPathCommand<TBuildingBlock, TPathAndValueEntity> : BuildingBlockChangeCommandBase<TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock<TPathAndValueEntity>
      where TPathAndValueEntity : PathAndValueEntity
   {
      protected IEnumerable<string> _path;
      private ObjectPath _newContainerPath;
      private TPathAndValueEntity _originalPathAndValueEntity;
      protected ObjectPath _newPath;
      protected ObjectPath _originalContainerPath;

      protected EditPathAndValueEntityPathCommand(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, ObjectPath newContainerPath)
         : base(buildingBlock)
      {
         _newContainerPath = newContainerPath;
         _originalPathAndValueEntity = pathAndValueEntity;
         setCommandConstants();
      }

      private void setCommandConstants()
      {
         ObjectType = new ObjectTypeResolver().TypeFor(_originalPathAndValueEntity);
         CommandType = AppConstants.Commands.EditCommand;
         var newPath = _newContainerPath.Clone<ObjectPath>().AndAdd(_originalPathAndValueEntity.Name);
         Description = AppConstants.Commands.EditPath(ObjectType, _originalPathAndValueEntity.Path, newPath);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _newContainerPath = null;
         _originalPathAndValueEntity = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);

         // A start value path is used as a key to look up the start value inside the building block
         // Therefore, it should not change while it's a member.
         // Simple remove/add cycle to ensure that we can find the start value later by the path.
         _buildingBlock.Remove(_originalPathAndValueEntity);

         _originalContainerPath = _originalPathAndValueEntity.ContainerPath.Clone<ObjectPath>();
         _originalPathAndValueEntity.ContainerPath = _newContainerPath;

         _buildingBlock.Add(_originalPathAndValueEntity);

         _newPath = _originalPathAndValueEntity.Path;
      }
   }
}