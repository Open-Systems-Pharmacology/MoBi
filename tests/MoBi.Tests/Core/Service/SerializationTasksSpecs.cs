using System;
using System.Collections.Generic;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Helpers;
using MoBi.Presentation;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SerializationTask : ContextSpecification<SerializationTask>
   {
      protected IXmlSerializationService _serializationService;
      protected IContextPersistor _contextPersistor;
      protected IDialogCreator _dialogCreator;
      protected IXmlContentSelector _contentSelector;
      protected IProjectConverterLogger _converterLogger;
      protected IMoBiContext _context;
      protected IPostSerializationStepsMaker _postSerializationSteps;
      protected IHeavyWorkManager _heavyWorkManager;
      protected string _fileToOpen;
      private Func<string, bool> _oldFileExists;

      protected override void Context()
      {
         _serializationService = A.Fake<IXmlSerializationService>();
         _contextPersistor = A.Fake<IContextPersistor>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _contentSelector = A.Fake<IXmlContentSelector>();
         _converterLogger = A.Fake<IProjectConverterLogger>();
         _context = A.Fake<IMoBiContext>();
         _postSerializationSteps = A.Fake<IPostSerializationStepsMaker>();
         _heavyWorkManager = new HeavyWorkManagerForSpecs();
         sut = new SerializationTask(_serializationService, _contextPersistor, new ObjectTypeResolver(), _dialogCreator, _contentSelector, _converterLogger,
            _context, _postSerializationSteps, _heavyWorkManager);


         _fileToOpen = "toto.mbp3";
         _oldFileExists = FileHelper.FileExists;
         FileHelper.FileExists = x => string.Equals(x, _fileToOpen);
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.FileExists = _oldFileExists;
      }
   }

   internal class When_loading_many_from_pkml : concern_for_SerializationTask
   {
      private IEnumerable<IObjectBase> _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _serializationService.VersionFrom(A<XElement>._)).Returns(ProjectVersions.Current);
         A.CallTo(() => _serializationService.ElementNameFor(A<Type>._)).Returns("EventGroupBuildingBlock");
      }

      protected override void Because()
      {
         _result = sut.LoadMany<IEventGroupBuildingBlock>(DomainHelperForSpecs.TestFileFullPath("Events.pkml"), false);
      }

      [Observation]
      public void should_return_the_deserialized_objects()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_clear_conversion_messages()
      {
         A.CallTo(() => _converterLogger.Clear()).MustHaveHappened();
      }

      [Observation]
      public void should_convert_to_concentration_model_if_required()
      {
         A.CallTo(() => _postSerializationSteps.PerformPostDeserializationFor(A<IReadOnlyList<IEventGroupBuildingBlock>>._, ProjectVersions.Current, false)).MustHaveHappened();
      }
   }

   public class When_opening_a_file_that_is_already_open_by_another_user : concern_for_SerializationTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.LockFile(_fileToOpen)).Throws(new CannotLockFileException(new Exception()));
      }

      protected override void Because()
      {
         sut.LoadProject(_fileToOpen);
      }

      [Observation]
      public void should_notify_that_the_project_is_already_open()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, AppConstants.Captions.OpenAnyway, AppConstants.Captions.CancelButton)).MustHaveHappened();
      }
   }

   public class When_opening_a_file_that_is_already_open_by_another_user_and_the_active_user_decides_to_open_the_project_anyway : concern_for_SerializationTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.LockFile(_fileToOpen)).Throws(new CannotLockFileException(new Exception()));
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.LoadProject(_fileToOpen);
      }

      [Observation]
      public void should_open_the_project_as_readonly()
      {
         _context.ProjectIsReadOnly.ShouldBeTrue();
      }

      [Observation]
      public void should_open_the_file()
      {
         A.CallTo(() => _contextPersistor.Load(_context, _fileToOpen)).MustHaveHappened();
      }
   }

   public class When_opening_a_file_that_is_already_open_by_another_user_and_the_active_user_decides_to_cancel_the_open_action : concern_for_SerializationTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.LockFile(_fileToOpen)).Throws(new CannotLockFileException(new Exception()));
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.LoadProject(_fileToOpen);
      }

      [Observation]
      public void should_not_open_the_project_as_readonly()
      {
         _context.ProjectIsReadOnly.ShouldBeFalse();
      }

      [Observation]
      public void should_not_open_the_file()
      {
         A.CallTo(() => _contextPersistor.Load(_context, _fileToOpen)).MustNotHaveHappened();
      }
   }
}