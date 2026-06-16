using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand;

public class AddTimeProfileSimulationAnalysisUICommand : ActiveObjectUICommand<IMoBiSimulation>
{
   private readonly IMoBiSimulationAnalysisCreator _simulationAnalysisCreator;

   public AddTimeProfileSimulationAnalysisUICommand(
      IMoBiSimulationAnalysisCreator simulationAnalysisCreator,
      IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
   {
      _simulationAnalysisCreator = simulationAnalysisCreator;
   }

   protected override void PerformExecute()
   {
      _simulationAnalysisCreator.CreateTimeProfileAnalysisFor(Subject);
   }
}
