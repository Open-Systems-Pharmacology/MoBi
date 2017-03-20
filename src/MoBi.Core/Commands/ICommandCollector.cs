using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Commands
{

   public interface IMoBiHistoryManager : IHistoryManager<IMoBiContext>, ICommandCollector
   {
   }

   public class MoBiHistoryManager : HistoryManager<IMoBiContext>, IMoBiHistoryManager
   {
      private readonly IMoBiContext _context;

      public MoBiHistoryManager(IMoBiContext executionContext, IExceptionManager exceptionManager, IEventPublisher eventPublisher)
         : base(executionContext, eventPublisher, exceptionManager, new HistoryItemFactory(), new RollBackCommandFactory())
      {
         _context = executionContext;
      }

      public void AddCommand(ICommand commandToAdd)
      {
         AddToHistory(commandToAdd);
         notifyProjectChanged();
      }

      private void notifyProjectChanged()
      {
         _context.ProjectChanged();
      }

      public IEnumerable<ICommand> All()
      {
         return History.All().Select(x => x.Command);
      }
   }
}