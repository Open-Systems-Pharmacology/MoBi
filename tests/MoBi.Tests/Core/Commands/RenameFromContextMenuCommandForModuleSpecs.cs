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
      protected IMoBiApplicationController _applicationController;

      protected override void Context()
      {
         var interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         var sourceModule = new Module().WithId("sourceModuleId").WithName("Source Module");
         var editTasks = A.Fake<EditTaskForModule>(options => options
            .WithArgumentsForConstructor(() => new EditTaskForModule(interactionTaskContext))
            .CallsBaseMethods());
         A.CallTo(() => interactionTaskContext.ApplicationController).Returns(_applicationController);
         A.CallTo(() => interactionTaskContext.NamingTask.RenameFor(A<IObjectBase>.Ignored, A<IReadOnlyList<string>>.Ignored)).Returns("Module1");

         var bb = new MoleculeBuildingBlock().WithId("newMoleculeBuildingBlockId");
         bb.Module = sourceModule;
         sourceModule.Add(bb);
         sut = new RenameFromContextMenuCommand<Module>(editTasks)
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
      public void should_not_call_applicationController_start()
      {
         A.CallTo(() => _applicationController.Start<ISelectRenamingPresenter>()).MustNotHaveHappened();
      }
   }
}