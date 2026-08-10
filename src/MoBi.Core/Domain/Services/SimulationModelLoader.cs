using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Services;

namespace MoBi.Core.Domain.Services
{
   public interface ISimulationModelLoader
   {
      /// <summary>
      ///    Wires lazy model loading onto a shell <paramref name="simulation" /> created during project load.
      /// </summary>
      void ConfigureLazyModelLoad(IMoBiSimulation simulation);

      /// <summary>
      ///    Deserializes and assigns the deferred model for <paramref name="simulation" /> and registers its
      ///    object graph so its entities become resolvable by Id.
      /// </summary>
      void MaterializeModel(IMoBiSimulation simulation);
   }

   public class SimulationModelLoader : ISimulationModelLoader
   {
      private readonly ISimulationContentRepository _simulationContentRepository;
      private readonly IXmlSerializationService _serializationService;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IRegisterTask _registerTask;
      private readonly ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;

      public SimulationModelLoader(ISimulationContentRepository simulationContentRepository, IXmlSerializationService serializationService,
         IMoBiProjectRetriever projectRetriever, IRegisterTask registerTask, ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater)
      {
         _simulationContentRepository = simulationContentRepository;
         _serializationService = serializationService;
         _projectRetriever = projectRetriever;
         _registerTask = registerTask;
         _simulationParameterOriginIdUpdater = simulationParameterOriginIdUpdater;
      }

      public void ConfigureLazyModelLoad(IMoBiSimulation simulation)
      {
         if (simulation is MoBiSimulation moBiSimulation)
            moBiSimulation.SetLazyModelLoader(() => MaterializeModel(moBiSimulation));
      }

      public void MaterializeModel(IMoBiSimulation simulation)
      {
         var content = _simulationContentRepository.ContentFor(simulation.Id);
         
         simulation.Model = _serializationService.DeserializeSimulationModel(content, _projectRetriever.Current);

         // The model's own references are resolved during deserialization; the origin simulation id of the
         // model parameters can only be set now that the model exists.
         _simulationParameterOriginIdUpdater.UpdateSimulationId(simulation);

         // Registers the now-materialized model and the rest of the simulation graph, which was not registered
         // when the project was loaded (RegisterTask.Register deliberately skips simulations).
         _registerTask.RegisterAllIn(simulation);
      }
   }
}