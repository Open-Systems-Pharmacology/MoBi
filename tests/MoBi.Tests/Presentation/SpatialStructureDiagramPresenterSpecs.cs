using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.SpaceDiagram;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;

namespace MoBi.Presentation
{
   public class TestableSpatialStructureDiagramPresenter : SpatialStructureDiagramPresenter
   {
      public TestableSpatialStructureDiagramPresenter(
         ISpatialStructureDiagramView view,
         IContainerBaseLayouter layouter,
         IUserSettings userSettings,
         IMoBiContext context,
         IDialogCreator dialogCreator,
         IMoBiConfiguration configuration,
         IDiagramTask diagramTask,
         IStartOptions runOptions,
         IDiagramModelFactory diagramModelFactory)
         : base(view, layouter, userSettings, context, dialogCreator, configuration, diagramTask, runOptions, diagramModelFactory)
      {
      }

      public IDiagramModel CreateModel() => CreateDiagramModel();
   }

   public abstract class concern_for_SpatialStructureDiagramPresenter : ContextSpecification<TestableSpatialStructureDiagramPresenter>
   {
      protected IDiagramModelFactory _diagramModelFactory;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _diagramModelFactory = A.Fake<IDiagramModelFactory>();
         _context = A.Fake<IMoBiContext>();
         var userSettings = A.Fake<IUserSettings>();
         A.CallTo(() => userSettings.DiagramOptions).Returns(new DiagramOptions());

         sut = new TestableSpatialStructureDiagramPresenter(A.Fake<ISpatialStructureDiagramView>(), A.Fake<IContainerBaseLayouter>(), userSettings,
            _context, A.Fake<IDialogCreator>(), A.Fake<IMoBiConfiguration>(), A.Fake<IDiagramTask>(), A.Fake<IStartOptions>(),
            _diagramModelFactory);
      }
   }

   public class When_creating_the_diagram_model_of_a_spatial_structure : concern_for_SpatialStructureDiagramPresenter
   {
      private IDiagramModel _diagramModel;
      private IDiagramModel _createdModel;

      protected override void Context()
      {
         base.Context();
         _diagramModel = new DiagramModel();
         A.CallTo(() => _diagramModelFactory.Create()).Returns(_diagramModel);
      }

      protected override void Because()
      {
         _createdModel = sut.CreateModel();
      }

      [Observation]
      public void should_create_the_ui_free_diagram_model()
      {
         _createdModel.ShouldBeEqualTo(_diagramModel);
      }
   }

   public class When_linking_two_nodes_in_a_spatial_structure_diagram : concern_for_SpatialStructureDiagramPresenter
   {
      protected override void Because()
      {
         sut.Link(new ContainerNode(), new ContainerNode(), null, null);
      }

      [Observation]
      public void should_not_create_a_neighborhood_from_the_diagram()
      {
         A.CallTo(() => _context.Resolve<IInteractionTasksForNeighborhood>()).MustNotHaveHappened();
      }
   }
}
