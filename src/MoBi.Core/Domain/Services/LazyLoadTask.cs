using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Domain.Services
{
   public class LazyLoadTask : ILazyLoadTask
   {
      private readonly IRegisterTask _registerTask;

      public LazyLoadTask(IRegisterTask registerTask)
      {
         _registerTask = registerTask;
      }

      public void Load<TObject>(TObject objectToLoad) where TObject : class, ILazyLoadable
      {
         var simulation = objectToLoad as IMoBiSimulation;
         if (simulation == null)
            return;

         //simulation are not loaded when project is laoded. Register them
         _registerTask.RegisterAllIn(simulation);
      }
   }
}
