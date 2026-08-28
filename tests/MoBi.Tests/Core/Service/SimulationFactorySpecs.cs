using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SimulationFactory : ContextForIntegration<SimulationFactory>
   {
      protected IIdGenerator _idGenerator;
      protected ICreationMetaDataFactory _metaDataFactory;
      protected ISimulationParameterOriginIdUpdater _parameterIdUpdater;
      private IDiagramManagerFactory _diagramManagerFactory;
      private ISimulationConfigurationFactory _simulationConfigurationFactory;
      private IDimensionValidator _dimensionValidator;
      private IModelConstructor _modelConstructor;
      private IMoBiContext _context;

      protected override void Context()
      {
         _idGenerator = IoC.Resolve<IIdGenerator>();
         _metaDataFactory = IoC.Resolve<ICreationMetaDataFactory>();
         _parameterIdUpdater = A.Fake<ISimulationParameterOriginIdUpdater>();
         _diagramManagerFactory = IoC.Resolve<IDiagramManagerFactory>();
         _simulationConfigurationFactory = IoC.Resolve<ISimulationConfigurationFactory>();
         _dimensionValidator = IoC.Resolve<IDimensionValidator>();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _context = IoC.Resolve<IMoBiContext>();
         _context.NewProject();

         sut = new SimulationFactory(_idGenerator,
            _metaDataFactory,
            _parameterIdUpdater,
            _diagramManagerFactory,
            _simulationConfigurationFactory,
            _dimensionValidator,
            _modelConstructor,
            _context);
      }
   }

   class When_creating_a_simulation_from_a_configuration : concern_for_SimulationFactory
   {
      private SimulationConfiguration _simulationConfiguration;
      private IMoBiSimulation _result;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();
      }

      protected override void Because()
      {
         var (simulation, _) = sut.CreateSimulationAndValidate(_simulationConfiguration, "name");
         _result = simulation;
      }

      [Observation]
      public void the_simulation_must_have_cloned_the_configuration()
      {
         _simulationConfiguration.ShouldNotBeEqualTo(_result.Configuration);
         _simulationConfiguration.ModuleConfigurations.Count.ShouldBeEqualTo(_result.Configuration.ModuleConfigurations.Count);
      }
   }

   class When_creating_several_simulations : concern_for_SimulationFactory
   {
      private IMoBiContext _fakeContext;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         _fakeContext = A.Fake<IMoBiContext>();
         //each call must get its own clone manager: it carries per-operation state (FormulaCache)
         A.CallTo(() => _fakeContext.Resolve<ICloneManagerForBuildingBlock>()).ReturnsLazily(() => IoC.Resolve<ICloneManagerForBuildingBlock>());
         sut = new SimulationFactory(_idGenerator,
            _metaDataFactory,
            _parameterIdUpdater,
            IoC.Resolve<IDiagramManagerFactory>(),
            IoC.Resolve<ISimulationConfigurationFactory>(),
            IoC.Resolve<IDimensionValidator>(),
            IoC.Resolve<IModelConstructor>(),
            _fakeContext);
         _simulationConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();
      }

      protected override void Because()
      {
         sut.CreateSimulationAndValidate(_simulationConfiguration, "one");
         sut.CreateSimulationAndValidate(_simulationConfiguration, "two");
      }

      [Observation]
      public void should_resolve_a_fresh_clone_manager_for_each_simulation()
      {
         A.CallTo(() => _fakeContext.Resolve<ICloneManagerForBuildingBlock>()).MustHaveHappenedTwiceExactly();
      }
   }

   class When_creating_an_empty_simulation : concern_for_SimulationFactory
   {
      private IMoBiSimulation _result;

      protected override void Because()
      {
         _result = sut.Create();
      }

      [Observation]
      public void creating_a_new_simulation_results_in_a_call_to_parameter_id_updater_to_update_simulation_id()
      {
         A.CallTo(() => _parameterIdUpdater.UpdateSimulationId(A<MoBiSimulation>.That.Matches(x => Equals(x, _result)))).MustHaveHappened();
      }

      [Observation]
      public void should_create_a_new_simulation()
      {
         _result.ShouldNotBeNull();
         _result.HasChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_initialise_also_the_build_configuration()
      {
          _result.Configuration.ShouldNotBeNull();
      }
   }
}	