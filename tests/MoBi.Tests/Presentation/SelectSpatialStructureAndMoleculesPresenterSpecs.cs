using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_SelectSpatialStructureAndMoleculesPresenter : ContextSpecification<SelectSpatialStructureAndMoleculesPresenter>
   {
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected ISelectSpatialStructureAndMoleculesView _view;
      private ISelectSpatialStructureAndMoleculesDTOMapper _mapper;

      protected override void Context()
      {
         _view = A.Fake<ISelectSpatialStructureAndMoleculesView>();
         _mapper = A.Fake<ISelectSpatialStructureAndMoleculesDTOMapper>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         sut = new SelectSpatialStructureAndMoleculesPresenter(_view, _buildingBlockRepository, _mapper);
      }
   }

   public class When_selecting_building_blocks_and_the_view_is_canceled : concern_for_SelectSpatialStructureAndMoleculesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new[] { new MoleculeBuildingBlock() });
         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new[] { new MoBiSpatialStructure() });
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.SelectBuildingBlocksForExtend();
      }

      [Observation]
      public void the_selections_should_be_null_or_empty()
      {
         sut.SelectedMolecules.ShouldBeEmpty();
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
         sut.SelectedMolecules.ShouldNotBeNull();
      }

      [Observation]
      public void the_selected_spatial_structure_is_still_set()
      {
         sut.SelectedSpatialStructure.ShouldBeNull();
      }
   }
}