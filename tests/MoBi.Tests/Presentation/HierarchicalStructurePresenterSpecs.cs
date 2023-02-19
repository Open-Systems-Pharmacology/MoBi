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
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;


namespace MoBi.Presentation
{
   public abstract class concern_for_HierarchicalStructurePresenterSpecs :
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
            Icon = ApplicationIcons.Favorites.IconName,
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

   internal class When_selecting_the_favorites_node : concern_for_HierarchicalStructurePresenterSpecs
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