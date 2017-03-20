using OSPSuite.Core.Commands.Core;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Presentation
{
   public abstract class concern_for_CommitSimulationChangesToBuildingBlockUICommandSpecs : ContextSpecification<CommitSimulationChangesToBuildingBlockUICommand>
   {
      protected ICreateCommitChangesToBuildingBlockCommandTaskRetriever _createCommitChangesCommandTaskRetriever;
      protected IMoBiContext _context;
      protected IPendingChangesChecker _pendingChangesChecker;

      protected override void Context()
      {
         _createCommitChangesCommandTaskRetriever = A.Fake<ICreateCommitChangesToBuildingBlockCommandTaskRetriever>();
         _context = A.Fake<IMoBiContext>();
         _pendingChangesChecker = A.Fake<IPendingChangesChecker>();
         sut = new CommitSimulationChangesToBuildingBlockUICommand(_createCommitChangesCommandTaskRetriever,_context, _pendingChangesChecker);
      }
   }

   class When_executing_a_CommitSimulationChangesToBuildingBlockUICommand : concern_for_CommitSimulationChangesToBuildingBlockUICommandSpecs
   {
      private ICreateCommitChangesToBuildingBlockCommandTask _createCommitChangesCommandTask;
      private IBuildingBlock _buildingBlock;
      private IMoBiSimulation _simulation;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _simulation =A.Fake<IMoBiSimulation>();
         _createCommitChangesCommandTask = A.Fake<ICreateCommitChangesToBuildingBlockCommandTask>();
         _command = A.Fake<IMoBiCommand>();
         A.CallTo(() => _createCommitChangesCommandTaskRetriever.TaskFor(A<IBuildingBlock>._)).Returns(_createCommitChangesCommandTask);
         sut.Initialize(_buildingBlock, _simulation);
         A.CallTo(() => _createCommitChangesCommandTask.CreateCommitToBuildingBlockCommand(_simulation,_buildingBlock)).Returns(_command);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_create_the_commit_command()
      {
         A.CallTo(() => _createCommitChangesCommandTask.CreateCommitToBuildingBlockCommand(_simulation,_buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_execute_the_command()
      {
         A.CallTo(() => _command.Execute(_context)).MustHaveHappened();
      }

      [Observation]
      public void should_check_if_still_changes_are_pending()
      {
         A.CallTo(() => _pendingChangesChecker.CheckForBuildingBlockChanges(A<IBuildingBlockInfo>._,_buildingBlock)).MustHaveHappened();
      }
   }

   class When_executing_a_CommitSimulationChangesToBuildingBlockUICommand_and_no_changes_to_commit: concern_for_CommitSimulationChangesToBuildingBlockUICommandSpecs
   {
      private ICreateCommitChangesToBuildingBlockCommandTask _createCommitChangesCommandTask;
      private IBuildingBlock _buildingBlock;
      private IMoBiSimulation _simulation;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _simulation = A.Fake<IMoBiSimulation>();
         _createCommitChangesCommandTask = A.Fake<ICreateCommitChangesToBuildingBlockCommandTask>();
         _command = A.Fake<MoBiEmptyCommand>();
         A.CallTo(() => _createCommitChangesCommandTaskRetriever.TaskFor(A<IBuildingBlock>._)).Returns(_createCommitChangesCommandTask);
         sut.Initialize(_buildingBlock, _simulation);
         A.CallTo(() => _createCommitChangesCommandTask.CreateCommitToBuildingBlockCommand(_simulation, _buildingBlock)).Returns(_command);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_create_the_commit_command()
      {
         A.CallTo(() => _createCommitChangesCommandTask.CreateCommitToBuildingBlockCommand(_simulation, _buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_check_if_still_changes_are_pending()
      {
         A.CallTo(() => _pendingChangesChecker.CheckForBuildingBlockChanges(A<IBuildingBlockInfo>._, _buildingBlock)).MustNotHaveHappened();
      }
   }
}	