using Antlr.Runtime.Misc;
using FakeItEasy;
using FluentNHibernate.Utils;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation
{
   public class concern_for_SelectOrganAndProteinsPresenter : ContextSpecification<SelectOrganAndProteinsPresenter>
   {
      protected ISelectOrganAndProteinsView _view;
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected ISelectMoleculesPresenter _selectMoleculesPresenter;
      protected ISelectContainerInTreePresenter _selectContainerInTreePresenter;
      protected IModuleToModuleAndSpatialStructureDTOMapper _moduleToModuleAndSpatialStructureDTOMapper;
      protected Module _module;
      protected MoleculeBuildingBlock _moleculeBuildingBlock;
      protected MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         _view = A.Fake<ISelectOrganAndProteinsView>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _selectMoleculesPresenter = A.Fake<ISelectMoleculesPresenter>();
         _selectContainerInTreePresenter = A.Fake<ISelectContainerInTreePresenter>();
         _moduleToModuleAndSpatialStructureDTOMapper = A.Fake<IModuleToModuleAndSpatialStructureDTOMapper>();
         _module = new Module().WithId("module");
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         _module.Add(_moleculeBuildingBlock);
         _spatialStructure = new MoBiSpatialStructure();
         _module.Add(_spatialStructure);
         IContainer organContainer = new Container();
         organContainer.WithId("organ");
         organContainer.Mode = ContainerMode.Physical;
         organContainer.ContainerType = ContainerType.Organ;
         _spatialStructure.Add(organContainer);

         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new[] { _moleculeBuildingBlock });
         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new[] { _spatialStructure });
         sut = new SelectOrganAndProteinsPresenter(_view, _buildingBlockRepository, _selectMoleculesPresenter, _selectContainerInTreePresenter, _moduleToModuleAndSpatialStructureDTOMapper);
      }
   }

   public class When_canceling_the_modal_dialog : concern_for_SelectOrganAndProteinsPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.SelectSelectOrganAndProteins(_module);
      }

      [Observation]
      public void the_selections_should_be_cleared()
      {
         sut.SelectedOrgan.ShouldBeNull();
         sut.SelectedMolecules.ShouldBeEmpty();
      }
   }

   public class When_selecting_molecules_and_organs_from_a_project : concern_for_SelectOrganAndProteinsPresenter
   {
      private QuantityType[] _proteinTypes;

      protected override void Because()
      {
         sut.SelectSelectOrganAndProteins(_module);
      }

      [Observation]
      public void the_sub_presenter_views_are_added_to_the_main_view()
      {
         A.CallTo(() => _view.AddMoleculeSelectionView(_selectMoleculesPresenter.View)).MustHaveHappened();
         A.CallTo(() => _view.AddOrganSelectionView(_selectContainerInTreePresenter.View)).MustHaveHappened();
      }

      [Observation]
      public void only_proteins_are_added_for_selection()
      {
         A.CallTo(() => _selectMoleculesPresenter.SelectMolecules(_module.Molecules, A<Func<MoleculeBuilder, bool>>.That.Matches(x => acceptsProteins(x)))).MustHaveHappened();
      }

      [Observation]
      public void spatial_structures_are_added_from_the_building_block_repository()
      {
         A.CallTo(() => _moduleToModuleAndSpatialStructureDTOMapper.MapFrom(_module)).MustHaveHappened();
      }

      [Observation]
      public void the_default_organ_is_selected_in_the_container_view()
      {
         A.CallTo(() => _selectContainerInTreePresenter.SelectById("organ")).MustHaveHappened();
      }

      private bool acceptsProteins(Func<MoleculeBuilder, bool> func)
      {
         var result = false;

         // rejects non-protein quantity types
         _proteinTypes = new[] { QuantityType.Molecule, QuantityType.Protein, QuantityType.Enzyme, QuantityType.OtherProtein, QuantityType.Transporter };
         EnumHelper.AllValuesFor<QuantityType>().Except(_proteinTypes).Each(quantityType => result |= func(new MoleculeBuilder { QuantityType = quantityType }));

         // accepts protein quantity types
         _proteinTypes.Each(x => result |= !func(new MoleculeBuilder { QuantityType = x }));

         return !result;
      }
   }
}
