using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class SaveNeighborhoodUICommand : ObjectUICommand<NeighborhoodBuilder>
   {
      private readonly ISpatialStructureContentExporter _spatialStructureContentExporter;

      public SaveNeighborhoodUICommand(ISpatialStructureContentExporter spatialStructureContentExporter)
      {
         _spatialStructureContentExporter = spatialStructureContentExporter;
      }

      protected override void PerformExecute()
      {
         _spatialStructureContentExporter.Save(Subject);
      }
   }
}