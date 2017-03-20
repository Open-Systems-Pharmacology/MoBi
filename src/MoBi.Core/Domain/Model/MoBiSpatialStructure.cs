using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Domain.Model
{
   public interface IMoBiSpatialStructure : ISpatialStructure, IWithDiagramFor<IMoBiSpatialStructure>
   {
   }

   public class MoBiSpatialStructure : SpatialStructure, IMoBiSpatialStructure
   {
      public IDiagramModel DiagramModel { get; set; }
      public IDiagramManager<IMoBiSpatialStructure> DiagramManager { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable sourceObject, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(sourceObject, cloneManager);
         var sourceSpatialStructure = sourceObject as IMoBiSpatialStructure;
         if (sourceSpatialStructure == null) return;

         this.UpdateDiagramFrom(sourceSpatialStructure);
      }
   }
}