using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Presenters.Diagram;

namespace MoBi.Presentation
{
   public abstract class concern_for_DiagramModelToImageTask : ContextSpecification<IDiagramModelToImageTask>
   {
      protected IMoBiApplicationController _applicationController;

      protected override void Context()
      {
         _applicationController = A.Fake<IMoBiApplicationController>();
         sut = new DiagramModelToImageTask(_applicationController);
      }
   }

   public class When_creating_a_image_for_a_given_diagram_model : concern_for_DiagramModelToImageTask
   {
      private IBaseDiagramPresenter<MoBiSpatialStructure> _presenter;
      private MoBiSpatialStructure _spatialStructure;
      private Bitmap _bitmap;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<IBaseDiagramPresenter<MoBiSpatialStructure>>();
         _spatialStructure = new MoBiSpatialStructure { DiagramModel = A.Fake<IDiagramModel>() };
         A.CallTo(() => _applicationController.Start<IBaseDiagramPresenter<MoBiSpatialStructure>>()).Returns(_presenter);
         _bitmap = new Bitmap(10, 10);
         A.CallTo(() => _presenter.GetBitmap(_spatialStructure.DiagramModel)).Returns(_bitmap);
      }

      [Observation]
      public void should_leverage_the_presneter_for_the_given_type_and_retrieve_the_bitmap()
      {
         sut.CreateFor(_spatialStructure).ShouldBeEqualTo(_bitmap);
      }
   }
}