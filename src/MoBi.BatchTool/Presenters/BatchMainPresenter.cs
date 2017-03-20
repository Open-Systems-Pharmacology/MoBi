using MoBi.BatchTool.Views;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.BatchTool.Presenters
{
   public interface IBatchMainPresenter : IPresenter<IBatchMainView>
   {
      void StartPkmlLoadFromFolder();
      void StartProjectLoadFromFolder();
      void StartGenerateProjectOverview();
   }

   public class BatchMainPresenter : AbstractPresenter<IBatchMainView, IBatchMainPresenter>, IBatchMainPresenter
   {
      private readonly IApplicationController _applicationController;

      public BatchMainPresenter(IBatchMainView view, IApplicationController applicationController) : base(view)
      {
         _applicationController = applicationController;
      }

      public void StartPkmlLoadFromFolder()
      {
         start<IPkmlFileFromFolderPresenter>();
      }

      public void StartProjectLoadFromFolder()
      {
         start<IProjectFromFolderPresenter>();
      }

      private void start<T>() where T : IPresenter
      {
         var presenter = _applicationController.Start<T>();
         _view.Hide();
      }

      public void StartGenerateProjectOverview()
      {
         start<IGenerateProjectOverviewPresenter>();
      }
   }
}