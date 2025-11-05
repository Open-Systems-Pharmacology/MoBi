using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_EditTaskForModule : ContextSpecification<EditTaskForModule>
   {
      protected IInteractionTaskContext _interactionTaskContext;
      private MoBiProject _project;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _project = DomainHelperForSpecs.NewProject();
         A.CallTo(() => _interactionTaskContext.Context.CurrentProject).Returns(_project);
         sut = new EditTaskForModule(_interactionTaskContext);
         _project.AddModule(new Module().WithName("newModule1"));
         _project.AddModule(new Module().WithName("newModule2"));
      }
   }

   public class When_asking_for_prohibited_names_for_a_module : concern_for_EditTaskForModule
   {
      private Module _module;
      private List<string> _prohibitedNames;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
      }

      protected override void Because()
      {
         _prohibitedNames = sut.GetForbiddenNames(_module, Enumerable.Empty<IObjectBase>()).ToList();
      }

      [Observation]
      public void all_modules_in_project_should_be_listed()
      {
         _prohibitedNames.ShouldContain("newModule1", "newModule2");
      }
   }
}