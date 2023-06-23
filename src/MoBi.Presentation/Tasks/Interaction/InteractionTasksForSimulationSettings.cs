using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForSimulationSettings
   {
      IMoBiCommand UpdateDefaultSimulationSettingsInProject();
      void Edit(SimulationSettings simulationSettings);
   }

   public class InteractionTasksForSimulationSettings : InteractionTasksForBuildingBlock<MoBiProject, SimulationSettings>, IInteractionTasksForSimulationSettings
   {
      private readonly ISimulationSettingsFactory _simulationSettingsFactory;

      public InteractionTasksForSimulationSettings(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<SimulationSettings> editTask, ISimulationSettingsFactory simulationSettingsFactory)
         : base(interactionTaskContext, editTask)
      {
         _simulationSettingsFactory = simulationSettingsFactory;
      }

      public override SimulationSettings CreateNewEntity(MoBiProject moleculeBuildingBlock)
      {
         return _simulationSettingsFactory.CreateDefault();
      }

      public override IMoBiCommand GetRemoveCommand(SimulationSettings objectToRemove, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         throw new System.NotImplementedException();
      }

      public override IMoBiCommand GetAddCommand(SimulationSettings itemToAdd, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         throw new System.NotImplementedException();
      }

      public IMoBiCommand UpdateDefaultSimulationSettingsInProject()
      {
         var filename = AskForPKMLFileToOpen();

         if (filename.IsNullOrEmpty())
            return new MoBiEmptyCommand();

         var simulationSettingsBlocks = LoadItems(filename);

         if (simulationSettingsBlocks == null || !simulationSettingsBlocks.Any())
            return new MoBiEmptyCommand();

         return new UpdateDefaultSimulationSettingsInProjectCommand(simulationSettingsBlocks.First()).Run(_interactionTaskContext.Context);
      }

      public void Edit(SimulationSettings simulationSettings)
      {
         EditBuildingBlock(simulationSettings);
      }
   }
}