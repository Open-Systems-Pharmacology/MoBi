using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForTopContainer : IInteractionTasksForChildren<IMoBiSpatialStructure, IContainer>
   {
   }

   public class InteractionTasksForTopContainer : InteractionTasksForContainerBase<IMoBiSpatialStructure>, IInteractionTasksForTopContainer
   {
      public InteractionTasksForTopContainer(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IContainer> editTask, IObjectPathFactory objectPathFactory) : base(interactionTaskContext, editTask, objectPathFactory)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IContainer entityToRemove, IMoBiSpatialStructure parent, IBuildingBlock buildingBlock)
      {
         return new RemoveTopContainerCommand((IMoBiSpatialStructure) buildingBlock, entityToRemove);
      }

      public override IMoBiCommand GetAddCommand(IContainer container, IMoBiSpatialStructure spatialStructure, IBuildingBlock buildingBlock)
      {
         return new AddTopContainerCommand(spatialStructure, container);
      }

      public override IContainer CreateNewEntity(IMoBiSpatialStructure spatialStructure)
      {
         var newEntity = base.CreateNewEntity(spatialStructure);
         newEntity.ContainerType = ContainerType.Organism;
         return newEntity;
      }
   }
}