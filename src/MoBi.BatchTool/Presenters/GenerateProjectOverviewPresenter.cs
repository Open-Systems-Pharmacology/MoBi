using System.Threading.Tasks;
using MoBi.BatchTool.Runners;
using MoBi.BatchTool.Services;
using MoBi.BatchTool.Views;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.BatchTool.Presenters
{
   public interface IGenerateProjectOverviewPresenter : IFileFromFolderPresenter
   {
   }

   public class GenerateProjectOverviewPresenter : FileFromFolderPresenter, IGenerateProjectOverviewPresenter
   {
      private readonly ProjectOverviewRunner _runner;

      public GenerateProjectOverviewPresenter(IFileFromFolderRunnerView view, IDialogCreator dialogCreator,
         ILogPresenter logPresenter, IBatchLogger batchLogger, ProjectOverviewRunner runner) : base(view, logPresenter, dialogCreator, batchLogger)
      {
         _runner = runner;
         _view.Caption = "MoBi Project Overview Runner";
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