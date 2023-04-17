using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForSpatialStructure : InteractionTasksForBuildingBlock<MoBiSpatialStructure>
   {
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;

      public InteractionTasksForSpatialStructure(IInteractionTaskContext interactionTaskContext, IEditTasksForSpatialStructure editTask, IMoBiSpatialStructureFactory spatialStructureFactory)
         : base(interactionTaskContext, editTask)
      {
         _spatialStructureFactory = spatialStructureFactory;
      }

      public override IMoBiCommand Remove(MoBiSpatialStructure buildingBlockToRemove, MoBiProject project, IBuildingBlock buildingBlock, bool silent)
      {
         var referringStartValuesBuildingBlocks = project.ReferringStartValuesBuildingBlocks(buildingBlockToRemove);
         if (referringStartValuesBuildingBlocks.Any())
         {
            throw new MoBiException(AppConstants.CannotRemoveBuildingBlockFromProject(buildingBlockToRemove.Name, referringStartValuesBuildingBlocks.Select(bb => bb.Name)));
         }
         return base.Remove(buildingBlockToRemove, project, buildingBlock, silent);
      }


      public override IMoBiCommand Merge(MoBiSpatialStructure buildingBlockToMerge, MoBiSpatialStructure targetBuildingBlock)
      {
         throw new MoBiException(AppConstants.Exceptions.MergingSpatialStructuresIsNotSupported);
      }

      public override MoBiSpatialStructure CreateNewEntity(MoBiProject moleculeBuildingBlock)
      {
         return _spatialStructureFactory.CreateDefault(spatialStructureName:string.Empty);
      }
   }
}