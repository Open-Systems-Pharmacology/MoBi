using System.Drawing;
using System.Drawing.Imaging;
using MoBi.Core.Services;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Presenters.Diagram;

namespace MoBi.Presentation.Tasks
{
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

      public void ExportTo<T>(T withDiagramModel, string imageFilePath) where T : IWithDiagramFor<T>
      {
         var image = CreateFor(withDiagramModel);
         image.Save(imageFilePath, ImageFormat.Png);
      }
   }
}