using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Domain.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;

namespace MoBi.Core
{
   public abstract class concern_for_SimulationModelLoader : ContextSpecification<ISimulationModelLoader>
   {
      protected ISimulationContentRepository _contentRepository;
      protected IXmlSerializationService _serializationService;
      protected IMoBiProjectRetriever _projectRetriever;
      protected IRegisterTask _registerTask;
      protected ISimulationParameterOriginIdUpdater _originIdUpdater;

      protected MoBiProject _project;
      protected MoBiSimulation _simulation;
      protected IModel _model;
      protected byte[] _content;

      protected override void Context()
      {
         _contentRepository = A.Fake<ISimulationContentRepository>();
         _serializationService = A.Fake<IXmlSerializationService>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _registerTask = A.Fake<IRegisterTask>();
         _originIdUpdater = A.Fake<ISimulationParameterOriginIdUpdater>();

         _project = A.Fake<MoBiProject>();
         A.CallTo(() => _projectRetriever.Current).Returns(_project);

         _simulation = new MoBiSimulation { Id = "sim-1" };
         _model = A.Fake<IModel>();
         _content = new byte[] { 1, 2, 3 };

         sut = new SimulationModelLoader(_contentRepository, _serializationService, _projectRetriever, _registerTask, _originIdUpdater);
      }
   }

   public class When_materializing_the_model_of_a_shell_simulation : concern_for_SimulationModelLoader
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _contentRepository.ContentFor("sim-1")).Returns(_content);
         A.CallTo(() => _serializationService.DeserializeSimulationModel(_content, _project)).Returns(_model);
      }

      protected override void Because()
      {
         sut.MaterializeModel(_simulation);
      }

      [Observation]
      public void should_assign_the_deserialized_model_to_the_simulation()
      {
         _simulation.Model.ShouldBeEqualTo(_model);
      }

      [Observation]
      public void should_update_the_parameter_origin_simulation_id()
      {
         A.CallTo(() => _originIdUpdater.UpdateSimulationId(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_register_the_materialized_simulation()
      {
         A.CallTo(() => _registerTask.RegisterAllIn(_simulation)).MustHaveHappened();
      }
   }

   public class When_configuring_lazy_model_load_on_a_shell_simulation : concern_for_SimulationModelLoader
   {
      private IModel _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _contentRepository.ContentFor("sim-1")).Returns(_content);
         A.CallTo(() => _serializationService.DeserializeSimulationModel(_content, _project)).Returns(_model);
         sut.ConfigureLazyModelLoad(_simulation);
      }

      protected override void Because()
      {
         // Accessing the model should trigger materialization through the wired lazy loader.
         _result = _simulation.Model;
      }

      [Observation]
      public void should_materialize_the_model_on_first_access()
      {
         _result.ShouldBeEqualTo(_model);
         _simulation.IsModelLoaded.ShouldBeTrue();
      }
   }
}
