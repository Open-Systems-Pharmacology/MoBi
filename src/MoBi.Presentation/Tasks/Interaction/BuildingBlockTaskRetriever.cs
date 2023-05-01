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

      private void retrieveTask<TParent, TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : class, IBuildingBlock where TParent : class
      {
         _task = _container.Resolve<IInteractionTasksForBuildingBlock<TParent, TBuildingBlock>>();
      }

      private void retrieveTask<TParent, TBuildingBlock>() where TBuildingBlock : class, IBuildingBlock where TParent : class
      {
         _task = _container.Resolve<IInteractionTasksForBuildingBlock<TParent, TBuildingBlock>>();
      }

      public void Visit(MoleculeBuildingBlock objToVisit)
      {
         retrieveTask<Module, MoleculeBuildingBlock>(objToVisit);
      }

      public void Visit(ObserverBuildingBlock objToVisit)
      {
         retrieveTask<Module, ObserverBuildingBlock>(objToVisit);
      }

      public void Visit(ReactionBuildingBlock objToVisit)
      {
         retrieveTask<Module, MoBiReactionBuildingBlock>();
      }

      public void Visit(PassiveTransportBuildingBlock objToVisit)
      {
         retrieveTask<Module, PassiveTransportBuildingBlock>(objToVisit);
      }

      public void Visit(MoleculeStartValuesBuildingBlock objToVisit)
      {
         retrieveTask<Module, MoleculeStartValuesBuildingBlock>(objToVisit);
      }

      public void Visit(ParameterStartValuesBuildingBlock objToVisit)
      {
         retrieveTask<Module, ParameterStartValuesBuildingBlock>(objToVisit);
      }

      public void Visit(MoBiReactionBuildingBlock objToVisit)
      {
         retrieveTask<Module, MoBiReactionBuildingBlock>(objToVisit);
      }

      public void Visit(EventGroupBuildingBlock objToVisit)
      {
         retrieveTask<Module, EventGroupBuildingBlock>(objToVisit);
      }

      public void Visit(MoBiSpatialStructure objToVisit)
      {
         retrieveTask<Module, MoBiSpatialStructure>(objToVisit);
      }

      public void Visit(SimulationSettings objToVisit)
      {
         retrieveTask<Module, SimulationSettings>(objToVisit);
      }
   }
}