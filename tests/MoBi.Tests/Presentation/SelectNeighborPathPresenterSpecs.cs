using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_SelectNeighborPathPresenter : ContextSpecification<ISelectNeighborPathPresenter>
   {
      protected ISelectNeighborPathView _view;
      protected ISelectContainerInTreePresenter _selectContainerInTreePresenter;
      protected MoBiSpatialStructure _spatialStructure1;
      protected MoBiSpatialStructure _spatialStructure2;
      protected IContainer _organism1;
      protected SpatialStructureDTO _organismDTO1;
      protected ModuleAndSpatialStructureDTO _moduleAndSpatialStructureDTO1, _moduleAndSpatialStructureDTO2;
      protected ObjectPathDTO _selectedPathDTO;
      private IBuildingBlockRepository _buildingBlockRepository;
      protected Container _organism2;
      protected SpatialStructureDTO _organismDTO2;
      protected ContainerDTO _containerDTO1;
      protected ContainerDTO _containerDTO2;
      private IModuleToModuleAndSpatialStructureDTOMapper _moduleDTOMapper;
      private IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _moduleDTOMapper = A.Fake<IModuleToModuleAndSpatialStructureDTOMapper>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _view = A.Fake<ISelectNeighborPathView>();
         _selectContainerInTreePresenter = A.Fake<ISelectContainerInTreePresenter>();
         _objectPathFactory = new ObjectPathFactoryForSpecs();

         A.CallTo(() => _view.BindTo(A<ObjectPathDTO>._))
            .Invokes(x => _selectedPathDTO = x.GetArgument<ObjectPathDTO>(0));

         sut = new SelectNeighborPathPresenter(_view, _selectContainerInTreePresenter, _moduleDTOMapper, _buildingBlockRepository, _objectPathFactory);

         _spatialStructure1 = new MoBiSpatialStructure
         {
            Module = new Module().WithName("module1")
         };
         _organism1 = new Container().WithContainerType(ContainerType.Organism);
         _spatialStructure1.AddTopContainer(_organism1);

         _spatialStructure2 = new MoBiSpatialStructure
         {
            Module = new Module().WithName("module2")
         };
         _organism2 = new Container().WithContainerType(ContainerType.Organism);
         _spatialStructure2.AddTopContainer(_organism2);

         _containerDTO1 = new ContainerDTO(_organism1);
         _organismDTO1 = new SpatialStructureDTO(_spatialStructure1)
         {
            TopContainers = new[] { _containerDTO1 }
         };
         _containerDTO2 = new ContainerDTO(_organism2);
         _organismDTO2 = new SpatialStructureDTO(_spatialStructure2)
         {
            TopContainers = new[] { _containerDTO2 }
         };

         _moduleAndSpatialStructureDTO1 = new ModuleAndSpatialStructureDTO(_spatialStructure1.Module)
         {
            SpatialStructure = _organismDTO1
         };
         _moduleAndSpatialStructureDTO2 = new ModuleAndSpatialStructureDTO(_spatialStructure2.Module)
         {
            SpatialStructure = _organismDTO2
         };
         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new[] { _spatialStructure1, _spatialStructure2 });
         A.CallTo(() => _moduleDTOMapper.MapFrom(_spatialStructure1.Module)).Returns(_moduleAndSpatialStructureDTO1);
         A.CallTo(() => _moduleDTOMapper.MapFrom(_spatialStructure2.Module)).Returns(_moduleAndSpatialStructureDTO2);
      }
   }

   public class When_initializing_the_select_neighborhood_path_presenter : concern_for_SelectNeighborPathPresenter
   {
      private IReadOnlyList<ObjectBaseDTO> _allObjectBaseForTree;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _selectContainerInTreePresenter.InitTreeStructure(A<IReadOnlyList<ObjectBaseDTO>>._))
            .Invokes(x => _allObjectBaseForTree = x.GetArgument<IReadOnlyList<ObjectBaseDTO>>(0));
      }

      protected override void Because()
      {
         _selectContainerInTreePresenter.SelectObjectBaseDTO(null);
         sut.Init("label");
      }

      [Observation]
      public void should_bind_the_view_to_the_selected_path_DTO()
      {
         _selectedPathDTO.ShouldNotBeNull();
      }

      [Observation]
      public void should_initialize_the_container_path_selection_with_the_organism_of_the_spatial_structure()
      {
         _allObjectBaseForTree.ShouldOnlyContain(_moduleAndSpatialStructureDTO1, _moduleAndSpatialStructureDTO2);
      }

      [Observation]
      public void should_update_the_label_used_in_the_view()
      {
         _view.Label.ShouldBeEqualTo("label");
      }
   }

   public class When_notify_that_the_selected_entity_was_changed : concern_for_SelectNeighborPathPresenter
   {
      [Observation]
      public void should_not_update_the_path_if_the_selected_entity_is_not_a_container()
      {
         _selectedPathDTO.Path = "A|B";
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(new Parameter()));
         _selectedPathDTO.Path.ShouldBeEqualTo("A|B");
         sut.CanClose.ShouldBeFalse();
      }

      [Observation]
      public void should_not_update_the_path_if_the_selected_entity_is_not_a_physical_container()
      {
         _selectedPathDTO.Path = "A|B";
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(new Container().WithMode(ContainerMode.Logical)));
         _selectedPathDTO.Path.ShouldBeEqualTo("A|B");
      }

      [Observation]
      public void should_update_the_path_if_the_selected_entity_is_a_physical_container()
      {
         _selectedPathDTO.Path = "A|B";
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(new Container().WithName("A").WithMode(ContainerMode.Physical)));
         _selectedPathDTO.Path.ShouldBeEqualTo("A");
         sut.CanClose.ShouldBeTrue();
      }

      [Observation]
      public void should_update_the_path_and_add_the_parent_path_of_the_root_container_if_one_is_defined()
      {
         _selectedPathDTO.Path = "A|B";
         var parentContainer = new Container { ParentPath = new ObjectPath("ROOT", "PARENT") };
         var container = new Container().WithMode(ContainerMode.Physical).Under(parentContainer).WithName("NEW_CONTAINER");
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(container));
         _selectedPathDTO.Path.ShouldBeEqualTo("ROOT|PARENT|NEW_CONTAINER");
      }
   }

   public class When_retrieving_the_neighborhood_path : concern_for_SelectNeighborPathPresenter
   {
      private ObjectPath _result;

      protected override void Context()
      {
         base.Context();
         _selectedPathDTO.Path = "A|B|C";
         A.CallTo(() => _selectContainerInTreePresenter.SelectedEntityPath).Returns(new ObjectPath("D|E|F"));
      }

      protected override void Because()
      {
         _result = sut.NeighborPath;
      }

      [Observation]
      public void should_return_the_path_from_the_selected_path_dto_even_if_the_tree_selection_is_pointing_to_another_object()
      {
         _result.PathAsString.ShouldBeEqualTo("A|B|C");
      }
   }

   public class When_selected_container_path_changes_with_physical : concern_for_SelectNeighborPathPresenter
   {
      protected override void Because()
      {
         var entity = new Container
         {
            Mode = ContainerMode.Physical
         };
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(entity));
      }

      [Observation]
      public void can_close_should_be_true()
      {
         sut.CanClose.ShouldBeTrue();
      }
   }

   public class When_selected_container_path_changes_with_logical : concern_for_SelectNeighborPathPresenter
   {
      protected override void Because()
      {
         var entity = new Container
         {
            Mode = ContainerMode.Logical
         };
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(entity));
      }

      [Observation]
      public void can_close_should_be_false()
      {
         sut.CanClose.ShouldBeFalse();
      }
   }
}