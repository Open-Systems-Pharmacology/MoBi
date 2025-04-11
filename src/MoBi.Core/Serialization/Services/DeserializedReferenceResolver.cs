using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Serialization.Services
{
   public interface IDeserializedReferenceResolver
   {
      void ResolveFormulaAndTemplateReferences(object deserializedObject, MoBiProject project);
   }

   public class DeserializedReferenceResolver : IDeserializedReferenceResolver
   {
      private readonly IReferencesResolver _referencesResolver;
      private readonly ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;

      public DeserializedReferenceResolver(IReferencesResolver referencesResolver, ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater)
      {
         _referencesResolver = referencesResolver;
         _simulationParameterOriginIdUpdater = simulationParameterOriginIdUpdater;
      }

      public void ResolveFormulaAndTemplateReferences(object deserializedObject, MoBiProject project)
      {
         switch (deserializedObject)
         {
            case MoBiProject proj:
               proj.All<SpatialStructure>().Each(resolveReferences);
               proj.Simulations.Each(resolveReferences);
               break;
            case IMoBiSimulation simulation:
               resolveReferences(simulation);
               break;
            case SimulationTransfer simulationTransfer:
               var sim = simulationTransfer.Simulation.DowncastTo<IMoBiSimulation>();
               updateOutputMappings(sim, simulationTransfer.OutputMappings);
               resolveReferences(sim);
               break;
            case IModel model:
               resolveReferences(model);
               break;
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
         resolveReferences(simulation.Model);
         resolveReferences(simulation.Configuration);
         _simulationParameterOriginIdUpdater.UpdateSimulationId(simulation);
      }

      private void resolveReferences(IModel model)
      {
         if (model == null) return;
         _referencesResolver.ResolveReferencesIn(model);
      }

      private void resolveReferences(SimulationConfiguration simulationConfiguration) => simulationConfiguration?.All<SpatialStructure>().Each(resolveReferences);

      private void resolveReferences(SpatialStructure spatialStructure)
      {
         spatialStructure?.ResolveReferencesInNeighborhoods();
      }
   }
}