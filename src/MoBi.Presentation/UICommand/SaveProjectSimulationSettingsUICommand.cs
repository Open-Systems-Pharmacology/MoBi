using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.UICommand
{
   public class LoadProjectSimulationSettingsUICommand : IUICommand
   {
      private readonly IInteractionTasksForSimulationSettings _interactionTasks;
      private readonly IMoBiContext _context;

      public LoadProjectSimulationSettingsUICommand(IInteractionTasksForSimulationSettings interactionTasks, IMoBiContext context)
      {
         _interactionTasks = interactionTasks;
         _context = context;
      }

      public void Execute()
      {
         _context.AddToHistory(_interactionTasks.UpdateDefaultSimulationSettingsInProject());
      }
   }

   public class SaveProjectSimulationSettingsUICommand : IUICommand
   {
      private readonly IEditTaskFor<SimulationSettings> _editTasks;
      private readonly IMoBiContext _context;

      public SaveProjectSimulationSettingsUICommand(IEditTaskFor<SimulationSettings> editTasks, IMoBiContext context)
      {
         _editTasks = editTasks;
         _context = context;
      }

      public void Execute()
      {
         _editTasks.Save(_context.CurrentProject.SimulationSettings);
      }
   }
}