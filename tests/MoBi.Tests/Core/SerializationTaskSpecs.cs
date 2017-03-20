using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.Core
{
   public abstract class concern_for_SerializationTask : ContextSpecification<ISerializationTask>
   {
      protected IXmlSerializationService _xmlSerializationService;
      protected IContextPersistor _contextPersistor;
      protected IObjectTypeResolver _objectTypeResolver;
      protected IDialogCreator _dialogCreator;
      protected IXmlContentSelector _xmlContentSelector;
      protected IProjectConverterLogger _projectConverterLogger;
      protected IMoBiContext _context;
      protected IPostSerializationStepsMaker _postSerializationSteps;
      protected IHeavyWorkManager _heavyWorkManager;

      protected override void Context()
      {
         _xmlSerializationService = A.Fake<IXmlSerializationService>();
         _contextPersistor = A.Fake<IContextPersistor>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _xmlContentSelector = A.Fake<IXmlContentSelector>();
         _projectConverterLogger = A.Fake<IProjectConverterLogger>();
         _context = A.Fake<IMoBiContext>();
         _postSerializationSteps = A.Fake<IPostSerializationStepsMaker>();
         _heavyWorkManager = A.Fake<IHeavyWorkManager>();

         sut = new SerializationTask(_xmlSerializationService, _contextPersistor, _objectTypeResolver, _dialogCreator, _xmlContentSelector, _projectConverterLogger, _context, _postSerializationSteps, _heavyWorkManager);
      }
   }

   public class When_the_serialization_task_is_saving_a_project : concern_for_SerializationTask
   {
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         _project.FilePath = "XXX";
         _context.ProjectIsReadOnly = true;
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.SaveProject();
      }

      [Observation]
      public void should_retrieve_the_current_project_from_the_context_and_lock_the_file()
      {
         A.CallTo(() => _context.LockFile(_project.FilePath)).MustHaveHappened();
      }

      [Observation]
      public void should_save_the_context_using_the_context_persistor()
      {
         A.CallTo(() => _contextPersistor.Save(_context)).MustHaveHappened();
      }

      [Observation]
      public void should_try_to_access_the_file_that_was_previously_saved()
      {
         A.CallTo(() => _context.AccessFile(_project.FilePath)).MustHaveHappened();
      }

      [Observation]
      public void should_ensure_that_the_project_is_not_readonly_as_it_was_just_saved()
      {
         _context.ProjectIsReadOnly.ShouldBeFalse();
      }
   }
}