using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

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
}