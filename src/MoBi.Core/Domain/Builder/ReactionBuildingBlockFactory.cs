using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Builder
{
   public interface IReactionBuildingBlockFactory
   {
      MoBiReactionBuildingBlock Create();
   }

   public class ReactionBuildingBlockFactory : IReactionBuildingBlockFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IDiagramManagerFactory _diagramManagerFactory;
      private readonly IReactionDiagramModelFactory _reactionDiagramModelFactory;

      public ReactionBuildingBlockFactory(IObjectBaseFactory objectBaseFactory, IDiagramManagerFactory diagramManagerFactory, IReactionDiagramModelFactory reactionDiagramModelFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _diagramManagerFactory = diagramManagerFactory;
         _reactionDiagramModelFactory = reactionDiagramModelFactory;
      }

      public MoBiReactionBuildingBlock Create()
      {
         var buildingBlock = _objectBaseFactory.Create<MoBiReactionBuildingBlock>();
         buildingBlock.DiagramManager = _diagramManagerFactory.Create<IMoBiReactionDiagramManager>();
         buildingBlock.DiagramModel = _reactionDiagramModelFactory.Create();
         return buildingBlock;
      }
   }
}