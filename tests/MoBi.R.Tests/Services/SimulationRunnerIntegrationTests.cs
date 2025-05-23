using System.IO;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.R.Services;
using OSPSuite.Utility;
using static MoBi.R.Tests.DomainHelperForSpecs;

namespace MoBi.R.Tests.Services
{
   public abstract class concern_for_SimulationRunnerIntegration : ContextForIntegration<ISimulationRunner>
   {
      protected string _populationFile;
      protected ISimulationPersister _simulationPersister;
      protected string _simulationFile;
      protected IModelCoreSimulation _simulation;
      protected IPopulationTask _populationTask;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _populationFile = TestFileFullPath("pop_10.csv");
         _simulationFile = TestFileFullPath("S1.pkml");
         _simulationPersister = OSPSuite.R.Api.GetSimulationPersister();
         _populationTask = OSPSuite.R.Api.GetPopulationTask();
         sut = OSPSuite.R.Api.GetSimulationRunner();

         _simulation = _simulationPersister.LoadSimulation(_simulationFile);
      }
   }

   public class
      When_performing_a_population_simulation_run_with_a_file_containing_only_a_subset_of_the_individual : concern_for_SimulationRunnerIntegration
   {
      private string _outputFolder;
      private string _subPopulationFile;
      private IndividualValuesCache _subPopulation;
      private SimulationResults _results;

      protected override void Context()
      {
         base.Context();
         var tmpFile = FileHelper.GenerateTemporaryFileName();
         _outputFolder = new FileInfo(tmpFile).DirectoryName;
         //Take the 3 out of 5 which would have indices 6 and 7 
         _subPopulationFile = _populationTask.SplitPopulation(_populationFile, 5, _outputFolder, "TestSplit")[3];
         _subPopulation = _populationTask.ImportPopulation(_subPopulationFile);
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation, Population = _subPopulation });
      }

      [Observation]
      public void should_create_results_matching_the_individual_ids_in_the_population()
      {
         _results.AllIndividualIds().ShouldOnlyContain(6, 7);
      }
   }

   public class When_performing_a_population_simulation_run : concern_for_SimulationRunnerIntegration
   {
      private IndividualValuesCache _population;
      private SimulationResults _results;

      protected override void Context()
      {
         base.Context();
         _population = _populationTask.ImportPopulation(_populationFile);
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population });
      }

      [Observation]
      public void should_create_results_matching_the_individual_ids_in_the_population()
      {
         _results.AllIndividualIds().ShouldOnlyContain(Enumerable.Range(0, _population.Count));
      }
   }
}