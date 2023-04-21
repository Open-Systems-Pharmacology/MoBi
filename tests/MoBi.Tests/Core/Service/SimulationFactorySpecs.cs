using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

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
      private IMoBiContext _context;

      protected override void Context()
      {
         _idGenerator = A.Fake<IIdGenerator>();
         _metaDataFactory = A.Fake<ICreationMetaDataFactory>();
         _parameterIdUpdater = A.Fake<ISimulationParameterOriginIdUpdater>();
         _diagramManagerFactory = A.Fake<IDiagramManagerFactory>();
         _simulationConfigurationFactory = A.Fake<ISimulationConfigurationFactory>();
         _dimensionValidator = A.Fake<IDimensionValidator>();
         _heavyWorkManager = A.Fake<IHeavyWorkManager>();
         _modelConstructor = A.Fake<IModelConstructor>();
         _context = A.Fake<IMoBiContext>();
         sut = new SimulationFactory(_idGenerator, _metaDataFactory, _parameterIdUpdater, _diagramManagerFactory, _simulationConfigurationFactory, _dimensionValidator, _heavyWorkManager, _modelConstructor, _context);
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