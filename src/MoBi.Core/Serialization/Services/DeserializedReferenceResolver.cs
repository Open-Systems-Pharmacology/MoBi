using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;

namespace MoBi.Core.Serialization.Services
{
   public interface IDeserializedReferenceResolver
   {
      void ResolveFormulaAndTemplateReferences(object deserializedObject, IMoBiProject project);
   }

   public class DeserializedReferenceResolver : IDeserializedReferenceResolver
   {
      private readonly IBuilingBlockReferenceUpdater _builingBlockReferenceUpdater;
      private readonly IReferencesResolver _referencesResolver;
      private readonly ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;

      public DeserializedReferenceResolver(IBuilingBlockReferenceUpdater builingBlockReferenceUpdater, IReferencesResolver referencesResolver, ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater)
      {
         _builingBlockReferenceUpdater = builingBlockReferenceUpdater;
         _referencesResolver = referencesResolver;
         _simulationParameterOriginIdUpdater = simulationParameterOriginIdUpdater;
      }

      public void ResolveFormulaAndTemplateReferences(object deserializedObject, IMoBiProject project)
      {
         if (deserializedObject.IsAnImplementationOf<IMoBiProject>())
         {
            var deserializedProject = deserializedObject.DowncastTo<IMoBiProject>();
            _builingBlockReferenceUpdater.UpdateTemplatesReferencesIn(deserializedProject);
         }
         else if (deserializedObject.IsAnImplementationOf<IMoBiSimulation>())
         {
            var simulation = deserializedObject.DowncastTo<IMoBiSimulation>();
            resolveReferences(simulation);
         }
         else if (deserializedObject.IsAnImplementationOf<SimulationTransfer>())
         {
            var simulationTransfer = deserializedObject.DowncastTo<SimulationTransfer>();
            resolveReferences(simulationTransfer.Simulation.DowncastTo<IMoBiSimulation>());
         }
         else if (deserializedObject.IsAnImplementationOf<IMoBiBuildConfiguration>())
         {
            _builingBlockReferenceUpdater.UpdateTemplatesReferencesIn(deserializedObject.DowncastTo<IMoBiBuildConfiguration>(), project);
         }
         else if (deserializedObject.IsAnImplementationOf<IModel>())
         {
            resolveReferences(deserializedObject.DowncastTo<IModel>());
         }
      }

      private void resolveReferences(IMoBiSimulation simulation)
      {
         if (simulation == null) return;
         //no need to update building block references at that stage. It will be done when the project itself is being deserialized
         resolveReferences(simulation.Model);
         _simulationParameterOriginIdUpdater.UpdateSimulationId(simulation);
      }

      private void resolveReferences(IModel model)
      {
         if (model == null) return;
         _referencesResolver.ResolveReferencesIn(model);
      }
   }
}