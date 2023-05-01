using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForSpatialStructure : InteractionTasksForBuildingBlock<Module, MoBiSpatialStructure>
   {
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;

      public InteractionTasksForSpatialStructure(IInteractionTaskContext interactionTaskContext, IEditTasksForSpatialStructure editTask, IMoBiSpatialStructureFactory spatialStructureFactory)
         : base(interactionTaskContext, editTask)
      {
         _spatialStructureFactory = spatialStructureFactory;
      }

      public override IMoBiCommand Remove(MoBiSpatialStructure buildingBlockToRemove, Module module, IBuildingBlock buildingBlock, bool silent)
      {
         var referringStartValuesBuildingBlocks = Context.CurrentProject.ReferringStartValuesBuildingBlocks(buildingBlockToRemove);
         if (referringStartValuesBuildingBlocks.Any())
         {
            throw new MoBiException(AppConstants.CannotRemoveBuildingBlockFromProject(buildingBlockToRemove.Name, referringStartValuesBuildingBlocks.Select(bb => bb.Name)));
         }

         return base.Remove(buildingBlockToRemove, buildingBlockToRemove.Module, buildingBlock, silent);
      }

      public override MoBiSpatialStructure CreateNewEntity(Module module)
      {
         return _spatialStructureFactory.CreateDefault(spatialStructureName: string.Empty);
      }
   }
}