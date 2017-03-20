using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Charts;

namespace MoBi.Presentation.Tasks
{
   public interface IChartTemplatingTask : OSPSuite.Presentation.Services.Charts.IChartTemplatingTask
   {
      /// <summary>
      ///    Returns the command that was used when replacing all the templates from a simulation settings
      /// </summary>
      ICommand ReplaceTemplatesInBuildingBlockCommand(ISimulationSettings simulationSettings, IEnumerable<CurveChartTemplate> curveChartTemplates);

      void InitFromTemplate(ICache<DataRepository, IMoBiSimulation> simulations, ICurveChart chart, IChartEditorPresenter chartEditorPresenter, CurveChartTemplate chartTemplate, Func<DataColumn, string> curveNameDefinition, bool triggeredManually);
   }

   public class ChartTemplatingTask : OSPSuite.Presentation.Services.Charts.ChartTemplatingTask, IChartTemplatingTask
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IMoBiContext _context;

      public ChartTemplatingTask(IChartTemplatePersistor chartTemplatePersistor, IChartFromTemplateService chartFromTemplateService,
         ICurveChartToCurveChartTemplateMapper chartTemplateMapper, IMoBiApplicationController applicationController,
         IDialogCreator dialogCreator, ICloneManagerForModel cloneManager, IMoBiContext context)
         : base(applicationController, chartTemplatePersistor, cloneManager, chartTemplateMapper, chartFromTemplateService)
      {
         _dialogCreator = dialogCreator;
         _context = context;
      }

      public AddChartTemplateToSimulationSettingsCommand AddChartTemplateToSimulationSettings(CurveChartTemplate template, ISimulation withSimulationSettings)
      {
         return new AddChartTemplateToSimulationSettingsCommand(template, withSimulationSettings as IMoBiSimulation).Run(_context);
      }

      protected override string AskForInput(string caption, string s, string defaultName, List<string> usedNames)
      {
         return _dialogCreator.AskForInput(caption, s, defaultName, usedNames);
      }

      public ICommand ReplaceTemplatesInBuildingBlockCommand(ISimulationSettings simulationSettings, IEnumerable<CurveChartTemplate> curveChartTemplates)
      {
         return new ReplaceBuildingBlockTemplatesCommand(simulationSettings, curveChartTemplates).Run(_context);
      }

      public void InitFromTemplate(ICache<DataRepository, IMoBiSimulation> simulations, ICurveChart chart, IChartEditorPresenter chartEditorPresenter, CurveChartTemplate chartTemplate, Func<DataColumn, string> curveNameDefinition, bool triggeredManually)
      {
         var allAvailableColumns = chartEditorPresenter.GetAllDataColumns().ToList();
         if (chartTemplate == null)
         {
            UpdateDefaultSettings(chartEditorPresenter, allAvailableColumns, simulations);
            return;
         }

         InitializeChartFromTemplate(chart, allAvailableColumns, chartTemplate, curveNameDefinition, triggeredManually);

         if (!chart.Curves.Any() && !triggeredManually)
            UpdateDefaultSettings(chartEditorPresenter, allAvailableColumns, simulations);
      }

      public override ICommand AddChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates)
      {
         if (withChartTemplates.IsAnImplementationOf<IMoBiSimulation>())
            return new AddChartTemplateToSimulationSettingsCommand(template, withChartTemplates.DowncastTo<IMoBiSimulation>()).Run(_context);

         return updateChartTemplates(withChartTemplates, x => x.AddChartTemplate(template));
      }

      public override ICommand UpdateChartTemplateCommand(CurveChartTemplate curveChartTemplate, IWithChartTemplates withChartTemplates, string templateName)
      {
         if (withChartTemplates.IsAnImplementationOf<IMoBiSimulation>())
            return new UpdateChartTemplateInSimulationSettingsCommand(curveChartTemplate, withChartTemplates.DowncastTo<IMoBiSimulation>(), templateName).Run(_context);

         return updateChartTemplates(withChartTemplates, x =>
         {
            withChartTemplates.RemoveChartTemplate(templateName);
            curveChartTemplate.Name = templateName;
            withChartTemplates.AddChartTemplate(curveChartTemplate);
         });
      }

      protected override ICommand ReplaceTemplatesCommand(IWithChartTemplates withChartTemplates, IEnumerable<CurveChartTemplate> curveChartTemplates)
      {
         if (withChartTemplates.IsAnImplementationOf<IMoBiSimulation>())
            return new ReplaceSimulationTemplatesCommand(withChartTemplates.DowncastTo<IMoBiSimulation>(), curveChartTemplates).Run(_context);

         return updateChartTemplates(withChartTemplates, x =>
         {
            withChartTemplates.RemoveAllChartTemplates();
            curveChartTemplates.Each(withChartTemplates.AddChartTemplate);
         });
      }

      private ICommand updateChartTemplates(IWithChartTemplates withChartTemplates, Action<IWithChartTemplates> action)
      {
         action(withChartTemplates);
         _context.PublishEvent(new ChartTemplatesChangedEvent(withChartTemplates));
         return new MoBiEmptyCommand();
      }
   }
}