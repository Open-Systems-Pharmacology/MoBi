using FakeItEasy;
using MoBi.Core.Events;
using MoBi.Presentation.Core;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_CloseSubjectPresenterInvoker : ContextSpecification<ICloseSubjectPresenterInvoker>
   {
      protected IApplicationController _applicationController;

      protected override void Context()
      {
         _applicationController= A.Fake<IApplicationController>();
         sut = new CloseSubjectPresenterInvoker(_applicationController);
      }
   }

   public class When_the_close_subject_presenter_invoker_is_notified_that_a_simulation_is_being_removed : concern_for_CloseSubjectPresenterInvoker
   {
      private ISimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation= A.Fake<ISimulation>();
      }

      protected override void Because()
      {
         sut.Handle(new SimulationRemovedEvent(_simulation));
      }

      [Observation]
      public void should_close_the_simulation()
      {
         A.CallTo(() => _applicationController.Close(_simulation)).MustHaveHappened();   
      }
   }

   public class When_the_close_subject_presenter_invoker_is_notified_that_the_project_is_being_closed : concern_for_CloseSubjectPresenterInvoker
   {
      protected override void Because()
      {
         sut.Handle(new ProjectClosedEvent());
      }

      [Observation]
      public void should_close_all_presenters()
      {
         A.CallTo(() => _applicationController.CloseAll()).MustHaveHappened();   
      }
   }

   public class When_the_close_subkect_presenter_invoker_is_notified_that_a_set_of_objects_is_being_removed : concern_for_CloseSubjectPresenterInvoker
   {
      private IObjectBase _subject1;
      private IObjectBase _subject2;

      protected override void Context()
      {
         base.Context();
         _subject1= A.Fake<IObjectBase>();
         _subject2= A.Fake<IObjectBase>();
      }

      protected override void Because()
      {
         sut.Handle(new RemovedEvent(new []{_subject1, _subject2}));
      }

      [Observation]
      public void should_close_the_presenter_for_each_removed_subject()
      {
         A.CallTo(() => _applicationController.Close(_subject1)).MustHaveHappened();   
         A.CallTo(() => _applicationController.Close(_subject2)).MustHaveHappened();   
      }
   }
}	