using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForNeighborhood : IInteractionTasksForChildren<IContainer, INeighborhoodBuilder>
   {
      IMoBiCommand Add(IContainer firstNeighbor, IContainer secondNeighbor);
      IMoBiCommand CreateRemoveCommand(INeighborhoodBuilder neighborhoodBuilder, IBuildingBlock buildingBlock);
   }

   public class InteractionTasksForNeighborhood : InteractionTasksForChildren<IContainer, INeighborhoodBuilder>, IInteractionTasksForNeighborhood
   {
      public InteractionTasksForNeighborhood(IInteractionTaskContext interactionTaskContext, IEditTaskFor<INeighborhoodBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }

      private IMoBiSpatialStructure getSpatialStructure()
      {
         return _interactionTaskContext.Active<IMoBiSpatialStructure>();
      }

      public IMoBiCommand Add(IContainer firstNeighbor, IContainer secondNeighbor)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = ObjectName,
         };

         var spatialStructure = getSpatialStructure();

         var neighborhoodBuilder = CreateNewEntity(spatialStructure.NeighborhoodsContainer);
         neighborhoodBuilder.FirstNeighbor = firstNeighbor;
         neighborhoodBuilder.SecondNeighbor = secondNeighbor;

         if (_editTask.EditEntityModal(neighborhoodBuilder, spatialStructure, macroCommand, spatialStructure))
         {
            macroCommand.AddCommand(GetAddCommand(neighborhoodBuilder, spatialStructure.NeighborhoodsContainer, spatialStructure).Run(Context));
            macroCommand.Description = AppConstants.Commands.AddToDescription(ObjectName, neighborhoodBuilder.Name,
               spatialStructure.Name);
            _editTask.Edit(neighborhoodBuilder);
            return macroCommand;
         }

         return new MoBiEmptyCommand();
      }

      public IMoBiCommand CreateRemoveCommand(INeighborhoodBuilder entityToRemove, IBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(entityToRemove, null, buildingBlock);
      }

      public override IMoBiCommand GetRemoveCommand(INeighborhoodBuilder entityToRemove, IContainer parent, IBuildingBlock buildingBlock)
      {
         var spatialStructure = getSpatialStructure();
         return new RemoveContainerFromSpatialStructureCommand(spatialStructure.NeighborhoodsContainer, entityToRemove, spatialStructure);
      }

      public override IMoBiCommand GetAddCommand(INeighborhoodBuilder neighborhood, IContainer parent, IBuildingBlock buildingBlock)
      {
         var spatialStructure = getSpatialStructure();
         return new AddContainerToSpatialStructureCommand(parent, neighborhood, spatialStructure);
      }

      public override INeighborhoodBuilder CreateNewEntity(IContainer parentContainer)
      {
         var neighborhoodBuilder = base.CreateNewEntity(parentContainer);
         var moleculeProperties = Context.Create<IContainer>().WithContainerType(ContainerType.Neighborhood)
            .WithMode(ContainerMode.Logical)
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithTag(AppConstants.NeighborhoodTag);
         neighborhoodBuilder.Add(moleculeProperties);
         return neighborhoodBuilder;
      }
   };
}