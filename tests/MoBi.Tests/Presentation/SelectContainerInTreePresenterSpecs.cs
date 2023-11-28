using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   internal class concern_for_SelectContainerInTreePresenter : ContextSpecification<SelectContainerInTreePresenter>
   {
      protected IObjectBaseDTOToSpatialStructureNodeMapper _objectBaseToSpatialStructureNodeMapper;
      protected IContainerToContainerDTOMapper _containerDTOMapper;
      protected ISelectEntityInTreeView _view;
      protected IObjectPathFactory _objectPathFactory;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _view = A.Fake<ISelectEntityInTreeView>();
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _context = A.Fake<IMoBiContext>();
         _containerDTOMapper = A.Fake<IContainerToContainerDTOMapper>();
         _objectBaseToSpatialStructureNodeMapper = A.Fake<IObjectBaseDTOToSpatialStructureNodeMapper>();
         sut = new SelectContainerInTreePresenter(_view, _objectPathFactory, _context, _containerDTOMapper, _objectBaseToSpatialStructureNodeMapper);
      }
   }

   internal class When_presenting_a_spatial_structure : concern_for_SelectContainerInTreePresenter
   {
      private SpatialStructure _spatialStructure;
      private SpatialStructureDTO _spatialStructureDTO;
      private IReadOnlyList<ObjectBaseDTO> _children;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new SpatialStructure();
         var topContainers = new List<ContainerDTO>
         {
            new ContainerDTO(new Container())
         };
         _spatialStructureDTO = new SpatialStructureDTO(_spatialStructure)
         {
            TopContainers = topContainers
         };
      }

      protected override void Because()
      {
         _children = sut.GetChildren(_spatialStructureDTO);
      }

      [Observation]
      public void should_map_the_spatial_structure()
      {
         _spatialStructureDTO.TopContainers.All(x => _children.Contains(x)).ShouldBeTrue();
      }
   }

   internal class When_presenting_a_container : concern_for_SelectContainerInTreePresenter
   {
      private IReadOnlyList<ObjectBaseDTO> _children;
      private Container _container;
      private Container _subContainer;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithId("id");
         A.CallTo(() => _context.Get<IEntity>(_container.Id)).Returns(_container);
         _subContainer = new Container();
         _container.Add(_subContainer);
      }

      protected override void Because()
      {
         _children = sut.GetChildren(new ContainerDTO(_container));
      }

      [Observation]
      public void a_new_container_dto_should_be_created_for_the_sub_container()
      {
         A.CallTo(() => _containerDTOMapper.MapFrom(_subContainer)).MustHaveHappened();
      }

      [Observation]
      public void should_have_a_sub_container_child()
      {
         _children.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_presenting_a_non_entity : concern_for_SelectContainerInTreePresenter
   {
      private ParameterValuesBuildingBlock _buildingBlock;
      private IReadOnlyList<ObjectBaseDTO> _children;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new ParameterValuesBuildingBlock().WithId("id");
         A.CallTo(() => _context.Get<IEntity>(_buildingBlock.Id)).Returns(null);

      }

      protected override void Because()
      {
         _children = sut.GetChildren(new ObjectBaseDTO(_buildingBlock));
      }

      [Observation]
      public void should_return_an_empty_list()
      {
         _children.ShouldBeEmpty();
      }
   }

   internal class When_presenting_a_distributed_parameter : concern_for_SelectContainerInTreePresenter
   {
      private DistributedParameter _distributedParameter;
      private IReadOnlyList<ObjectBaseDTO> _children;

      protected override void Context()
      {
         base.Context();
         _distributedParameter = new DistributedParameter().WithId("distParm");
         A.CallTo(() => _context.Get<IEntity>(_distributedParameter.Id)).Returns(_distributedParameter);

      }

      protected override void Because()
      {
         _children = sut.GetChildren(new DistributedParameterDTO(_distributedParameter));
      }

      [Observation]
      public void should_return_an_empty_list()
      {
         _children.ShouldBeEmpty();
      }
   }
}