using System;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
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

         //perform the rename
         _oldName = _container.Name;
         _container.Name = _newName;

         //the container has been renamed, we need to update the path of all neighborhoods connected to it
         //no need to update referenced in the neighborhood as they have not changed
         var updateNeighborhoods = updateNeighborhoodsDef(containerPath, objectPathFactory.CreateAbsoluteObjectPath(_container));
         updateNeighborhoods(x => x.FirstNeighborPath);
         updateNeighborhoods(x => x.SecondNeighborPath);

         context.PublishEvent(new RenamedEvent(_container));
      }

      private Action<Func<NeighborhoodBuilder, ObjectPath>> updateNeighborhoodsDef(string oldContainerPath, string newContainerPath) =>
         (neighborPathFunc) =>
         {
            _buildingBlock.Neighborhoods.Select(x => new
               {
                  NeihgborPath = neighborPathFunc(x),
                  NeighborPathString = neighborPathFunc(x).ToString()
               })
               .Where(x => x.NeighborPathString.StartsWith(oldContainerPath))
               .Each(x => x.NeihgborPath.ReplaceWith(x.NeighborPathString.Replace(oldContainerPath, newContainerPath).ToPathArray()));
         };

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _container = context.Get<IContainer>(_containerId);
      }
   }
}