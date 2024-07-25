using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Events;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class RenameFromContextMenuCommandForModuleSpecs : ContextSpecification<RenameFromContextMenuCommand<Module>>
   {
      protected EditTaskForModule _editTask;
      protected override void Context()
      {
         var sourceModule = new Module().WithId("sourceModuleId").WithName("Source Module");
         _editTask = A.Fake<EditTaskForModule>();
         sut = new RenameFromContextMenuCommand<Module>(_editTask)
         {
            Subject = sourceModule
         };
      }
   }

   internal class When_renaming_a_module_from_contextmenucommand : RenameFromContextMenuCommandForModuleSpecs
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_call_rename_with_null_as_buildingblock()
      {
         A.CallTo(() => _editTask.Rename(A<Module>.Ignored, A< IEnumerable<IObjectBase>>.Ignored, null)).MustHaveHappened();
      }
   }
}