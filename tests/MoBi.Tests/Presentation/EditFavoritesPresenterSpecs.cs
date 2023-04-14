using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditFavoritesPresenter : ContextSpecification<IEditFavoritesInSpatialStructurePresenter>
   {
      protected IEditFavoritesView _view;
      protected IFavoriteRepository _favoriteRepository;
      protected IEntityPathResolver _entityPathResolver;
      protected IMoBiContext _context;
      protected IEditParameterListPresenter _editParameterListPresenter;
      protected IFavoriteTask _favoriteTask;
      protected MoBiSpatialStructure _spatialStructure;
      protected IParameter _para;
      protected IParameter _fav;

      protected override void Context()
      {
         _view = A.Fake<IEditFavoritesView>();
         _editParameterListPresenter = A.Fake<IEditParameterListPresenter>();
         _favoriteRepository = A.Fake<IFavoriteRepository>();
         _entityPathResolver = new EntityPathResolver(new ObjectPathFactory(A.Fake<IAliasCreator>()));
         _favoriteTask = A.Fake<IFavoriteTask>();

         sut = new EditFavoritesInSpatialStructurePresenter(_view, _favoriteRepository, _entityPathResolver, _editParameterListPresenter, _favoriteTask);
         _spatialStructure = new MoBiSpatialStructure();
         var cont = new Container().WithName("root");
         _para = new Parameter().WithName("P").WithParentContainer(cont);
         _fav = new Parameter().WithName("F").WithParentContainer(cont);
         _spatialStructure.AddTopContainer(cont);
         A.CallTo(() => _favoriteRepository.All()).Returns(new[] {_entityPathResolver.PathFor(_fav)});
      }
   }

   internal class When_editing_the_spatial_structure : concern_for_EditFavoritesPresenter
   {
      protected override void Because()
      {
         sut.Edit(_spatialStructure);
      }

      [Observation]
      public void should_show_the_favorites()
      {
         A.CallTo(() => _editParameterListPresenter.Edit(A<IEnumerable<IParameter>>._)).MustHaveHappened();
      }
   }

   public class When_the_favorite_parameters_is_moving_selected_parameters_up : concern_for_EditFavoritesPresenter
   {
      private IParameter _parameter1;
      private IParameter _parameter2;
      private IEnumerable<string> _movedParameters;
      private IParameter[] _selectedParameters;

      protected override void Context()
      {
         base.Context();
         _parameter1 = new Parameter().WithName("PATH1");
         _parameter2 = new Parameter().WithName("PATH2");

         _selectedParameters = new[] {_parameter1, _parameter2};
         A.CallTo(() => _editParameterListPresenter.SelectedParameters).Returns(_selectedParameters);

         A.CallTo(() => _favoriteTask.MoveUp(A<IEnumerable<string>>._))
            .Invokes(x => _movedParameters = x.GetArgument<IEnumerable<string>>(0));
      }

      protected override void Because()
      {
         sut.MoveUp();
      }

      [Observation]
      public void should_retrieve_the_selected_parameters_from_the_view_and_move_them_up()
      {
         _movedParameters.ShouldOnlyContainInOrder("PATH1", "PATH2");
      }

      [Observation]
      public void should_update_the_parameter_selection()
      {
         _editParameterListPresenter.SelectedParameters.ShouldBeEqualTo(_selectedParameters);
      }
   }

   public class When_the_favorite_parameters_presenter_is_notified_that_the_order_of_favorites_was_updated : concern_for_EditFavoritesPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_spatialStructure);
      }

      protected override void Because()
      {
         sut.Handle(new FavoritesOrderChangedEvent());
      }

      [Observation]
      public void should_refresh_the_edited_parameters()
      {
         A.CallTo(() => _editParameterListPresenter.Edit(A<IEnumerable<IParameter>>._)).MustHaveHappenedTwiceExactly() ;
      }
   }
}