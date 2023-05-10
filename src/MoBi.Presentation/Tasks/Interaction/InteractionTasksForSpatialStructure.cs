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
         var referringStartValuesBuildingBlocks = module.ReferringStartValueBuildingBlocks(buildingBlockToRemove);
         if (referringStartValuesBuildingBlocks.Any())
         {
            throw new MoBiException(AppConstants.CannotRemoveBuildingBlockFromModule(buildingBlockToRemove.Name, referringStartValuesBuildingBlocks.Select(bb => bb.Name)));
         }

         return base.Remove(buildingBlockToRemove, buildingBlockToRemove.Module, buildingBlock, silent);
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