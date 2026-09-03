using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;

namespace MoBi.Core
{
   public abstract class concern_for_ReactionBuildingBlockFactory : ContextSpecification<ReactionBuildingBlockFactory>
   {
      protected IMoBiReactionDiagramManager _diagramManager;
      protected IDiagramModel _diagramModel;

      protected override void Context()
      {
         var objectBaseFactory = A.Fake<IObjectBaseFactory>();
         var diagramManagerFactory = A.Fake<IDiagramManagerFactory>();
         var reactionDiagramModelFactory = A.Fake<IReactionDiagramModelFactory>();
         _diagramManager = A.Fake<IMoBiReactionDiagramManager>();
         _diagramModel = A.Fake<IDiagramModel>();
         A.CallTo(() => objectBaseFactory.Create<MoBiReactionBuildingBlock>()).Returns(new MoBiReactionBuildingBlock());
         A.CallTo(() => diagramManagerFactory.Create<IMoBiReactionDiagramManager>()).Returns(_diagramManager);
         A.CallTo(() => reactionDiagramModelFactory.Create()).Returns(_diagramModel);
         sut = new ReactionBuildingBlockFactory(objectBaseFactory, diagramManagerFactory, reactionDiagramModelFactory);
      }
   }

   public class When_creating_a_reaction_building_block : concern_for_ReactionBuildingBlockFactory
   {
      private MoBiReactionBuildingBlock _result;

      protected override void Because()
      {
         _result = sut.Create();
      }

      [Observation]
      public void should_set_the_diagram_manager_created_by_the_diagram_manager_factory()
      {
         _result.DiagramManager.ShouldBeEqualTo(_diagramManager);
      }

      [Observation]
      public void should_set_the_diagram_model_created_by_the_reaction_diagram_model_factory()
      {
         _result.DiagramModel.ShouldBeEqualTo(_diagramModel);
      }
   }
}
