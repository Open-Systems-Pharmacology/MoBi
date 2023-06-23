using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveChartTemplateFromBuildingBlockCommand : ChartTemplateInBuildingBlockCommand
   {
      private byte[] _serializationStream;

      public RemoveChartTemplateFromBuildingBlockCommand(CurveChartTemplate chartTemplate, SimulationSettings buildingBlock) : base(chartTemplate, buildingBlock)
      {
         CommandType = AppConstants.Commands.DeleteCommand;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddChartTemplateToBuildingBlockCommand(_chartTemplate, _buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _chartTemplate = context.Deserialize<CurveChartTemplate>(_serializationStream);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _buildingBlock.RemoveChartTemplate(_chartTemplate.Name);
         _serializationStream = context.Serialize(_chartTemplate);
         Description = AppConstants.Commands.RemoveChartTemplateFromBuildingBlock(_chartTemplate.Name, _buildingBlock.Name);
         context.PublishEvent(new BuildingBlockChartTemplatesModifiedEvent(_buildingBlock));
      }
   }
}