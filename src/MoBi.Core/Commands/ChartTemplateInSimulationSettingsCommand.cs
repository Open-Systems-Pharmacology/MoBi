using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Events;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class ChartTemplateInSimulationSettingsCommand : SimulationChangeCommandBase
   {
      protected CurveChartTemplate _chartTemplate;

      protected ChartTemplateInSimulationSettingsCommand(CurveChartTemplate chartTemplate, IMoBiSimulation simulation) : base(chartTemplate, simulation)
      {
         _chartTemplate = chartTemplate;
         ObjectType = ObjectTypes.ChartTemplate;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.PublishEvent(new ChartTemplatesChangedEvent(_simulation));
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _chartTemplate = null;
      }
   }
}