using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class EditStartValuePathCommand<TBuildingBlock, TStartValue> : BuildingBlockChangeCommandBase<TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock<TStartValue>, IStartValuesBuildingBlock<TStartValue>
      where TStartValue : class, IObjectBase, IStartValue
   {
      protected IEnumerable<string> _path;
      private ObjectPath _newContainerPath;
      private TStartValue _originalStartValue;
      protected ObjectPath _newStartValuePath;
      protected ObjectPath _originalContainerPath;

      protected EditStartValuePathCommand(TBuildingBlock buildingBlock, TStartValue startValue, ObjectPath newContainerPath)
         : base(buildingBlock)
      {
         _newContainerPath = newContainerPath;
         _originalStartValue = startValue;
         setCommandConstants();
      }

      private void setCommandConstants()
      {
         ObjectType = new ObjectTypeResolver().TypeFor<TStartValue>();
         CommandType = AppConstants.Commands.EditCommand;
         var newStartValuePath = _newContainerPath.Clone<ObjectPath>().AndAdd(_originalStartValue.Name);
         Description = AppConstants.Commands.EditPath(ObjectType, _originalStartValue.Path, newStartValuePath);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _newContainerPath = null;
         _originalStartValue = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);

         // A start value path is used as a key to look up the start value inside the building block
         // Therefore, it should not change while it's a member.
         // Simple remove/add cycle to ensure that we can find the start value later by the path.
         _buildingBlock.Remove(_originalStartValue);

         _originalContainerPath = _originalStartValue.ContainerPath.Clone<ObjectPath>();
         _originalStartValue.ContainerPath = _newContainerPath;

         _buildingBlock.Add(_originalStartValue);

         _newStartValuePath = _originalStartValue.Path;
      }
   }
}