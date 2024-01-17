using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectOrganAndProteinsPresenter : IDisposablePresenter
   {
      IReadOnlyList<MoleculeBuilder> SelectedMolecules { get; }
      IContainer SelectedOrgan { get; }
      void SelectSelectOrganAndProteins(Module defaultModule);
   }

   public class SelectOrganAndProteinsPresenter : AbstractDisposablePresenter<ISelectOrganAndProteinsView, ISelectOrganAndProteinsPresenter>, ISelectOrganAndProteinsPresenter
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly ISelectMoleculesPresenter _selectMoleculesPresenter;
      private readonly ISelectContainerInTreePresenter _selectContainerInTreePresenter;
      private readonly IModuleToModuleAndSpatialStructureDTOMapper _moduleAndSpatialStructureDTOMapper;

      public SelectOrganAndProteinsPresenter(
         ISelectOrganAndProteinsView view,
         IBuildingBlockRepository buildingBlockRepository,
         ISelectMoleculesPresenter selectMoleculesPresenter,
         ISelectContainerInTreePresenter selectContainerInTreePresenter,
         IModuleToModuleAndSpatialStructureDTOMapper moduleAndSpatialStructureDTOMapper) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _selectMoleculesPresenter = selectMoleculesPresenter;
         _selectContainerInTreePresenter = selectContainerInTreePresenter;
         _moduleAndSpatialStructureDTOMapper = moduleAndSpatialStructureDTOMapper;
         _selectMoleculesPresenter.StatusChanged += selectionChanged;
         _selectContainerInTreePresenter.OnSelectedEntityChanged += selectionChanged;
         AddSubPresenters(_selectMoleculesPresenter, _selectContainerInTreePresenter);
         _view.AddMoleculeSelectionView(_selectMoleculesPresenter.View);
         _view.AddOrganSelectionView(_selectContainerInTreePresenter.View);
      }

      public IReadOnlyList<MoleculeBuilder> SelectedMolecules { get; private set; }
      public IContainer SelectedOrgan { get; private set; }

      private void selectionChanged(object sender, EventArgs e)
      {
         _view.SelectionChanged();
      }

      public void SelectSelectOrganAndProteins(Module defaultModule)
      {
         _view.Caption = AppConstants.Captions.SelectOrganAndMolecules;

         _selectMoleculesPresenter.SelectMolecules(defaultModule.Molecules, x => x.QuantityType.Is(QuantityType.Protein));
         _selectContainerInTreePresenter.InitTreeStructure(treeNodes());

         _selectContainerInTreePresenter.SelectById(defaultSelectionInSpatialStructure(defaultModule));
         _view.Display();

         if (_view.Canceled)
         {
            SelectedMolecules = new List<MoleculeBuilder>();
            SelectedOrgan = null;
            return;
         }

         SelectedMolecules = _selectMoleculesPresenter.SelectedMolecules.Select(x => x.MoleculeBuilder).ToList();
         SelectedOrgan = _selectContainerInTreePresenter.SelectedEntity as IContainer;
      }

      private static string defaultSelectionInSpatialStructure(Module defaultModule)
      {
         var organs = organsFrom(defaultModule);
         if (organs.Count == 1)
            return organs.First().Id;

         return defaultModule.Id;
      }

      private static IReadOnlyList<IContainer> organsFrom(Module defaultModule)
      {
         if (defaultModule.SpatialStructure == null)
            return new List<IContainer>();

         return defaultModule.SpatialStructure.Where(x => x.ContainerType.Equals(ContainerType.Organ)).ToList();
      }

      public override bool CanClose => selectedContainerIsOrgan() && _subPresenterManager.CanClose;

      private bool selectedContainerIsOrgan() =>
         _selectContainerInTreePresenter.SelectedEntity is IContainer container && container.ContainerType.Equals(ContainerType.Organ);

      private IReadOnlyList<ObjectBaseDTO> treeNodes() =>
         _buildingBlockRepository.SpatialStructureCollection.Select(x => _moduleAndSpatialStructureDTOMapper.MapFrom(x.Module)).ToList();
   }
}