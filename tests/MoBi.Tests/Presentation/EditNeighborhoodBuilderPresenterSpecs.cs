using FakeItEasy;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditNeighborhoodBuilderPresenter : ContextSpecification<IEditNeighborhoodBuilderPresenter>
   {
      protected IEditNeighborhoodBuilderView _view;
      protected IEditTaskFor<INeighborhoodBuilder> _editTask;
      protected ISelectNeighborPathPresenter _firstNeighborPresenter;
      protected ISelectNeighborPathPresenter _secondNeighborPresenter;
      protected INeighborhoodBuilder _neighborhoodBuilder;
      protected Container _neighborhoodsContainer;
      protected SpatialStructure _spatialStructure;

      protected override void Context()
      {
         _view = A.Fake<IEditNeighborhoodBuilderView>();
         _editTask = A.Fake<IEditTaskFor<INeighborhoodBuilder>>();
         _firstNeighborPresenter = A.Fake<ISelectNeighborPathPresenter>();
         _secondNeighborPresenter = A.Fake<ISelectNeighborPathPresenter>();
         sut = new EditNeighborhoodBuilderPresenter(_view, _editTask, _firstNeighborPresenter, _secondNeighborPresenter);

         _neighborhoodBuilder = new NeighborhoodBuilder();
         _neighborhoodsContainer = new Container();

         _spatialStructure = new SpatialStructure();
         sut.BuildingBlock = _spatialStructure;
      }
   }

   public class When_editing_a_neighborhood_builder_in_the_edit_neighborhood_builder_presenter : concern_for_EditNeighborhoodBuilderPresenter
   {
      protected override void Because()
      {
         sut.Edit(_neighborhoodBuilder, _neighborhoodsContainer);
      }

      [Observation]
      public void should_initialize_the_first_and_second_neighbor_presenter_with_the_spatial_structure()
      {
         A.CallTo(() => _firstNeighborPresenter.Init(_spatialStructure, AppConstants.Captions.FirstNeighbor)).MustHaveHappened();
         A.CallTo(() => _secondNeighborPresenter.Init(_spatialStructure, AppConstants.Captions.SecondNeighbor)).MustHaveHappened();
      }

      [Observation]
      public void should_bind_the_view_to_a_dto()
      {
         A.CallTo(() => _view.BindTo(A<ObjectBaseDTO>._)).MustHaveHappened();
      }
   }

   public class When_notified_that_the_name_of_the_neighborhood_builder_was_updated : concern_for_EditNeighborhoodBuilderPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_neighborhoodBuilder, _neighborhoodsContainer);
      }

      protected override void Because()
      {
         sut.UpdateName("toto");
      }

      [Observation]
      public void should_have_updated_the_name_of_the_edited_neighborhood_builder()
      {
         _neighborhoodBuilder.Name.ShouldBeEqualTo("toto");
      }
   }
}