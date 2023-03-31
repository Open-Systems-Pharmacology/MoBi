using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Core.Serialization.ORM.Mappers;
using MoBi.Core.Serialization.ORM.MetaData;
using MoBi.Core.Serialization.Services;
using OSPSuite.Infrastructure.Serialization.Services;

namespace MoBi.Core.Serialization.ORM
{
   public interface IProjectPersistor : IPersistor<MoBiProject>
   {
   }

   public class ProjectPersistor : IProjectPersistor
   {
      private readonly ISessionManager _sessionManager;
      private readonly IProjectToProjectMetaDataMapper _projectToProjectMetaDataMapper;
      private readonly IProjectMetaDataToProjectMapper _projectMetaDataToProjectMapper;
      private readonly IPostSerializationStepsMaker _postSerializationStepsMaker;
      private readonly ISerializationContextFactory _serializationContextFactory;

      public ProjectPersistor(ISessionManager sessionManager, IProjectToProjectMetaDataMapper projectToProjectMetaDataMapper,
         IProjectMetaDataToProjectMapper projectMetaDataToProjectMapper, IPostSerializationStepsMaker postSerializationStepsMaker,
         ISerializationContextFactory serializationContextFactory)
      {
         _sessionManager = sessionManager;
         _projectToProjectMetaDataMapper = projectToProjectMetaDataMapper;
         _projectMetaDataToProjectMapper = projectMetaDataToProjectMapper;
         _postSerializationStepsMaker = postSerializationStepsMaker;
         _serializationContextFactory = serializationContextFactory;
      }

      public void Save(MoBiProject project, IMoBiContext context)
      {
         using (_serializationContextFactory.Create())
         {
            var session = _sessionManager.CurrentSession;
            var projectMetaData = projectMetaDataFrom(project);
            var projectFromDb = projectFromDatabase();
            if (projectFromDb == null)
               session.Save(projectMetaData);
            else
               projectFromDb.UpdateFrom(projectMetaData, session);
         }
      }

      public MoBiProject Load(IMoBiContext context)
      {
         var projectFromDb = projectFromDatabase();
         if (projectFromDb == null)
            return null;

         if (!ProjectVersions.CanLoadVersion(projectFromDb.Version))
            throw new InvalidProjectFileException(projectFromDb.Version);

         var project = projectFrom(projectFromDb);

         _postSerializationStepsMaker.PerformPostDeserializationFor(project, projectFromDb.Version);

         context.LoadFrom(project);

         return project;
      }

      private ProjectMetaData projectFromDatabase()
      {
         var session = _sessionManager.CurrentSession;
         var projectsFromDb = session.CreateCriteria<ProjectMetaData>().List<ProjectMetaData>();
         if (projectsFromDb.Count == 0) return null;
         return projectsFromDb[0];
      }

      private ProjectMetaData projectMetaDataFrom(MoBiProject project)
      {
         return _projectToProjectMetaDataMapper.MapFrom(project);
      }

      private MoBiProject projectFrom(ProjectMetaData projectMetaData)
      {
         return _projectMetaDataToProjectMapper.MapFrom(projectMetaData);
      }
   }
}