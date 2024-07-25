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
   public abstract class RenameFromContextMenuCommandForBuildingBlockSpecs : ContextSpecification<RenameFromContextMenuCommand<MoleculeBuildingBlock>>
   {
      protected IMoBiApplicationController _applicationController;
      protected IInteractionTaskContext _interactionTaskContext;
      private EditTasksForBuildingBlock<MoleculeBuildingBlock> _editTasks;
      private ISelectRenamingPresenter _selectRenamingPresenter;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _selectRenamingPresenter = A.Fake<ISelectRenamingPresenter>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         var sourceModule = new Module().WithId("sourceModuleId").WithName("Source Module");
         _editTasks = A.Fake<EditTasksForBuildingBlock<MoleculeBuildingBlock>>(options => options
            .WithArgumentsForConstructor(() => new EditTasksForBuildingBlock<MoleculeBuildingBlock>(_interactionTaskContext))
            .CallsBaseMethods());
         A.CallTo(() => _applicationController.Start<ISelectRenamingPresenter>()).Returns(_selectRenamingPresenter);
         A.CallTo(() => _interactionTaskContext.ApplicationController).Returns(_applicationController);
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(A<IObjectBase>.Ignored, A<IReadOnlyList<string>>.Ignored)).Returns("Module1");

         var bb = new MoleculeBuildingBlock().WithId("newMoleculeBuildingBlockId");
         bb.Module = sourceModule;
         sourceModule.Add(bb);
         sut = new RenameFromContextMenuCommand<MoleculeBuildingBlock>(_editTasks)
         {
            Subject = bb
         };
      }
   }

   internal class When_renaming_a_buildingBlock_from_contextmenucommand : RenameFromContextMenuCommandForBuildingBlockSpecs
   {
      private AddedEvent _event;

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_call_getpossiblechangesfrom()
      {
         A.CallTo(() => _interactionTaskContext.CheckNamesVisitor.GetPossibleChangesFrom(A<IObjectBase>.Ignored, A<string>.Ignored, A<IBuildingBlock>.Ignored, A<string>.Ignored)).MustHaveHappened();
      }
   }
}