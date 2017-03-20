using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;

namespace MoBi.Presentation.UICommand
{
   public class UndoCommand : IUICommand
   {
      private readonly IMoBiContext _context;

      public UndoCommand(IMoBiContext context)
      {
         _context = context;
      }

      public void Execute()
      {
         _context.HistoryManager.Undo();
      }
   }
}