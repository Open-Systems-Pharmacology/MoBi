using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForTopContainer : IInteractionTasksForChildren<MoBiSpatialStructure, IContainer>
   {
   }

   public class InteractionTasksForTopContainer : InteractionTasksForContainerBase<MoBiSpatialStructure>, IInteractionTasksForTopContainer
   {
      public InteractionTasksForTopContainer(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IContainer> editTask, IObjectPathFactory objectPathFactory) : base(interactionTaskContext, editTask, objectPathFactory)
      {
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
   }
}