using System;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.BDDHelper;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.Diagram;

namespace MoBi.Presentation
{
   internal class TestMoBiBaseDiagramPresenter : MoBiBaseDiagramPresenter<IMoBiBaseDiagramView, IBaseDiagramPresenter, MoBiReactionBuildingBlock>
   {
      public TestMoBiBaseDiagramPresenter(
         IMoBiBaseDiagramView view,
         IContainerBaseLayouter layouter,
         IDialogCreator dialogCreator,
         IDiagramModelFactory diagramModelFactory,
         IUserSettings userSettings,
         IMoBiContext context,
         IDiagramTask diagramTask,
         IStartOptions runOptions)
         : base(view, layouter, dialogCreator, diagramModelFactory, userSettings, context, diagramTask, runOptions)
      {
         base._model = A.Fake<MoBiReactionBuildingBlock>();
         base._model.DiagramManager = A.Fake<IDiagramManager<MoBiReactionBuildingBlock>>();
      }

      public override void Link(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         throw new NotImplementedException();
      }

      protected override void Unlink(IBaseNode node1, IBaseNode node2, object portObject1, object portObject2)
      {
         throw new NotImplementedException();
      }
   }

   internal class concern_for_MoBiBaseDiagramPresenter : ContextSpecification<TestMoBiBaseDiagramPresenter>
   {
      protected override void Context()
      {
         sut = new TestMoBiBaseDiagramPresenter(
            A.Fake<IMoBiBaseDiagramView>(),
            A.Fake<IContainerBaseLayouter>(),
            A.Fake<IDialogCreator>(),
            A.Fake<IDiagramModelFactory>(),
            A.Fake<IUserSettings>(),
            A.Fake<IMoBiContext>(),
            A.Fake<IDiagramTask>(),
            A.Fake<IStartOptions>());
      }
   }

   internal class When_handling_null_entity_should_not_throw : concern_for_MoBiBaseDiagramPresenter
   {
      [Observation]
      public void should_not_throw_an_exception()
      {
         sut.Handle(new EntitySelectedEvent(null, null));
      }
   }
}