using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;

namespace MoBi.Core.Commands
{
   public class AddChartTemplateToSimulationSettingsCommand : ChartTemplateInSimulationSettingsCommand
   {
      private readonly string _chartTemplateName;

      public AddChartTemplateToSimulationSettingsCommand(CurveChartTemplate chartTemplate, IMoBiSimulation simulation)
         : base(chartTemplate, simulation)
      {
         CommandType = AppConstants.Commands.AddCommand;
         _chartTemplateName = _chartTemplate.Name;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveChartTemplateFromSimulationSettingsCommand(_chartTemplate, _simulation).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _simulation.AddChartTemplate(_chartTemplate);
         Description = AppConstants.Commands.AddChartTemplateToSimulation(_chartTemplate.Name, _simulation.Name);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _chartTemplate = _simulation.ChartTemplateByName(_chartTemplateName);
      }
   }
}