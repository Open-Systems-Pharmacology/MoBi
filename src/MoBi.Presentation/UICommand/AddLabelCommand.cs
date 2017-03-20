using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using OSPSuite.Presentation.Services.Commands;

namespace MoBi.Presentation.UICommand
{
   public class AddLabelCommand : IUICommand
   {
      private readonly ILabelTask _labelTask;
      private readonly IMoBiContext _context;

      public AddLabelCommand(ILabelTask labelTask, IMoBiContext context)
      {
         _labelTask = labelTask;
         _context = context;
      }

      public void Execute()
      {
         _labelTask.AddLabelTo(_context.HistoryManager);
      }
   }
}