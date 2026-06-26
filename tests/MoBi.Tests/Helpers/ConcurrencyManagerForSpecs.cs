using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.HelpersForTests
{
   //Runs each item's work synchronously and in order.
   public class ConcurrencyManagerForSpecs : IConcurrencyManager
   {
      public Task<IDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>(IReadOnlyList<TData> data, Func<TData, CancellationToken, TResult> func, CancellationToken cancellationToken, int numberOfCoresToUse) where TData : IWithId
      {
         var results = new Dictionary<TData, ConcurrencyManagerResult<TResult>>();
         foreach (var datum in data)
         {
            try
            {
               results[datum] = new ConcurrencyManagerResult<TResult>(datum.Id, func(datum, cancellationToken));
            }
            catch (Exception exception)
            {
               results[datum] = new ConcurrencyManagerResult<TResult>(datum.Id, exception.Message);
            }
         }

         return Task.FromResult<IDictionary<TData, ConcurrencyManagerResult<TResult>>>(results);
      }

      public Task RunAsync<TData>(IReadOnlyList<TData> data, Action<TData, CancellationToken> action, CancellationToken cancellationToken, int numberOfCoresToUse)
      {
         foreach (var datum in data)
            action(datum, cancellationToken);

         return Task.CompletedTask;
      }
   }
}
