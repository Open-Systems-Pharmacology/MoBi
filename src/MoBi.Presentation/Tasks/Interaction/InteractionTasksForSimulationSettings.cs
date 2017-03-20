using System;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForSimulationSettings : InteractionTasksForBuildingBlock<ISimulationSettings>
   {
      private readonly ISimulationSettingsFactory _simulationSettingsFactory;


      public InteractionTasksForSimulationSettings(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<ISimulationSettings> editTask, ISimulationSettingsFactory simulationSettingsFactory) 
         : base(interactionTaskContext, editTask)
      {
         _simulationSettingsFactory = simulationSettingsFactory;
      }

      public override IMoBiCommand Merge(ISimulationSettings buildingBlockToMerge, ISimulationSettings targetBuildingBlock)
      {
         throw new NotSupportedException("Merge is not supported for Simulation Settings");
      }

      public override ISimulationSettings CreateNewEntity(IMoBiProject moleculeBuildingBlock)
      {
         return _simulationSettingsFactory.CreateDefault();
      }
   }
}