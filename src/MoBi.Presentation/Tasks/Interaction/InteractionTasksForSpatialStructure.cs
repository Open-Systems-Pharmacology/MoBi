using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForSpatialStructure : IInteractionTasksForBuildingBlock<Module, MoBiSpatialStructure>
   {
   }

   public class InteractionTasksForSpatialStructure : InteractionTasksForBuildingBlock<Module, MoBiSpatialStructure>, IInteractionTasksForSpatialStructure
   {
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;

      public InteractionTasksForSpatialStructure(IInteractionTaskContext interactionTaskContext, IEditTasksForSpatialStructure editTask, IMoBiSpatialStructureFactory spatialStructureFactory)
         : base(interactionTaskContext, editTask)
      {
         _spatialStructureFactory = spatialStructureFactory;
      }

      public override IMoBiCommand GetRemoveCommand(MoBiSpatialStructure objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<MoBiSpatialStructure>(objectToRemove, parent);
      }

      public override IMoBiCommand GetAddCommand(MoBiSpatialStructure itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<MoBiSpatialStructure>(itemToAdd, parent);
      }

      public override MoBiSpatialStructure CreateNewEntity(Module module)
      {
         return _spatialStructureFactory.CreateDefault(spatialStructureName: string.Empty);
      }
   }
}