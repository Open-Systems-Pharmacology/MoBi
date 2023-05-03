using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Visitor;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IBuildingBlockTaskRetriever
   {
      IInteractionTasksForBuildingBlock TaskFor(IBuildingBlock buildingBlock);
   }

   public class BuildingBlockTaskRetriever : IBuildingBlockTaskRetriever,
      IStrictVisitor,
      IVisitor<MoleculeBuildingBlock>,
      IVisitor<ObserverBuildingBlock>,
      IVisitor<ReactionBuildingBlock>,
      IVisitor<MoBiReactionBuildingBlock>,
      IVisitor<MoBiSpatialStructure>,
      IVisitor<EventGroupBuildingBlock>,
      IVisitor<PassiveTransportBuildingBlock>,
      IVisitor<MoleculeStartValuesBuildingBlock>,
      IVisitor<ParameterStartValuesBuildingBlock>,
      IVisitor<SimulationSettings>
   {
      private readonly IContainer _container;
      private IInteractionTasksForBuildingBlock _task;

      public BuildingBlockTaskRetriever(IContainer container)
      {
         _container = container;
      }

      public IInteractionTasksForBuildingBlock TaskFor(IBuildingBlock buildingBlock)
      {
         try
         {
            this.Visit(buildingBlock);
            return _task;
         }
         finally
         {
            _task = null;
         }
      }

      private void retrieveTask<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : class, IBuildingBlock
      {
         _task = _container.Resolve<IInteractionTasksForBuildingBlock<Module, TBuildingBlock>>();
      }

      private void retrieveTask<TBuildingBlock>() where TBuildingBlock : class, IBuildingBlock
      {
         _task = _container.Resolve<IInteractionTasksForBuildingBlock<Module, TBuildingBlock>>();
      }

      public void Visit(MoleculeBuildingBlock objToVisit)
      {
         retrieveTask<MoleculeBuildingBlock>(objToVisit);
      }

      public void Visit(ObserverBuildingBlock objToVisit)
      {
         retrieveTask<ObserverBuildingBlock>(objToVisit);
      }

      public void Visit(ReactionBuildingBlock objToVisit)
      {
         retrieveTask<MoBiReactionBuildingBlock>();
      }

      public void Visit(PassiveTransportBuildingBlock objToVisit)
      {
         retrieveTask<PassiveTransportBuildingBlock>(objToVisit);
      }

      public void Visit(MoleculeStartValuesBuildingBlock objToVisit)
      {
         retrieveTask<MoleculeStartValuesBuildingBlock>(objToVisit);
      }

      public void Visit(ParameterStartValuesBuildingBlock objToVisit)
      {
         retrieveTask<ParameterStartValuesBuildingBlock>(objToVisit);
      }

      public void Visit(MoBiReactionBuildingBlock objToVisit)
      {
         retrieveTask<MoBiReactionBuildingBlock>(objToVisit);
      }

      public void Visit(EventGroupBuildingBlock objToVisit)
      {
         retrieveTask<EventGroupBuildingBlock>(objToVisit);
      }

      public void Visit(MoBiSpatialStructure objToVisit)
      {
         retrieveTask<MoBiSpatialStructure>(objToVisit);
      }

      public void Visit(SimulationSettings objToVisit)
      {
         retrieveTask<SimulationSettings>(objToVisit);
      }
   }
}