using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class UpdateSimulationUICommand : ActiveObjectUICommand<IReadOnlyList<IMoBiSimulation>>
   {
      private readonly IParallelSimulationUpdateTask _parallelSimulationUpdateTask;
      private readonly IMoBiContext _context;

      public UpdateSimulationUICommand(IParallelSimulationUpdateTask parallelSimulationUpdateTask,
         IMoBiContext context,
         IActiveSubjectRetriever activeSubjectRetriever) :
         base(activeSubjectRetriever)
      {
         _parallelSimulationUpdateTask = parallelSimulationUpdateTask;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(_parallelSimulationUpdateTask.UpdateSimulations(Subject));
      }
   }
}