using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Core.Commands
{
   public class RemoveContainerFromSpatialStructureCommand : RemoveObjectBaseCommand<IContainer, IContainer>
   {
      private IMoBiSpatialStructure _spatialStructure;
      private Cache<string, INeighborhoodBuilder> _removedNeighborhoods;
      public string SpatialStructureId { get; private set; }


      public RemoveContainerFromSpatialStructureCommand(IContainer parent, IContainer childToRemove, IMoBiSpatialStructure spatialStructure)
         : base(parent, childToRemove, spatialStructure)
      {
         _spatialStructure = spatialStructure;
         SpatialStructureId = _spatialStructure.Id;
      }

      protected override void RemoveFrom(IContainer childToRemove, IContainer parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
         _spatialStructure.DiagramManager.RemoveObjectBase(childToRemove);
         _removedNeighborhoods = new Cache<string, INeighborhoodBuilder>(x => x.Id);
         removeNeighborhoods(childToRemove,  _removedNeighborhoods,_spatialStructure,context);
         childToRemove.GetChildren<IEntity>().Each(x => unregisterAllChildrenAndRemoveTheirNeighborHoods(x,_removedNeighborhoods, context));
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _removedNeighborhoods.Each(context.Unregister);
         context.PublishEvent(new RemovedEvent(_removedNeighborhoods));
      }

      private void removeNeighborhoods(IContainer entityToRemove, Cache<string, INeighborhoodBuilder> removedIds, IMoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         var containerTask = context.Resolve<IContainerTask>();
         var neighborhoodsToDelete = containerTask.AllNeighborhoodBuildersConnectedWith(spatialStructure, entityToRemove).ToList();
         foreach (var neighborhoodBuilder in neighborhoodsToDelete)
         {
            if (!removedIds.Contains(neighborhoodBuilder.Id))
            {
               spatialStructure.RemoveNeighborhood(neighborhoodBuilder);
               spatialStructure.DiagramManager.RemoveObjectBase(neighborhoodBuilder);
               removedIds.Add(neighborhoodBuilder);
            }
         }
      }

      private void unregisterAllChildrenAndRemoveTheirNeighborHoods(IObjectBase entityToRemove, Cache<string, INeighborhoodBuilder> removedIds, IMoBiContext context)
      {
         if (removedIds.Contains(entityToRemove.Id))
            return;

         var containerToRemove = entityToRemove as IContainer;

         if (containerToRemove == null)
            return;

         removeNeighborhoods(containerToRemove,removedIds,_spatialStructure, context);
         containerToRemove.GetChildren<IEntity>().Each(x => unregisterAllChildrenAndRemoveTheirNeighborHoods(x, removedIds, context));
      }



      protected override void ClearReferences()
      {
         base.ClearReferences();
         _spatialStructure = null;
         // Keep references to neighborhoods to execute the reverse command.
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _spatialStructure = context.Get<IMoBiSpatialStructure>(SpatialStructureId);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {

         var command = new MoBiMacroCommand()
         {
            ObjectType = ObjectType,
            CommandType = AppConstants.Commands.AddCommand,
            Description = AppConstants.Commands.AddToDescription(ObjectType, _itemToRemove.Name, _parent.Name)
         }.AsInverseFor(this);

         command.Add(new AddContainerToSpatialStructureCommand(_parent, _itemToRemove, _spatialStructure));
         foreach (var neighborhood in _removedNeighborhoods)
         {
            command.Add(new AddContainerToSpatialStructureCommand(_spatialStructure.NeighborhoodsContainer,
                  neighborhood, _spatialStructure));
            
         }
         return command;
      }
   }
}