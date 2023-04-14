using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiSpatialStructure : ContextSpecification<MoBiSpatialStructure>
   {
      protected override void Context()
      {
         sut = new MoBiSpatialStructure();
      }
   }

   public class When_updating_properties_from_source_spatial_structure : concern_for_MoBiSpatialStructure
   {
      private ICloneManager _cloneManager;
      private ISpatialStructureDiagramManager _diagramManager;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         _diagramManager = A.Fake<ISpatialStructureDiagramManager>();
         _spatialStructure = new MoBiSpatialStructure { DiagramManager = _diagramManager};
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_spatialStructure, _cloneManager);
      }

      [Observation]
      public void the_source_diagram_manager_creates_new_diagram_manager_for_target()
      {
         A.CallTo(() => _diagramManager.Create()).MustHaveHappened();
      }
   }
}
