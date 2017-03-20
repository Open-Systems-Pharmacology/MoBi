using OSPSuite.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ProjectChartView : BaseMdiChildView, IProjectChartView
   {
      public ProjectChartView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IProjectChartPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddView(IView view)
      {
         this.FillWith(view);
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return ApplicationIcons.SimulationComparison; }
      }
   }
}