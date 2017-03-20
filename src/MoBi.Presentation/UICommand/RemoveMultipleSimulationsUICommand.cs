using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class RemoveMultipleSimulationsUICommand : ObjectUICommand<IReadOnlyList<IMoBiSimulation>>
   {
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForSimulation _simulationTasks;

      public RemoveMultipleSimulationsUICommand(IMoBiContext context, IInteractionTasksForSimulation simulationTasks)
      {
         _context = context;
         _simulationTasks = simulationTasks;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(_simulationTasks.RemoveMultipleSimulations(Subject));
      }
   }
}