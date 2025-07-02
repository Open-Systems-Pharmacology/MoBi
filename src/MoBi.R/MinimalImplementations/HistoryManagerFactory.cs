using Autofac;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.Serialization.ORM.History;

namespace MoBi.R.MinimalImplementations
{
   public class HistoryManagerFactory: IHistoryManagerFactory
   {
      private readonly IComponentContext _context;
      public HistoryManagerFactory(IComponentContext context)
      {
         _context = context;
      }

      public IHistoryManager Create()
      {
         return _context.Resolve<IHistoryManager>();
      }
   }
}
