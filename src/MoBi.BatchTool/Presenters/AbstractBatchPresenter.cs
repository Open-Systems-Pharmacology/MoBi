using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using MoBi.BatchTool.Services;
using OSPSuite.Core.Services;
using MoBi.BatchTool.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.BatchTool.Presenters
{
   public interface IBatchPresenter : IPresenter
   {
      void Exit();
      void Run();
   }

   public abstract class AbstractBatchPresenter<TView, TPresenter> : AbstractPresenter<TView, TPresenter>
      where TPresenter : IPresenter
      where TView : IView<TPresenter>, IBatchView
   {
      private readonly ILogPresenter _logPresenter;
      protected readonly IDialogCreator _dialogCreator;
      private readonly IBatchLogger _batchLogger;
      private bool _isRunning;
      private readonly bool _startedFromCommandLine;

      protected AbstractBatchPresenter(TView view,  ILogPresenter logPresenter, IDialogCreator dialogCreator, IBatchLogger batchLogger) : base(view)
      {
         _logPresenter = logPresenter;
         _dialogCreator = dialogCreator;
         _batchLogger = batchLogger;
         _startedFromCommandLine = false;
         _isRunning = false;
         _view.AddLogView(logPresenter.View);
      }

      public override void Initialize()
      {
         View.Display();
      }

      public void Exit()
      {
         if (_isRunning)
         {
            var ans = _dialogCreator.MessageBoxYesNo("Batch is running. Really exit?");
            if (ans == ViewResult.No) return;
         }

         Application.Exit();
      }



      private bool shouldClose => _startedFromCommandLine && !Debugger.IsAttached;

      public async void Run()
      {
         if (_isRunning) return;
         _isRunning = true;
         _logPresenter.ClearLog();
         _view.CalculateEnabled = false;
         try
         {
            await StartBatch();
         }
         catch (Exception e)
         {
            _batchLogger.AddError(e.FullMessage());
         }

         _isRunning = false;
         _view.CalculateEnabled = true;

         if (shouldClose)
            Exit();
      }

      protected abstract Task StartBatch();
   }
}