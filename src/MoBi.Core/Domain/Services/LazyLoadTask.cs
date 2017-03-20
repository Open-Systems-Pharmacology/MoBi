using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Domain.Services
{
   public class LazyLoadTask : ILazyLoadTask
   {
      private readonly IRegisterAllVisitor _registerAllVisitor;

      public LazyLoadTask(IRegisterAllVisitor registerAllVisitor)
      {
         _registerAllVisitor = registerAllVisitor;
      }

      public void Load<TObject>(TObject objectToLoad) where TObject : class, ILazyLoadable
      {
         var simulation = objectToLoad as IMoBiSimulation;
         if (simulation == null)
            return;

         //simulation are not loaded when project is laoded. Register them
         _registerAllVisitor.RegisterAllIn(simulation);
      }
   }
}
