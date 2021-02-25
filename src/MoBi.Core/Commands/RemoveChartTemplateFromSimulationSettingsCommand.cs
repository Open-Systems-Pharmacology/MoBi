using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;

namespace MoBi.Core.Commands
{
   public class RemoveChartTemplateFromSimulationSettingsCommand : ChartTemplateInSimulationSettingsCommand
   {
      private byte[] _serializationStream;

      public RemoveChartTemplateFromSimulationSettingsCommand(CurveChartTemplate chartTemplate, IMoBiSimulation simulation) : base(chartTemplate, simulation)
      {
         CommandType = AppConstants.Commands.DeleteCommand;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddChartTemplateToSimulationSettingsCommand(_chartTemplate, _simulation).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _chartTemplate = context.Deserialize<CurveChartTemplate>(_serializationStream);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _simulation.RemoveChartTemplate(_chartTemplate.Name);
         _serializationStream = context.Serialize(_chartTemplate);
         Description = AppConstants.Commands.RemoveChartTemplateFromSimulation(_chartTemplate.Name, _simulation.Name);
      }
   }
}