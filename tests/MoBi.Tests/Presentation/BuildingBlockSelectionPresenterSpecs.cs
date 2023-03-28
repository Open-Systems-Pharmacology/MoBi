using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
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
         sut.Edit(null);
      }

      [Observation]
      public void should_set_the_first_available_building_block_as_selected()
      {
         _buildingBlockSelectionDTO.SelectedObject.ShouldBeEqualTo(_templateBuildingBlock1);
      }
   }

   public class When_editing_a_building_block_info_that_references_a_template_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      protected override void Context()
      {
         base.Context();
      }

      protected override void Because()
      {
         sut.Edit(_templateBuildingBlock2);
      }

      [Observation]
      public void should_set_the_first_available_building_block_as_selected()
      {
         _buildingBlockSelectionDTO.SelectedObject.ShouldBeEqualTo(_templateBuildingBlock2);
      }

      [Observation]
      public void the_list_of_available_building_blocks_should_not_contain_the_template_building_block_twice()
      {
         sut.AllAvailableBlocks.ShouldOnlyContainInOrder(_templateBuildingBlock1, _templateBuildingBlock2);
      }
   }

   public class When_editing_a_building_block_info_that_references_a_building_block_that_is_not_a_template_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      protected override void Because()
      {
         sut.Edit(_simulationBuildingBlock);
      }

      [Observation]
      public void should_set_the_first_available_building_block_as_selected()
      {
         _buildingBlockSelectionDTO.SelectedObject.ShouldBeEqualTo(_simulationBuildingBlock);
      }

      [Observation]
      public void the_list_of_available_building_blocks_should_contain_the_simulation_building_block_first()
      {
         sut.AllAvailableBlocks.ShouldOnlyContainInOrder(_templateBuildingBlock1, _templateBuildingBlock2);
      }
   }

   public class When_editing_a_building_block_info_that_references_a_building_block_that_is_not_a_template_building_block_and_switching_to_a_template_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_simulationBuildingBlock);
      }

      protected override void Because()
      {
         _buildingBlockSelectionDTO.SelectedObject = _templateBuildingBlock1;
         sut.SelectedBuildingBlockChanged();
      }

      [Observation]
      public void should_set_the_template_building_block_to_the_newly_selected_building_block()
      {
         sut.SelectedBuildingBlock.ShouldBeEqualTo(_templateBuildingBlock1);
      }
   }

   public class When_retrieving_the_display_name_of_a_building_block : concern_for_BuildingBlockSelectionPresenter
   {
      [Observation]
      public void should_return_the_name_of_the_building_block_for_a_template_building_block()
      {
         sut.DisplayNameFor(_templateBuildingBlock1).ShouldBeEqualTo(_templateBuildingBlock1.Name);
      }

   }
}