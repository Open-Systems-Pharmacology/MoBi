using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface IEditBuildingBlockStarter
   {
      void EditBuildingBlock(IBuildingBlock buildingBlock);
      void EditMolecule(IMoleculeBuildingBlock buildingBlock, IMoleculeBuilder moleculeBuilder);
   }

   public class EditBuildingBlockStarter : IEditBuildingBlockStarter
   {
      private readonly IBuildingBlockTaskRetriever _buildingBlockTaskRetriever;
      private readonly IInteractionTasksForMoleculeBuildingBlock _editTasksForMoleculeBuildingBlock;

      public EditBuildingBlockStarter(IBuildingBlockTaskRetriever buildingBlockTaskRetriever, IInteractionTasksForMoleculeBuildingBlock editTasksForMoleculeBuildingBlock)
      {
         _buildingBlockTaskRetriever = buildingBlockTaskRetriever;
         _editTasksForMoleculeBuildingBlock = editTasksForMoleculeBuildingBlock;
      }

      public void EditBuildingBlock(IBuildingBlock buildingBlock)
      {
         var task = _buildingBlockTaskRetriever.TaskFor(buildingBlock);
         task.EditBuildingBlock(buildingBlock);
      }

      public void EditMolecule(IMoleculeBuildingBlock buildingBlock, IMoleculeBuilder moleculeBuilder)
      {
         _editTasksForMoleculeBuildingBlock.Edit(buildingBlock, moleculeBuilder);
      }
   }
}