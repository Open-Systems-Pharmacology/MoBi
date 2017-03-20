using System.Collections.Generic;
using System.Linq;
using NHibernate;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.MetaData.Extensions
{
   public static class UpdatableCollectionExtensions
   {
      public static void UpdateFrom<TKey, TValue>(this ICollection<TValue> targetCollection, ICollection<TValue> sourceCollection, ISession session)
         where TValue : MetaData<TKey>, IUpdatableFrom<TValue>
      {
         var targetDictionary = targetCollection.ToDictionary(entity => entity.Id);
         foreach (var sourceChild in sourceCollection)
         {
            // new child was added to source dictionary...but is not available in the target dictionary
            if (!targetCollection.Contains(sourceChild))
            {
               targetCollection.Add(sourceChild);
               continue;
            }
            targetDictionary[sourceChild.Id].UpdateFrom(sourceChild, session);
         }

         foreach (var child in targetCollection.ToList())
         {
            if (sourceCollection.Contains(child)) continue;

            //does not exist. Remove
            targetCollection.Remove(child);
         }
      }
   }
}