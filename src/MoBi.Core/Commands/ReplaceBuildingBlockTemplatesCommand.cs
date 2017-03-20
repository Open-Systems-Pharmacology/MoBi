using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ReplaceBuildingBlockTemplatesCommand : MoBiMacroCommand
   {
      private ISimulationSettings _simulationSettings;
      private IEnumerable<CurveChartTemplate> _newTemplates;

      public ReplaceBuildingBlockTemplatesCommand(ISimulationSettings simulationSettings, IEnumerable<CurveChartTemplate> newTemplates)
      {
         _simulationSettings = simulationSettings;
         _newTemplates = newTemplates;

         ObjectType = ObjectTypes.ChartTemplate;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditChartTemplateInBuildingBlock(simulationSettings.Name);
      }

      public override void Execute(IMoBiContext context)
      {
         var allCommands = new List<IMoBiCommand>();
         //add the required commands
         _simulationSettings.ChartTemplates.Each(t  => allCommands.Add(new RemoveChartTemplateFromBuildingBlockCommand(t, _simulationSettings)));
         _newTemplates.Each(t => allCommands.Add(new AddChartTemplateToBuildingBlockCommand(t, _simulationSettings)));

         //hide from the history browser ( only if at least two command)
         if (allCommands.Count >= 2)
            allCommands.Each(x => x.Visible = false);

         AddRange(allCommands);

         //now execute all commands
         base.Execute(context);

         //clear references
         _simulationSettings = null;
         _newTemplates = null;
      }
   }
}