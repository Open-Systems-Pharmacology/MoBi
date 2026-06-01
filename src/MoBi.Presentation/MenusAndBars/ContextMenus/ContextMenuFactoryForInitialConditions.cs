using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuFactoryForInitialConditions : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IMoBiContext _context;

      public ContextMenuFactoryForInitialConditions(IMoBiContext context) => _context = context;

      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
         => new ContextMenuForInitialConditions(_context);

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
         => presenter.IsAnImplementationOf<InitialConditionsPresenter>()
            && objectRequestingContextMenu == null;
   }
}