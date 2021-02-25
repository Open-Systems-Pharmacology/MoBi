using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class AddTopContainerCommand : AddObjectBaseCommand<IContainer, IMoBiSpatialStructure>
   {
      public AddTopContainerCommand(IMoBiSpatialStructure parent, IContainer itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override void AddTo(IContainer child, IMoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         spatialStructure.AddTopContainer(child);
         spatialStructure.DiagramManager.AddObjectBase(child);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveTopContainerCommand(_parent, _itemToAdd).AsInverseFor(this);
      }
   }

   public class RemoveTopContainerCommand : RemoveObjectBaseCommand<IContainer, IMoBiSpatialStructure>
   {
      private Cache<string, IObjectBase> _removedIds;

      public RemoveTopContainerCommand(IMoBiSpatialStructure parent, IContainer itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override void RemoveFrom(IContainer childToRemove, IMoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         spatialStructure.RemoveTopContainer(childToRemove);
         _removedIds = new Cache<string, IObjectBase>(x => x.Id);
         removeNeighborhoods(childToRemove, _removedIds, spatialStructure, context);
         unregisterAllChildrenAndRemoveTheirNeighborHoods(childToRemove, _removedIds, spatialStructure, context);
         spatialStructure.DiagramManager.RemoveObjectBase(childToRemove);
      }

      private void removeNeighborhoods(IContainer entityToRemove, ICache<string, IObjectBase> removedIds, IMoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         var containerTask = context.Resolve<IContainerTask>();
         var neighborhoodsToDelete = containerTask.AllNeighborhoodBuildersConnectedWith(spatialStructure, entityToRemove).ToList();
         foreach (var neighborhoodBuilder in neighborhoodsToDelete)
         {
            if (!removedIds.Contains(neighborhoodBuilder.Id))
            {
               spatialStructure.RemoveNeighborhood(neighborhoodBuilder);
               removedIds.Add(neighborhoodBuilder);
            }
         }
      }

      private void unregisterAllChildrenAndRemoveTheirNeighborHoods(IObjectBase entityToRemove, ICache<string, IObjectBase> removedIds, IMoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         if (removedIds.Contains(entityToRemove.Id))
            return;

         var containerToRemove = entityToRemove as IContainer;

         if (containerToRemove == null)
            return;

         removeNeighborhoods(containerToRemove, removedIds, spatialStructure, context);
         containerToRemove.GetChildren<IEntity>().Each(x => unregisterAllChildrenAndRemoveTheirNeighborHoods(x, removedIds, spatialStructure, context));
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _removedIds.Each(context.Unregister);
         context.PublishEvent(new RemovedEvent(_removedIds));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddTopContainerCommand(_parent, _itemToRemove).AsInverseFor(this);
      }
   }
}