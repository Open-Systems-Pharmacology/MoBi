using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Events;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditChartTemplateManagerPresenter : IPresenter<IEditChartTemplateManagerView>, ISimulationSettingsItemPresenter
   {
      bool HasChanged { get; }
      IEnumerable<CurveChartTemplate> EditedTemplates { get; }
      void CommitChanges();
      void CancelChanges();
   }

   public class EditChartTemplateManagerPresenter : AbstractCommandCollectorPresenter<IEditChartTemplateManagerView, 
      IEditChartTemplateManagerPresenter>, 
      IEditChartTemplateManagerPresenter,
      IListener<BuildingBlockChartTemplatesModifiedEvent>,
      ILatchable
   {
      private readonly IChartTemplateManagerPresenter _chartTemplateManagerPresenter;
      private readonly ICloneManager _cloneManager;
      private readonly IChartTemplatingTask _chartTemplatingTask;
      private ISimulationSettings _simulationSettings;

      public bool IsLatched { get; set; }

      public EditChartTemplateManagerPresenter(
         IEditChartTemplateManagerView view, 
         IChartTemplateManagerPresenter chartTemplateManagerPresenter, 
         ICloneManager cloneManager,
         IChartTemplatingTask chartTemplatingTask) : base(view)
      {
         _chartTemplateManagerPresenter = chartTemplateManagerPresenter;
         _cloneManager = cloneManager;
         _chartTemplatingTask = chartTemplatingTask;
         AddSubPresenters(_chartTemplateManagerPresenter);
         _view.Caption = _chartTemplateManagerPresenter.BaseView.Caption;
      }

      public void Edit(ISimulationSettings simulationSettings)
      {
         rebind(simulationSettings);
         _view.SetSubView(_chartTemplateManagerPresenter.BaseView);
         _simulationSettings = simulationSettings;
      }

      private void rebind(ISimulationSettings simulationSettings)
      {
         var chartTemplatesToEdit = simulationSettings.ChartTemplates.Select(x =>
         {
            var curveChartTemplate = _cloneManager.Clone(x);
            // setting IsDefault is not part of general cloning and is done when cloning the set only
            curveChartTemplate.IsDefault = x.IsDefault;
            return curveChartTemplate;
         });
         _chartTemplateManagerPresenter.EditTemplates(chartTemplatesToEdit.OrderBy(x => x.Name));
      }

      public bool HasChanged
      {
         get { return _chartTemplateManagerPresenter.HasChanged; }
      }

      public IEnumerable<CurveChartTemplate> EditedTemplates
      {
         get { return _chartTemplateManagerPresenter.EditedTemplates; }
      }

      public void CommitChanges()
      {
         if (!_chartTemplateManagerPresenter.HasChanged) return;

         this.DoWithinLatch(() => AddCommand(_chartTemplatingTask.ReplaceTemplatesInBuildingBlockCommand(_simulationSettings, EditedTemplates)));
         
         rebind(_simulationSettings);
      }

      public void CancelChanges()
      {
         rebind(_simulationSettings);
      }

      public void Handle(BuildingBlockChartTemplatesModifiedEvent eventToHandle)
      {
         if(!canHandle(eventToHandle))
            return;

         rebind(_simulationSettings);
      }

      private bool canHandle(BuildingBlockChartTemplatesModifiedEvent eventToHandle)
      {
         return eventToHandle.SimulationSettings == _simulationSettings && !IsLatched;
      }

      public ApplicationIcon Icon
      {
         get { return ApplicationIcons.TimeProfileAnalysis; }
      }
   }
}