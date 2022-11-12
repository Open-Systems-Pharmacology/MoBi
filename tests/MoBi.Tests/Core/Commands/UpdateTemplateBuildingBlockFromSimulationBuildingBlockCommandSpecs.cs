using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateTemplateBuildingBlockFromSimulationBuildingBlockCommand : ContextSpecification<UpdateTemplateBuildingBlockFromSimulationBuildingBlockCommand<IParameterStartValuesBuildingBlock>>
   {
      protected IMoBiSimulation _simulation;
      protected IParameterStartValuesBuildingBlock _oldTemplateBuildingBlock;
      protected IParameterStartValuesBuildingBlock _clonedSimulationBuildingBlock;
      protected IBuildingBlockInfo _simulationBuildingBlockInfo;
      protected IBuildingBlock _simulationBuildingBlock;
      protected IMoBiContext _context;
      protected IBuildingBlockReferenceUpdater _buildingBlockInfoUpdater;

      protected override void Context()
      {
         _simulation = A.Fake<IMoBiSimulation>();
         _buildingBlockInfoUpdater= A.Fake<IBuildingBlockReferenceUpdater>();
         _oldTemplateBuildingBlock = A.Fake<IParameterStartValuesBuildingBlock>().WithName("OLD TEMPLATE");
         _clonedSimulationBuildingBlock = A.Fake<IParameterStartValuesBuildingBlock>().WithName("CLONE SIMULATION");
         _oldTemplateBuildingBlock.Version = 7;
         _clonedSimulationBuildingBlock.Version = 4;
         _simulationBuildingBlockInfo = A.Fake<IBuildingBlockInfo>();
         _simulationBuildingBlockInfo.SimulationChanges = 5;
         _simulationBuildingBlockInfo.UntypedTemplateBuildingBlock = _oldTemplateBuildingBlock;
         _simulationBuildingBlock = A.Fake<IBuildingBlock>();
         _simulationBuildingBlockInfo.UntypedBuildingBlock = _simulationBuildingBlock;
         _simulationBuildingBlock.Version = _clonedSimulationBuildingBlock.Version;
         _simulationBuildingBlock.Name = "TRALALA";
         A.CallTo(_simulation.MoBiBuildConfiguration).WithReturnType<IBuildingBlockInfo>().Returns(_simulationBuildingBlockInfo);
         _context= A.Fake<IMoBiContext>();
         A.CallTo(() =>_context.Resolve<IBuildingBlockReferenceUpdater>()).Returns(_buildingBlockInfoUpdater);
         sut = new UpdateTemplateBuildingBlockFromSimulationBuildingBlockCommand<IParameterStartValuesBuildingBlock>(_oldTemplateBuildingBlock, _clonedSimulationBuildingBlock, _simulation);
      }
   }

   public class When_executing_the_update_template_from_building_block_command : concern_for_UpdateTemplateBuildingBlockFromSimulationBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_ensure_that_the_version_of_the_template_building_block_and_clone_building_block_are_the_same()
      {
         _simulationBuildingBlock.Version.ShouldBeEqualTo(_clonedSimulationBuildingBlock.Version);
      }

      [Observation]
      public void should_have_set_the_version_of_the_clone_simulation_building_block_to_be_the_version_of_the_previous_template_plus_one()
      {
         _simulationBuildingBlock.Version.ShouldBeEqualTo(_oldTemplateBuildingBlock.Version + 1);
      }


      [Observation]
      public void should_have_reseted_the_simulation_changes_count_to_0()
      {
         _simulationBuildingBlockInfo.SimulationChanges.ShouldBeEqualTo((uint)0);
      }

      [Observation]
      public void should_update_the_references_to_the_old_building_block_using_the_new_building_block()
      {
         A.CallTo(() => _buildingBlockInfoUpdater.UpdateTemplateReference(_context.CurrentProject,_clonedSimulationBuildingBlock)).MustHaveHappened();
      }
   }
}