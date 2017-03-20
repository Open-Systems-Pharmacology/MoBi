using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.Views
{
   public interface IImportExportListView
   {
      void AttachPresenter(IImportExportPresenter presenter);
      void EnableDisable(bool enable);
   }
}