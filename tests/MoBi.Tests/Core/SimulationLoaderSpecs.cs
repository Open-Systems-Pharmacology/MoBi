using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Exchange;
using MoBi.Helpers;

namespace MoBi.Core
{
   public abstract class concern_for_SimulationLoader : ContextSpecification<ISimulationLoader>
   {
      protected INameCorrector _nameCorrector;
      protected ICloneManagerForSimulation _cloneManager;
      protected MoBiProject _project;
      protected IMoBiSimulation _simulation;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _nameCorrector = A.Fake<INameCorrector>();
         _cloneManager = A.Fake<ICloneManagerForSimulation>();
         _context = A.Fake<IMoBiContext>();
         sut = new SimulationLoader(_cloneManager, _nameCorrector, _context);

         _project = DomainHelperForSpecs.NewProject();
         _simulation = A.Fake<IMoBiSimulation>().WithId("SimId");
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.Project).Returns(_project);
      }
   }

   public class When_adding_a_simulation_to_project_that_does_not_contain_any_simulation_or_building_block : concern_for_SimulationLoader
   {
      private ObserverBuildingBlock _clonedBuildingBlock;
      private SimulationConfiguration _simulationConfiguration;
      private Module _clonedModule;
      private IndividualBuildingBlock _clonedIndividual;
      private SimulationConfiguration _clonedSimulationConfiguration;
      private MoBiSimulation _clonedSimulation;
      private Module _projectModule;
      private IndividualBuildingBlock _projectIndividual;

      protected override void Context()
      {
         base.Context();
         var originalBuildingBlock = new ObserverBuildingBlock().WithId("SP1");
         var module = new Module
         {
            originalBuildingBlock
         };

         _clonedBuildingBlock = new ObserverBuildingBlock().WithId("SP2");
         _clonedModule = new Module
         {
            _clonedBuildingBlock
         };
         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));
         _simulationConfiguration.Individual = new IndividualBuildingBlock().WithId("ind1");

         _clonedIndividual = new IndividualBuildingBlock().WithId("ind2");
         _clonedSimulationConfiguration = new SimulationConfiguration();
         _clonedSimulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(_clonedModule));
         _clonedSimulationConfiguration.Individual = _clonedIndividual;
         
         _projectModule = new Module();
         A.CallTo(() => _context.Clone(_clonedModule)).Returns(_projectModule);

         _projectIndividual = new IndividualBuildingBlock();
         A.CallTo(() => _cloneManager.CloneBuildingBlock(_clonedIndividual)).Returns(_projectIndividual);

         _clonedSimulation = new MoBiSimulation { Configuration = _clonedSimulationConfiguration };
         A.CallTo(() => _cloneManager.CloneSimulation(_simulation)).Returns(_clonedSimulation);
         A.CallTo(() => _simulation.Configuration).Returns(_simulationConfiguration);
         A.CallTo(_nameCorrector).WithReturnType<bool>().Returns(true);

         // Force cloning of the simulation
         _project.AddSimulation(new MoBiSimulation().WithId(_simulation.Id));
      }

      protected override void Because()
      {
         sut.AddSimulationToProject(_simulation);
      }

      [Observation]
      public void should_add_clone_of_individual_to_the_project()
      {
         _project.IndividualsCollection.Single().ShouldBeEqualTo(_projectIndividual);
      }

      [Observation]
      public void should_add_the_simulation_directly_to_the_project_and_its_building_blocks()
      {
         _project.Simulations.ShouldContain(_simulation);
      }

      [Observation]
      public void the_added_simulation_should_be_marked_as_changed()
      {
         _clonedSimulation.HasChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_add_clone_of_module_to_the_project_as_well()
      {
         _project.Modules[0].ShouldBeEqualTo(_projectModule);
      }
   }

   public class When_adding_a_simulation_to_project_that_does_already_exists_by_name_and_the_user_cancels_the_rename : concern_for_SimulationLoader
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_nameCorrector).WithReturnType<bool>().Returns(false);
      }

      protected override void Because()
      {
         sut.AddSimulationToProject(_simulation);
      }

      [Observation]
      public void should_not_add_the_simulation_to_the_project()
      {
         _project.Simulations.Contains(_simulation).ShouldBeFalse();
      }
   }

   public class When_loading_a_simulation_transfer_containing_observed_data_already_available_in_the_project : concern_for_SimulationLoader
   {
      private SimulationTransfer _simulationTransfer;
      private DataRepository _dataRepository;
      private DataRepository _newDataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = new DataRepository("Id");
         _newDataRepository = new DataRepository("New");
         _simulationTransfer = new SimulationTransfer();
         _simulationTransfer.Simulation = _simulation;
         _simulationTransfer.AllObservedData = new List<DataRepository> { _dataRepository, _newDataRepository };
         _project.AddObservedData(_dataRepository);
      }

      protected override void Because()
      {
         sut.AddSimulationToProject(_simulationTransfer);
      }

      [Observation]
      public void should_only_add_new_observed_to_project()
      {
         _project.AllObservedData.ShouldOnlyContain(_dataRepository, _newDataRepository);
      }
   }
}