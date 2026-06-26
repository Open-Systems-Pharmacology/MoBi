using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands.Core;

namespace MoBi.Presentation.Tasks;

public interface IParallelSimulationUpdateTask
{
   ICommand UpdateSimulations(IReadOnlyList<IMoBiSimulation> simulationsToUpdate);
}

public class ParallelSimulationUpdateTask : IParallelSimulationUpdateTask
{
   private readonly IMoBiContext _context;
   private readonly ISimulationUpdateTask _simulationUpdateTask;

   public ParallelSimulationUpdateTask(IMoBiContext context, ISimulationUpdateTask simulationUpdateTask)
   {
      _context = context;
      _simulationUpdateTask = simulationUpdateTask;
   }

   public ICommand UpdateSimulations(IReadOnlyList<IMoBiSimulation> simulationsToUpdate)
   {
      if (simulationsToUpdate.Count == 1)
         return _simulationUpdateTask.UpdateSimulation(simulationsToUpdate[0]);

      using (var presenter = _context.Resolve<ISimulationUpdatePresenter>())
      {
         return presenter.Start(simulationsToUpdate);
      }
   }
}