using MoBi.BatchTool.Services;
using MoBi.BatchTool.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.BatchTool.Presenters
{
   public interface IFileFromFolderPresenter : IBatchPresenter, IPresenter<IFileFromFolderRunnerView>
   {
      void SelectInputFolder();
   }

   public abstract class FileFromFolderPresenter : AbstractBatchPresenter<IFileFromFolderRunnerView, IFileFromFolderPresenter>
   {
      protected readonly FileFromFolderDTO _dto;

      protected FileFromFolderPresenter(IFileFromFolderRunnerView view, ILogPresenter logPresenter, IDialogCreator dialogCreator, IBatchLogger batchLogger) : base(view, logPresenter, dialogCreator, batchLogger)
      {
         _dto = new FileFromFolderDTO();
         _view.BindTo(_dto);
      }

      public void SelectInputFolder()
      {
         var inputFolder = _dialogCreator.AskForFolder("Select input folder", Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(inputFolder)) return;
         _dto.InputFolder = inputFolder;
      }


   }
}