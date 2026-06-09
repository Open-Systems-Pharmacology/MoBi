using System.Drawing;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Presenters.Diagram;

namespace MoBi.Presentation.Tasks
{
   public interface IDiagramModelToImageTask
   {
      Bitmap CreateFor<T>(T withDiagramModel) where T : IWithDiagramFor<T>;
   }

   public class DiagramModelToImageTask : IDiagramModelToImageTask
   {
      private readonly IMoBiApplicationController _applicationController;

      public DiagramModelToImageTask(IMoBiApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public Bitmap CreateFor<T>(T withDiagramModel) where T : IWithDiagramFor<T>
      {
         using (var presenter = _applicationController.Start<IBaseDiagramPresenter<T>>())
         {
            presenter.Edit(withDiagramModel);
            return presenter.GetBitmap(withDiagramModel.DiagramModel);
         }
      }
   }
}