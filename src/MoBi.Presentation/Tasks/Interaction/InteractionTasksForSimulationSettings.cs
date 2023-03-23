using System;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForSimulationSettings : InteractionTasksForBuildingBlock<SimulationSettings>
   {
      private readonly ISimulationSettingsFactory _simulationSettingsFactory;


      public InteractionTasksForSimulationSettings(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<SimulationSettings> editTask, ISimulationSettingsFactory simulationSettingsFactory) 
         : base(interactionTaskContext, editTask)
      {
         _simulationSettingsFactory = simulationSettingsFactory;
      }

      public override IMoBiCommand Merge(SimulationSettings buildingBlockToMerge, SimulationSettings targetBuildingBlock)
      {
         throw new NotSupportedException("Merge is not supported for Simulation Settings");
      }

      public override SimulationSettings CreateNewEntity(IMoBiProject moleculeBuildingBlock)
      {
         return _simulationSettingsFactory.CreateDefault();
      }
   }
}