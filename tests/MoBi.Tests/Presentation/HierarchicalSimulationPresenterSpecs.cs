using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Assets;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;
using OSPSuite.SimModel;

namespace MoBi.Presentation
{
   internal abstract class concern_for_HierarchicalSimulationPresenter : ContextSpecification<HierarchicalSimulationPresenter>
   {
      private IHierarchicalStructureView _view;
      protected IMoBiContext _context;
      private IObjectBaseToObjectBaseDTOMapper _dtoMapper;
      private ISimulationSettingsToObjectBaseDTOMapper _simulationSettingsMapper;
      protected ObjectBaseDTO _favorites;
      private ITreeNodeFactory _treeNodeFactory;
      private IViewItemContextMenuFactory _contextMenuFactory;
      protected INeighborhoodToNeighborDTOMapper _neighborhoodDTOMapper;
      private IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _view = A.Fake<IHierarchicalStructureView>();
         _context = A.Fake<IMoBiContext>();
         _simulationSettingsMapper = A.Fake<ISimulationSettingsToObjectBaseDTOMapper>();
         _dtoMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         _neighborhoodDTOMapper = A.Fake<INeighborhoodToNeighborDTOMapper>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();

         _favorites = new ObjectBaseDTO
         {
            Name = Captions.Favorites,
            Icon = ApplicationIcons.Favorites,
            Id = Captions.Favorites
         };
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();

         A.CallTo(() => _treeNodeFactory.CreateForFavorites())
            .Returns(new ObjectWithIdAndNameNode<ObjectBaseDTO>(_favorites));

         _contextMenuFactory = A.Fake<IViewItemContextMenuFactory>();

         sut = new HierarchicalSimulationPresenter(_view, _context, _dtoMapper, _simulationSettingsMapper,
            _treeNodeFactory, _contextMenuFactory, _neighborhoodDTOMapper, _entityPathResolver);

         sut.ShowOutputSchema = () => { };
         sut.ShowSolverSettings = () => { };
      }
   }

   internal class When_getting_child_objects_from_simulation_presenter : concern_for_HierarchicalSimulationPresenter
   {
      private IReadOnlyList<ObjectBaseDTO> _result;
      private ObjectBaseDTO _dto;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         var neighbor1 = new Container().WithName("path");
         var neighbor2 = new Container().WithName("path");
         _simulation = new MoBiSimulation()
         {
            Model = new Model
            {
               Root = new Container()
            }
         }.WithName("SimulationName");

         var neighborhood = new Neighborhood
         {
            FirstNeighbor = neighbor1,
            SecondNeighbor = neighbor2,
         }.WithId("neighborhood");

         _dto = new ObjectBaseDTO(neighborhood);

         A.CallTo(() => _neighborhoodDTOMapper.MapFrom(neighborhood)).Returns(new List<NeighborDTO>
         {
            new NeighborDTO(new ObjectPath("SimulationName", "neighborhood1", "path")),
            new NeighborDTO(new ObjectPath("SimulationName", "neighborhood2", "path"))
         });

         sut.Edit(_simulation);
      }

      protected override void Because()
      {
         _result = sut.GetChildObjects(_dto, child => !child.IsAnImplementationOf<IParameter>());
      }

      [Observation]
      public void neighbors_must_have_names_without_simulation_name()
      {
         _result.Each(x =>
         {
            x.Name.Contains("SimulationName").ShouldBeFalse();
         });
      }
   }

   internal class When_selecting_the_favorites_node : concern_for_HierarchicalSimulationPresenter
   {
      protected override void Because()
      {
         sut.Select(_favorites);
      }


      [Observation]
      public void should_throw_Favorite_selected_event()
      {
         A.CallTo(() => _context.PublishEvent(A<FavoritesSelectedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_not_throw_entity_selected_event()
      {
         A.CallTo(() => _context.PublishEvent(A<EntitySelectedEvent>._)).MustNotHaveHappened();
      }
   }



   internal class When_selecting_a_node_with_an_object_base : concern_for_HierarchicalSimulationPresenter
   {
      private ObjectBaseDTO _dto;

      protected override void Context()
      {
         base.Context();
         _dto = new ObjectBaseDTO(new Container());
      }

      protected override void Because()
      {
         sut.Select(_dto);
      }

      [Observation]
      public void should_raise_entity_selected_event()
      {
         A.CallTo(() => _context.PublishEvent(A<EntitySelectedEvent>.That.Matches(x => x.ObjectBase.Equals(_dto.ObjectBase)))).MustHaveHappened();
      }
   }

   internal class When_selecting_a_neighbor_node : concern_for_HierarchicalSimulationPresenter
   {
      private ObjectBaseDTO _dto;
      private MoBiSimulation _simulation;
      private Container _resolvedContainer;

      protected override void Context()
      {
         base.Context();
         _dto = new NeighborDTO(new ObjectPath("container"));

         _resolvedContainer = new Container().WithName("container");
         _simulation = new MoBiSimulation
         {
            Model = new Model
            {
               Root = new Container().WithChild(_resolvedContainer)
               
            }
         }.WithName("SimulationName");

         sut.Edit(_simulation);
         sut.ShowOutputSchema = () => { };
         sut.ShowSolverSettings = () => { };
      }

      protected override void Because()
      {
         sut.Select(_dto);
      }

      [Observation]
      public void should_raise_entity_selected_event()
      {
         A.CallTo(() => _context.PublishEvent(A<EntitySelectedEvent>.That.Matches(x => x.ObjectBase == _resolvedContainer))).MustHaveHappened();
      }
   }
}