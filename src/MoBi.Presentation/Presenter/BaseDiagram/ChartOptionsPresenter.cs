using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Views.BaseDiagram;

namespace MoBi.Presentation.Presenter.BaseDiagram
{
   public interface IChartOptionsPresenter : ISimpleEditPresenter<ChartOptions>
   {
   }

   public class ChartOptionsPresenter : SimpleEditPresenter<ChartOptions>, IChartOptionsPresenter
   {
      public ChartOptionsPresenter(IChartOptionsView view) : base(view)
      {
      }
   }
}