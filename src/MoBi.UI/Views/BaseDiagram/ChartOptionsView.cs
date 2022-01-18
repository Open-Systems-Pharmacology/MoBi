using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Charts;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views.BaseDiagram
{
   public partial class ChartOptionsView : BaseUserControl, IChartOptionsView
   {
      private readonly IChartLayoutTemplateRepository _chartLayoutTemplateRepository;
      private ScreenBinder<ChartOptions> _screenBinder;
      private ISimpleEditPresenter<ChartOptions> _presenter;

      public ChartOptionsView(IChartLayoutTemplateRepository chartLayoutTemplateRepository)
      {
         _chartLayoutTemplateRepository = chartLayoutTemplateRepository;
         InitializeComponent();
         InitializeBinding();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         defaultYScalingLayoutItem.Text = AppConstants.Captions.DefaultChartYScaling.FormatForLabel();
         defaultLayoutLayoutItem.Text = AppConstants.Captions.DefaultLayout.FormatForLabel();
         chartBackgroundColorLayoutItem.Text = Captions.ChartColor.FormatForLabel();
         chckColorGroupObservedData.Text = Captions.ShouldColorGroupObservedData;
         chartDiagramBackgroundColorLayoutItem.Text = Captions.DiagramBackground.FormatForLabel();
      }

      public void AttachPresenter(ISimpleEditPresenter<ChartOptions> presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<ChartOptions>();
         _screenBinder.Bind(options => options.SimulationInCurveName).To(chkSimulationInCurveName);
         _screenBinder.Bind(options => options.TopContainerInCurveName).To(chkTopContainerInCurveName);
         _screenBinder.Bind(options => options.DefaultChartYScaling).To(cbPreferredChartYScaling).WithValues(getAllScalings);
         _screenBinder.Bind(options => options.DefaultChartBackColor).To(chartBackgroundColorEdit);
         _screenBinder.Bind(options => options.DefaultChartDiagramBackColor).To(diagramColorEdit);

        /* _screenBinder.Bind(options => options.ColorGroupObservedDataFromSameFolder)
            .To(chckColorGroupObservedData)
            .WithCaption(Captions.ShouldColorGroupObservedData);*/

         _screenBinder.Bind(options => options.DefaultLayoutName)
            .To(cbeDefaultLayoutName)
            .WithValues(getDefaultLayoutNames);
      }

      private IEnumerable<Scalings> getAllScalings(ChartOptions arg)
      {
         return EnumHelper.AllValuesFor<Scalings>();
      }

      private IEnumerable<string> getDefaultLayoutNames(ChartOptions chartOptions)
      {
         return _chartLayoutTemplateRepository.All().Select(x => x.Name);
      }

      public void Show(ChartOptions chartOptions)
      {
         _screenBinder.BindToSource(chartOptions);
      }

      public override bool HasError => _screenBinder.HasError;
   }
}