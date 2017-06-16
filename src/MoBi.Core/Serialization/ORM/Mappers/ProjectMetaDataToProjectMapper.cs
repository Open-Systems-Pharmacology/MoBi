using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Core.Serialization.ORM.MetaData;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Serialization.ORM.Mappers
{
   public interface IProjectMetaDataToProjectMapper : IMapper<ProjectMetaData, IMoBiProject>
   {
   }

   public class ProjectMetaDataToProjectMapper : IProjectMetaDataToProjectMapper
   {
      private readonly IXmlSerializationService _serializationService;
      private readonly ISerializationContextFactory _serializationContextFactory;
      private readonly IDeserializedReferenceResolver _deserializedReferenceResolver;
      private IMoBiProject _project;

      public ProjectMetaDataToProjectMapper(IXmlSerializationService serializationService, ISerializationContextFactory serializationContextFactory, IDeserializedReferenceResolver deserializedReferenceResolver)
      {
         _serializationService = serializationService;
         _serializationContextFactory = serializationContextFactory;
         _deserializedReferenceResolver = deserializedReferenceResolver;
      }

      public IMoBiProject MapFrom(ProjectMetaData projectMetaData)
      {
         try
         {
            // deserialization is a three stage process. First we need to load the observed data
            // then all other entities and finish with the project charts
            using (var serializationContext = _serializationContextFactory.Create())
            {
               _project = deserializeContent<IMoBiProject>(projectMetaData.Content, serializationContext);
               _project.Name = projectMetaData.Name;
               _project.Description = projectMetaData.Description;

               //add observed data to global
               addObservedDataToSerializationContext(serializationContext);

               //load simulation first
               foreach (var simulationMetaData in projectMetaData.Simulations)
               {
                  addSimulationToProject(simulationMetaData, serializationContext);
               }

               //then load the rest of the entities
               foreach (var entityMetaData in projectMetaData.Children)
               {
                  addEntityToProject(entityMetaData, serializationContext);
               }

               _deserializedReferenceResolver.ResolveFormulaAndTemplateReferences(_project, _project);
               return _project;
            }
         }
         finally
         {
            _project = null;
         }
      }

      private void addObservedDataToSerializationContext(SerializationContext serializationContext)
      {
         _project.AllObservedData.Each(x =>
         {
            serializationContext.AddRepository(x);
            serializationContext.Register(x);
         });
      }

      private void addSimulationToProject(SimulationMetaData simulationMetaData, SerializationContext serializationContext)
      {
         var simulation = deserializeContent<IMoBiSimulation>(simulationMetaData.Content, serializationContext);
         _project.AddSimulation(simulation);
         deserializeResults(simulation, simulationMetaData, serializationContext);
         //Ensure that all references to simulations (Simulation itself, results etc) are available in the serialization context
         serializationContext.AddRepository(simulation.Results);
         simulation.HistoricResults.Each(serializationContext.AddRepository);
         serializationContext.Register(simulation);
      }

      private T deserializeContent<T>(MetaDataContent content, SerializationContext serializationContext)
      {
         return _serializationService.Deserialize<T>(content.Data, _project, serializationContext);
      }

      private void addEntityToProject(EntityMetaData entity, SerializationContext serializationContext)
      {
         if (entity.IsAnImplementationOf<SimulationMetaData>())
            return;

         var deserializedEntity = deserializeContent<object>(entity.Content, serializationContext);
         if (deserializedEntity.IsAnImplementationOf<CurveChart>())
            addChartToProject(deserializedEntity);

         else if (deserializedEntity.IsAnImplementationOf<ParameterIdentification>())
            _project.AddParameterIdentification(deserializedEntity as ParameterIdentification);

         else if (deserializedEntity.IsAnImplementationOf<SensitivityAnalysis>())
            _project.AddSensitivityAnalysis(deserializedEntity as SensitivityAnalysis);

         else if (deserializedEntity.IsAnImplementationOf<IWithId>())
            _project.AddBuildingBlock(deserializedEntity as IBuildingBlock);

         else

            throw new MoBiException($"Don't know what to do with {deserializedEntity.GetType()}");
      }

      private void addChartToProject(object deserializedEntity)
      {
         var chart = deserializedEntity.DowncastTo<CurveChart>();
         //only add charts with at least one curve otherwise chart is corrupted
         if (!chart.Curves.Any())
            return;

         _project.AddChart(chart);
      }

      private void deserializeResults(IMoBiSimulation moBiSimulation, SimulationMetaData simulationMetaData, SerializationContext serializationContext)
      {
         foreach (var historicalResult in simulationMetaData.HistoricalResults)
         {
            var dataRepository = deserializeContent<DataRepository>(historicalResult.Content, serializationContext);
            dataRepository.SetPersistable(true);
            moBiSimulation.HistoricResults.Add(dataRepository);
         }
      }
   }
}