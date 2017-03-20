using System.Drawing;
using OSPSuite.Core.Diagram;

namespace MoBi.Core.Services
{
   public interface IDiagramModelToImageTask
   {
      Bitmap CreateFor<T>(T withDiagramModel) where T : IWithDiagramFor<T>;
      void ExportTo<T>(T withDiagramModel, string imageFilePath) where T : IWithDiagramFor<T>;
   }
}  