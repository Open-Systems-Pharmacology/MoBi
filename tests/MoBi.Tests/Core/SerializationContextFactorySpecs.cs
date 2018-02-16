using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Xml;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core
{
   public abstract class concern_for_SerializationContextFactory : ContextSpecification<ISerializationContextFactory>
   {
      protected ISerializationDimensionFactory _dimensionFactory;
      protected IObjectBaseFactory _objectBaseFactory;
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IContainer _container;
      protected IMoBiProjectRetriever _projectRetriever;
      protected IMoBiProject _project;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<ISerializationDimensionFactory>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _container = A.Fake<IContainer>();

         sut = new SerializationContextFactory(_dimensionFactory, _objectBaseFactory, _cloneManagerForModel, _container);

         _projectRetriever = A.Fake<IMoBiProjectRetriever>();

         A.CallTo(() => _container.Resolve<IMoBiProjectRetriever>()).Returns(_projectRetriever);

         _project = new MoBiProject();
         A.CallTo(() => _projectRetriever.Current).Returns(_project);
      }
   }

   public class When_creating_a_serialization_context_in_an_empty_project : concern_for_SerializationContextFactory
   {
      private SerializationContext _result;

      protected override void Because()
      {
         _result = sut.Create();
      }

      [Observation]
      public void should_return_a_serialization_context_without_any_data_repositories_or_simulation_registered()
      {
         _result.IdRepository.All().ShouldBeEmpty();
         _result.Repositories.ShouldBeEmpty();
      }
   }

   public class When_creating_a_serialization_context_based_on_a_parent_serialization_context : concern_for_SerializationContextFactory
   {
      private SerializationContext _result;
      private SerializationContext _parentContext;
      private DataRepository _dataRepo1;
      private IWithId _simmulation;

      protected override void Context()
      {
         base.Context();
         _dataRepo1 = new DataRepository("OBS1");
         _simmulation = new MoBiSimulation().WithId("ID");
         _parentContext = new SerializationContext(_dimensionFactory, _objectBaseFactory, new WithIdRepository(), new[] {_dataRepo1}, _cloneManagerForModel, _container);
         _parentContext.Register(_simmulation);
      }

      protected override void Because()
      {
         _result = sut.Create(_parentContext);
      }

      [Observation]
      public void should_add_the_repositories_defined_in_the_parent_context()
      {
         _result.Repositories.ShouldContain(_dataRepo1);
      }

      [Observation]
      public void should_register_the_object_registered_in_the_parent_context()
      {
         _result.IdRepository.All().ShouldContain(_simmulation);
      }
   }

   public class When_creating_a_serialization_context_with_a_defined_project : concern_for_SerializationContextFactory
   {
      private SerializationContext _result;
      private DataRepository _observedData;
      private MoBiSimulation _simmulation;
      private DataRepository _simulationResults;

      protected override void Context()
      {
         base.Context();
         _observedData = new DataRepository("OBS1");
         _simulationResults = new DataRepository("RES");
         _simmulation = new MoBiSimulation().WithId("ID");

         _simmulation.Results = _simulationResults;

         _project.AddSimulation(_simmulation);
         _project.AddObservedData(_observedData);
      }

      protected override void Because()
      {
         _result = sut.Create();
      }

      [Observation]
      public void should_add_the_repositories_defined_in_the_parent_context()
      {
         _result.Repositories.ShouldContain(_observedData, _simulationResults);
      }

      [Observation]
      public void should_register_the_object_registered_in_the_parent_context()
      {
         _result.IdRepository.All().ShouldContain(_simmulation, _observedData, _simulationResults);
      }
   }
}