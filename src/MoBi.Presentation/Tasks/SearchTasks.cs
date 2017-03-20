using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{

   public interface ISearchTasks
   {
      IEnumerable<SearchResult> StartSearch(SearchOptions options, IObjectBase localSearchTarget);
   }

   class SearchTasks : ISearchTasks
   {
      private readonly ISearchVisitor _searchVisitor;
      private readonly IMoBiContext _context;

      public SearchTasks(ISearchVisitor searchVisitor, IMoBiContext context)
      {
         _searchVisitor = searchVisitor;
         _context = context;
      }

      public IEnumerable<SearchResult> StartSearch(SearchOptions options, IObjectBase localSearchTarget)
      {
         if (options.Expression.IsNullOrEmpty()) return Enumerable.Empty<SearchResult>();
         
         _searchVisitor.SearchFor = options.Expression;
         _searchVisitor.RegExSearch = options.RegEx;
         _searchVisitor.WholeWord = options.WholeWord;
         _searchVisitor.CaseSensitiv = options.CaseSensitive;
         var project = _context.CurrentProject;
         IEnumerable<SearchResult> results;
         switch (options.Scope)
         {
            case SearchScope.Project:
               
               results = _searchVisitor.SearchIn(project,project);
               break;
            case SearchScope.AllOfSameType:
               if (localSearchTarget == null) return Enumerable.Empty<SearchResult>();
               results = searchInAllOfSameType(localSearchTarget.GetType(),project);
               break;
            case SearchScope.Local:
               if (localSearchTarget == null) return Enumerable.Empty<SearchResult>();
               results = _searchVisitor.SearchIn(localSearchTarget,project);
               break;
            default:
               throw new ArgumentOutOfRangeException("scope");
         }

         return results;
      }

      private IEnumerable<SearchResult> searchInAllOfSameType(Type buildingBlockType, IMoBiProject project)
      {
         var result = new List<SearchResult>();
         IEnumerable<IObjectBase> buildingBlocks = getBuildingBlocksOfType(buildingBlockType);
         buildingBlocks.Each(buildingBlock => result.AddRange(_searchVisitor.SearchIn(buildingBlock,project)));
         return result;
      }

      private IEnumerable<IObjectBase> getBuildingBlocksOfType(Type buildingBlockType)
      {
         if (buildingBlockType.IsAnImplementationOf<IModelCoreSimulation>())
         {
            return _context.CurrentProject.Simulations;
         }
         if (buildingBlockType.IsAnImplementationOf<IMoleculeBuildingBlock>())
         {
            return _context.CurrentProject.MoleculeBlockCollection;
         }
         if (buildingBlockType.IsAnImplementationOf<IReactionBuildingBlock>())
         {
            return _context.CurrentProject.ReactionBlockCollection;
         }
         if (buildingBlockType.IsAnImplementationOf<ISpatialStructure>())
         {
            return _context.CurrentProject.SpatialStructureCollection;
         }
         if (buildingBlockType.IsAnImplementationOf<IObserverBuildingBlock>())
         {
            return _context.CurrentProject.ObserverBlockCollection;
         }
         if (buildingBlockType.IsAnImplementationOf<IEventGroupBuildingBlock>())
         {
            return _context.CurrentProject.EventBlockCollection;
         }
         if (buildingBlockType.IsAnImplementationOf<IMoleculeStartValuesBuildingBlock>())
         {
            return _context.CurrentProject.MoleculeStartValueBlockCollection;
         }
         if (buildingBlockType.IsAnImplementationOf<IParameterStartValuesBuildingBlock>())
         {
            return _context.CurrentProject.ParametersStartValueBlockCollection;
         }
         return null;
      }
   }
}