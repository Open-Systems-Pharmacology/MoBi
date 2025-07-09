using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public class concern_for_SelectBuildingBlockTypePresenter : ContextSpecification<SelectBuildingBlockTypePresenter>
   {
      protected ISelectBuildingBlockTypeView _view;
      protected Module _module;

      protected override void Context()
      {
         _module = new Module();
         _view = A.Fake<ISelectBuildingBlockTypeView>();
         sut = new SelectBuildingBlockTypePresenter(_view);
      }
   }

   public class When_selecting_building_block_type_and_the_view_is_not_canceled : concern_for_SelectBuildingBlockTypePresenter
   {
      private BuildingBlockType _result;
      private SelectBuildingBlockTypeDTO _dto;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _view.BindTo(A<SelectBuildingBlockTypeDTO>._))
            .Invokes(x => _dto = x.GetArgument<SelectBuildingBlockTypeDTO>(0));
         A.CallTo(() => _view.Display())
            .Invokes(() => _dto.SelectedBuildingBlockType = BuildingBlockType.Molecules);
      }

      protected override void Because()
      {
         _result = sut.GetBuildingBlockType(_module);
      }

      [Observation]
      public void the_returned_type_should_be_correct()
      {
         _result.ShouldBeEqualTo(BuildingBlockType.Molecules);
      }
   }

   public class When_selecting_building_block_type_and_the_view_is_canceled : concern_for_SelectBuildingBlockTypePresenter
   {
      private BuildingBlockType _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.GetBuildingBlockType(_module);
      }

      [Observation]
      public void the_returned_type_should_be_correct()
      {
         _result.ShouldBeEqualTo(BuildingBlockType.None);
      }
   }
}