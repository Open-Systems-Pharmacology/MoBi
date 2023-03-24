using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Service
{
   public abstract class concern_for_RenameBuildingBlockTask : ContextSpecification<IRenameBuildingBlockTask>
   {
      private IMoBiProjectRetriever _projectRetriever;
      protected IMoBiProject _project;

      protected override void Context()
      {
         _project= A.Fake<IMoBiProject>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         A.CallTo(() => _projectRetriever.Current).Returns(_project);
         sut = new RenameBuildingBlockTask(_projectRetriever);
      }
   }

   public class When_renaming_a_building_block_used_in_simulations : concern_for_RenameBuildingBlockTask
   {
      private IBuildingBlock _templateBuildingBlock;
      private IMoBiSimulation _sim;
      private IBuildingBlockInfo _buildingBlockInfo;

      protected override void Context()
      {
         base.Context();
         _templateBuildingBlock= A.Fake<IBuildingBlock>();
         _templateBuildingBlock.Name = "toto";
         _sim= A.Fake<IMoBiSimulation>();
         _buildingBlockInfo= A.Fake<IBuildingBlockInfo>();
         // A.CallTo(() => _sim.Configuration.BuildingInfoForTemplate(_templateBuildingBlock)).Returns(_buildingBlockInfo);
         A.CallTo(() => _project.SimulationsCreatedUsing(_templateBuildingBlock)).Returns(new[] { _sim });
      }

      protected override void Because()
      {
         sut.RenameInSimulationUsingTemplateBuildingBlock(_templateBuildingBlock);
      }

      [Observation]
      public void should_rename_the_building_block_info_referencing_it()
      {
         _buildingBlockInfo.UntypedBuildingBlock.Name.ShouldBeEqualTo(_templateBuildingBlock.Name);   
      }
   }
}	