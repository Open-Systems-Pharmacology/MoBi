using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.SimModel;
using OSPSuite.Utility.Extensions;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_SimulationRunner : ContextSpecification<ISimulationRunner>
   {
      protected IMoBiContext _context;
      protected IOutputSelectionsRetriever _outputSelectionsRetriever;
      protected ISimulationPersistableUpdater _simulationPersistableUpdater;
      protected IDisplayUnitUpdater _displayUnitUpdater;
      private IMoBiApplicationController _applicationController;
      protected ISimModelManagerFactory _simModelManagerFactory;
      protected IKeyPathMapper _keyPathMapper;
      protected IEntityValidationTask _eventValidationTask;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _outputSelectionsRetriever = A.Fake<IOutputSelectionsRetriever>();
         _simulationPersistableUpdater = A.Fake<ISimulationPersistableUpdater>();
         _displayUnitUpdater = A.Fake<IDisplayUnitUpdater>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _simModelManagerFactory = A.Fake<ISimModelManagerFactory>();
         _keyPathMapper = A.Fake<IKeyPathMapper>();
         _eventValidationTask = A.Fake<IEntityValidationTask>();

         sut = new SimulationRunner(
            _context,
            _applicationController,
            _outputSelectionsRetriever,
            _simulationPersistableUpdater,
            _displayUnitUpdater,
            _simModelManagerFactory,
            _keyPathMapper,
            _eventValidationTask);
      }
   }

   public class When_running_a_simulation_for_which_validation_was_canceled : concern_for_SimulationRunner
   {
      private IMoBiSimulation _simulation;
      private OutputSelections _settings;

      protected override void Context()
      {
         base.Context();
         _settings = A.Fake<OutputSelections>();
         A.CallTo(() => _settings.HasSelection).Returns(false);
         _simulation = A.Fake<IMoBiSimulation>();
         var outputSelections = new OutputSelections();
         outputSelections.AddOutput(new QuantitySelection("A", QuantityType.Drug));


         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.OutputSelections).Returns(outputSelections);
         A.CallTo(() => _eventValidationTask.Validate(_simulation)).Returns(false);
      }

      protected override void Because()
      {
         sut.RunSimulation(_simulation);
      }

      [Observation]
      public void should_not_run_the_simulation()
      {
         A.CallTo(() => _simModelManagerFactory.Create()).MustNotHaveHappened();
      }
   }


   public class When_running_a_simulation_for_which_selection_is_canceled : concern_for_SimulationRunner
   {
      private IMoBiSimulation _simulation;
      private OutputSelections _settings;

      protected override void Context()
      {
         base.Context();
         _settings = A.Fake<OutputSelections>();
         A.CallTo(() => _settings.HasSelection).Returns(false);
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _outputSelectionsRetriever.OutputSelectionsFor(_simulation)).Returns(null);
         A.CallTo(() => _eventValidationTask.Validate(_simulation)).Returns(true);
      }

      protected override void Because()
      {
         sut.RunSimulation(_simulation);
      }

      [Observation]
      public void should_retrieve_the_settings_for_the_simulation_if_they_are_not_available_on_the_simulation()
      {
         A.CallTo(() => _outputSelectionsRetriever.OutputSelectionsFor(_simulation)).MustHaveHappened();
      }
   }

   public class When_running_a_simulation_and_forcing_settings_selection_and_the_user_cancels_the_selection : concern_for_SimulationRunner
   {
      private IMoBiSimulation _simulation;
      private OutputSelections _outputSelections;

      protected override void Context()
      {
         base.Context();
         _outputSelections = A.Fake<OutputSelections>();
         A.CallTo(() => _outputSelections.HasSelection).Returns(true);
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.OutputSelections).Returns(_outputSelections);
         A.CallTo(() => _outputSelectionsRetriever.OutputSelectionsFor(_simulation)).Returns(null);
         A.CallTo(() => _eventValidationTask.Validate(_simulation)).Returns(true);
      }

      protected override void Because()
      {
         sut.RunSimulation(_simulation, defineSettings: true);
      }

      [Observation]
      public void should_retrieve_the_settings_for_the_simulation_even_if_some_where_previously_defined()
      {
         A.CallTo(() => _outputSelectionsRetriever.OutputSelectionsFor(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_not_run_the_simulation()
      {
         A.CallTo(() => _simModelManagerFactory.Create()).MustNotHaveHappened();
      }
   }

   public class When_running_a_simulation_and_forcing_settings_selection_and_the_user_accepts_the_new_the_selection : concern_for_SimulationRunner
   {
      private IMoBiSimulation _simulation;
      private OutputSelections _outputSelections;
      private OutputSelections _newOutputSelection;

      protected override void Context()
      {
         base.Context();
         _outputSelections = new OutputSelections();
         _outputSelections.AddOutput(new QuantitySelection("A", QuantityType.Drug));

         _newOutputSelection = new OutputSelections();
         _newOutputSelection.AddOutput(new QuantitySelection("B", QuantityType.Drug));

         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.OutputSelections).Returns(_outputSelections);
         A.CallTo(() => _outputSelectionsRetriever.OutputSelectionsFor(_simulation)).Returns(_newOutputSelection);
         A.CallTo(() => _eventValidationTask.Validate(_simulation)).Returns(true);
      }

      protected override void Because()
      {
         sut.RunSimulation(_simulation, defineSettings: true);
      }

      [Observation]
      public void should_retrieve_the_settings_for_the_simulation_even_if_some_where_previously_defined()
      {
         A.CallTo(() => _outputSelectionsRetriever.OutputSelectionsFor(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_record_a_command_to_update_the_new_simulation_output_in_the_simulation()
      {
         A.CallTo(() => _context.AddToHistory(A<ICommand>.That.Matches(x => x.IsAnImplementationOf<UpdateOutputSelectionsInSimulationCommand>()))).MustHaveHappened();
      }

      [Observation]
      public void should_have_updated_the_simulation_output_selection()
      {
         _simulation.SimulationSettings.OutputSelections.ShouldBeEqualTo(_newOutputSelection);
      }

      [Observation]
      public void should_run_the_simulation()
      {
         A.CallTo(() => _simModelManagerFactory.Create()).MustHaveHappened();
      }
   }

   public class When_the_simulation_runner_is_running_a_simulation : concern_for_SimulationRunner
   {
      private MoBiSimulation _simulation;
      private OutputSelections _outputSelections;
      private ISimModelManager _simModelManager;
      private SimulationRunResults _simulationResults;
      private DataRepository _newResults;
      private DataRepository _oldResults;
      private IMoleculeBuilder _drug;
      private DataColumn _concentrationColumn;
      private DataColumn _fractionColumn;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         _simModelManager = A.Fake<ISimModelManager>();
         _outputSelections = new OutputSelections();
         _drug = new MoleculeBuilder().WithName("DRUG");
         _drug.AddParameter(new Parameter().WithName(AppConstants.Parameters.MOLECULAR_WEIGHT).WithFormula(new ConstantFormula(400)));
         _outputSelections.AddOutput(new QuantitySelection("A", QuantityType.Drug));
         _simulation.BuildConfiguration = new MoBiBuildConfiguration
         {
            SimulationSettings = new SimulationSettings
            {
               OutputSelections = _outputSelections
            },
            Molecules = new MoleculeBuildingBlock
            {
               _drug
            }
         };

         A.CallTo(() => _simModelManagerFactory.Create()).Returns(_simModelManager);
         _oldResults = new DataRepository("OLD");
         _simulation.ResultsDataRepository = _oldResults;

         _newResults = new DataRepository("NEW");
         _simulationResults = new SimulationRunResults(success: true, warnings: Enumerable.Empty<SolverWarning>(), results: _newResults);
         A.CallTo(() => _simModelManager.RunSimulation(_simulation, null)).Returns(_simulationResults);
         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimension);
         _concentrationColumn = new DataColumn("Drug", DomainHelperForSpecs.ConcentrationDimension, baseGrid);
         _fractionColumn = new DataColumn("Fraction", DomainHelperForSpecs.FractionDimension, baseGrid);
         _newResults.Add(_concentrationColumn);
         _newResults.Add(_fractionColumn);
         A.CallTo(() => _keyPathMapper.MoleculeNameFrom(_concentrationColumn)).Returns(_drug.Name);
         A.CallTo(() => _eventValidationTask.Validate(_simulation)).Returns(true);
      }

      protected override void Because()
      {
         sut.RunSimulation(_simulation);
      }

      [Observation]
      public void should_publish_the_simulation_started_event()
      {
         A.CallTo(() => _context.PublishEvent(A<SimulationRunStartedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_simulation_settings_from_the_selected_output()
      {
         A.CallTo(() => _simulationPersistableUpdater.UpdatePersistableFromSettings(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_run_the_simulation()
      {
         A.CallTo(() => _simModelManager.RunSimulation(_simulation, null)).MustHaveHappened();
      }

      [Observation]
      public void should_move_the_previous_results_as_historical_results_in_the_simulation()
      {
         _simulation.HistoricResults.ShouldContain(_oldResults);
      }

      [Observation]
      public void should_set_the_newly_calculated_results_as_new_results()
      {
         _simulation.ResultsDataRepository.ShouldBeEqualTo(_newResults);
      }

      [Observation]
      public void should_have_updated_the_display_unit_in_the_new_results()
      {
         A.CallTo(() => _displayUnitUpdater.UpdateDisplayUnitsIn(_newResults)).MustHaveHappened();
      }

      [Observation]
      public void should_publish_the_simulation_run_finished_event()
      {
         A.CallTo(() => _context.PublishEvent(A<SimulationRunFinishedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_molecule_weight_of_all_concentration_columns_related_to_a_molecule_with_a_mol_weight_parameter()
      {
         _concentrationColumn.DataInfo.MolWeight.ShouldBeEqualTo(_drug.Parameter(AppConstants.Parameters.MOLECULAR_WEIGHT).Value);
         _fractionColumn.DataInfo.MolWeight.ShouldBeNull();
      }
   }
}