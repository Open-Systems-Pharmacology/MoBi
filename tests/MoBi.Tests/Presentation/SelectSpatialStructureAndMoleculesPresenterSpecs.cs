using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_SelectSpatialStructureAndMoleculesPresenter : ContextSpecification<SelectBuildingBlocksForExtendPresenter>
   {
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected ISelectSpatialStructureAndMoleculesView _view;

      protected override void Context()
      {
         _view = A.Fake<ISelectSpatialStructureAndMoleculesView>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         sut = new SelectBuildingBlocksForExtendPresenter(_view, _buildingBlockRepository);
      }
   }

   public class When_Selecting_building_blocks_and_the_view_is_canceled : concern_for_SelectSpatialStructureAndMoleculesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new [] { new MoleculeBuildingBlock() });
         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new [] { new MoBiSpatialStructure() });
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.SelectBuildingBlocksForExtend();
      }

      [Observation]
      public void the_selections_should_be_null()
      {
         sut.SelectedMoleculeBuildingBlock.ShouldBeNull();
         sut.SelectedSpatialStructure.ShouldBeNull();
      }
   }

   public class When_selecting_building_blocks_when_there_is_no_spatial_structures : concern_for_SelectSpatialStructureAndMoleculesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new List<MoleculeBuildingBlock> { new MoleculeBuildingBlock() });
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.SelectBuildingBlocksForExtend();
      }

      [Observation]
      public void the_selected_building_block_should_be_null()
      {
         sut.SelectedMoleculeBuildingBlock.ShouldNotBeNull();
      }

      [Observation]
      public void the_selected_spatial_structure_is_still_set()
      {
         sut.SelectedSpatialStructure.ShouldBeNull();
      }
   }

   public class When_selecting_building_blocks_when_there_is_no_molecules : concern_for_SelectSpatialStructureAndMoleculesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new List<MoBiSpatialStructure> { new MoBiSpatialStructure() });
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.SelectBuildingBlocksForExtend();
      }

      [Observation]
      public void the_selected_building_block_should_be_null()
      {
         sut.SelectedMoleculeBuildingBlock.ShouldBeNull();
      }

      [Observation]
      public void the_selected_spatial_structure_is_still_set()
      {
         sut.SelectedSpatialStructure.ShouldNotBeNull();
      }
   }
}
