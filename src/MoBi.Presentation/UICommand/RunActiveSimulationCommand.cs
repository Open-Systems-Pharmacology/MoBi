using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;
using System.Threading.Tasks;

namespace MoBi.Presentation.UICommand
{
   public abstract class RunActiveSimulationCommandBase : IUICommand
   {
      private readonly ISimulationRunner _simulationRunner;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private readonly bool _defineSetting;

      protected RunActiveSimulationCommandBase(ISimulationRunner simulationRunner, IActiveSubjectRetriever activeSubjectRetriever, bool defineSetting)
      {
         _simulationRunner = simulationRunner;
         _activeSubjectRetriever = activeSubjectRetriever;
         _defineSetting = defineSetting;
      }

      public void Execute()
      {
         var activeSimulation = _activeSubjectRetriever.Active<IMoBiSimulation>();
         if (activeSimulation == null) return;

         _simulationRunner.RunSimulation(activeSimulation, _defineSetting);
      }
   }

   public class RunActiveSimulationWithSettingsCommand : RunActiveSimulationCommandBase
   {
      public RunActiveSimulationWithSettingsCommand(ISimulationRunner simulationRunner, IActiveSubjectRetriever activeSubjectRetriever)
         : base(simulationRunner, activeSubjectRetriever, true)
      {
      }
   }

   public class RunActiveSimulationCommand : RunActiveSimulationCommandBase
   {
      public RunActiveSimulationCommand(ISimulationRunner simulationRunner, IActiveSubjectRetriever activeSubjectRetriever)
         : base(simulationRunner, activeSubjectRetriever, false)
      {
      }
   }

   public class RunSimulationCommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly ISimulationRunner _simulationRunner;

      public RunSimulationCommand(ISimulationRunner simulationRunner, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _simulationRunner = simulationRunner;
      }

      protected override void PerformExecute()
      {
         _simulationRunner.RunSimulation(Subject);
      }
   }
}