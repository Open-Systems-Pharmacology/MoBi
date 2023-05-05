using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Services
{
   public interface IBuildingBlockRetriever
   {
      IBuildingBlock GetBuildingBlockFor(IObjectBase objectBase, IBuildingBlock buildingBlock);
   }

   public class BuildingBlockRetriever : IBuildingBlockRetriever, IVisitor<MoBiReactionBuildingBlock>, IVisitor<MoleculeBuildingBlock>,
                                         IVisitor<MoBiSpatialStructure>, IVisitor<ObserverBuildingBlock>, IVisitor<EventGroupBuildingBlock>,
                                         IVisitor<PassiveTransportBuildingBlock>, IVisitor<InitialConditionsBuildingBlock>, IVisitor<ParameterStartValuesBuildingBlock>
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private IEntity _entity;
      private bool _found;

      public BuildingBlockRetriever(IBuildingBlockRepository buildingBlockRepository)
      {
         _buildingBlockRepository = buildingBlockRepository;
      }

      public IBuildingBlock GetBuildingBlockFor(IObjectBase objectBase, IBuildingBlock buildingBlock)
      {
         try
         {
            var formula = objectBase as IFormula;

            if (formula != null)
               return buildingBlockForFormula(formula, buildingBlock);

            _entity = objectBase as IEntity;
            var allBuildingBlocks = _buildingBlockRepository.All();
            
            if (_entity == null)
               throw new ArgumentException(AppConstants.Exceptions.BuildingBlockNotFoundFor(objectBase));

            //looking for the first building block containing the root container or the entity itself if null
            _entity = _entity.RootContainer ?? _entity;
            _found = false;
            return allBuildingBlocks.FirstOrDefault(containsEntity);
         }
         finally
         {
            _entity = null;
         }
      }

      private IBuildingBlock buildingBlockForFormula(IFormula formula, IBuildingBlock  buildingBlock)
      {
         var allBuildingBlocks = _buildingBlockRepository.All().ToList();
         var templateBuildingBlock = allBuildingBlocks.FirstOrDefault(bb => bb.FormulaCache.Contains(formula));
         if (templateBuildingBlock != null)
            return templateBuildingBlock;

         if (buildingBlock == null)
            return null;

         //try to find template building block according to name and type
         templateBuildingBlock = allBuildingBlocks.Where(x => x.IsAnImplementationOf(buildingBlock.GetType())).FirstOrDefault(x => x.IsNamed(buildingBlock.Name));
         if (templateBuildingBlock == null)
            return null;

         if (templateBuildingBlock.FormulaCache.Any(f => f.IsNamed(formula.Name)))
            return templateBuildingBlock;

         return null;
      }

      private bool containsEntity(IBuildingBlock buildingBlock)
      {
         buildingBlock.AcceptVisitor(this);
         return _found;
      }

      public void Visit(MoBiReactionBuildingBlock reactionBuildingBlock)
      {
         _found = reactionBuildingBlock.Contains(_entity);
      }

      public void Visit(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         _found = moleculeBuildingBlock.Contains(_entity);
      }

      public void Visit(MoBiSpatialStructure spatialStructure)
      {
         _found = spatialStructure.Contains(_entity);
      }

      public void Visit(ObserverBuildingBlock observerBuildingBlock)
      {
         _found = observerBuildingBlock.Contains(_entity);
      }

      public void Visit(EventGroupBuildingBlock eventGroupBuildingBlock)
      {
         _found = eventGroupBuildingBlock.Contains(_entity);
      }

      public void Visit(InitialConditionsBuildingBlock objToVisit)
      {
         _found = objToVisit.Contains(_entity);
      }

      public void Visit(ParameterStartValuesBuildingBlock objToVisit)
      {
         _found = objToVisit.Contains(_entity);
      }

      public void Visit(PassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         _found = passiveTransportBuildingBlock.Contains(_entity);
      }
   }
}