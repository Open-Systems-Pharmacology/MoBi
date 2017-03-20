using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ILegendPresenter : IPresenter<ILegendView>
   {
      void AddLegendItems(IReadOnlyList<LegendItemDTO> legendItems);
      int TargetColumnFor(LegendItemDTO legendItem);
      int TargetRowFor(LegendItemDTO legendItem);
   }

   public class LegendPresenter : AbstractPresenter<ILegendView, ILegendPresenter>, ILegendPresenter
   {
      private const int DEFAULT_LEGEND_HEIGHT = 2;
      private List<LegendItemDTO> _legendItems;

      public int LegendHeight { set; get; } = DEFAULT_LEGEND_HEIGHT;

      public LegendPresenter(ILegendView view) : base(view)
      {
      }

      public void AddLegendItems(IReadOnlyList<LegendItemDTO> legendItems)
      {
         _legendItems = legendItems.ToList();
         legendItems.Each(item => _view.AddLegendItem(item));
      }

      public int TargetColumnFor(LegendItemDTO legendItem)
      {
         return _legendItems.IndexOf(legendItem) / LegendHeight;
      }

      public int TargetRowFor(LegendItemDTO legendItem)
      {
         return _legendItems.IndexOf(legendItem) % LegendHeight;
      }
   }
}
