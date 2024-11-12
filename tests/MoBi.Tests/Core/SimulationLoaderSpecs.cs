using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.Core
{
   public abstract class concern_for_SimulationLoader : ContextSpecification<ISimulationLoader>
   {
      protected INameCorrector _nameCorrector;
      protected ICloneManagerForSimulation _cloneManager;
      protected MoBiProject _project;
      protected IMoBiSimulation _simulation;
      protected IMoBiContext _context;
      protected SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         _nameCorrector = A.Fake<INameCorrector>();
         _cloneManager = A.Fake<ICloneManagerForSimulation>();
         _context = A.Fake<IMoBiContext>();
         sut = new SimulationLoader(_cloneManager, _nameCorrector, _context);

         _project = DomainHelperForSpecs.NewProject();
         _simulation = new MoBiSimulation().WithId("SimId");
         _simulationConfiguration = new SimulationConfiguration();
         var originalBuildingBlock = new ObserverBuildingBlock().WithId("SP1");
         var module = new Module
         {
            originalBuildingBlock
         }.WithName("moduleName");
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));
         _simulationConfiguration.Individual = new IndividualBuildingBlock().WithId("ind1");
         _simulation.Configuration = _simulationConfiguration;
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.Project).Returns(_project);
      }
   }

   public class When_adding_a_simulation_to_project_where_the_simulation_name_is_unique_but_module_names_collide : concern_for_SimulationLoader
   {
      protected override void Context()
      {
         base.Context();
         _project.AddModule(new Module().WithName("moduleName"));
         _simulation.Name = "Sim1";

         A.CallTo(() => _cloneManager.CloneSimulationConfiguration(A<SimulationConfiguration>._)).Returns(_simulationConfiguration);
      }

      protected override void Because()
      {
         sut.AddSimulationToProject(_simulation);
      }

      [Observation]
      public void the_added_module_should_have_been_renamed()
      {
         A.CallTo(() => _nameCorrector.AutoCorrectName(A<IEnumerable<string>>._, _simulation.Modules.First())).MustHaveHappened();
      }
   }

   public class When_adding_a_simulation_to_project_that_contains_a_simulation_with_the_same_name : concern_for_SimulationLoader
   {
      private ObserverBuildingBlock _clonedBuildingBlock;

      private Module _clonedModule;
      private IndividualBuildingBlock _clonedIndividual;
      private SimulationConfiguration _clonedSimulationConfiguration;
      private MoBiMacroCommand _commands;

      protected override void Context()
      {
         base.Context();
         _project.AddModule(new Module().WithName("moduleName"));
         _project.AddModule(new Module().WithName("newModuleName"));

         _clonedBuildingBlock = new ObserverBuildingBlock().WithId("SP2");
         _clonedModule = new Module
         {
            _clonedBuildingBlock
         };

         _clonedIndividual = new IndividualBuildingBlock().WithId("ind2");
         _clonedSimulationConfiguration = new SimulationConfiguration();
         _clonedSimulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(_clonedModule));
         _clonedSimulationConfiguration.Individual = _clonedIndividual;
         _simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module().WithName("Sim1 1")));
         _simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module().WithName("Sim1 2")));
         _simulation.Name = "Sim1";

         A.CallTo(() => _cloneManager.CloneSimulationConfiguration(_simulationConfiguration)).Returns(_clonedSimulationConfiguration);

         A.CallTo(_nameCorrector).WithReturnType<bool>()
            .Invokes(() => { _simulation.Name = "new SimName"; }).Returns(true);

         A.CallTo(() => _nameCorrector.CorrectName(A<IEnumerable<IObjectBase>>.Ignored, _simulation))
            .Returns(true);
      }

      protected override void Because()
      {
         _commands = sut.AddSimulationToProject(_simulation) as MoBiMacroCommand;
      }

      [Observation]
      public void module_add_commands_are_added_silently()
      {
         _commands.All().OfType<AddModuleCommand>().All(command => command.Silent).ShouldBeTrue();
      }

      [Observation]
      public void should_add_clone_of_individual_to_the_project()
      {
         _project.IndividualsCollection.Single().ShouldBeEqualTo(_clonedIndividual);
      }

      [Observation]
      public void should_add_the_simulation_directly_to_the_project_and_its_building_blocks()
      {
         _project.Simulations.ShouldContain(_simulation);
      }

      [Observation]
      public void the_added_simulation_should_be_marked_as_changed()
      {
         _simulation.HasChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_add_clone_of_module_to_the_project_as_well()
      {
         _project.Modules.ShouldContain(_clonedModule);
      }

      [Observation]
      public void the_modules_in_the_simulation_named_after_simulation_have_been_changed()
      {
         _simulation.Modules[1].Name.ShouldBeEqualTo($"new SimName 1");
         _simulation.Modules[2].Name.ShouldBeEqualTo($"new SimName 2");
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