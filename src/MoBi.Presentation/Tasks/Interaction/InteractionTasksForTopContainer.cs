using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using MoBi.Presentation.Mappers;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForTopContainer : IInteractionTasksForChildren<MoBiSpatialStructure, IContainer>
   {
      ObjectPath BuildObjectPath(IContainer container);
   }

   public class InteractionTasksForTopContainer : InteractionTasksForContainerBase<MoBiSpatialStructure>, IInteractionTasksForTopContainer
   {
      private readonly IInteractionTasksForChildren<IContainer, IContainer> _interactionTaskForNeighborhood;

      public InteractionTasksForTopContainer(
         IInteractionTaskContext interactionTaskContext,
         IEditTaskFor<IContainer> editTask,
         IObjectPathFactory objectPathFactory,
         IInteractionTasksForChildren<IContainer, IContainer> interactionTaskForNeighborhood,
         IParameterValuesTask parameterValuesTask) : base(interactionTaskContext, editTask, objectPathFactory, parameterValuesTask)
      {
         _interactionTaskForNeighborhood = interactionTaskForNeighborhood;
      }

      public override IMoBiCommand GetRemoveCommand(IContainer entityToRemove, MoBiSpatialStructure parent, IBuildingBlock buildingBlock)
      {
         return new RemoveTopContainerCommand((MoBiSpatialStructure) buildingBlock, entityToRemove);
      }

      public override IMoBiCommand GetAddCommand(IContainer container, MoBiSpatialStructure spatialStructure, IBuildingBlock buildingBlock)
      {
         return new AddTopContainerCommand(spatialStructure, container);
      }

      public override IContainer CreateNewEntity(MoBiSpatialStructure spatialStructure)
      {
         var newEntity = base.CreateNewEntity(spatialStructure);
         newEntity.ContainerType = ContainerType.Organism;
         return newEntity;
      }

      public ObjectPath BuildObjectPath(IContainer container)
      {
         var objectPath = _objectPathFactory.CreateAbsoluteObjectPath(container);
         if (container.ParentPath == null)
            return objectPath;

         objectPath.AddAtFront(container.ParentPath);
         return objectPath;
      }  

      protected override IMoBiCommand AddNeighborhoodsToSpatialStructure(IReadOnlyList<NeighborhoodBuilder> neighborhoods, MoBiSpatialStructure spatialStructure)
      {
         //we need to make sure that we use the correct interaction task for the spatial structure in order to add the neighborhoods to the top container
         return _interactionTaskForNeighborhood.AddTo(neighborhoods, spatialStructure.NeighborhoodsContainer, spatialStructure);
      }
   }
}