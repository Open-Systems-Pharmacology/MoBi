using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Utility.Extensions;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Tasks
{
   public interface ISearchTask
   {
      IEnumerable<SearchResult> StartSearch(SearchOptions options, IObjectBase localSearchTarget);
   }

   public class SearchTask : ISearchTask
   {
      private readonly ISearchVisitor _searchVisitor;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly ISimulationRepository _simulationRepository;

      public SearchTask(ISearchVisitor searchVisitor, IMoBiProjectRetriever projectRetriever, IBuildingBlockRepository buildingBlockRepository, ISimulationRepository simulationRepository)
      {
         
         _searchVisitor = searchVisitor;
         _projectRetriever = projectRetriever;
         _buildingBlockRepository = buildingBlockRepository;
         _simulationRepository = simulationRepository;
      }

      public IEnumerable<SearchResult> StartSearch(SearchOptions options, IObjectBase localSearchTarget)
      {
         if (options.Expression.IsNullOrEmpty())
            return Enumerable.Empty<SearchResult>();

         _searchVisitor.SearchFor = options.Expression;
         _searchVisitor.RegExSearch = options.RegEx;
         _searchVisitor.WholeWord = options.WholeWord;
         _searchVisitor.CaseSensitive = options.CaseSensitive;

         switch (options.Scope)
         {
            case SearchScope.Project:
               return _searchVisitor.SearchIn(_projectRetriever.Current, _buildingBlockRepository.All());

            case SearchScope.AllOfSameType:
               if (localSearchTarget == null)
                  return Enumerable.Empty<SearchResult>();

               return searchInAllOfSameType(localSearchTarget.GetType(), _buildingBlockRepository.All());
            case SearchScope.Local:
               if (localSearchTarget == null)
                  return Enumerable.Empty<SearchResult>();

               return _searchVisitor.SearchIn(localSearchTarget, _buildingBlockRepository.All());
            default:
               throw new ArgumentOutOfRangeException(nameof(options.Scope));
         }

      }

      private IEnumerable<SearchResult> searchInAllOfSameType(Type buildingBlockType, IReadOnlyList<IBuildingBlock> buildingBlocksToSearch)
      {
         var result = new List<SearchResult>();
         var buildingBlocks = getBuildingBlocksOfType(buildingBlockType);
         buildingBlocks.Each(buildingBlock => result.AddRange(_searchVisitor.SearchIn(buildingBlock, buildingBlocksToSearch)));
         return result;
      }

      private IEnumerable<IObjectBase> getBuildingBlocksOfType(Type buildingBlockType)
      {
         if (buildingBlockType.IsAnImplementationOf<IModelCoreSimulation>())
            return _simulationRepository.All();

         if (buildingBlockType.IsAnImplementationOf<MoleculeBuildingBlock>())
            return _buildingBlockRepository.MoleculeBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<ReactionBuildingBlock>())
            return _buildingBlockRepository.ReactionBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<SpatialStructure>())
            return _buildingBlockRepository.SpatialStructureCollection;

         if (buildingBlockType.IsAnImplementationOf<ObserverBuildingBlock>())
            return _buildingBlockRepository.ObserverBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<EventGroupBuildingBlock>())
            return _buildingBlockRepository.EventBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<InitialConditionsBuildingBlock>())
            return _buildingBlockRepository.InitialConditionBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<ParameterValuesBuildingBlock>())
            return _buildingBlockRepository.ParametersValueBlockCollection;

         return null;
      }
   }
}