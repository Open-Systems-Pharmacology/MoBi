using System.Collections.Generic;
using MoBi.Core.Services;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IProjectChartPresenter : ISingleStartPresenter<CurveChart>
   {
      void Show(CurveChart chart, IReadOnlyList<DataRepository> data);
   }

   public class ProjectChartPresenter : SubjectPresenter<IProjectChartView, IProjectChartPresenter, CurveChart>, IProjectChartPresenter
   {
      private readonly IComparisonChartPresenter _chartPresenter;
      private readonly IMoBiProjectRetriever _projectRetriever;

      public ProjectChartPresenter(IProjectChartView view, IComparisonChartPresenter chartPresenter, IMoBiProjectRetriever projectRetriever) : base(view)
      {
         _chartPresenter = chartPresenter;
         _projectRetriever = projectRetriever;
         view.AddView(chartPresenter.BaseView);
         AddSubPresenters(_chartPresenter);
      }

      public override void Edit(CurveChart chart)
      {
         Show(chart, new List<DataRepository>());
      }

      public override object Subject
      {
         get { return _chartPresenter.Chart; }
      }

      public void Show(CurveChart chart, IReadOnlyList<DataRepository> data)
      {
         _chartPresenter.Show(chart, data);
         _chartPresenter.UpdateTemplatesFor(_projectRetriever.Current);
         UpdateCaption();
         View.Display();
      }

      protected override void UpdateCaption()
      {
         View.Caption = _chartPresenter.Chart.Name;
      }
   }
}