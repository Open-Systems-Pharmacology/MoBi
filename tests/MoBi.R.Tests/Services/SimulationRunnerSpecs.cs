using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using MoBi.CLI.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Services;
using OSPSuite.SimModel;
using OSPSuite.Utility.Events;
using static MoBi.HelpersForTests.DomainHelperForSpecs;
using SimulationRunner = OSPSuite.R.Services.SimulationRunner;
using Simulation = OSPSuite.R.Domain.Simulation;
using static MoBi.R.Tests.HelperForSpecs;

namespace MoBi.R.Tests.Services
{
   public abstract class concern_for_SimulationRunner : ContextForIntegration<ISimulationRunner>
   {
      protected ISimModelManager _simModelManager;
      protected ISimulationResultsCreator _simulationResultsCreator;
      protected ISimulationPersistableUpdater _simulationPersitableUpdater;
      protected IPopulationRunner _populationRunner;
      protected IPopulationTask _populationTask;
      protected IProgressManager _progressManager;
      protected IProjectTask _projectTask;

      protected override void Context()
      {
         _simModelManager = A.Fake<ISimModelManager>();
         _simulationPersitableUpdater = A.Fake<ISimulationPersistableUpdater>();
         _populationRunner = A.Fake<IPopulationRunner>();
         _populationTask = A.Fake<IPopulationTask>();
         _progressManager = A.Fake<IProgressManager>();
         _simulationResultsCreator = new SimulationResultsCreator();
         _projectTask = Api.GetProjectTask();

         sut = new SimulationRunner(_simModelManager, _populationRunner, _simulationResultsCreator, _simulationPersitableUpdater, _populationTask,
            _progressManager);
      }
   }

   public class When_running_a_simulation : concern_for_SimulationRunner
   {
      private IModelCoreSimulation _simulation;
      private SimulationResults _results;
      private SimulationRunResults _simulationRunResults;

      protected override void Context()
      {
         base.Context();
         _simulationRunResults = new SimulationRunResults(Enumerable.Empty<SolverWarning>(),
            IndividualSimulationDataRepositoryFor("Sim"));
         _simulation = new ModelCoreSimulation();
         A.CallTo(_simModelManager).WithReturnType<Task<SimulationRunResults>>().Returns(_simulationRunResults);
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation });
      }

      [Observation]
      public void should_update_the_persistable_flag_in_the_simulation_based_on_the_simulation_settings()
      {
         A.CallTo(() => _simulationPersitableUpdater.UpdateSimulationPersistable(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_return_results_for_the_expected_outputs()
      {
         _results.AllIndividualResults.Count.ShouldBeEqualTo(1);
         _results.AllIndividualResults.ElementAt(0).AllValues.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_running_a_simulation_from_mobi_project : concern_for_SimulationRunner
   {
      private Simulation _simulation;
      private SimulationResults _results;

      protected override void Context()
      {
         base.Context();
         var projectFile = DataTestFileFullPath("SampleProjectWith2Simulations.mbp3");
         var project = _projectTask.LoadProject(projectFile);
         _simulation = _projectTask.AllSimulations(project).FirstOrDefault();
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation });
      }

      [Observation]
      public void should_return_results_for_the_execution_of_the_simulation()
      {
         _results.ShouldNotBeNull();
      }
   }
}