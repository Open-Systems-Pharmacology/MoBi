using System.Threading.Tasks;
using MoBi.BatchTool.Runners;
using MoBi.BatchTool.Services;
using MoBi.BatchTool.Views;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

namespace MoBi.BatchTool.Presenters
{
   public interface IPkmlFileFromFolderPresenter : IFileFromFolderPresenter
   {
   }

   public class PkmlFileFromFolderPresenter : FileFromFolderPresenter, IPkmlFileFromFolderPresenter
   {
      private readonly PkmlFileFromFolderRunner _runner;

      public PkmlFileFromFolderPresenter(IFileFromFolderRunnerView view, IDialogCreator dialogCreator,  
         ILogPresenter logPresenter, IBatchLogger batchLogger,  PkmlFileFromFolderRunner runner) : base(view, logPresenter, dialogCreator,batchLogger)
      {
         _runner = runner;
         _view.Caption = "Simulation Pkml Files Runner";
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