using System.Drawing;
using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_MoveDiagramNodeCommand : ContextSpecification<MoveDiagramNodeCommand>
   {
      protected MoBiReactionBuildingBlock _buildingBlock;
      protected IBaseNode _destinationNode, _targetNode;
      protected PointF _parentLocation;
      protected IMoBiContext _context;
      protected IDiagramModel _diagramModel;

      protected override void Context()
      {
         _buildingBlock = A.Fake<MoBiReactionBuildingBlock>().WithId("id");
         _destinationNode = A.Fake<IBaseNode>();
         _targetNode = A.Fake<IBaseNode>();
         _parentLocation = new PointF();
         _context = A.Fake<IMoBiContext>();
         _diagramModel = A.Fake<IDiagramModel>();
         _buildingBlock.DiagramModel = _diagramModel;

         A.CallTo(() => _context.Get<MoBiReactionBuildingBlock>("id")).Returns(_buildingBlock);
         A.CallTo(() => _destinationNode.Name).Returns("NamedNode");
         A.CallTo(() => _diagramModel.FindByName(_destinationNode.Name)).Returns(_targetNode);

         sut = new MoveDiagramNodeCommand(_buildingBlock, "NamedNode", _destinationNode);
      }
   }

   public class When_reverting_command_to_move_a_node_in_the_diagram : concern_for_MoveDiagramNodeCommand
   {
      private IBaseNode _copyNode;
      protected override void Context()
      {
         base.Context();
         _copyNode = A.Fake<IBaseNode>();
         A.CallTo(() => _copyNode.Name).Returns("NamedNode");

         A.CallTo(() => _targetNode.Copy()).Returns(_copyNode);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void a_call_to_move_target_back_should_have_happened()
      {
         A.CallTo(() => _targetNode.CopyLayoutInfoFrom(_copyNode, A<PointF>.Ignored)).MustHaveHappened();         
      }
   }

   public class When_moving_nodes_in_a_diagram : concern_for_MoveDiagramNodeCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void call_to_move_target_node_should_have_happened()
      {
         A.CallTo(() => _targetNode.CopyLayoutInfoFrom(_destinationNode, _parentLocation)).MustHaveHappened();
      }

      [Observation]
      public void a_call_to_copy_the_original_node_must_have_happened()
      {
         A.CallTo(() => _targetNode.Copy()).MustHaveHappened();
      }
   }
}
