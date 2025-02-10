using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

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
         updateObjectPathsInSpatialStructure(context);
         updateDiagram();

         Description = AppConstants.Commands.UpdateParentPath(_container.EntityPath(), (_newParentPath ?? new ObjectPath()).ToPathString());
      }

      private void updateDiagram()
      {
         if (_buildingBlock is MoBiSpatialStructure spatialStructure) 
            spatialStructure.DiagramManager.RefreshObjectBase(_container);
      }

      private void updateObjectPathsInSpatialStructure(IMoBiContext context)
      {
         //should probably never be null
         if (!(_buildingBlock is SpatialStructure spatialStructure))
            return;

         var oldContainerPathString = createContainerPath(_oldParentPath);
         var newContainerPathString = createContainerPath(_newParentPath);

         bool referencesPath(ObjectPath objectPath) => objectPath != null && objectPath.PathAsString.StartsWith(oldContainerPathString);

         //we know that the object path exists in this context
         void updatePaths(ObjectPath objectPath)
         {
            var updatedPath = $"{newContainerPathString}{objectPath.PathAsString.Substring(oldContainerPathString.Length)}";
            objectPath.ReplaceWith(updatedPath.ToPathArray());
         }

         //this takes care of all parameters in the structure as well as in the neighborhoods
         spatialStructure.TopContainers
            .Union(spatialStructure.Neighborhoods)
            .SelectMany(x => x.GetPathsReferencing(referencesPath))
            .Select(x => x.Path)
            .Each(updatePaths);

         var allReferencingPathsInNeighborhood = spatialStructure.Neighborhoods
            .SelectMany(x => new[] {new {neighborhood = x, neighbor = x.FirstNeighborPath}, new {neighborhood = x, neighbor = x.SecondNeighborPath}})
            .Where(x => referencesPath(x.neighbor));


         allReferencingPathsInNeighborhood.Each(x =>
         {
            updatePaths(x.neighbor);
            context.PublishEvent(new NeighborhoodChangedEvent(x.neighborhood));
         });
      }

      private string createContainerPath(ObjectPath parentPath)
      {
         var objectPath = (parentPath ?? new ObjectPath()).Clone<ObjectPath>().AndAdd(_container.Name);
         return objectPath.ToPathString();
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