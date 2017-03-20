using System.Collections.Generic;
using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditFavoritesInSimulationPresenter : ContextSpecification<EditFavoritesInSimulationPresenter>
   {
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IEntityPathResolver _entityPathResolver;
      protected IFavoriteTask _favoriteTask;
      protected IInteractionTasksForParameter _interactionTasksForParameter;
      protected IFavoriteRepository _favoriteRepository;
      protected IEditFavoritesView _view;
      protected IQuantityTask _quantityTask;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IFormulaToFormulaBuilderDTOMapper _formulaToFormulaBuilderDTOMapper;
      protected IParameterToFavoriteParameterDTOMapper _parameterToFavoriteParameterDTOMapper;

      protected override void Context()
      {
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _favoriteTask = A.Fake<IFavoriteTask>();
         _interactionTasksForParameter = A.Fake<IInteractionTasksForParameter>();
         _favoriteRepository = A.Fake<IFavoriteRepository>();
         _view = A.Fake<IEditFavoritesView>();
         _quantityTask = A.Fake<IQuantityTask>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _formulaToFormulaBuilderDTOMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _parameterToFavoriteParameterDTOMapper = A.Fake<IParameterToFavoriteParameterDTOMapper>();
         sut = new EditFavoritesInSimulationPresenter(_view, _quantityTask, _interactionTaskContext, _formulaToFormulaBuilderDTOMapper, _parameterToFavoriteParameterDTOMapper,
            _favoriteRepository, _interactionTasksForParameter, _favoriteTask, _entityPathResolver, _viewItemContextMenuFactory);
      }
   }

   public class When_handling_a_quantity_changed_event_and_the_quantity_is_a_favorite : concern_for_EditFavoritesInSimulationPresenter
   {
      private IParameter _quantity;
      private IMoBiSimulation _simulation;
      private FavoriteParameterDTO _favoriteParameterDTO;

      protected override void Context()
      {
         base.Context();
         _quantity = new Parameter();
         A.CallTo(() => _favoriteRepository.All()).Returns(new[] {string.Empty});
         _favoriteParameterDTO = new FavoriteParameterDTO(_quantity);
         A.CallTo(() => _parameterToFavoriteParameterDTOMapper.MapFrom(_quantity)).Returns(_favoriteParameterDTO);
         
         _simulation = new MoBiSimulation
         {
            BuildConfiguration = new MoBiBuildConfiguration(),
            Model = new Model
            {
               Root = new Container()
            }
         };
         _simulation.Model.Root.Add(_quantity);
         sut.Edit(_simulation);
      }

      protected override void Because()
      {
         sut.Handle(new AddParameterToFavoritesEvent(""));
      }

      [Observation]
      public void the_view_should_be_rebound()
      {
         A.CallTo(() => _view.Show(A<IEnumerable<FavoriteParameterDTO>>._)).MustHaveHappened();
      }
   }
}
