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
      protected MoleculeBuildingBlock _bb;
      protected Module _sourceModule;
      protected IMoBiApplicationController _applicationController;
      private EditTaskForModule _editTasks;
      private IInteractionTaskContext _interactionTaskContext;
      private ISelectRenamingPresenter _selectRenamingPresenter;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _selectRenamingPresenter = A.Fake<ISelectRenamingPresenter>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _sourceModule = new Module().WithId("sourceModuleId").WithName("Source Module");
         _editTasks = A.Fake<EditTaskForModule>(options => options
            .WithArgumentsForConstructor(() => new EditTaskForModule(_interactionTaskContext))
            .CallsBaseMethods());
         A.CallTo(() => _applicationController.Start<ISelectRenamingPresenter>()).Returns(_selectRenamingPresenter);
         A.CallTo(() => _interactionTaskContext.ApplicationController).Returns(_applicationController);
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(A<IObjectBase>.Ignored, A<IReadOnlyList<string>>.Ignored)).Returns("Module1");

         _bb = new MoleculeBuildingBlock().WithId("newMoleculeBuildingBlockId");
         _bb.Module = _sourceModule;
         _sourceModule.Add(_bb);
         sut = new RenameFromContextMenuCommand<Module>(_editTasks)
         {
            Subject = _sourceModule
         };
      }
   }

   internal class When_renaming_a_module_from_contextmenucommand : RenameFromContextMenuCommandForModuleSpecs
   {
      private AddedEvent _event;

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_rename_module_but_not_related_objects()
      {
         A.CallTo(() => _applicationController.Start<ISelectRenamingPresenter>()).MustNotHaveHappened();
      }
   }
}