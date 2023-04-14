using FakeItEasy;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.SpaceDiagram;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditSpatialStructurePresenterSpecs :
      ContextSpecification<EditSpatialStructurePresenter>
   {
      protected IEditSpatialStructureView _view;
      protected IHierarchicalSpatialStructurePresenter _hierarchicalPresenter;
      protected IFormulaCachePresenter _formulaCachePresenter;
      protected IEditContainerPresenter _editContainerPresenter;
      protected ISpatialStructureDiagramPresenter _diagramPresenter;
      protected IEditFavoritesInSpatialStructurePresenter _favoritesPresenter;
      protected IUserDefinedParametersPresenter _userDefinedParametersPresenter;
      protected IEditNeighborhoodBuilderPresenter _editNeighborhoodBuilderPresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditSpatialStructureView>();
         _hierarchicalPresenter = A.Fake<IHierarchicalSpatialStructurePresenter>();
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();
         _editContainerPresenter = A.Fake<IEditContainerPresenter>();
         _diagramPresenter = A.Fake<ISpatialStructureDiagramPresenter>();
         _favoritesPresenter = A.Fake<IEditFavoritesInSpatialStructurePresenter>();
         _userDefinedParametersPresenter = A.Fake<IUserDefinedParametersPresenter>();
         _editNeighborhoodBuilderPresenter = A.Fake<IEditNeighborhoodBuilderPresenter>();
         sut = new EditSpatialStructurePresenter(
            _view,
            _formulaCachePresenter,
            _favoritesPresenter,
            _userDefinedParametersPresenter,
            _hierarchicalPresenter,
            _diagramPresenter,
            _editContainerPresenter,
            new HeavyWorkManagerForSpecs(),
            _editNeighborhoodBuilderPresenter);
      }
   }

   internal class When_selecting_the_favorites : concern_for_EditSpatialStructurePresenterSpecs
   {
      private MoBiSpatialStructure _spSt;

      protected override void Context()
      {
         base.Context();
         _spSt = A.Fake<MoBiSpatialStructure>();
         sut.Edit(_spSt);
      }

      protected override void Because()
      {
         sut.Handle(new FavoritesSelectedEvent(_spSt));
      }

      [Observation]
      public void should_set_edit_view_to_favorites()
      {
         A.CallTo(() => _view.SetEditView(_favoritesPresenter.BaseView)).MustHaveHappened();
      }
   }

   internal class When_selecting_the_user_defined_nodes : concern_for_EditSpatialStructurePresenterSpecs
   {
      private MoBiSpatialStructure _spSt;

      protected override void Context()
      {
         base.Context();
         _spSt = A.Fake<MoBiSpatialStructure>();
         sut.Edit(_spSt);
      }

      protected override void Because()
      {
         sut.Handle(new UserDefinedSelectedEvent(_spSt));
      }

      [Observation]
      public void should_display_the_user_defined_parameters_defined_in_the_spatial_structure()
      {
         A.CallTo(() => _userDefinedParametersPresenter.ShowUserDefinedParametersIn(_spSt)).MustHaveHappened();
         A.CallTo(() => _view.SetEditView(_userDefinedParametersPresenter.BaseView)).MustHaveHappened();
      }
   }

   internal class When_selecting_the_favorites_of_a_simulation_but_the_view_is_editing_a_spatial_structure : concern_for_EditSpatialStructurePresenterSpecs
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(A.Fake<MoBiSpatialStructure>());
      }

      protected override void Because()
      {
         sut.Handle(new FavoritesSelectedEvent(A.Fake<IMoBiSimulation>()));
      }

      [Observation]
      public void should_not_select_the_favorite_view()
      {
         //Once because favorite view is the default view
         A.CallTo(() => _view.SetEditView(_favoritesPresenter.BaseView)).MustHaveHappenedOnceExactly();
      }
   }

   internal class When_edit_spatial_structure_presenter_handles_a_select_event_for_a_distributed_parameter : concern_for_EditSpatialStructurePresenterSpecs
   {
      private IDistributedParameter _distributedParameter;
      private IContainer _parent;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _parent = new Container().WithName("bla");
         _distributedParameter = new DistributedParameter().WithName("Dis").WithParentContainer(_parent);
         _spatialStructure = new MoBiSpatialStructure().WithTopContainer(_parent);
         sut.Edit(_spatialStructure);
      }

      protected override void Because()
      {
         sut.Handle(new EntitySelectedEvent(_distributedParameter, A.Fake<object>()));
      }

      [Observation]
      public void should_call_edit_for_parent_container()
      {
         A.CallTo(() => _editContainerPresenter.Edit(_parent)).MustHaveHappened();
      }

      [Observation]
      public void should_select_distributed_parameter()
      {
         A.CallTo(() => _editContainerPresenter.SelectParameter(_distributedParameter)).MustHaveHappened();
      }
   }

   internal class When_edit_spatial_structure_presenter_handles_a_select_event_for_a_neighborhood_builder : concern_for_EditSpatialStructurePresenterSpecs
   {
      private MoBiSpatialStructure _spatialStructure;
      private NeighborhoodBuilder _neighborhoodBuilder;

      protected override void Context()
      {
         base.Context();
         _neighborhoodBuilder = new NeighborhoodBuilder();
         _spatialStructure = new MoBiSpatialStructure();
         _spatialStructure.NeighborhoodsContainer = new Container().WithName("Neighborhoods");
         _spatialStructure.AddNeighborhood(_neighborhoodBuilder);
         sut.Edit(_spatialStructure);
      }

      protected override void Because()
      {
         sut.Handle(new EntitySelectedEvent(_neighborhoodBuilder, A.Fake<object>()));
      }

      [Observation]
      public void should_call_edit_for_neighborhood_builder()
      {
         A.CallTo(() => _editNeighborhoodBuilderPresenter.Edit(_neighborhoodBuilder)).MustHaveHappened();
      }
   }

   public class When_tell_edit_spatial_structure_presenter_to_load_diagram :
      concern_for_EditSpatialStructurePresenterSpecs
   {
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = A.Fake<MoBiSpatialStructure>();
         sut.Edit(_spatialStructure);
      }

      protected override void Because()
      {
         base.Because();
         sut.LoadDiagram();
      }

      [Observation]
      public void should_initialize_diagram_presenter()
      {
         A.CallTo(() => _diagramPresenter.Edit(_spatialStructure)).MustHaveHappened();
      }
   }

   public class When_tell_edit_spatial_structure_presenter_to_edit_spatial_structure :
      concern_for_EditSpatialStructurePresenterSpecs
   {
      private MoBiSpatialStructure _spatialStructure;
      private IContainer _topContainer;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new MoBiSpatialStructure();
         _topContainer = new Container();
         _spatialStructure.AddTopContainer(_topContainer);
      }

      protected override void Because()
      {
         sut.Edit(_spatialStructure);
      }

      [Observation]
      public void should_initialise_hierarchical_presenter_and_formula_cache_presenter()
      {
         A.CallTo(() => _hierarchicalPresenter.Edit(_spatialStructure)).MustHaveHappened();
         A.CallTo(() => _formulaCachePresenter.Edit(_spatialStructure)).MustHaveHappened();
      }

      [Observation]
      public void should_not_initialise_diagram()
      {
         A.CallTo(() => _diagramPresenter.Edit(_spatialStructure)).MustNotHaveHappened();
      }

      [Observation]
      public void should_initialise_and_show_favorites()
      {
         A.CallTo(() => _favoritesPresenter.Edit(_spatialStructure)).MustHaveHappened();
         A.CallTo(() => _view.SetEditView(_favoritesPresenter.BaseView)).MustHaveHappened();
      }
   }
}