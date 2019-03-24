using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks
{
   public interface ISimModelManagerFactory
   {
      ISimModelManager Create();
   }
}