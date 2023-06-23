using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Commands
{
   public class SetParentPathCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IContainer _container;
      private readonly ObjectPath _oldParentPath;
      private readonly ObjectPath _newParentPath;
      private readonly string _containerId;

      public SetParentPathCommand(IContainer container, ObjectPath newParentPath, IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _container = container;
         _containerId = container.Id;
         _newParentPath = newParentPath;
         _oldParentPath = container.ParentPath;
         ObjectType = ObjectTypes.Container;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _container.ParentPath = _newParentPath;
         Description = AppConstants.Commands.UpdateParentPath(_container.EntityPath(), (_newParentPath ?? new ObjectPath()).ToPathString());
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _container = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _container = context.Get<IContainer>(_containerId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetParentPathCommand(_container, _oldParentPath, _buildingBlock).AsInverseFor(this);
      }
   }
}