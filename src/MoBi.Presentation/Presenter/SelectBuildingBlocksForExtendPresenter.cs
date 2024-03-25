using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectSpatialStructureAndMoleculesPresenter : IDisposablePresenter
   {
      IReadOnlyList<MoBiSpatialStructure> AllSpatialStructures { get; }

      void SelectBuildingBlocksForExtend(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure);
      void SelectBuildingBlocksForRefresh(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure);
      SpatialStructure SelectedSpatialStructure { get; }
      IReadOnlyList<MoleculeBuilder> SelectedMolecules { get; }
   }

   public class SelectSpatialStructureAndMoleculesPresenter : AbstractDisposablePresenter<ISelectSpatialStructureAndMoleculesView, ISelectSpatialStructureAndMoleculesPresenter>, ISelectSpatialStructureAndMoleculesPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private SelectSpatialStructureDTO _dto;
      private readonly ISelectSpatialStructureDTOMapper _mapper;
      private readonly ISelectMoleculesPresenter _selectMoleculesPresenter;

      public SelectSpatialStructureAndMoleculesPresenter(
         ISelectSpatialStructureAndMoleculesView view,
         IBuildingBlockRepository buildingBlockRepository,
         ISelectSpatialStructureDTOMapper mapper,
         ISelectMoleculesPresenter selectMoleculesPresenter) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _mapper = mapper;
         _selectMoleculesPresenter = selectMoleculesPresenter;

         _subPresenterManager.Add(_selectMoleculesPresenter);
         _selectMoleculesPresenter.StatusChanged += moleculeSelectionStatusChanged;
         _view.AddMoleculeSelectionView(_selectMoleculesPresenter.View);
      }

      private void moleculeSelectionStatusChanged(object sender, EventArgs e) => _view.MoleculeSelectionChanged();

      public void SelectBuildingBlocksForRefresh(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure) => selectBuildingBlocks(defaultMolecules, defaultSpatialStructure, AppConstants.Captions.RefreshDescription);

      public void SelectBuildingBlocksForExtend(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure) => selectBuildingBlocks(defaultMolecules, defaultSpatialStructure, AppConstants.Captions.ExtendDescription);

      private void selectBuildingBlocks(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure, string descriptionText)
      {
         _view.SetDescriptionText(descriptionText);
         _view.Caption = AppConstants.Captions.SelectSpatialStructureAndMolecules;
         _dto = _mapper.MapFrom(defaultSpatialStructure ?? AllSpatialStructures.FirstOrDefault());
         _selectMoleculesPresenter.SelectMolecules(defaultMolecules);

         _view.Show(_dto);
         _view.Display();
         if (!_view.Canceled)
            return;

         _dto.SpatialStructure = null;
      }

      public override bool CanClose => _subPresenterManager.CanClose;

      public IReadOnlyList<MoleculeBuilder> SelectedMolecules => _selectMoleculesPresenter.SelectedMolecules.Select(x => x.MoleculeBuilder).ToList();

      public SpatialStructure SelectedSpatialStructure => _dto.SpatialStructure;

      public IReadOnlyList<MoBiSpatialStructure> AllSpatialStructures => _buildingBlockRepository.SpatialStructureCollection;
   }
}