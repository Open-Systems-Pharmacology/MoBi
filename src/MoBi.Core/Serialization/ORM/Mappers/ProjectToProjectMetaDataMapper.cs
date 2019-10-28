using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM.MetaData;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using DataRepositoryMetaData = MoBi.Core.Serialization.ORM.MetaData.DataRepositoryMetaData;

namespace MoBi.Core.Serialization.ORM.Mappers
{
   public interface IProjectToProjectMetaDataMapper : IMapper<IMoBiProject, ProjectMetaData>

   {
   }

   public class ProjectToProjectMetaDataMapper : IProjectToProjectMetaDataMapper
   {
      private readonly IXmlSerializationService _serializationService;

      public ProjectToProjectMetaDataMapper(IXmlSerializationService serializationService)
      {
         _serializationService = serializationService;
      }

      public ProjectMetaData MapFrom(IMoBiProject project)
      {
         var projectMetaData = new ProjectMetaData {Name = project.Name, Description = project.Description};
         serializeContent(projectMetaData, project);

         project.AllBuildingBlocks().Each(x => projectMetaData.AddChild(mapFrom(x)));
         project.Simulations.Each(x => projectMetaData.AddChild(mapFrom(x)));
         project.Charts.Each(x => projectMetaData.AddChild(mapFrom(x)));
         project.AllParameterAnalysables.Each(x => projectMetaData.AddChild(mapFrom(x)));

         return projectMetaData;
      }

      private EntityMetaData mapFrom(IWithId entityWithId)
      {
         var entityMetaData = new EntityMetaData {Id = entityWithId.Id};
         serialize(entityMetaData, entityWithId);
         return entityMetaData;
      }

      private DataRepositoryMetaData mapFrom(DataRepository dataRepository)
      {
         var dataRepositoryMetaData = new DataRepositoryMetaData {Id = dataRepository.Id};
         serializeContent(dataRepositoryMetaData, dataRepository);
         return dataRepositoryMetaData;
      }

      private SimulationMetaData mapFrom(IMoBiSimulation simulation)
      {
         var simulationMetaData = new SimulationMetaData {Id = simulation.Id};
         if (!simulation.HasChanged)
            return simulationMetaData;

         serialize(simulationMetaData, simulation);
         simulation.HistoricResults.Where(x => x.IsPersistable()).Each(res => simulationMetaData.AddHistoricalResults(mapFrom(res)));
         return simulationMetaData;
      }

      private EntityMetaData mapFrom(CurveChart chart)
      {
         var entityMetaData = new EntityMetaData {Id = ShortGuid.NewGuid()};
         serialize(entityMetaData, chart);
         return entityMetaData;
      }

      private void serialize(EntityMetaData entityMetaData, object entity)
      {
         entityMetaData.Discriminator = entity.GetType().Name;
         serializeContent(entityMetaData, entity);
      }

      private void serializeContent<T>(MetaDataWithContent<T> metaDataWithContent, object entity)
      {
         metaDataWithContent.Content.Data = _serializationService.SerializeAsBytes(entity);
      }
   }
}