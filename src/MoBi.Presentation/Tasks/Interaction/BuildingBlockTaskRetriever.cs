using OSPSuite.Utility.Visitor;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
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
                                             IVisitor<IObserverBuildingBlock>,
                                             IVisitor<IReactionBuildingBlock>,
                                             IVisitor<IMoBiReactionBuildingBlock>,
                                             IVisitor<IMoBiSpatialStructure>,
                                             IVisitor<IEventGroupBuildingBlock>,
                                             IVisitor<IPassiveTransportBuildingBlock>,
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

      private void retrieveTask<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock: class, IBuildingBlock
      {
         _task = _container.Resolve<IInteractionTasksForBuildingBlock<TBuildingBlock>>();
      }
      private void retrieveTask<TBuildingBlock>() where TBuildingBlock : class, IBuildingBlock
      {
         _task = _container.Resolve<IInteractionTasksForBuildingBlock<TBuildingBlock>>();
      }
 
      public void Visit(MoleculeBuildingBlock objToVisit)
      {
         retrieveTask(objToVisit);
      }

      public void Visit(IObserverBuildingBlock objToVisit)
      {
         retrieveTask(objToVisit);
      }

      public void Visit(IReactionBuildingBlock objToVisit)
      {
         retrieveTask<IMoBiReactionBuildingBlock>();
      }

      public void Visit(IPassiveTransportBuildingBlock objToVisit)
      {
         retrieveTask(objToVisit);
      }
      public void Visit(MoleculeStartValuesBuildingBlock objToVisit)
      {
         retrieveTask(objToVisit);
      }

      public void Visit(ParameterStartValuesBuildingBlock objToVisit)
      {
         retrieveTask(objToVisit);
      }

      public void Visit(IMoBiReactionBuildingBlock objToVisit)
      {
         retrieveTask(objToVisit);
      }

      public void Visit(IEventGroupBuildingBlock objToVisit)
      {
         retrieveTask(objToVisit);
      }

      public void Visit(IMoBiSpatialStructure objToVisit)
      {
         retrieveTask(objToVisit);
      }

      public void Visit(SimulationSettings objToVisit)
      {
         retrieveTask(objToVisit);
      }
   }
}