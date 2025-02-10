using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;

namespace MoBi.Core.Commands
{
   public class RenameContainerCommand : BuildingBlockChangeCommandBase<SpatialStructure>
   {
      private IContainer _container;
      private readonly string _newName;
      private string _oldName;
      private readonly string _containerId;

      public RenameContainerCommand(IContainer container, string newName, SpatialStructure spatialStructure) : base(spatialStructure)
      {
         _container = container;
         _newName = newName;
         _containerId = container.Id;
         ObjectType = ObjectTypes.Container;
         CommandType = AppConstants.Commands.RenameCommand;
         Description = AppConstants.Commands.RenameDescription(_container, newName);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RenameContainerCommand(_container, _oldName, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _container = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         //perform the rename
         _oldName = _container.Name;
         _container.Name = _newName;

         context.PublishEvent(new RenamedEvent(_container));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _container = context.Get<IContainer>(_containerId);
      }
   }
}