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
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectSpatialStructureAndMoleculesPresenter : IDisposablePresenter
   {
      IReadOnlyList<MoBiSpatialStructure> AllSpatialStructures { get; }

      void SelectBuildingBlocksForExtend(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure);
      SpatialStructure SelectedSpatialStructure { get; }
      IReadOnlyList<MoleculeBuilder> SelectedMolecules { get; }
   }

   public class SelectSpatialStructureAndMoleculesPresenter : AbstractDisposablePresenter<ISelectSpatialStructureAndMoleculesView, ISelectSpatialStructureAndMoleculesPresenter>, ISelectSpatialStructureAndMoleculesPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private SelectSpatialStructureAndMoleculesDTO _dto;
      private readonly ISelectSpatialStructureAndMoleculesDTOMapper _mapper;

      public SelectSpatialStructureAndMoleculesPresenter(ISelectSpatialStructureAndMoleculesView view, IBuildingBlockRepository buildingBlockRepository, ISelectSpatialStructureAndMoleculesDTOMapper mapper) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _mapper = mapper;
      }

      public void SelectBuildingBlocksForExtend(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure)
      {
         setViewCaption();
         _dto = _mapper.MapFrom(_buildingBlockRepository.MoleculeBlockCollection, defaultSpatialStructure ?? AllSpatialStructures.FirstOrDefault());
         selectDefaultMolecules(defaultMolecules);

         _view.Show(_dto);
         _view.Display();
         if (!_view.Canceled)
            return;

         _dto.SpatialStructure = null;
      }

      private void selectDefaultMolecules(MoleculeBuildingBlock defaultMolecules)
      {
         _dto.Molecules.Where(x => Equals(x.BuildingBlock, defaultMolecules)).Each(x =>
         {
            x.Selected = true;
            _dto.SelectionUpdated(x);
         });
      }

      private void setViewCaption()
      {
         _view.Caption = AppConstants.Captions.SelectSpatialStructureAndMolecules;
      }

      public IReadOnlyList<MoleculeBuilder> SelectedMolecules => _dto.SelectedMolecules.Select(x => x.MoleculeBuilder).ToList();

      public SpatialStructure SelectedSpatialStructure => _dto.SpatialStructure;

      public IReadOnlyList<MoBiSpatialStructure> AllSpatialStructures => _buildingBlockRepository.SpatialStructureCollection;
   }
}