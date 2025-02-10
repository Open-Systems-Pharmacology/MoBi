using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class ChartTemplateInBuildingBlockCommand : BuildingBlockChangeCommandBase<SimulationSettings>
   {
      protected CurveChartTemplate _chartTemplate;

      protected ChartTemplateInBuildingBlockCommand(CurveChartTemplate chartTemplate, SimulationSettings buildingBlock) : base(buildingBlock)
      {
         _chartTemplate = chartTemplate;
         ObjectType = ObjectTypes.ChartTemplate;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         DoExecute(context);
         context.PublishEvent(new ChartTemplatesChangedEvent(_buildingBlock));
      }

      protected abstract void DoExecute(IMoBiContext context);

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _chartTemplate = null;
      }
   }
}