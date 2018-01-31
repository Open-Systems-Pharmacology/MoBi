using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditFavoritesPresenter : ContextSpecification<IEditFavoritesInSpatialStructurePresenter>
   {
      protected IEditParameterListView _view;
      protected IQuantityTask _quantityTask;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IParameterToParameterDTOMapper _favoriteMapper;
      protected IFavoriteRepository _favoriteRepository;
      protected IInteractionTasksForParameter _parameterTask;
      protected IFavoriteTask _favoriteTask;
      protected IEntityPathResolver _entityPathResolver;
      protected IMoBiContext _context;
      private IViewItemContextMenuFactory _contextMenufactory;

      protected override void Context()
      {
         _view = A.Fake<IEditParameterListView>();
         _quantityTask = A.Fake<IQuantityTask>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _favoriteMapper = A.Fake<IParameterToParameterDTOMapper>();
         _favoriteRepository = A.Fake<IFavoriteRepository>();
         _parameterTask = A.Fake<IInteractionTasksForParameter>();
         _favoriteTask = A.Fake<IFavoriteTask>();
         _entityPathResolver = new EntityPathResolver(new ObjectPathFactory(A.Fake<IAliasCreator>()));
         _contextMenufactory = A.Fake<IViewItemContextMenuFactory>();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _interactionTaskContext.Context).Returns(_context);
         sut = new EditFavoritesInSpatialStructurePresenter(_view, _quantityTask, _interactionTaskContext,
            _formulaMapper, _favoriteMapper, _favoriteRepository, _parameterTask, _favoriteTask, _entityPathResolver,_contextMenufactory);
      }
   }

   class When_go_to_is_called_for_a_favorite : concern_for_EditFavoritesPresenter
   {
      private ParameterDTO _favoriteDTO;
      private IParameter _parameter;

      protected override void Context()
      {
         
         base.Context();
         _parameter = new Parameter().WithName("P");
         _favoriteDTO = new ParameterDTO(_parameter);
      }

      protected override void Because()
      {
         sut.GoTo(_favoriteDTO);
      }

      [Observation]
      public void should_raise_an_entity_selcted_event()
      {
         A.CallTo(() => _context.PublishEvent(A<EntitySelectedEvent>.That
            .Matches(e=>e.ObjectBase.Equals(_parameter))))
            .MustHaveHappened();
         
      }
   }

   internal class When_editing_the_spatial_structure : concern_for_EditFavoritesPresenter
   {
      private IMoBiSpatialStructure _spatialStructure;
      private IParameter _para;
      private IParameter _fav;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new MoBiSpatialStructure();
         var cont = new Container().WithName("root");
         _para = new Parameter().WithName("P").WithParentContainer(cont);
         _fav = new Parameter().WithName("F").WithParentContainer(cont);
         _spatialStructure.AddTopContainer(cont);
         A.CallTo(() => _favoriteRepository.All()).Returns(new[] {_entityPathResolver.PathFor(_fav)});
         A.CallTo(() => _favoriteMapper.MapFrom(_fav)).Returns(new ParameterDTO(_fav));
      }

      protected override void Because()
      {
         sut.Edit(_spatialStructure);
      }

      [Observation]
      public void should_show_the_favorites()
      {
         A.CallTo(() => _favoriteMapper.MapFrom(_fav)).MustHaveHappened();
         A.CallTo(() => _favoriteMapper.MapFrom(_para)).MustNotHaveHappened();
      }
   }
}