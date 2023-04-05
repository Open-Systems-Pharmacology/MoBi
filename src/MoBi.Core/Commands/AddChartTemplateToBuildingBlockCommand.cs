using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddChartTemplateToBuildingBlockCommand : ChartTemplateInBuildingBlockCommand
   {
      private readonly string _chartTemplateName;

      public AddChartTemplateToBuildingBlockCommand(CurveChartTemplate chartTemplate, SimulationSettings buildingBlock) : base(chartTemplate, buildingBlock)
      {
         CommandType = AppConstants.Commands.AddCommand;
         _chartTemplateName = _chartTemplate.Name;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveChartTemplateFromBuildingBlockCommand(_chartTemplate, _buildingBlock).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _buildingBlock.AddChartTemplate(_chartTemplate);
         Description = AppConstants.Commands.AddChartTemplateToBuildingBlock(_chartTemplate.Name, _buildingBlock.Name);
         context.PublishEvent(new BuildingBlockChartTemplatesModifiedEvent(_buildingBlock));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _chartTemplate = _buildingBlock.ChartTemplates.FindByName(_chartTemplateName);
      }
   }
}