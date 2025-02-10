using FakeItEasy;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_EditIndividualBuildingBlockPresenter : ContextSpecification<EditIndividualBuildingBlockPresenter>
   {
      protected IIndividualBuildingBlockPresenter _individualBuildingBlockPresenter;
      protected IEditIndividualBuildingBlockView _view;
      protected IndividualBuildingBlock _individual;
      private IFormulaCachePresenter _formulaCachePresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditIndividualBuildingBlockView>();
         _individualBuildingBlockPresenter = A.Fake<IIndividualBuildingBlockPresenter>();
         _individual = new IndividualBuildingBlock();
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();
         sut = new EditIndividualBuildingBlockPresenter(_view, _individualBuildingBlockPresenter, _formulaCachePresenter);
      }
   }

   public class When_editing_a_individual_building_block : concern_for_EditIndividualBuildingBlockPresenter
   {
      protected override void Because()
      {
         sut.Edit(_individual);
      }

      [Observation]
      public void the_sub_presenter_is_also_editing_the_profile()
      {
         A.CallTo(() => _individualBuildingBlockPresenter.Edit(_individual)).MustHaveHappened();
      }

      [Observation]
      public void the_caption_name_should_be_set()
      {
         _view.Caption.ShouldBeEqualTo(AppConstants.Captions.IndividualBuildingBlockCaption(_individual.Name));
      }
   }
}
