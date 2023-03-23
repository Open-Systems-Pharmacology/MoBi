using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter.Main;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_ContextMenuFactoryForProjectItem : ContextSpecification<IContextMenuSpecificationFactory<IViewItem>>
   {
      protected IBuildingBlockExplorerPresenter _presenter;

      protected override void Context()
      {
         _presenter = A.Fake<IBuildingBlockExplorerPresenter>();
         sut = new ContextMenuFactoryForBuildingBlock<MoleculeBuildingBlock>();
      }
   }

   public class When_asked_if_satisfied_by_a_correct_view_item : concern_for_ContextMenuFactoryForProjectItem
   {
      private bool _result;

      protected override void Because()
      {
         _result = sut.IsSatisfiedBy(new BuildingBlockViewItem(A.Fake<MoleculeBuildingBlock>()), _presenter);
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_asked_if_satisfied_by_a_view_item_with_a_type_that_does_not_match : concern_for_ContextMenuFactoryForProjectItem
   {
      private bool _result;

      protected override void Because()
      {
         _result = sut.IsSatisfiedBy(new BuildingBlockViewItem(A.Fake<IBuildingBlock>()), _presenter);
      }

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }
   }

  
}