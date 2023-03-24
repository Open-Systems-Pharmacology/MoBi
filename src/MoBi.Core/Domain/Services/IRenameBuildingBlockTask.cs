using System.Linq;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Services
{
   public interface IRenameBuildingBlockTask
   {
      /// <summary>
      ///    Update build the building block names of simulation using the given building block;
      /// </summary>
      void RenameInSimulationUsingTemplateBuildingBlock(IBuildingBlock templateBuildingBlock);
   }

   public class RenameBuildingBlockTask : IRenameBuildingBlockTask
   {
      private readonly IMoBiProjectRetriever _projectRetriever;

      public RenameBuildingBlockTask(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public void RenameInSimulationUsingTemplateBuildingBlock(IBuildingBlock templateBuildingBlock)
      {
         var allSimulationUsingBuildingBlocks = _projectRetriever.Current.SimulationsCreatedUsing(templateBuildingBlock);

         // TODO rename bb rename in SimulationConfiguration?
         
         // foreach (var usedBuildingBlock in allSimulationUsingBuildingBlocks.Select(x => x.MoBiBuildConfiguration.BuildingInfoForTemplate(templateBuildingBlock)))
         // {
         //    usedBuildingBlock.UntypedBuildingBlock.Name = templateBuildingBlock.Name;
         // }
      }
   }
}