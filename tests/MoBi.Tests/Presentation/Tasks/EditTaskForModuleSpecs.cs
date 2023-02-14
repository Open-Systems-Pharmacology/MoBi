using System.Linq;
using FakeItEasy;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_EditTaskForModule : ContextSpecification<EditTaskForModule>
   {
      protected IInteractionTaskContext _interactionTaskContext;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         
         sut = new EditTaskForModule(_interactionTaskContext);
      }
   }

   public class When_asking_for_prohibited_names_for_a_module : concern_for_EditTaskForModule
   {
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         sut.GetForbiddenNames(_module, Enumerable.Empty<IObjectBase>());
      }

      [Observation]
      public void all_modules_in_project_should_be_listed()
      {
         A.CallTo(() => _interactionTaskContext.Context.CurrentProject.Modules).MustHaveHappened();
      }
   }
}
