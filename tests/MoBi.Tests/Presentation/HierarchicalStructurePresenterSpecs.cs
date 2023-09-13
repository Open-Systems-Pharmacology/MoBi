using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Assets;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_HierarchicalStructurePresenter :
      ContextSpecification<IHierarchicalStructurePresenter>
   {
      private IHierarchicalStructureView _view;
      protected IMoBiContext _context;
      private IObjectBaseToObjectBaseDTOMapper _dtoMapper;
      private ISimulationSettingsToObjectBaseDTOMapper _simulationSettingsMapper;
      protected ObjectBaseDTO _favorites;
      private ITreeNodeFactory _treeNodeFactory;
      private IViewItemContextMenuFactory _contextMenuFactory;

      protected override void Context()
      {
         _view = A.Fake<IHierarchicalStructureView>();
         _context = A.Fake<IMoBiContext>();
         _simulationSettingsMapper = A.Fake<ISimulationSettingsToObjectBaseDTOMapper>();
         _dtoMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();

         _favorites = new ObjectBaseDTO()
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
            _treeNodeFactory, _contextMenuFactory);
      }
   }

   internal class When_getting_child_objects : concern_for_HierarchicalStructurePresenter
   {
      private IReadOnlyList<ObjectBaseDTO> _result;
      private ObjectBaseDTO _dto;

      protected override void Context()
      {
         base.Context();
         var neighborhoodBuilder = new NeighborhoodBuilder
         {
            FirstNeighborPath = new ObjectPath("neighbor1", "path"),
            SecondNeighborPath = new ObjectPath("neighbor2", "path"),
         }.WithId("neighborhood");

         _dto = new ObjectBaseDTO(neighborhoodBuilder);
      }

      protected override void Because()
      {
         _result = sut.GetChildObjects(_dto, child => !child.IsAnImplementationOf<IParameter>());
      }

      [Observation]
      public void neighbors_must_have_unique_id()
      {
         _result[1].Id.ShouldBeEqualTo("neighborhood-neighbor2|path");
         _result[0].Id.ShouldBeEqualTo("neighborhood-neighbor1|path");
      }
   }

   internal class When_selecting_the_favorites_node : concern_for_HierarchicalStructurePresenter
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
}