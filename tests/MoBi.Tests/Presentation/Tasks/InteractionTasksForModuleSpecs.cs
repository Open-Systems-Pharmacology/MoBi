using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_InteractionTasksForModule : ContextSpecification<InteractionTasksForModule>
   {
      protected IInteractionTaskContext _context;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         sut = new InteractionTasksForModule(_context, new EditTaskForModule(_context));
      }
   }

   public class When_creating_a_new_module : concern_for_InteractionTasksForModule
   {
      protected override void Because()
      {
         sut.CreateNewModuleWithBuildingBlocks();
      }

      [Observation]
      public void the_application_controller_should_create_new_presenter()
      {
         A.CallTo(() => _context.ApplicationController.Start<ICreateModulePresenter>()).MustHaveHappened();
      }
   }

   public class When_the_presenter_doesnt_return_a_module : concern_for_InteractionTasksForModule
   {
      private ICreateModulePresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ICreateModulePresenter>();
         A.CallTo(() => _presenter.CreateModule()).Returns(null);
         A.CallTo(() => _context.ApplicationController.Start<ICreateModulePresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         sut.CreateNewModuleWithBuildingBlocks();
      }

      [Observation]
      public void the_result_should_be_null()
      {
         A.CallTo(() => _context.Context.AddToHistory(A<IMoBiCommand>._)).MustNotHaveHappened();
      }
   }

   public class When_the_presenter_returns_a_module : concern_for_InteractionTasksForModule
   {
      private ICreateModulePresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ICreateModulePresenter>();
         A.CallTo(() => _presenter.CreateModule()).Returns(new Module());
         A.CallTo(() => _context.ApplicationController.Start<ICreateModulePresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         sut.CreateNewModuleWithBuildingBlocks();
      }

      [Observation]
      public void the_result_should_be_null()
      {
         A.CallTo(() => _context.Context.AddToHistory(A<AddModuleCommand>._)).MustHaveHappened();
      }
   }
}
