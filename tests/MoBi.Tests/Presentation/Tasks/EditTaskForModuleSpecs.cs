using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using Module = OSPSuite.Core.Domain.Module;

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

   public class When_renaming_a_module : ContextSpecification<EditTaskForModule>
   {
      private Module _module;
      private List<IObjectBase> _existingObjectsInParent;
      private EditTaskForModule _sut;
      private IInteractionTaskContext _interactionTaskContext;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _sut = A.Fake<EditTaskForModule>(options => options
            .WithArgumentsForConstructor(() => new EditTaskForModule(_interactionTaskContext))
            .CallsBaseMethods());

         _module = new Module().WithName("newModule1");
         _existingObjectsInParent = new List<IObjectBase> { new Module().WithName("newModule2") };
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(A<IObjectBase>.Ignored, A<IReadOnlyList<string>>.Ignored)).Returns("Module1");
      }

      protected override void Because()
      {
         _sut.Rename(_module, _existingObjectsInParent, new MoleculeBuildingBlock().WithId("newMoleculeBuildingBlockId"));
      }

      [Observation]
      public void should_rename_module_but_not_related_objects()
      {
         A.CallTo(_sut).Where(x => x.Method.Name.Contains("GetRenameCommandFor") && x.Arguments.Get<IBuildingBlock>(1) == null).MustHaveHappened();
      }
   }
}