using System;
using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation
{
   public class concern_for_SelectSpatialStructureAndMoleculesPresenter : ContextSpecification<SelectSpatialStructureAndMoleculesPresenter>
   {
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected ISelectSpatialStructureAndMoleculesView _view;
      protected ISelectSpatialStructureDTOMapper _mapper;
      protected ISelectMoleculesPresenter _selectMoleculesPresenter;

      protected override void Context()
      {
         _view = A.Fake<ISelectSpatialStructureAndMoleculesView>();
         _mapper = new SelectSpatialStructureDTOMapper();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _selectMoleculesPresenter = A.Fake<ISelectMoleculesPresenter>();
         sut = new SelectSpatialStructureAndMoleculesPresenter(_view, _buildingBlockRepository, _mapper, _selectMoleculesPresenter);
      }
   }

   public class When_initializing_the_view_with_default_building_blocks : concern_for_SelectSpatialStructureAndMoleculesPresenter
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private MoBiSpatialStructure _moBiSpatialStructure;
      private MoBiSpatialStructure _unselectedSpatialStructure;
      private MoleculeBuilder _molecule;
      private MoleculeBuildingBlock[] _moleculeBuildingBlocks;
      private MoBiSpatialStructure[] _moBiSpatialStructures;
      private MoleculeBuildingBlock _unselectedMoleculeBuildingBlock;
      private MoleculeBuilder _unselectedMolecule;

      protected override void Context()
      {
         base.Context();
         _molecule = new MoleculeBuilder();
         _unselectedMolecule = new MoleculeBuilder();
         _moleculeBuildingBlock = new MoleculeBuildingBlock {_molecule};
         _unselectedMoleculeBuildingBlock = new MoleculeBuildingBlock {_unselectedMolecule};

         _moleculeBuildingBlocks = new[] {_unselectedMoleculeBuildingBlock, _moleculeBuildingBlock};
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(_moleculeBuildingBlocks);
         _moBiSpatialStructure = new MoBiSpatialStructure();
         _unselectedSpatialStructure = new MoBiSpatialStructure();
         _moBiSpatialStructures = new[] {_unselectedSpatialStructure, _moBiSpatialStructure};
         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(_moBiSpatialStructures);
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.SelectBuildingBlocksForExtend(_moleculeBuildingBlock, _moBiSpatialStructure);
      }

      [Observation]
      public void the_default_selections_should_be_made()
      {
         A.CallTo(() => _selectMoleculesPresenter.SelectMolecules(_moleculeBuildingBlock, null)).MustHaveHappened();
         sut.SelectedSpatialStructure.ShouldBeEqualTo(_moBiSpatialStructure);
      }
   }

   public class When_selecting_building_blocks_and_the_view_is_canceled : concern_for_SelectSpatialStructureAndMoleculesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new[] {new MoleculeBuildingBlock()});
         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new[] {new MoBiSpatialStructure()});
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.SelectBuildingBlocksForExtend(null, null);
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
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new List<MoleculeBuildingBlock> {new MoleculeBuildingBlock()});
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.SelectBuildingBlocksForExtend(null, null);
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

   public class When_selecting_molecules_only_for_refresh : concern_for_SelectSpatialStructureAndMoleculesPresenter
   {
      private MoleculeBuilder _molecule1;
      private MoleculeBuilder _molecule2;

      protected override void Context()
      {
         base.Context();
         _molecule1 = new MoleculeBuilder {Name = "Molecule1"};
         _molecule2 = new MoleculeBuilder {Name = "Molecule2"};
      }

      protected override void Because()
      {
         sut.SelectMoleculesForRefresh(null, new List<string> {_molecule1.Name});
      }

      [Observation]
      public void the_view_should_not_show_the_spatial_structure_selection()
      {
         A.CallTo(() => _view.HideSpatialStructureSelection()).MustHaveHappened();
      }

      [Observation]
      public void the_selectable_molecules_should_not_include_molecules_not_in_the_list()
      {
         A.CallTo(() => _selectMoleculesPresenter.SelectMolecules(null, A<Func<MoleculeBuilder, bool>>.That.Matches(x => includesOnlyMolecule1(x)))).MustHaveHappened();
      }

      private bool includesOnlyMolecule1(Func<MoleculeBuilder, bool> canSelect)
      {
         return canSelect(_molecule1) && !canSelect(_molecule2);
      }
   }
}