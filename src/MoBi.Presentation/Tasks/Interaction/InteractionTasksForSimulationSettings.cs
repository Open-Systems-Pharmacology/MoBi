using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForSimulationSettings
   {
      void LoadDefaultSimulationSettingsInProject();
      void Edit(SimulationSettings simulationSettings);
      void UpdateDefaultSimulationSettingsInProject(SolverSettings solverSettings, OutputSchema outputSchema);
      void UpdateDefaultOutputSelectionsInProject(OutputSelections outputSelections);
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

      public void LoadDefaultSimulationSettingsInProject()
      {
         var filename = AskForPKMLFileToOpen();

         if (filename.IsNullOrEmpty())
            return;

         var simulationSettingsBlocks = LoadItems(filename);

         if (simulationSettingsBlocks == null || !simulationSettingsBlocks.Any())
            return;

         replaceSimulationSettingsInProject(simulationSettingsBlocks.First());
      }

      private void replaceSimulationSettingsInProject(SimulationSettings simulationSettings)
      {
         Context.CurrentProject.SimulationSettings = simulationSettings;
         Context.PublishEvent(new DefaultSimulationSettingsUpdatedEvent(Context.CurrentProject.SimulationSettings));
      }

      public void UpdateDefaultSimulationSettingsInProject(SolverSettings solverSettings, OutputSchema outputSchema)
      {
         Context.CurrentProject.SimulationSettings.OutputSchema = outputSchema;
         Context.CurrentProject.SimulationSettings.Solver = solverSettings;

         Context.PublishEvent(new DefaultSimulationSettingsUpdatedEvent(Context.CurrentProject.SimulationSettings));
      }

      public void UpdateDefaultOutputSelectionsInProject(OutputSelections outputSelections)
      {
         Context.CurrentProject.SimulationSettings.OutputSelections = outputSelections;
         Context.PublishEvent(new DefaultSimulationSettingsUpdatedEvent(Context.CurrentProject.SimulationSettings));
      }

      public void Edit(SimulationSettings simulationSettings)
      {
         EditBuildingBlock(simulationSettings);
      }
   }
}