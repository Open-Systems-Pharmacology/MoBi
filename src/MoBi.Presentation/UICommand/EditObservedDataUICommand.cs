using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class EditObservedDataUICommand : ObjectUICommand<DataRepository>
   {
      private readonly IApplicationController _applicationController;
      private readonly IMoBiContext _context;

      public EditObservedDataUICommand(IApplicationController applicationController, IMoBiContext context)
      {
         _applicationController = applicationController;
         _context = context;
      }

      protected override void PerformExecute()
      {
         var presenter = _applicationController.Open(Subject, _context.HistoryManager);
         presenter.Edit(Subject);
      }
   }
}