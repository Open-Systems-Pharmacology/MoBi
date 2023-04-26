using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
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
      private IHeavyWorkManager _heavyWorkManager;
      private IModelConstructor _modelConstructor;
      private ICloneManagerForBuildingBlock _cloneManager;
      private IMoBiContext _context;

      protected override void Context()
      {
         _idGenerator = IoC.Resolve<IIdGenerator>();
         _metaDataFactory = IoC.Resolve<ICreationMetaDataFactory>();
         _parameterIdUpdater = A.Fake<ISimulationParameterOriginIdUpdater>();
         _diagramManagerFactory = IoC.Resolve<IDiagramManagerFactory>();
         _simulationConfigurationFactory = IoC.Resolve<ISimulationConfigurationFactory>();
         _dimensionValidator = IoC.Resolve<IDimensionValidator>();
         _heavyWorkManager = IoC.Resolve<IHeavyWorkManager>();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _context = IoC.Resolve<IMoBiContext>();
         _context.NewProject();
         
         _cloneManager = IoC.Resolve<ICloneManagerForBuildingBlock>();
         sut = new SimulationFactory(_idGenerator, 
            _metaDataFactory, 
            _parameterIdUpdater, 
            _diagramManagerFactory,
            _simulationConfigurationFactory,
            _dimensionValidator, 
            _heavyWorkManager, 
            _modelConstructor, 
            _context, 
            _cloneManager);
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