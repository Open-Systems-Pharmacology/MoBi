using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;

namespace MoBi.Presentation
{
   public abstract class concern_for_LegendPresenter : ContextSpecification<LegendPresenter>
   {
      protected ILegendView _legendView;

      protected override void Context()
      {
         _legendView = A.Fake<ILegendView>();
         sut = new LegendPresenter(_legendView);
      }
   }

   public class When_adding_legend_entries_to_the_legend : concern_for_LegendPresenter
   {
      private IReadOnlyList<LegendItemDTO> _legendItems;
      private LegendItemDTO _firstLegendItem;
      private LegendItemDTO _secondLegendItem;
      private LegendItemDTO _thirdLegendItem;

      protected override void Context()
      {
         base.Context();
         _firstLegendItem = new LegendItemDTO();
         _secondLegendItem = new LegendItemDTO();
         _thirdLegendItem = new LegendItemDTO();
         _legendItems = new[]
         {
            _firstLegendItem,
            _secondLegendItem,
            _thirdLegendItem
         };
      }

      protected override void Because()
      {
         sut.AddLegendItems(_legendItems);
      }

      [Observation]
      public void all_the_legend_items_must_be_added()
      {
         A.CallTo(() => _legendView.AddLegendItem(_firstLegendItem)).MustHaveHappened();
         A.CallTo(() => _legendView.AddLegendItem(_secondLegendItem)).MustHaveHappened();
         A.CallTo(() => _legendView.AddLegendItem(_thirdLegendItem)).MustHaveHappened();
      }

      [Observation]
      public void the_presenter_calculates_the_row_and_column_for_each_item()
      {
         sut.TargetColumnFor(_firstLegendItem).ShouldBeEqualTo(0);
         sut.TargetColumnFor(_secondLegendItem).ShouldBeEqualTo(0);
         sut.TargetColumnFor(_thirdLegendItem).ShouldBeEqualTo(1);

         sut.TargetRowFor(_firstLegendItem).ShouldBeEqualTo(0);
         sut.TargetRowFor(_secondLegendItem).ShouldBeEqualTo(1);
         sut.TargetRowFor(_thirdLegendItem).ShouldBeEqualTo(0);
      }
   }
}
