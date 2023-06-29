using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectBuildingBlocksForExtendPresenter : IDisposablePresenter
   {
      IEnumerable<MoleculeBuildingBlock> GetMolecules();
      IEnumerable<MoBiSpatialStructure> GetSpatialStructures();

      void SelectBuildingBlocksForExtend(bool moleculeRequired = true);
      MoBiSpatialStructure SelectedSpatialStructure { get; }
      MoleculeBuildingBlock SelectedMoleculeBuildingBlock { get; }
   }

   public class SelectBuildingBlocksForExtendPresenter : AbstractDisposablePresenter<ISelectSpatialStructureAndMoleculesView, ISelectBuildingBlocksForExtendPresenter>, ISelectBuildingBlocksForExtendPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private SelectSpatialStructureAndMoleculesDTO _dto;

      public SelectBuildingBlocksForExtendPresenter(ISelectSpatialStructureAndMoleculesView view, IBuildingBlockRepository buildingBlockRepository) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
      }

      public void SelectBuildingBlocksForExtend(bool moleculeRequired = true)
      {
         setViewCaption(moleculeRequired);
         _dto = createDTO(GetMolecules().FirstOrDefault(), GetSpatialStructures().FirstOrDefault(), moleculeRequired);
         _view.Show(_dto);
         _view.Display();
         if (!_view.Canceled)
            return;

         _dto.Molecules = null;
         _dto.SpatialStructure = null;
      }

      private void setViewCaption(bool moleculeRequired)
      {
         _view.Caption = moleculeRequired ? AppConstants.Captions.SelectSpatialStructureAndMolecules : AppConstants.Captions.SelectSpatialStructure;
      }

      private SelectSpatialStructureAndMoleculesDTO createDTO(MoleculeBuildingBlock moleculeBuildingBlock, MoBiSpatialStructure spatialStructure, bool moleculeRequired)
      {
         var dto = new SelectSpatialStructureAndMoleculesDTO(moleculeRequired)
         {
            Molecules = moleculeBuildingBlock,
            SpatialStructure = spatialStructure
         };
         return dto;
      }

      public MoBiSpatialStructure SelectedSpatialStructure => _dto.SpatialStructure;
      public MoleculeBuildingBlock SelectedMoleculeBuildingBlock => _dto.Molecules;

      public IEnumerable<MoleculeBuildingBlock> GetMolecules()
      {
         return _buildingBlockRepository.MoleculeBlockCollection;
      }

      public IEnumerable<MoBiSpatialStructure> GetSpatialStructures()
      {
         return _buildingBlockRepository.SpatialStructureCollection;
      }
   }
}