using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class RenameContainerCommand : BuildingBlockChangeCommandBase<ISpatialStructure>
   {
      private IContainer _container;
      private readonly string _newName;
      private string _oldName;
      private readonly string _containerId;

      public RenameContainerCommand(IContainer container, string newName, ISpatialStructure spatialStructure) : base(spatialStructure)
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
         var objectPathFactory = context.ObjectPathFactory;
         var containerPath = objectPathFactory.CreateAbsoluteObjectPath(_container);
         var allNeighborhoodsConnectedToContainer = _buildingBlock.AllNeighborhoodBuildersConnectedWith(containerPath);
         renameContainer(context);
         //the container has been renamed, we need to update the path of all neighborhoods connected to it
         var newContainerPath = objectPathFactory.CreateAbsoluteObjectPath(_container);
         allNeighborhoodsConnectedToContainer.Each(x =>
         {
            if (x.FirstNeighborPath.Equals(containerPath))
               x.FirstNeighborPath = newContainerPath;

            if (x.SecondNeighborPath.Equals(containerPath))
               x.SecondNeighborPath = newContainerPath;
         });

         //no need to update referenced in the neighborhood as they have not changed

         context.PublishEvent(new RenamedEvent(_container));
      }

      private void renameContainer(IMoBiContext context)
      {
         _oldName = _container.Name;
         _container.Name = _newName;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _container = context.Get<IContainer>(_containerId);
      }
   }
}