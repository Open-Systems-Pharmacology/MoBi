using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectSpatialStructureAndMoleculesPresenter : IDisposablePresenter
   {
      IReadOnlyList<MoBiSpatialStructure> AllSpatialStructures { get; }

      void SelectBuildingBlocksForExtend(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure);
      void SelectMoleculesForRefresh(MoleculeBuildingBlock defaultMolecules, IReadOnlyList<string> selectableMoleculeNames);
      SpatialStructure SelectedSpatialStructure { get; }
      IReadOnlyList<MoleculeBuilder> SelectedMolecules { get; }
   }

   public class SelectSpatialStructureAndMoleculesPresenter : AbstractDisposablePresenter<ISelectSpatialStructureAndMoleculesView, ISelectSpatialStructureAndMoleculesPresenter>, ISelectSpatialStructureAndMoleculesPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private SelectSpatialStructureDTO _spatialStructureSelectionDTO;
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

      public void SelectMoleculesForRefresh(MoleculeBuildingBlock defaultMolecules, IReadOnlyList<string> selectableMoleculeNames) => selectMolecules(defaultMolecules, selectableMoleculeNames, AppConstants.Captions.RefreshDescription);

      private void selectMolecules(MoleculeBuildingBlock defaultMolecules, IReadOnlyList<string> selectableMoleculeNames, string refreshDescription)
      {
         _view.SetDescriptionText(refreshDescription);
         _view.Caption = AppConstants.Captions.SelectMolecules;

         _selectMoleculesPresenter.SelectMolecules(defaultMolecules, x => selectableMoleculeNames.Contains(x.Name));

         _view.HideSpatialStructureSelection();

         _view.Display();
      }

      public void SelectBuildingBlocksForExtend(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure) => selectBuildingBlocks(defaultMolecules, defaultSpatialStructure, AppConstants.Captions.ExtendDescription);

      private void selectBuildingBlocks(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure, string descriptionText)
      {
         _view.SetDescriptionText(descriptionText);
         _view.Caption = AppConstants.Captions.SelectSpatialStructureAndMolecules;
         _spatialStructureSelectionDTO = _mapper.MapFrom(defaultSpatialStructure ?? AllSpatialStructures.FirstOrDefault());
         _selectMoleculesPresenter.SelectMolecules(defaultMolecules);

         _view.ShowSpatialStructureSelection(_spatialStructureSelectionDTO);
         _view.Display();
         if (!_view.Canceled)
            return;

         _spatialStructureSelectionDTO.SpatialStructure = null;
      }

      public override bool CanClose => _subPresenterManager.CanClose;

      public IReadOnlyList<MoleculeBuilder> SelectedMolecules => _selectMoleculesPresenter.SelectedMolecules.Select(x => x.MoleculeBuilder).ToList();

      public SpatialStructure SelectedSpatialStructure => _spatialStructureSelectionDTO?.SpatialStructure;

      public IReadOnlyList<MoBiSpatialStructure> AllSpatialStructures => _buildingBlockRepository.SpatialStructureCollection;
   }
}