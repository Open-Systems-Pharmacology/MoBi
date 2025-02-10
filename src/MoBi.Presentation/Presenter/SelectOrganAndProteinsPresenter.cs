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
      IReadOnlyList<IContainer> SelectedContainers { get; }
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

         _selectContainerInTreePresenter.AllowMultiSelect = true;
      }

      public IReadOnlyList<MoleculeBuilder> SelectedMolecules { get; private set; }
      public IReadOnlyList<IContainer> SelectedContainers { get; private set; }

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
            SelectedContainers = null;
            return;
         }

         SelectedMolecules = _selectMoleculesPresenter.SelectedMolecules.Select(x => x.MoleculeBuilder).ToList();
         SelectedContainers = _selectContainerInTreePresenter.SelectedContainers;
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

      public override bool CanClose => selectionsAreAllowed() && _subPresenterManager.CanClose;

      private bool selectionsAreAllowed()
      {
         return selectedContainersAreOrgans() && moleculeIsSelected();
      }

      private bool moleculeIsSelected()
      {
         return _selectMoleculesPresenter.SelectedMolecules.Count != 0;
      }

      private bool selectedContainersAreOrgans()
      {
         var selectedEntities = _selectContainerInTreePresenter.SelectedContainers;

         return (onlyOrgansSelected(selectedEntities) || onlyOrganismSelected(selectedEntities)) && selectedEntities.Count != 0;
      }

      private bool onlyOrganismSelected(IReadOnlyList<IContainer> selectedEntities)
      {
         return selectedEntities.All(x => x.ContainerType.Equals(ContainerType.Organism));
      }

      private bool onlyOrgansSelected(IReadOnlyList<IContainer> selectedEntities)
      {
         return selectedEntities.All(x => x.ContainerType.Equals(ContainerType.Organ));
      }

      private IReadOnlyList<ObjectBaseDTO> treeNodes() =>
         _buildingBlockRepository.SpatialStructureCollection.Select(x => _moduleAndSpatialStructureDTOMapper.MapFrom(x.Module)).ToList();
   }
}