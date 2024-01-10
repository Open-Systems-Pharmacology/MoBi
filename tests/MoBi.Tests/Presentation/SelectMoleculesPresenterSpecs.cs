using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_SelectMoleculesPresenter : ContextSpecification<SelectMoleculesPresenter>
   {
      protected ISelectMoleculesDTOMapper _selectMoleculesDTOMapper;
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected ISelectMoleculesView _view;

      protected override void Context()
      {
         _view = A.Fake<ISelectMoleculesView>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _selectMoleculesDTOMapper = new SelectMoleculesDTOMapper();
         sut = new SelectMoleculesPresenter(_view, _buildingBlockRepository, _selectMoleculesDTOMapper);
         
      }
   }

   public class When_initializing_the_selection_with_default_building_blocks : concern_for_SelectMoleculesPresenter
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private MoleculeBuilder _moleculeBuilder;

      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = new MoleculeBuilder();
         _moleculeBuildingBlock = new MoleculeBuildingBlock { _moleculeBuilder };
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new[] { _moleculeBuildingBlock });
      }

      protected override void Because()
      {
         sut.SelectMolecules(_moleculeBuildingBlock);
      }

      [Observation]
      public void the_default_molecules_should_be_selected()
      {
         sut.SelectedMolecules.Select(x => x.MoleculeBuilder).ShouldOnlyContain(_moleculeBuilder);
      }

      [Observation]
      public void should_bind_the_view_to_the_dto()
      {
         A.CallTo(() => _view.BindTo(A<IReadOnlyList<MoleculeSelectionDTO>>.That.Matches(x => contains(x, _moleculeBuilder)))).MustHaveHappened();
      }

      private bool contains(IReadOnlyList<MoleculeSelectionDTO> moleculeSelectionDTOs, MoleculeBuilder moleculeBuilder)
      {
         return moleculeSelectionDTOs.Any(x => x.MoleculeBuilder.Equals(moleculeBuilder));
      }
   }
}
