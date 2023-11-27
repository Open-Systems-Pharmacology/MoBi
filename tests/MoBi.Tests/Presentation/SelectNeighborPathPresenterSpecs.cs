using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
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
      protected IContainerToContainerDTOMapper _containerDTOMapper;
      protected MoBiSpatialStructure _spatialStructure;
      protected IContainer _organism;
      protected ContainerDTO _organismDTO;
      protected ObjectPathDTO _selectedPathDTO;
      private IBuildingBlockRepository _buildingBlockRepository;

      protected override void Context()
      {
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _view = A.Fake<ISelectNeighborPathView>();
         _selectContainerInTreePresenter = A.Fake<ISelectContainerInTreePresenter>();
         _containerDTOMapper = A.Fake<IContainerToContainerDTOMapper>();

         A.CallTo(() => _view.BindTo(A<ObjectPathDTO>._))
            .Invokes(x => _selectedPathDTO = x.GetArgument<ObjectPathDTO>(0));

         sut = new SelectNeighborPathPresenter(_view, _selectContainerInTreePresenter, _containerDTOMapper, _buildingBlockRepository);

         _spatialStructure = new MoBiSpatialStructure();
         _spatialStructure.Module = new Module().WithName("module");
         _organism = new Container().WithContainerType(ContainerType.Organism);
         _spatialStructure.AddTopContainer(_organism);
         _organismDTO = new ContainerDTO(_organism);

         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new []{_spatialStructure});
         A.CallTo(() => _containerDTOMapper.MapFrom(_organism)).Returns(_organismDTO);
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
         _allObjectBaseForTree.ShouldOnlyContain(_organismDTO);
      }

      [Observation]
      public void should_update_the_label_used_in_the_view()
      {
         _view.Label.ShouldBeEqualTo("label");
      }
   }

   public class When_initializing_the_select_neighborhood_path_presenter_with_a_spatial_structure_without_a_top_container : concern_for_SelectNeighborPathPresenter
   {
      private IReadOnlyList<ObjectBaseDTO> _allObjectBaseForTree;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _selectContainerInTreePresenter.InitTreeStructure(A<IReadOnlyList<ObjectBaseDTO>>._))
            .Invokes(x => _allObjectBaseForTree = x.GetArgument<IReadOnlyList<ObjectBaseDTO>>(0));

         _spatialStructure.RemoveTopContainer(_organism);
      }

      protected override void Because()
      {
         sut.Init("label");
      }

      [Observation]
      public void should_not_initialize_the_tree_structure()
      {
         _allObjectBaseForTree.ShouldBeNull();
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
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(new Parameter(), new ObjectPath()));
         _selectedPathDTO.Path.ShouldBeEqualTo("A|B");
      }

      [Observation]
      public void should_not_update_the_path_if_the_selected_entity_is_not_a_physical_container()
      {
         _selectedPathDTO.Path = "A|B";
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(new Container().WithMode(ContainerMode.Logical), new ObjectPath()));
         _selectedPathDTO.Path.ShouldBeEqualTo("A|B");
      }

      [Observation]
      public void should_update_the_path_if_the_selected_entity_is_a_physical_container()
      {
         _selectedPathDTO.Path = "A|B";
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(new Container().WithMode(ContainerMode.Physical), new ObjectPath("NEW|PATH")));
         _selectedPathDTO.Path.ShouldBeEqualTo("NEW|PATH");
      }

      [Observation]
      public void should_update_the_path_and_add_the_parent_path_of_the_root_container_if_one_is_defined()
      {
         _selectedPathDTO.Path = "A|B";
         var parentContainer = new Container {ParentPath = new ObjectPath("ROOT", "PARENT")};
         var container = new Container().WithMode(ContainerMode.Physical).Under(parentContainer);
         _selectContainerInTreePresenter.OnSelectedEntityChanged += Raise.With(new SelectedEntityChangedArgs(container, new ObjectPath("NEW|PATH")));
         _selectedPathDTO.Path.ShouldBeEqualTo("ROOT|PARENT|NEW|PATH");
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
}