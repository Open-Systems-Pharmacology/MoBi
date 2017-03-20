using System.Threading.Tasks;

namespace MoBi.BatchTool.Runners
{
   public interface IBatchRunner
   {
      Task RunBatch(dynamic parameters);
   }
}