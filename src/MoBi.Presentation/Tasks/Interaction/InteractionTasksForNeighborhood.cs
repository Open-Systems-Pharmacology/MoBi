using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForNeighborhood : IInteractionTasksForChildren<IContainer, NeighborhoodBuilder>
   {
      IMoBiCommand Add(ObjectPath firstNeighbor, ObjectPath secondNeighbor);
      IMoBiCommand CreateRemoveCommand(NeighborhoodBuilder neighborhoodBuilder, IBuildingBlock buildingBlock);
   }

   public class InteractionTasksForNeighborhood : InteractionTasksForChildren<IContainer, NeighborhoodBuilder>, IInteractionTasksForNeighborhood
   {
      public InteractionTasksForNeighborhood(IInteractionTaskContext interactionTaskContext, IEditTaskFor<NeighborhoodBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }

      private MoBiSpatialStructure getSpatialStructure()
      {
         return _interactionTaskContext.Active<MoBiSpatialStructure>();
      }

      public IMoBiCommand Add(ObjectPath firstNeighborAbsoluteObjectPath, ObjectPath secondNeighborAbsoluteObjectPath)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = ObjectName,
         };

         var spatialStructure = getSpatialStructure();

         var neighborhoodBuilder = CreateNewEntity(spatialStructure.NeighborhoodsContainer);
         neighborhoodBuilder.FirstNeighborPath = firstNeighborAbsoluteObjectPath;
         neighborhoodBuilder.SecondNeighborPath = secondNeighborAbsoluteObjectPath;

         if (!_editTask.EditEntityModal(neighborhoodBuilder, spatialStructure, macroCommand, spatialStructure))
            return new MoBiEmptyCommand();

         //this needs to happen BEFORE we add the run the add command so that the diagram will refresh as expected
         neighborhoodBuilder.ResolveReference(spatialStructure);
         macroCommand.AddCommand(GetAddCommand(neighborhoodBuilder, spatialStructure.NeighborhoodsContainer, spatialStructure).RunCommand(Context));
         macroCommand.Description = AppConstants.Commands.AddToDescription(ObjectName, neighborhoodBuilder.Name, spatialStructure.Name);
         _editTask.Edit(neighborhoodBuilder);
         return macroCommand;
      }

      protected override void PerformPostAddActions(NeighborhoodBuilder newNeighborhood, IContainer parent, IBuildingBlock buildingBlockToAddTo)
      {
         base.PerformPostAddActions(newNeighborhood, parent, buildingBlockToAddTo);
         //this should be a spatial structure by construction but better be safe
         var spatialStructure = buildingBlockToAddTo as SpatialStructure;
         if (spatialStructure == null) return;

         //let's update the references for the neighbors 
         newNeighborhood.ResolveReference(spatialStructure);
      }

      public IMoBiCommand CreateRemoveCommand(NeighborhoodBuilder entityToRemove, IBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(entityToRemove, null, buildingBlock);
      }

      public override IMoBiCommand GetRemoveCommand(NeighborhoodBuilder entityToRemove, IContainer parent, IBuildingBlock buildingBlock)
      {
         var spatialStructure = getSpatialStructure();
         return new RemoveContainerFromSpatialStructureCommand(spatialStructure.NeighborhoodsContainer, entityToRemove, spatialStructure);
      }

      public override IMoBiCommand GetAddCommand(NeighborhoodBuilder neighborhood, IContainer parent, IBuildingBlock buildingBlock)
      {
         var spatialStructure = getSpatialStructure();
         return new AddContainerToSpatialStructureCommand(parent, neighborhood, spatialStructure);
      }

      public override NeighborhoodBuilder CreateNewEntity(IContainer parentContainer)
      {
         var neighborhoodBuilder = base.CreateNewEntity(parentContainer);
         var moleculeProperties = Context.Create<IContainer>()
            .WithContainerType(ContainerType.Neighborhood)
            .WithMode(ContainerMode.Logical)
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithTag(AppConstants.NeighborhoodTag);
         neighborhoodBuilder.Add(moleculeProperties);
         return neighborhoodBuilder;
      }
   };
}