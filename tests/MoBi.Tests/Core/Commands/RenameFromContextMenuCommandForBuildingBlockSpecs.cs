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
      protected EditTasksForBuildingBlock<MoleculeBuildingBlock> _editTasks;
      protected MoleculeBuildingBlock _buildingBlock;
      protected override void Context()
      {
         _editTasks = A.Fake<EditTasksForBuildingBlock<MoleculeBuildingBlock>>();
         _buildingBlock = new MoleculeBuildingBlock().WithId("newMoleculeBuildingBlockId");
         sut = new RenameFromContextMenuCommand<MoleculeBuildingBlock>(_editTasks)
         {
            Subject = _buildingBlock
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
      public void should_call_rename_with_moleculebuildingblock()
      {
         A.CallTo(() => _editTasks.Rename(A<MoleculeBuildingBlock>.Ignored, A<IEnumerable<IObjectBase>>.Ignored, _buildingBlock)).MustHaveHappened();
      }
   }
}