using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class HistoryManagerRetriever : IHistoryManagerRetriever
   {
      private readonly IMoBiContext _context;

      public HistoryManagerRetriever(IMoBiContext context)
      {
         _context = context;
      }

      public IHistoryManager Current => _context.HistoryManager;
   }
}