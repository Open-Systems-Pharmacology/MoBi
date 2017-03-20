using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Diagram.Extensions;

namespace MoBi.Core.Domain.Model
{
   public static class WithDiagramModelExtensions
   {
      public static void UpdateDiagramFrom<T>(this T target, T source) where T : IWithDiagramFor<T>
      {
         initializeDiagramManager(source, target);
         cloneDiagramModel(source, target);
      }

      private static void initializeDiagramManager<T>(T source, T target) where T : IWithDiagramFor<T>
      {
         //already initialized or nothing to initialize
         if (target.DiagramManager != null)
            return;

         target.DiagramManager = source.DiagramManager?.Create();
      }

      private static void cloneDiagramModel<T>(T source, T target) where T : IWithDiagramFor<T>
      {
         var sourceDiagramModel = source.DiagramModel;

         if (sourceDiagramModel == null)
            return;

         target.DiagramModel = sourceDiagramModel.Create();
         target.InitializeDiagramManager(source.DiagramManager.DiagramOptions);

         new LayoutCopyService().Copy(source.DiagramModel, target.DiagramModel);
      }
   }
}