using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Core.Snapshots.Mappers;
using MoBi.Core.Snapshots.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using SnapshotProject = MoBi.Core.Snapshots.Project;
using ModelProject = MoBi.Core.Domain.Model.MoBiProject;
using SnapshotContext = MoBi.Core.Snapshots.Mappers.SnapshotContext;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SnapshotTask : ContextSpecificationAsync<SnapshotTask>
   {
      protected IDialogCreator _dialogCreator;
      protected IMoBiContext _moBiContext;
      protected IParameter _parameter;
      protected string _parameterType = "parameter";
      protected IJsonSerializer _jsonSerializer;
      protected Parameter _parameterSnapshot;
      protected ISnapshotMapper _snapshotMapper;
      protected IObjectTypeResolver _objectTypeResolver;
      protected ProjectMapper _projectMapper;
      private IMoBiProjectRetriever _projectRetriever;

      protected override Task Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _moBiContext = A.Fake<IMoBiContext>();
         _jsonSerializer = A.Fake<IJsonSerializer>();
         _snapshotMapper = A.Fake<ISnapshotMapper>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _projectMapper = A.Fake<ProjectMapper>();
         sut = new SnapshotTask(_jsonSerializer, _snapshotMapper, _dialogCreator, _objectTypeResolver, _moBiContext, _projectRetriever, _projectMapper);

         _parameter = A.Fake<IParameter>();
         _parameter.Name = "Param";
         A.CallTo(() => _objectTypeResolver.TypeFor((IWithName)_parameter)).Returns(_parameterType);

         _parameterSnapshot = new Parameter();
         A.CallTo(() => _snapshotMapper.MapToSnapshot(_parameter)).Returns(_parameterSnapshot);
         return Task.FromResult(true);
      }
   }

   public class When_exporting_a_subject_snapshot_to_file_and_the_user_cancels_the_action : concern_for_SnapshotTask
   {
      protected override async Task Context()
      {
         await base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(null);
      }

      protected override async Task Because()
      {
         await sut.ExportModelToSnapshotAsync(_parameter);
      }

      [Observation]
      public void should_not_export_the_snapshot_to_file()
      {
         A.CallTo(() => _jsonSerializer.Serialize(A<object>._, A<string>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_load_the_object_to_export()
      {
         A.CallTo(() => _moBiContext.Load(_parameter)).MustNotHaveHappened();
      }
   }

   public class When_exporting_a_subject_snapshot_to_file : concern_for_SnapshotTask
   {
      private readonly string _fileFullPath = "A FILE";
      private string _message;

      protected override async Task Context()
      {
         await base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>()
            .Invokes(x => _message = x.GetArgument<string>(0))
            .Returns(_fileFullPath);
      }

      protected override async Task Because()
      {
         await sut.ExportModelToSnapshotAsync(_parameter);
      }

      [Observation]
      public void should_load_the_object_to_export()
      {
         A.CallTo(() => _moBiContext.Load(_parameter)).MustHaveHappened();
      }

      [Observation]
      public void should_ask_the_user_to_select_the_file_where_the_snapshot_will_be_exported_for_the_given_subject()
      {
         _message.Contains(_parameter.Name).ShouldBeTrue();
         _message.Contains(_parameterType).ShouldBeTrue();
      }

      [Observation]
      public void should_retrieve_the_mapper_creating_the_snapshot_object_for_the_subject_to_export()
      {
         A.CallTo(() => _snapshotMapper.MapToSnapshot(_parameter)).MustHaveHappened();
      }

      [Observation]
      public void should_export_the_snapshot_to_file()
      {
         A.CallTo(() => _jsonSerializer.Serialize(_parameterSnapshot, _fileFullPath)).MustHaveHappened();
      }
   }

   public class When_loading_objects_from_snapshot : concern_for_SnapshotTask
   {
      private readonly string _fileName = "FileName";
      private List<ModelProject> _projects;
      private Type _snapshotType;
      private object _snapshot1;
      private object _snapshot2;
      private ModelProject _project1;
      private ModelProject _project2;

      protected override async Task Context()
      {
         await base.Context();
         _snapshotType = typeof(SnapshotProject);
         _snapshot1 = new SnapshotProject();
         _snapshot2 = new SnapshotProject();
         _project1 = new ModelProject();
         _project2 = new ModelProject();

         A.CallTo(() => _snapshotMapper.SnapshotTypeFor<MoBiProject>()).Returns(_snapshotType);
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_fileName);
         A.CallTo(() => _jsonSerializer.DeserializeAsArray(_fileName, _snapshotType)).Returns(new[] { _snapshot1, _snapshot2 });
         A.CallTo(() => _snapshotMapper.MapToModel(_snapshot1, A<SnapshotContext>._)).Returns(_project1);
         A.CallTo(() => _snapshotMapper.MapToModel(_snapshot2, A<SnapshotContext>._)).Returns(_project2);
      }

      protected override async Task Because()
      {
         _projects = (await sut.LoadModelsFromSnapshotFileAsync<MoBiProject>()).ToList();
      }

      [Observation]
      public void should_ask_the_user_to_select_the_file_containing_the_snapshot_to_load()
      {
         A.CallTo(() => _dialogCreator.AskForFileToOpen(A<string>._, Constants.Filter.JSON_FILE_FILTER, Constants.DirectoryKey.REPORT, null, null)).MustHaveHappened();
      }

      [Observation]
      public void should_deserialize_the_file_to_the_matching_snapshot_type_and_return_the_expecting_model_objects()
      {
         _projects.ShouldContain(_project1, _project2);
      }
   }

   public class When_loading_objects_from_snapshot_and_the_user_cancels_the_action : concern_for_SnapshotTask
   {
      private List<ModelProject> _projects;

      protected override async Task Context()
      {
         await base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(null);
      }

      protected override async Task Because()
      {
         _projects = (await sut.LoadModelsFromSnapshotFileAsync<ModelProject>()).ToList();
      }

      [Observation]
      public void should_ask_the_user_to_select_the_file_containing_the_snapshot_to_load()
      {
         A.CallTo(() => _dialogCreator.AskForFileToOpen(A<string>._, Constants.Filter.JSON_FILE_FILTER, Constants.DirectoryKey.REPORT, null, null)).MustHaveHappened();
      }

      [Observation]
      public void should_return_an_empty_enumeration_of_model_being_loaded_from_snapshot()
      {
         _projects.ShouldBeEmpty();
      }
   }

   public class When_loading_a_project_from_snapshot_file : concern_for_SnapshotTask
   {
      private ModelProject _project;
      private readonly string _fileName = @"C:\test\SuperProject.json";

      protected override async Task Context()
      {
         await base.Context();
         var projectSnapshot = new SnapshotProject();
         var project = new ModelProject();

         A.CallTo(() => _jsonSerializer.DeserializeAsArray(_fileName, typeof(Project))).Returns(new object[] { projectSnapshot, });
         A.CallTo(() => _projectMapper.MapToModel(projectSnapshot, A<ProjectContext>._)).Returns(project);
      }

      protected override async Task Because()
      {
         _project = await sut.LoadProjectFromSnapshotFileAsync(_fileName);
      }

      [Observation]
      public void should_return_a_project_with_the_name_set_to_the_name_of_the_file()
      {
         _project.Name.ShouldBeEqualTo("SuperProject");
      }

      [Observation]
      public void should_have_marked_the_project_has_changed()
      {
         _project.HasChanged.ShouldBeTrue();
      }
   }
}
