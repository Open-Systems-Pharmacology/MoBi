using Autofac;
using MoBi.Core.Services;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Infrastructure.Serialization.ORM.History;

namespace MoBi.R.MinimalImplementations
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
