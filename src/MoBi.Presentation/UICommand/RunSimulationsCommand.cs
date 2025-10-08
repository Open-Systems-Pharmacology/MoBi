using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RunSimulationsCommand : ObjectUICommand<IReadOnlyList<IMoBiSimulation>>
   {
      private readonly ISimulationRunner _simulationRunner;

      public RunSimulationsCommand(ISimulationRunner simulationRunner)
      {
         _simulationRunner = simulationRunner;
      }

      protected override async void PerformExecute()
      {
         await _simulationRunner.SecureAwait(x => Task.WhenAll(Subject.Select(simulation => x.RunSimulationAsync(simulation))));
      }
   }
}