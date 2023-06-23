using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class ShowBuildingBlockDiffUICommand : IUICommand
   {
      private readonly ISimulationComparisonTask _simulationComparisonTask;
      private IBuildingBlock _templateBuildingBlock;
      private IMoBiSimulation _simulation;

      public ShowBuildingBlockDiffUICommand(ISimulationComparisonTask simulationComparisonTask)
      {
         _simulationComparisonTask = simulationComparisonTask;
      }

      public ShowBuildingBlockDiffUICommand Initialize(IBuildingBlock templateBuildingBlock, IMoBiSimulation simulation)
      {
         _templateBuildingBlock = templateBuildingBlock;
         _simulation = simulation;
         return this;
      }

      public void Execute()
      {
         // var buildingBlockInfo = _simulation.MoBiBuildConfiguration.BuildingInfoForTemplate(_templateBuildingBlock);
         // var simulationBuildingBlock = buildingBlockInfo.UntypedBuildingBlock;
         // _simulationComparisonTask.ShowDifferencesBetween(_templateBuildingBlock, simulationBuildingBlock);
      }
   }
}