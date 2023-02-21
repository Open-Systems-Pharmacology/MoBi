using MoBi.Core.Domain.Model;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.Presenter.BaseDiagram
{
   public interface IDiagramPopupMenuBaseWithContext : IDiagramPopupMenuBase
   {
      T Get<T>(string id) where T : class, IObjectBase;
   }

   public class DiagramPopupMenuBaseWithContext : DiagramPopupMenuBase, IDiagramPopupMenuBaseWithContext
   {
      private readonly IMoBiContext _context;

      public DiagramPopupMenuBaseWithContext(IMoBiBaseDiagramPresenter presenter, IMoBiContext context, IStartOptions runOptions, IContainer container)
         : base(presenter, runOptions, container)
      {
         _context = context;
      }

      public T Get<T>(string id) where T : class, IObjectBase
      {
         return _context.Get<T>(id);
      }
   }
}