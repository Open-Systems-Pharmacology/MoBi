using FakeItEasy;

using BTS.BDDHelper;
using BTS.BDDHelper.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using SBSuite.Core.Domain;
using SBSuite.Core.Domain.Builder;

using SBSuite.Core.Domain.Services;


namespace MoBi.Core.Service
{
   public abstract class concern_for_SimulationCommitToBuildingBlockTasksSpecs : ContextSpecification<ISimulationCommitToBuildingBlockTasks>
   {
      protected ICreateCommitChangesToBuildingBlockCommandTaskRetriever _taskRetriever;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;


      protected override void Context()
      {
         _taskRetriever = A.Fake<ICreateCommitChangesToBuildingBlockCommandTaskRetriever>();
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         sut = new SimulationCommitToBuildingBlockTasks(_taskRetriever,_cloneManagerForBuildingBlock);
      }
   }

   class When_tell_a_SimulationCommitToBuildingBlockTasks_to_CommitChanges : concern_for_SimulationCommitToBuildingBlockTasksSpecs
   {
      private IMoBiSimulation _simulation;
      private IBuildingBlock _buildingBlock;
      private IMoBiCommand _resultCommand;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.MoBiBuildConfiguration).Returns( A.Fake<IMoBiBuildConfiguration>());
         _buildingBlock = A.Fake<IBuildingBlock>();
         var buildingBlockTask = A.Fake<ICreateCommitChangesToBuildingBlockCommandTask>();
         A.CallTo(() => _taskRetriever.TaskFor(_buildingBlock)).Returns(buildingBlockTask);
         A.CallTo(() => buildingBlockTask.CreateCommitToBuildingBlockCommand(_simulation,_buildingBlock)).Returns(A.Fake<IMoBiCommand>());
      }

      protected override void Because()
      {
         _resultCommand=sut.CommitSimulaitionChangesToBuildingBlock(_simulation, _buildingBlock);
      }

      [Observation]
      public void should_ask_for_building_block_specific_creat_command_task()
      {
         A.CallTo(() => _taskRetriever.TaskFor(_buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_return_macroCommand_conntaining_nessary_commands()
      {
         var macroCommand = _resultCommand as IMoBiMacroCommand;
         macroCommand.ShouldNotBeNull();
         macroCommand.Count.ShouldBeEqualTo(2);
         //TODO retrieve single Commands To Check
      }

   }

   class When_to_a_SimulationCommitToBuildingBlockTasks_an_empty_command_was_given_by_sub_task : concern_for_SimulationCommitToBuildingBlockTasksSpecs
   {
      private IMoBiSimulation _simulation;
      private IBuildingBlock _buildingBlock;
      private IMoBiCommand _resultCommand;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         var buildingBlockTask = A.Fake<ICreateCommitChangesToBuildingBlockCommandTask>();
         A.CallTo(() => _taskRetriever.TaskFor(_buildingBlock)).Returns(buildingBlockTask);
         A.CallTo(() => buildingBlockTask.CreateCommitToBuildingBlockCommand(_simulation, _buildingBlock)).Returns(new MoBiEmptyCommand());
      }

      protected override void Because()
      {
         _resultCommand = sut.CommitSimulaitionChangesToBuildingBlock(_simulation, _buildingBlock);
      }

      [Observation]
      public void should_return_empty_command()
      {
         _resultCommand.ShouldBeAnInstanceOf<MoBiEmptyCommand>();
      }

   }
}	