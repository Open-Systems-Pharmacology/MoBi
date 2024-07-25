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
      protected IInteractionTaskContext _interactionTaskContext;
      private EditTasksForBuildingBlock<MoleculeBuildingBlock> _editTasks;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         var sourceModule = new Module().WithId("sourceModuleId").WithName("Source Module");
         _editTasks = A.Fake<EditTasksForBuildingBlock<MoleculeBuildingBlock>>(options => options
            .WithArgumentsForConstructor(() => new EditTasksForBuildingBlock<MoleculeBuildingBlock>(_interactionTaskContext))
            .CallsBaseMethods());
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