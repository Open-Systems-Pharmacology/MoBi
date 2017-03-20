using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;

namespace MoBi.Core.Commands
{
   public class UpdateChartTemplateInSimulationSettingsCommand : ChartTemplateInSimulationSettingsCommand
   {
      private readonly string _templateNameToUpdate;
      private CurveChartTemplate _oldTemplate;

      public UpdateChartTemplateInSimulationSettingsCommand(CurveChartTemplate chartTemplate, IMoBiSimulation simulation, string templateNameToUpdate) : base(chartTemplate, simulation)
      {
         _templateNameToUpdate = templateNameToUpdate;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateChartTemplateInSimulationSettingsCommand(_oldTemplate, _simulation, _templateNameToUpdate).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _oldTemplate = _simulation.ChartTemplateByName(_templateNameToUpdate);
         _chartTemplate.Name = _templateNameToUpdate;
         _simulation.AddChartTemplate(_chartTemplate);
         Description = AppConstants.Commands.UpdateChartTemplateInSimulation(_templateNameToUpdate, _simulation.Name);
      }
   }
}