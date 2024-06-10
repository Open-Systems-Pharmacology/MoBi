using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Core.Serialization.ORM.MetaData;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Serialization.ORM.Mappers
{
   public interface IProjectMetaDataToProjectMapper : IMapper<ProjectMetaData, MoBiProject>
   {
   }

   public class ProjectMetaDataToProjectMapper : IProjectMetaDataToProjectMapper
   {
      private readonly IXmlSerializationService _serializationService;
      private readonly ISerializationContextFactory _serializationContextFactory;
      private readonly IDeserializedReferenceResolver _deserializedReferenceResolver;
      private readonly IModuleFactory _moduleFactory;

      private MoBiProject _project;

      public ProjectMetaDataToProjectMapper(
         IXmlSerializationService serializationService,
         ISerializationContextFactory serializationContextFactory,
         IDeserializedReferenceResolver deserializedReferenceResolver,
         IModuleFactory moduleFactory)
      {
         _serializationService = serializationService;
         _serializationContextFactory = serializationContextFactory;
         _deserializedReferenceResolver = deserializedReferenceResolver;
         _moduleFactory = moduleFactory;
      }

      public MoBiProject MapFrom(ProjectMetaData projectMetaData)
      {
         try
         {
            // deserialization is a three stage process. First we need to load the observed data
            // then all other entities and finish with the project charts
            using (var serializationContext = _serializationContextFactory.Create())
            {
               _project = deserializeContent<MoBiProject>(projectMetaData.Content, serializationContext);
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
               var nonProjectBuildingBlocks = new List<IBuildingBlock>();
               foreach (var entityMetaData in projectMetaData.Children)
               {
                  addEntityToProject(entityMetaData, serializationContext, nonProjectBuildingBlocks);
               }

               // convert and add the entities that are not direct children of project
               addNonProjectBuildingBlocks(nonProjectBuildingBlocks);

               _deserializedReferenceResolver.ResolveFormulaAndTemplateReferences(_project, _project);
               return _project;
            }
         }
         finally
         {
            _project = null;
         }
      }

      private void addNonProjectBuildingBlocks(List<IBuildingBlock> moduleBuildingBlocks)
      {
         var simulationSettingsBlocks = moduleBuildingBlocks.OfType<SimulationSettings>().ToList();
         simulationSettingsBlocks.Each(x => moduleBuildingBlocks.Remove(x));


         
         if(hasUniqueBuildingBlocks(moduleBuildingBlocks))
            addModuleBuildingBlocks(moduleBuildingBlocks);
         else
            addBuildingBlocksToModule(moduleBuildingBlocks);
         
         addSimulationSettingsBuildingBlocks(simulationSettingsBlocks);
      }
      private void addBuildingBlocksToModule(List<IBuildingBlock> moduleBuildingBlocks) =>
         moduleBuildingBlocks.Each(addBuildingBlockAsModule);
      

      private void addSimulationSettingsBuildingBlocks(List<SimulationSettings> simulationSettingsBlocks)
      {
         if (simulationSettingsBlocks.Count == 1)
            _project.SimulationSettings = simulationSettingsBlocks.First();

         // TODO what if there is more than 1 simulation settings in the project being converted
         // else
         //    throw new MoBiException($"Project contains multiple simulation settings");
      }

      private void addModuleBuildingBlocks(List<IBuildingBlock> moduleBuildingBlocks)
      {
         if (!moduleBuildingBlocks.Any())
            return;
         var module = _moduleFactory.CreateModuleWithName(AppConstants.DefaultNames.DefaultSingleModuleName);
         moduleBuildingBlocks.Each(module.Add);
         _project.AddModule(module);
      }

      private void addBuildingBlockAsModule(IBuildingBlock buildingBlock)
      {
         var module = _moduleFactory.CreateDedicatedModuleFor(buildingBlock);
         _project.AddModule(module);
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
         serializationContext.AddRepository(simulation.ResultsDataRepository);
         simulation.HistoricResults.Each(serializationContext.AddRepository);
         serializationContext.Register(simulation);
      }

      private T deserializeContent<T>(MetaDataContent content, SerializationContext serializationContext)
      {
         return _serializationService.Deserialize<T>(content.Data, _project, serializationContext);
      }

      private void addEntityToProject(EntityMetaData entity, SerializationContext serializationContext, List<IBuildingBlock> moduleBuildingBlocks)
      {
         if (entity.IsAnImplementationOf<SimulationMetaData>())
            return;

         var deserializedEntity = deserializeContent<object>(entity.Content, serializationContext);
         if (deserializedEntity.IsAnImplementationOf<CurveChart>())
            addChartToProject(deserializedEntity);

         else if (deserializedEntity is ParameterIdentification parameterIdentification)
            _project.AddParameterIdentification(parameterIdentification);

         else if (deserializedEntity is SensitivityAnalysis sensitivityAnalysis)
            _project.AddSensitivityAnalysis(sensitivityAnalysis);

         else if (deserializedEntity is Module module)
            _project.AddModule(module);

         else if (deserializedEntity is IndividualBuildingBlock individualBuildingBlock)
            _project.AddIndividualBuildingBlock(individualBuildingBlock);

         else if (deserializedEntity is ExpressionProfileBuildingBlock expressionProfile)
            _project.AddExpressionProfileBuildingBlock(expressionProfile);

         else if (deserializedEntity is MoleculeBuildingBlock moleculeBuildingBlock)
            moduleBuildingBlocks.Add(moleculeBuildingBlock);

         else if (deserializedEntity is PassiveTransportBuildingBlock passiveTransport)
            moduleBuildingBlocks.Add(passiveTransport);

         else if (deserializedEntity is EventGroupBuildingBlock eventGroupBuildingBlock)
            moduleBuildingBlocks.Add(eventGroupBuildingBlock);

         else if (deserializedEntity is InitialConditionsBuildingBlock initialConditionsBuildingBlock)
            moduleBuildingBlocks.Add(initialConditionsBuildingBlock);

         else if (deserializedEntity is ParameterValuesBuildingBlock parameterValuesBuildingBlock)
            moduleBuildingBlocks.Add(parameterValuesBuildingBlock);

         else if (deserializedEntity is MoBiReactionBuildingBlock moBiReactionBuildingBlock)
            moduleBuildingBlocks.Add(moBiReactionBuildingBlock);

         else if (deserializedEntity is MoBiSpatialStructure moBiSpatialStructure)
            moduleBuildingBlocks.Add(moBiSpatialStructure);

         else if (deserializedEntity is ObserverBuildingBlock observerBuildingBlock)
            moduleBuildingBlocks.Add(observerBuildingBlock);

         else if (deserializedEntity is SimulationSettings simulationSettings)
            moduleBuildingBlocks.Add(simulationSettings);

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

      private bool hasUniqueBuildingBlocks(List<IBuildingBlock> moduleBuildingBlocks)
      {
         var uniqueTypes = new HashSet<Type>
         {
            typeof(MoleculeBuildingBlock),
            typeof(PassiveTransportBuildingBlock),
            typeof(EventGroupBuildingBlock),
            typeof(InitialConditionsBuildingBlock),
            typeof(ParameterValuesBuildingBlock),
            typeof(MoBiReactionBuildingBlock),
            typeof(MoBiSpatialStructure),
            typeof(ObserverBuildingBlock)
         };

         return moduleBuildingBlocks
            .Select(block => block.GetType())
            .Where(type => uniqueTypes.Contains(type))
            .GroupBy(type => type)
            .All(group => group.Count() == 1);

      }
   }
}