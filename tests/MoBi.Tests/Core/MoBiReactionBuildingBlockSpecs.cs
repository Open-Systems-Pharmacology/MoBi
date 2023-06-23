using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiReactionBuildingBlock : ContextSpecification<MoBiReactionBuildingBlock>
   {
      protected override void Context()
      {
         sut = new MoBiReactionBuildingBlock();
      }
   }

   public class When_updating_properties_from_source_building_block : concern_for_MoBiReactionBuildingBlock
   {
      private ICloneManager _cloneManager;
      private IReactionDiagramManager<MoBiReactionBuildingBlock> _sourceDiagramManager;
      private MoBiReactionBuildingBlock _sourceReactionBuildingBlock;
      private IReactionDiagramManager<MoBiReactionBuildingBlock> _newDiagramManagerFromSource;
      private IDiagramModel _newDiagramModelFromSource;
      private IDiagramModel _sourceDiagramModel;

      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         _sourceDiagramManager = A.Fake<IReactionDiagramManager<MoBiReactionBuildingBlock>>();
         _sourceDiagramModel = A.Fake<IDiagramModel>();
         _newDiagramManagerFromSource = A.Fake<IReactionDiagramManager<MoBiReactionBuildingBlock>>();
         _newDiagramModelFromSource = A.Fake<IDiagramModel>();
         A.CallTo(() => _sourceDiagramManager.Create()).Returns(_newDiagramManagerFromSource);
         A.CallTo(() => _sourceDiagramModel.Create()).Returns(_newDiagramModelFromSource);
         _sourceReactionBuildingBlock = new MoBiReactionBuildingBlock {DiagramManager = _sourceDiagramManager, DiagramModel = _sourceDiagramModel};
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceReactionBuildingBlock, _cloneManager);
      }

      [Observation]
      public void the_source_diagram_manager_creates_new_diagram_manager_for_target()
      {
         sut.DiagramManager.ShouldBeEqualTo(_newDiagramManagerFromSource);
      }

      [Observation]
      public void the_source_diagram_manager_creates_new_diagram_model_for_target()
      {
         sut.DiagramModel.ShouldBeEqualTo(_newDiagramModelFromSource);
      }

      [Observation]
      public void should_initialize_the_diagram_model()
      {
         sut.DiagramModel.ShouldBeEqualTo(_newDiagramModelFromSource);
         A.CallTo(() => _newDiagramManagerFromSource.InitializeWith(sut, A<IDiagramOptions>._)).MustHaveHappened();
      }
   }
}