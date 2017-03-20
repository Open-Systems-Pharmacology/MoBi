using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class SendSimulationToPKSimUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IPKSimExportTask _pksimExportTask;
      private readonly IMoBiContext _context;

      public SendSimulationToPKSimUICommand(IPKSimExportTask pksimExportTask, IMoBiContext context)
      {
         _pksimExportTask = pksimExportTask;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _pksimExportTask.SendSimulationToPKSim(Subject);
      }
   }
}