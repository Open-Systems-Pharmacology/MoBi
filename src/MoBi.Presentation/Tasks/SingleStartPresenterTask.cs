using System;
using MoBi.Core.Domain.Model;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation.Tasks
{
   public class SingleStartPresenterTask : ISingleStartPresenterTask
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiContext _context;

      public SingleStartPresenterTask(IMoBiApplicationController applicationController, IMoBiContext context)
      {
         _applicationController = applicationController;
         _context = context;
      }

      public void StartForSubject<T>(T subject)
      {
         var presenter = _applicationController.Open(subject, _context.HistoryManager);
         try
         {
            presenter.Edit(subject);
         }
         catch (Exception)
         {
            //exception while loading the subject. We need to close the presenter to avoid memory leaks
            _applicationController.Close(subject);
            throw;
         }
      }
   }
}