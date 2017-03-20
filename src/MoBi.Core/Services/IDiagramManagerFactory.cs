using OSPSuite.Core.Diagram;

namespace MoBi.Core.Services
{
   public interface IDiagramManagerFactory
   {
      T Create<T>() where T : IDiagramManager;
   }
}
