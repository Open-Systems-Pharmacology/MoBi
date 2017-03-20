using MoBi.BatchTool.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.BatchTool.Views
{
   public interface IFileFromFolderRunnerView : IView<IFileFromFolderPresenter>, IBatchView
   {
      void BindTo(FileFromFolderDTO fileFromFolderDTO);
   }
}