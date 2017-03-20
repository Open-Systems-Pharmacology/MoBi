using System.Threading.Tasks;
using MoBi.BatchTool.Runners;
using MoBi.BatchTool.Services;
using MoBi.BatchTool.Views;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.BatchTool.Presenters
{
   public interface IProjectFromFolderPresenter : IFileFromFolderPresenter
   {
   }

   public class ProjectFromFolderPresenter : FileFromFolderPresenter, IProjectFromFolderPresenter
   {
      private readonly ProjectFromFolderRunner _runner;

      public ProjectFromFolderPresenter(IFileFromFolderRunnerView view, IDialogCreator dialogCreator,  
         ILogPresenter logPresenter,  IBatchLogger batchLogger,  ProjectFromFolderRunner runner) : base(view,  logPresenter, dialogCreator, batchLogger)
      {
         _runner = runner;
         _view.Caption = "MoBi Project Files Runner";
      }
      protected override Task StartBatch()
      {
         return _runner.RunBatch(new
         {
            inputFolder = _dto.InputFolder
         });
      }
   }

}