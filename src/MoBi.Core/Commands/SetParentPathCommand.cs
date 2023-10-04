using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
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
         updateObjectPathsInSpatialStructure();
         Description = AppConstants.Commands.UpdateParentPath(_container.EntityPath(), (_newParentPath ?? new ObjectPath()).ToPathString());
      }

      private void updateObjectPathsInSpatialStructure()
      {
         //should probably never be null
         var spatialStructure = _buildingBlock as SpatialStructure;
         if (spatialStructure == null)
            return;

         var oldObjectPathString = createContainerPath(_oldParentPath);
         var newObjectPathString = createContainerPath(_newParentPath);

         bool referencesPath(ObjectPath objectPath) => objectPath != null && objectPath.PathAsString.StartsWith(oldObjectPathString);
         
         //we know that the object path exists in this context
         void updatePaths(ObjectPath objectPath) => objectPath.ReplaceWith(objectPath.PathAsString.Replace(oldObjectPathString, newObjectPathString).ToPathArray());

         //this takes care of all parameters in the structure as well as in the neighborhoods
         var allReferencingPaths = spatialStructure.TopContainers
            .Union(spatialStructure.Neighborhoods)
            .SelectMany(x => x.GetPathsReferencing(referencesPath))
            .Select(x => x.path);

         allReferencingPaths.Each(updatePaths);

         var allReferencingPathsInNeighborhood = spatialStructure.Neighborhoods
            .SelectMany(x => new[] {x.FirstNeighborPath, x.SecondNeighborPath})
            .Where(referencesPath);

         allReferencingPathsInNeighborhood.Each(updatePaths);
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