using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Domain.Services
{
   public interface IEntitiesInBuildingBlockRetriever<T>
   {
      IReadOnlyList<T> AllFrom(IBuildingBlock buildingBlock, Func<T, bool> criteriaFunc = null);
   }

   public class EntitiesInBuildingBlockRetriever<T> :
      IEntitiesInBuildingBlockRetriever<T>,
      IVisitor<T>
   {
      private readonly List<T> _allEntities = new List<T>();
      private Func<T, bool> _criteriaFunc;

      public IReadOnlyList<T> AllFrom(IBuildingBlock buildingBlock, Func<T, bool> criteriaFunc = null)
      {
         try
         {
            _criteriaFunc = criteriaFunc ?? (x => true);
            buildingBlock.AcceptVisitor(this);
            return _allEntities.ToList();
         }
         finally
         {
            _allEntities.Clear();
         }
      }

      public void Visit(T objToVisit)
      {
         if (!_criteriaFunc(objToVisit))
            return;

         _allEntities.Add(objToVisit);
      }
   }
}