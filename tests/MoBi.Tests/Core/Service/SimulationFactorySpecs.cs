using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SimulationFactory : ContextSpecification<ISimulationFactory>
   {
      protected IIdGenerator _idGenerator;
      protected ICreationMetaDataFactory _metaDataFactory;
      protected ISimulationParameterOriginIdUpdater _parameterIdUpdater;
      private IDiagramManagerFactory _diagramManagerFactory;

      protected override void Context()
      {
         _idGenerator = A.Fake<IIdGenerator>();
         _metaDataFactory = A.Fake<ICreationMetaDataFactory>();
         _parameterIdUpdater = A.Fake<ISimulationParameterOriginIdUpdater>();
         _diagramManagerFactory = A.Fake<IDiagramManagerFactory>();
         sut = new SimulationFactory(_idGenerator,_metaDataFactory, _parameterIdUpdater, _diagramManagerFactory);
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
          _result.BuildConfiguration.ShouldNotBeNull();
          _result.MoBiBuildConfiguration.ShouldNotBeNull();
      }
   }
}	