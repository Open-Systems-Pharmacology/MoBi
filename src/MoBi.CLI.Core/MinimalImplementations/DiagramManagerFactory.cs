using Autofac;
using MoBi.Core.Services;
using OSPSuite.Core.Diagram;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class DiagramManagerFactory : IDiagramManagerFactory
   {
      private readonly IComponentContext _context;

      public DiagramManagerFactory(IComponentContext context)
      {
         _context = context;
      }

      public T Create<T>() where T : IDiagramManager
      {
         return _context.Resolve<T>();
      }
   }
}