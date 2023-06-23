using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForContainer : InteractionTasksForContainerBase<IContainer>
   {
      public InteractionTasksForContainer(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IContainer> editTask, IObjectPathFactory objectPathFactory) : base(interactionTaskContext, editTask, objectPathFactory)
      {
      }

      public override IMoBiCommand GetAddCommand(IContainer container, IContainer parent, IBuildingBlock buildingBlock)
      {
         return new AddContainerToSpatialStructureCommand(parent, container, buildingBlock.DowncastTo<MoBiSpatialStructure>());
      }

      public override IMoBiCommand GetRemoveCommand(IContainer entityToRemove, IContainer parent, IBuildingBlock buildingBlock)
      {
         return new RemoveContainerFromSpatialStructureCommand(parent, entityToRemove, buildingBlock.DowncastTo<MoBiSpatialStructure>());
      }

      public override IContainer CreateNewEntity(IContainer container)
      {
         var newEntity = base.CreateNewEntity(container);
         newEntity.ContainerType = ContainerType.Organ;
         return newEntity;
      }
   }
}