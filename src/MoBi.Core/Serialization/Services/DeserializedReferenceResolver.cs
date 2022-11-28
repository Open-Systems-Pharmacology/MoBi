using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Serialization.Services
{
   public interface IDeserializedReferenceResolver
   {
      void ResolveFormulaAndTemplateReferences(object deserializedObject, IMoBiProject project);
   }

   public class DeserializedReferenceResolver : IDeserializedReferenceResolver
   {
      private readonly IBuildingBlockReferenceUpdater _buildingBlockReferenceUpdater;
      private readonly IReferencesResolver _referencesResolver;
      private readonly ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;

      public DeserializedReferenceResolver(IBuildingBlockReferenceUpdater buildingBlockReferenceUpdater, IReferencesResolver referencesResolver, ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater)
      {
         _buildingBlockReferenceUpdater = buildingBlockReferenceUpdater;
         _referencesResolver = referencesResolver;
         _simulationParameterOriginIdUpdater = simulationParameterOriginIdUpdater;
      }

      public void ResolveFormulaAndTemplateReferences(object deserializedObject, IMoBiProject project)
      {
         if (deserializedObject.IsAnImplementationOf<IMoBiProject>())
         {
            var deserializedProject = deserializedObject.DowncastTo<IMoBiProject>();
            _buildingBlockReferenceUpdater.UpdateTemplatesReferencesIn(deserializedProject);
         }
         else if (deserializedObject.IsAnImplementationOf<IMoBiSimulation>())
         {
            var simulation = deserializedObject.DowncastTo<IMoBiSimulation>();
            resolveReferences(simulation);
         }
         else if (deserializedObject.IsAnImplementationOf<SimulationTransfer>())
         {
            var simulationTransfer = deserializedObject.DowncastTo<SimulationTransfer>();
            var simulation = simulationTransfer.Simulation.DowncastTo<IMoBiSimulation>();
            updateOutputMappings(simulation, simulationTransfer.OutputMappings);
            resolveReferences(simulation);
         }
         else if (deserializedObject.IsAnImplementationOf<IMoBiBuildConfiguration>())
         {
            _buildingBlockReferenceUpdater.UpdateTemplatesReferencesIn(deserializedObject.DowncastTo<IMoBiBuildConfiguration>(), project);
         }
         else if (deserializedObject.IsAnImplementationOf<IModel>())
         {
            resolveReferences(deserializedObject.DowncastTo<IModel>());
         }
      }

      private void updateOutputMappings(IMoBiSimulation simulation, OutputMappings outputMappings)
      {
         if (outputMappings == null)
            return;

         simulation.OutputMappings = outputMappings;
         //It is required to update the references as the simulation created may have another reference as the one deserialized
         outputMappings.Each(x => x.UpdateSimulation(simulation));
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