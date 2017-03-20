using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_SimulationRunner : ContextSpecification<ISimulationRunner>
   {
      private IMoBiContext _context;
      protected IOutputSelectionsRetriever _outputSelectionsRetriever;
      private ISimModelExporter _simModelExporter;
      private IDataNamingService _dataNamingService;
      private ISimulationPersistableUpdater _simulationPersistableUpdater;
      private IDisplayUnitUpdater _displayUnitUpdater;
      private IMoBiApplicationController _applicationController;
      private IDisplayUnitRetriever _displayUnitRetreiver;
      private ISimModelSimulationFactory _simModelSimulationFactory;

      protected override void Context()
      {
         _displayUnitRetreiver = A.Fake<IDisplayUnitRetriever>();
         _context = A.Fake<IMoBiContext>();
         _outputSelectionsRetriever = A.Fake<IOutputSelectionsRetriever>();
         _simModelExporter = A.Fake<ISimModelExporter>();
         _dataNamingService = A.Fake<IDataNamingService>();
         _simulationPersistableUpdater = A.Fake<ISimulationPersistableUpdater>();
         _displayUnitUpdater = A.Fake<IDisplayUnitUpdater>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _simModelSimulationFactory= A.Fake<ISimModelSimulationFactory>();

         sut = new SimulationRunner(_context, _applicationController,
            _outputSelectionsRetriever, _simModelExporter, _dataNamingService, _simulationPersistableUpdater, _displayUnitUpdater,  _displayUnitRetreiver, _simModelSimulationFactory);
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

   public class When_running_a_simulation_with_persistable_parameters : concern_for_SimulationRunner
   {
      private IMoBiSimulation _simulation;
      private Parameter _p1;
      private QuantitySelection _selection;
      private OutputSelections _outputSelections;

      protected override void Context()
      {
         base.Context();
         _outputSelections = A.Fake<OutputSelections>();
         _simulation = A.Fake<IMoBiSimulation>();
         _p1 = new Parameter {Persistable = true};
         _simulation.Model.Root = new Container();
         _selection = new QuantitySelection("A", QuantityType.Parameter);
         _simulation.Model.Root.Add(_p1);
         A.CallTo(() => _outputSelectionsRetriever.SelectionFrom(_p1)).Returns(_selection);
         A.CallTo(() => _outputSelectionsRetriever.OutputSelectionsFor(_simulation)).Returns(null);
         A.CallTo(() => _simulation.OutputSelections).Returns(_outputSelections);
      }

      protected override void Because()
      {
         sut.RunSimulation(_simulation);
      }

      [Observation]
      public void should_add_the_parameters_to_the_output_selection()
      {
         A.CallTo(() => _outputSelections.AddOutput(_selection)).MustHaveHappened();
      }
   }

   public class When_running_a_simulation_and_forcing_settings_selection : concern_for_SimulationRunner
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
   }
}