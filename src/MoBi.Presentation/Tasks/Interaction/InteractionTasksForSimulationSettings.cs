using System;
using System.Collections.Generic;
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
      void UpdateDefaultSimulationSettingsInProject(OutputSchema outputSchema, SolverSettings solverSettings);
      void UpdateDefaultOutputSelectionsInProject(IReadOnlyList<QuantitySelection> selectedQuantities);
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
         throw new NotImplementedException();
      }

      public override IMoBiCommand GetAddCommand(SimulationSettings itemToAdd, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         throw new NotImplementedException();
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

      public void UpdateDefaultSimulationSettingsInProject(OutputSchema outputSchema, SolverSettings solverSettings)
      {
         Context.CurrentProject.SimulationSettings.OutputSchema = Context.Clone(outputSchema);
         Context.CurrentProject.SimulationSettings.Solver = Context.Clone(solverSettings);

         Context.PublishEvent(new DefaultSimulationSettingsUpdatedEvent(Context.CurrentProject.SimulationSettings));
      }

      public void UpdateDefaultOutputSelectionsInProject(IReadOnlyList<QuantitySelection> selectedQuantities)
      {
         var projectSelections = Context.CurrentProject.SimulationSettings.OutputSelections;
         projectSelections.Clear();
         selectedQuantities.Each(x => projectSelections.AddOutput(x.Clone()));
         Context.PublishEvent(new DefaultSimulationSettingsUpdatedEvent(Context.CurrentProject.SimulationSettings));
      }

      public void Edit(SimulationSettings simulationSettings)
      {
         EditBuildingBlock(simulationSettings);
      }
   }
}