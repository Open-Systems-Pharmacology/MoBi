using FakeItEasy;
using MoBi.Core.Repositories;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_ContainerToContainerDTOMapper : ContextSpecification<IContainerToContainerDTOMapper>
   {
      private IObjectPathFactory _objectPathFactory;
      private IIconRepository _iconRepository;
      protected IContainer _container;
      protected ContainerDTO _containerDTO;

      protected override void Context()
      {
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _iconRepository = A.Fake<IIconRepository>();
         sut = new ContainerToContainerDTOMapper(_objectPathFactory, _iconRepository);
         _container = new Container().WithName("CONT");
      }

      protected override void Because()
      {
         _containerDTO = sut.MapFrom(_container);
      }
   }

   public class When_mapping_a_container_without_a_parent_and_parent_path_to_a_container_dto : concern_for_ContainerToContainerDTOMapper
   {
      [Observation]
      public void should_mark_the_parent_path_as_editable()
      {
         _containerDTO.ParentPathEditable.ShouldBeTrue();
      }

      [Observation]
      public void should_set_the_parent_to_empty()
      {
         _containerDTO.ParentPath.ShouldBeNullOrEmpty();
      }
   }

   public class When_mapping_a_container_without_a_parent_with_a_parent_path_to_a_container_dto : concern_for_ContainerToContainerDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _container.ParentPath = new ObjectPath("A", "B", "C");
      }

      [Observation]
      public void should_mark_the_parent_path_as_editable()
      {
         _containerDTO.ParentPathEditable.ShouldBeTrue();
      }

      [Observation]
      public void should_set_the_parent_to_the_string_representation_of_the_container_parent_path()
      {
         _containerDTO.ParentPath.ShouldBeEqualTo("A|B|C");
      }
   }

   public class When_mapping_a_container_with_a_parent_to_a_container_dto : concern_for_ContainerToContainerDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         var parent = new Container().WithName("P");
         var grandParent = new Container().WithName("GP");
         grandParent.Add(parent);
         _container.ParentPath = new ObjectPath("A", "B", "C");
         parent.Add(_container);
      }

      [Observation]
      public void should_mark_the_parent_path_as_non_editable()
      {
         _containerDTO.ParentPathEditable.ShouldBeFalse();
      }

      [Observation]
      public void should_set_the_parent_to_the_string_representation_of_the_parent_path()
      {
         _containerDTO.ParentPath.ShouldBeEqualTo("GP|P");
      }
   }

}