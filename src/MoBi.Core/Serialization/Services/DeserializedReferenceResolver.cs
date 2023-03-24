using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
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
         switch (deserializedObject)
         {
            case IMoBiProject proj:
               _buildingBlockReferenceUpdater.UpdateTemplatesReferencesIn(proj);
               proj.All<ISpatialStructure>().Each(resolveReferences);
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
            case IMoBiBuildConfiguration buildConfiguration:
               _buildingBlockReferenceUpdater.UpdateTemplatesReferencesIn(buildConfiguration, project);
               resolveReferences(buildConfiguration.MoBiSpatialStructure);
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
         //no need to update building block references at that stage. It will be done when the project itself is being deserialized
         resolveReferences(simulation.Model);
         resolveReferences(simulation.Configuration);
         _simulationParameterOriginIdUpdater.UpdateSimulationId(simulation);
      }

      private void resolveReferences(IModel model)
      {
         if (model == null) return;
         _referencesResolver.ResolveReferencesIn(model);
      }

      private void resolveReferences(SimulationConfiguration simulationConfiguration) => resolveReferences(simulationConfiguration?.SpatialStructure);

      private void resolveReferences(ISpatialStructure spatialStructure)
      {
         spatialStructure?.ResolveReferencesInNeighborhoods();
      }
   }
}