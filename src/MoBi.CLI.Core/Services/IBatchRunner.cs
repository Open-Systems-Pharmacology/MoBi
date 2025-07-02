using System.Threading.Tasks;

namespace MoBi.CLI.Core.Services
{
   public interface IBatchRunner<TBatchOptions>
   {
      Task RunBatchAsync(TBatchOptions runOptions);
   }
}