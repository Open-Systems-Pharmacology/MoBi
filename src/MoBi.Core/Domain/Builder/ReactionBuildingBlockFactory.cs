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
      private readonly IDiagramModelFactory _diagramModelFactory;

      public ReactionBuildingBlockFactory(IObjectBaseFactory objectBaseFactory, IDiagramManagerFactory diagramManagerFactory, IDiagramModelFactory diagramModelFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _diagramManagerFactory = diagramManagerFactory;
         _diagramModelFactory = diagramModelFactory;
      }

      public MoBiReactionBuildingBlock Create()
      {
         var buildingBlock = _objectBaseFactory.Create<MoBiReactionBuildingBlock>();
         buildingBlock.DiagramManager = _diagramManagerFactory.Create<IMoBiReactionDiagramManager>();
         buildingBlock.DiagramModel = _diagramModelFactory.Create();
         return buildingBlock;
      }
   }
}