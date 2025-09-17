using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectMoleculesPresenter : IPresenter<ISelectMoleculesView>
   {
      void SelectMolecules(MoleculeBuildingBlock defaultMolecules, Func<MoleculeBuilder, bool> canSelect = null);
      IReadOnlyList<MoleculeSelectionDTO> SelectedMolecules { get; }
      void SelectionChanged();
      void UpdateValidationsFor(MoleculeSelectionDTO selectedDTO);
   }

   public class SelectMoleculesPresenter : AbstractPresenter<ISelectMoleculesView, ISelectMoleculesPresenter>, ISelectMoleculesPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly ISelectMoleculesDTOMapper _mapper;
      private SelectMoleculesDTO _dto;

      public IReadOnlyList<MoleculeSelectionDTO> SelectedMolecules => _dto.SelectedMolecules;

      public SelectMoleculesPresenter(ISelectMoleculesView view, IBuildingBlockRepository buildingBlockRepository, ISelectMoleculesDTOMapper mapper) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _mapper = mapper;
      }

      public override bool CanClose => _dto.Molecules.All(x => x.IsValid());

      public void SelectMolecules(MoleculeBuildingBlock defaultMolecules, Func<MoleculeBuilder, bool> canSelect = null)
      {
         _dto = _mapper.MapFrom(_buildingBlockRepository.MoleculeBlockCollection, canSelect);
         selectDefaultMolecules(defaultMolecules);
         _view.BindTo(_dto.Molecules);
      }

      public void SelectionChanged() => OnStatusChanged();
      public void UpdateValidationsFor(MoleculeSelectionDTO selectedDTO) => 
         _dto.Molecules.Where(molecule => string.Equals(molecule.MoleculeName, selectedDTO.MoleculeName)).Each(_view.UpdateValidation);

      private void selectDefaultMolecules(MoleculeBuildingBlock defaultMolecules)
      {
         _dto.Molecules.Where(x => Equals(x.BuildingBlock, defaultMolecules)).Each(x =>
         {
            x.Selected = true;
         });
         _dto.SelectionUpdated();
      }
   }
}