using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_BuildingBlockSelectionPresenter : ContextSpecification<IBuildingBlockSelectionPresenter<IMoBiReactionBuildingBlock>>
   {
      private IMoBiContext _context;
      private IBuildingBlockSelectionView _view;
      private IBuildingBlockRepository _buildingBlockRepository;
      private IInteractionTasksForBuildingBlock<IMoBiReactionBuildingBlock> _interactionTask;
      protected IBuildingBlockInfo<IMoBiReactionBuildingBlock> _buildingBlockInfo;
      protected List<IMoBiReactionBuildingBlock> _allBuildingBlocks;
      protected IMoBiReactionBuildingBlock _templateBuildingBlock1;
      protected IMoBiReactionBuildingBlock _templateBuildingBlock2;
      protected BuildingBlockSelectionDTO _buildingBlockSelectionDTO;
      protected IMoBiReactionBuildingBlock _simulationBuildingBlock;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _view = A.Fake<IBuildingBlockSelectionView>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _interactionTask = A.Fake<IInteractionTasksForBuildingBlock<IMoBiReactionBuildingBlock>>();
         sut = new BuildingBlockSelectionPresenter<IMoBiReactionBuildingBlock>(_view, _buildingBlockRepository, _interactionTask, _context);

         _buildingBlockInfo = new ReactionBuildingBlockInfo();
         _templateBuildingBlock1 = A.Fake<IMoBiReactionBuildingBlock>().WithName("BB1");
         _templateBuildingBlock2 = A.Fake<IMoBiReactionBuildingBlock>().WithName("BB2");
         _simulationBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("BBSIM");
         _allBuildingBlocks = new List<IMoBiReactionBuildingBlock> {_templateBuildingBlock1, _templateBuildingBlock2};
         A.CallTo(() => _buildingBlockRepository.All<IMoBiReactionBuildingBlock>()).Returns(_allBuildingBlocks);

         A.CallTo(() => _view.BindTo(A<BuildingBlockSelectionDTO>._))
            .Invokes(x => _buildingBlockSelectionDTO = x.GetArgument<BuildingBlockSelectionDTO>(0));
      }
   }

   public class When_editing_a_building_block_info_that_does_not_reference_any_building_blocks : concern_for_BuildingBlockSelectionPresenter
   {
      protected override void Because()
      {
         sut.Edit(_buildingBlockInfo);
      }

      [Observation]
      public void should_set_the_first_available_building_block_as_selected()
      {
         _buildingBlockInfo.TemplateBuildingBlock.ShouldBeEqualTo(_templateBuildingBlock1);
         _buildingBlockInfo.BuildingBlock.ShouldBeEqualTo(_buildingBlockInfo.TemplateBuildingBlock);
         _buildingBlockSelectionDTO.BuildingBlock.ShouldBeEqualTo(_templateBuildingBlock1);
      }
   }

   public class When_editing_a_building_block_info_that_references_a_template_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      protected override void Context()
      {
         base.Context();
         _buildingBlockInfo.BuildingBlock = _templateBuildingBlock2;
         _buildingBlockInfo.TemplateBuildingBlock = _templateBuildingBlock2;
      }

      protected override void Because()
      {
         sut.Edit(_buildingBlockInfo);
      }

      [Observation]
      public void should_set_the_first_available_building_block_as_selected()
      {
         _buildingBlockInfo.TemplateBuildingBlock.ShouldBeEqualTo(_templateBuildingBlock2);
         _buildingBlockSelectionDTO.BuildingBlock.ShouldBeEqualTo(_templateBuildingBlock2);
      }

      [Observation]
      public void the_list_of_avaiable_building_blocks_should_not_contain_the_template_building_block_twice()
      {
         sut.AllAvailableBlocks.ShouldOnlyContainInOrder(_templateBuildingBlock1, _templateBuildingBlock2);
      }
   }

   public class When_editing_a_building_block_info_that_references_a_building_block_that_is_not_a_template_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      protected override void Context()
      {
         base.Context();
         _buildingBlockInfo.BuildingBlock = _simulationBuildingBlock;
         _buildingBlockInfo.TemplateBuildingBlock = _templateBuildingBlock2;
      }

      protected override void Because()
      {
         sut.Edit(_buildingBlockInfo);
      }

      [Observation]
      public void should_set_the_first_available_building_block_as_selected()
      {
         _buildingBlockInfo.TemplateBuildingBlock.ShouldBeEqualTo(_templateBuildingBlock2);
         _buildingBlockSelectionDTO.BuildingBlock.ShouldBeEqualTo(_simulationBuildingBlock);
      }

      [Observation]
      public void the_list_of_avaiable_building_blocks_should_contain_the_simulation_building_block_first()
      {
         sut.AllAvailableBlocks.ShouldOnlyContainInOrder(_simulationBuildingBlock, _templateBuildingBlock1, _templateBuildingBlock2);
      }
   }

   public class When_editing_a_building_block_info_that_references_a_building_block_that_is_not_a_template_building_block_and_switching_to_a_template_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      protected override void Context()
      {
         base.Context();
         _buildingBlockInfo.BuildingBlock = _simulationBuildingBlock;
         _buildingBlockInfo.TemplateBuildingBlock = _templateBuildingBlock2;
         _buildingBlockInfo.SimulationChanges = 3;
         sut.Edit(_buildingBlockInfo);
      }

      protected override void Because()
      {
         _buildingBlockSelectionDTO.BuildingBlock = _templateBuildingBlock1;
         sut.SelectedBuildingBlockChanged();
      }

      [Observation]
      public void should_set_the_template_building_block_to_the_newly_selected_building_block()
      {
         _buildingBlockInfo.TemplateBuildingBlock.ShouldBeEqualTo(_templateBuildingBlock1);
      }

      [Observation]
      public void should_reset_the_simulation_changes_count()
      {
         _buildingBlockInfo.SimulationChanges.ShouldBeEqualTo((uint)0);
      }
   }

   public class When_editing_a_building_block_info_that_references_a_building_block_that_is_not_a_template_building_block_and_switching_to_a_template_building_block_and_back_to_the_simulation_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      protected override void Context()
      {
         base.Context();
         _buildingBlockInfo.BuildingBlock = _simulationBuildingBlock;
         _buildingBlockInfo.SimulationChanges = 3;
         _buildingBlockInfo.TemplateBuildingBlock = _templateBuildingBlock2;
         sut.Edit(_buildingBlockInfo);
         _buildingBlockSelectionDTO.BuildingBlock = _templateBuildingBlock1;
         sut.SelectedBuildingBlockChanged();
      }

      protected override void Because()
      {
         _buildingBlockSelectionDTO.BuildingBlock = _simulationBuildingBlock;
         sut.SelectedBuildingBlockChanged();
      }

      [Observation]
      public void should_set_the_original_template_building_block_corresponding_to_the_simulation_building_block()
      {
         _buildingBlockInfo.TemplateBuildingBlock.ShouldBeEqualTo(_templateBuildingBlock2);
      }

      [Observation]
      public void should_reset_the_simulation_changes_count()
      {
         _buildingBlockInfo.SimulationChanges.ShouldBeEqualTo((uint)3);
      }
   }

   public class When_retrieving_the_display_name_of_a_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      [Observation]
      public void should_return_the_name_of_the_building_block_for_a_template_building_block()
      {
         sut.DisplayNameFor(_templateBuildingBlock1).ShouldBeEqualTo(_templateBuildingBlock1.Name);
      }

      [Observation]
      public void should_return_a_composite_name_for_a_building_block_that_is_not_a_template_building_block_showing_that_this_is_not_a_templaet_building_block()
      {
         var dusplayName = sut.DisplayNameFor(_simulationBuildingBlock);
         dusplayName.ShouldNotBeEqualTo(_simulationBuildingBlock.Name);
         dusplayName.Contains(_simulationBuildingBlock.Name).ShouldBeTrue();
         dusplayName.Contains(AppConstants.Warnings.ThisItNotATemplateBuildingBlock).ShouldBeTrue();
      }
   }
}