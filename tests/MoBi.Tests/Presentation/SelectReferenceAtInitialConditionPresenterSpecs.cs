using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation
{
   public abstract class concern_for_SelectReferenceAtInitialConditionPresenter : ContextSpecification<SelectReferenceAtInitialConditionPresenter>
   {
      protected ISelectReferenceView _view;
      protected IMoBiContext _context;
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected IContainer _organism;
      protected IContainer _plasma;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         _context = A.Fake<IMoBiContext>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         A.CallTo(() => _context.ObjectPathFactory).Returns(new ObjectPathFactoryForSpecs());

         _organism = new Container().WithName("Organism").WithMode(ContainerMode.Logical);
         _plasma = new Container().WithName("Plasma").WithMode(ContainerMode.Physical);
         _organism.Add(_plasma);

         var molecules = new MoleculeBuildingBlock { new MoleculeBuilder().WithName("Drug"), new MoleculeBuilder().WithName("Metabolite") };
         var otherMolecules = new MoleculeBuildingBlock { new MoleculeBuilder().WithName("Drug") };
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new[] { molecules, otherMolecules });

         sut = new SelectReferenceAtInitialConditionPresenter(_view, new ObjectBaseToObjectBaseDTOMapper(), _context, A.Fake<IUserSettings>(),
            A.Fake<IObjectBaseToDummyMoleculeDTOMapper>(), A.Fake<IParameterToDummyParameterDTOMapper>(), A.Fake<IObjectBaseDTOToReferenceNodeMapper>(),
            A.Fake<IObjectPathCreatorAtInitialCondition>(), _buildingBlockRepository, new MoleculeBuilderToDummyMoleculeDTOMapper());
      }
   }

   public class When_retrieving_the_children_of_a_physical_container_for_initial_conditions : concern_for_SelectReferenceAtInitialConditionPresenter
   {
      private IReadOnlyList<ObjectBaseDTO> _children;
      private IContainer _subContainer;

      protected override void Context()
      {
         base.Context();
         _subContainer = new Container().WithName("Sub").WithMode(ContainerMode.Physical);
         _plasma.Add(_subContainer);
         _plasma.Add(new Container().WithName(Constants.MOLECULE_PROPERTIES).WithMode(ContainerMode.Logical));
         _plasma.Add(new Parameter().WithName("Volume"));
      }

      protected override void Because()
      {
         _children = sut.GetChildObjects(new ObjectBaseDTO(_plasma)).ToList();
      }

      [Observation]
      public void should_return_the_sub_containers_and_each_molecule_once()
      {
         _children.Select(x => x.Name).ShouldOnlyContainInOrder("Sub", "Drug", "Metabolite");
      }

      [Observation]
      public void the_molecules_should_be_located_in_the_physical_container()
      {
         _children.OfType<DummyMoleculeDTO>().Each(x => x.StructureParent.ShouldBeEqualTo(_plasma));
      }
   }

   public class When_retrieving_the_children_of_a_logical_container_for_initial_conditions : concern_for_SelectReferenceAtInitialConditionPresenter
   {
      private IReadOnlyList<ObjectBaseDTO> _children;

      protected override void Because()
      {
         _children = sut.GetChildObjects(new ObjectBaseDTO(_organism)).ToList();
      }

      [Observation]
      public void should_only_return_the_sub_containers()
      {
         _children.Select(x => x.Name).ShouldOnlyContain("Plasma");
      }
   }

   public class When_retrieving_the_selected_initial_condition_paths : concern_for_SelectReferenceAtInitialConditionPresenter
   {
      private IReadOnlyList<ObjectPath> _selections;

      protected override void Context()
      {
         base.Context();
         var moleculeDTOs = sut.GetChildObjects(new ObjectBaseDTO(_plasma)).OfType<DummyMoleculeDTO>().ToList();
         A.CallTo(() => _view.AllSelectedDTOs).Returns(moleculeDTOs);
      }

      protected override void Because()
      {
         _selections = sut.GetAllSelections();
      }

      [Observation]
      public void should_return_the_path_of_the_molecule_in_the_container()
      {
         _selections.Select(x => x.PathAsString).ShouldOnlyContain("Organism|Plasma|Drug", "Organism|Plasma|Metabolite");
      }

      [Observation]
      public void the_selection_should_be_valid()
      {
         sut.CanClose.ShouldBeTrue();
      }
   }

   public class When_a_container_is_selected_for_initial_conditions : concern_for_SelectReferenceAtInitialConditionPresenter
   {
      protected override void Context()
      {
         base.Context();
         var moleculeDTO = sut.GetChildObjects(new ObjectBaseDTO(_plasma)).OfType<DummyMoleculeDTO>().First();
         A.CallTo(() => _view.AllSelectedDTOs).Returns(new[] { moleculeDTO, new ObjectBaseDTO(_plasma) });
      }

      [Observation]
      public void the_selection_should_not_be_valid()
      {
         sut.CanClose.ShouldBeFalse();
      }
   }
}
