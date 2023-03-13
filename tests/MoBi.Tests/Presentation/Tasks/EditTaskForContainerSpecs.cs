using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTaskForContainer : ContextSpecification<EditTaskForContainer>
   {
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IInteractionTask _interactionTask;
      private IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _interactionTask = A.Fake<IInteractionTask>();
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
         sut = new EditTaskForContainer(_interactionTaskContext, _spatialStructureFactory, _objectPathFactory);
      }
   }

   public class When_saving_an_entity_to_pkml_but_the_file_dialog_is_canceled : concern_for_EditTaskForContainer
   {
      private IContainer _entityToSave;

      protected override void Context()
      {
         base.Context();
         _entityToSave = new Container();
         A.CallTo(() => _interactionTask.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.Save(_entityToSave);
      }

      [Observation]
      public void A_call_to_interaction_task_saving_the_container_must_not_have_happened()
      {
         A.CallTo(() => _spatialStructureFactory.Create()).MustNotHaveHappened();
      }
   }

   public class When_saving_an_entity_to_pkml : concern_for_EditTaskForContainer
   {
      private IContainer _entityToSave;
      private Container _parentContainer;

      protected override void Context()
      {
         base.Context();
         _parentContainer = new Container();
         _entityToSave = new Container {ParentContainer = _parentContainer};
         A.CallTo(() => _interactionTask.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._)).Returns("FilePath");
         A.CallTo(() => _spatialStructureFactory.Create()).Returns(new MoBiSpatialStructure());
      }

      protected override void Because()
      {
         sut.Save(_entityToSave);
      }

      [Observation]
      public void the_entity_must_have_the_parent_container_restored_after_saving()
      {
         _entityToSave.ParentContainer.ShouldBeEqualTo(_parentContainer);
      }
   }

   public class When_renaming_a_container_that_is_not_in_a_spatial_structure : concern_for_EditTaskForContainer
   {
      private IContainer _container;
      private EventGroupBuildingBlock _eventBuildingBlock;
      private IRenameObjectPresenter _renameObjectPresenter;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("OLD");
         _eventBuildingBlock = new EventGroupBuildingBlock();
         _renameObjectPresenter = A.Fake<IRenameObjectPresenter>();
         A.CallTo(_renameObjectPresenter).WithReturnType<string>().Returns("NEW");
         A.CallTo(() => _interactionTaskContext.ApplicationController.Start<IRenameObjectPresenter>()).Returns(_renameObjectPresenter);
      }

      protected override void Because()
      {
         sut.Rename(_container, _eventBuildingBlock);
      }

      [Observation]
      public void should_be_able_to_rename_the_container()
      {
         _container.Name.ShouldBeEqualTo("NEW");
      }
   }

   public class When_renaming_a_container_that_is_in_a_spatial_structure : concern_for_EditTaskForContainer
   {
      private IContainer _container;
      private SpatialStructure _spatialStructure;
      private IRenameObjectPresenter _renameObjectPresenter;
      private IMoBiCommand _renameCommand;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("OLD");
         _spatialStructure = new SpatialStructure();
         _renameObjectPresenter = A.Fake<IRenameObjectPresenter>();
         A.CallTo(_renameObjectPresenter).WithReturnType<string>().Returns("NEW");
         A.CallTo(() => _interactionTaskContext.ApplicationController.Start<IRenameObjectPresenter>()).Returns(_renameObjectPresenter);

         A.CallTo(() => _interactionTaskContext.Context.AddToHistory(A<IMoBiCommand>._))
            .Invokes(x => _renameCommand = x.GetArgument<IMoBiCommand>(0));
      }

      protected override void Because()
      {
         sut.Rename(_container, _spatialStructure);
      }

      [Observation]
      public void should_be_able_to_rename_the_container()
      {
         _container.Name.ShouldBeEqualTo("NEW");
      }

      [Observation]
      public void should_have_used_a_rename_container_command_specifically()
      {
         //at least one command with the special case command
         _renameCommand.DowncastTo<MoBiMacroCommand>().All().Any(x => x.IsAnImplementationOf<RenameContainerCommand>()).ShouldBeTrue();
      }
   }
}