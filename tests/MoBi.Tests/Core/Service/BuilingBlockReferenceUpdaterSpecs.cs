using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Service
{
   public abstract class concern_for_BuilingBlockReferenceUpdater : ContextSpecification<IBuildingBlockReferenceUpdater>
   {
      protected override void Context()
      {
         sut = new BuildingBlockReferenceUpdater();
      }
   }

   public class When_upading_the_references_to_all_template_building_blocks_defined_in_the_project : concern_for_BuilingBlockReferenceUpdater
   {
      private IMoBiProject _project;
      private IBuildingBlockInfo _buildInfoSim1;
      private IBuildingBlockInfo _buildInfoSim2;
      private IBuildingBlock _template1;
      private IBuildingBlock _template2;

      protected override void Context()
      {
         base.Context();
         _project= A.Fake<IMoBiProject>();
         var sim1= A.Fake<IMoBiSimulation>();
         var sim2= A.Fake<IMoBiSimulation>();
         _template1 = A.Fake<IBuildingBlock>().WithName("Template1");
         _template2 = A.Fake<IBuildingBlock>().WithName("Template2");
         _buildInfoSim1= A.Fake<IBuildingBlockInfo>();
         _buildInfoSim1.TemplateBuildingBlockId = "T1";

         _buildInfoSim2= A.Fake<IBuildingBlockInfo>();
         _buildInfoSim2.TemplateBuildingBlockId = "T2";

         // A.CallTo(() => sim1.MoBiBuildConfiguration.AllBuildingBlockInfos()).Returns(new[]{_buildInfoSim1});
         // A.CallTo(() => sim2.MoBiBuildConfiguration.AllBuildingBlockInfos()).Returns(new[]{_buildInfoSim2});
         A.CallTo(() => _project.Simulations).Returns(new[]{sim1,sim2});
         A.CallTo(() => _project.TemplateById(_buildInfoSim1.TemplateBuildingBlockId)).Returns(_template1);
         A.CallTo(() => _project.TemplateById(_buildInfoSim2.TemplateBuildingBlockId)).Returns(_template2);
      }

      protected override void Because()
      {
         sut.UpdateTemplatesReferencesIn(_project);
      }

      [Observation]
      public void should_set_the_reference_to_the_used_template_building_block_in_all_simulation()
      {
         _buildInfoSim1.UntypedTemplateBuildingBlock.ShouldBeEqualTo(_template1);
         _buildInfoSim2.UntypedTemplateBuildingBlock.ShouldBeEqualTo(_template2);
      }
   }

   public class When_updating_the_references_to_a_given_template_building_block : concern_for_BuilingBlockReferenceUpdater
   {
      private IMoBiProject _project;
      private IBuildingBlock _templateBuildingBlock;
      private IBuildingBlockInfo _buildingBlockInfo;

      protected override void Context()
      {
         base.Context();
         _project= A.Fake<IMoBiProject>();
         var sim1 = A.Fake<IMoBiSimulation>();
         _templateBuildingBlock= A.Fake<IBuildingBlock>();
         _buildingBlockInfo= A.Fake<IBuildingBlockInfo>();  
         A.CallTo(() => _project.SimulationsCreatedUsing(_templateBuildingBlock)).Returns(new []{sim1});
         // A.CallTo(() => sim1.MoBiBuildConfiguration.BuildingInfoForTemplate(_templateBuildingBlock)).Returns(_buildingBlockInfo);
      }
      protected override void Because()
      {
         sut.UpdateTemplateReference(_project,_templateBuildingBlock);
      }

      [Observation]
      public void should_update_the_reference_to_that_template_in_all_simulation_using_a_template_with_the_same_id()
      {
         _buildingBlockInfo.UntypedTemplateBuildingBlock.ShouldBeEqualTo(_templateBuildingBlock);
      }
   }
}	