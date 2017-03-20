using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ReplaceSimulationTemplatesCommand : MoBiMacroCommand
   {
      private IMoBiSimulation _simulation;
      private IEnumerable<CurveChartTemplate> _newTemplates;

      public ReplaceSimulationTemplatesCommand(IMoBiSimulation simulation, IEnumerable<CurveChartTemplate> newTemplates)
      {
         _simulation = simulation;
         _newTemplates = newTemplates;

         ObjectType = ObjectTypes.ChartTemplate;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditChartTemplateInSimulation(simulation.Name);
      }

      public override void Execute(IMoBiContext context)
      {
         var allCommands = new List<IMoBiCommand>();
         //add the required commands
         _simulation.ChartTemplates.Each(t => allCommands.Add(new RemoveChartTemplateFromSimulationSettingsCommand(t, _simulation)));
         _newTemplates.Each(t => allCommands.Add(new AddChartTemplateToSimulationSettingsCommand(t, _simulation)));

         //hide from the history browser ( only if at least two command)
         if (allCommands.Count >= 2)
            allCommands.Each(x => x.Visible = false);

         AddRange(allCommands);

         //now execute all commands
         base.Execute(context);

         //clear references
         _simulation = null;
         _newTemplates = null;
      }
   }
}